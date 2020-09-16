using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ash.SpriteSystem;
using Ash.Systems;
using Ash.UI;
using System;
using System.Text;

namespace Ash
{
    /// <summary>
    /// A scene to be used in <c>Core.Scene</c>. GiaScenes construct their systems and entities
    /// during their constructor, and will not unload their assets until specifically
    /// disposed, so feel free to swap between GiaScenes as game flow. If you want this to be destroyed
    /// on scene switch, set <c>GiaScene.DestroyOnExit = true</c>
    /// </summary>
    public class GiaScene : IScene, IDisposable
    {
        public enum AspectRatioResolution
        {
            /// <summary>
            /// The final image will fill the entire screen, no matter the ratio difference.
            /// </summary>
            Stretch,

            /// <summary>
            /// The final image will be stretched, but will maintain its aspect ratio, centered(or not with AspectOrigin)
            /// with letterboxing.
            /// </summary>
            MaintainRatio,

            /// <summary>
            /// Same as MaintainRatio, except it will overscale to fill the letterbox.
            /// </summary>
            MaintainRatioFill
        }

        /// <summary>
        /// A delegate that takes a scene context and outputs a string that will
        /// be shown on the titlebar during analytic ticks.
        /// </summary>
        public delegate string WindowTitleNub(GiaScene scene);
        /// <summary>
        /// A list of <c>GiaScene.WindowTitleNub</c> delegate methods. Anything in here
        /// will be ran and output into the window title for nice 
        /// </summary>
        public FastList<WindowTitleNub> Nubs;
        /// <summary>
        /// This isnt used in anything, but it can be useful for organizing
        /// your scenes.
        /// </summary>
        public string SceneName;
        /// <summary>
        /// This is the World used in all the systems and entities in this Scene.
        /// It comes from <c>DefaultECS</c>, the ECS that Gia uses internally.
        /// Pass this to any Systems you create.
        /// </summary>
        public World World;
        /// <summary>
        /// A parallel running context, I don't recommend making a parallel system
        /// without some prior experience, they're kinda messy in monogame.
        /// </summary>
        public IParallelRunner Runner;
        /// <summary>
        /// The content manager for this scene. Try to load everything you can in
        /// here rather than <c>Core.Content</c>. Everything loaded here gets
        /// disposed and unloaded when the scene gets .Disposed
        /// </summary>
        public NezContentManager Content;

        /// <summary>
        /// This determines how the final image gets placed on the screen if you are
        /// not using <c>GiaScene.AutoResizeToNativeResolution = true</c>
        /// </summary>
        public AspectRatioResolution AspectRatio;
        /// <summary>
        /// For certain AspectRatio settings, it may be necessary to have an "origin"
        /// for the final image. It is expected to be a normal. By default this is <c>Vector2(0.5f,0.5f)</c>.
        /// </summary>
        public Vector2 AspectOrigin;

        public FastList<ISystem<GiaScene>> UpdateSystems;
        public FastList<ISystem<GiaScene>> RenderSystems;

        public Batcher Batcher;

        public RenderTarget2D SceneTarget;
        public SamplerState SamplerState;

        public Camera View;
        public RectangleF ScreenBounds;
        RectangleF FinalScreenOutput;
        float ScreenScale;

        public bool DestroyOnExit;
        public bool AutoResizeToNativeResolution;

        public Vector2 MouseDelta;
        public Vector2 WorldMousePosition;
        public Vector2 MousePosition;
        public Vector2 LateralWorldMouseDelta;

        public GiaScene()
        {
            Gia.BeingConstructed = this;

            View = new Camera();
            View.Origin = new Vector2(Screen.Width * 0.5f, Screen.Height * 0.5f);
            ScreenBounds = new RectangleF(0, 0, Screen.Width, Screen.Height);
            FinalScreenOutput = new RectangleF(0, 0, Screen.Width, Screen.Height);
            AutoResizeToNativeResolution = true;

            AspectRatio = AspectRatioResolution.Stretch;
            AspectOrigin = new Vector2(0.5f, 0.5f);
            SamplerState = SamplerState.PointClamp;

            DestroyOnExit = false;
            Nubs = new FastList<WindowTitleNub>();
            Runner = new DefaultParallelRunner(Environment.ProcessorCount);
            World = new World();
            Content = new NezContentManager();
            UpdateSystems = new FastList<ISystem<GiaScene>>();
            RenderSystems = new FastList<ISystem<GiaScene>>();
            Batcher = Graphics.Instance.Batcher;
            SceneTarget = new RenderTarget2D(Core.GraphicsDevice, Screen.Width, Screen.Height);

            ConstructSystemGraph(UpdateSystems, RenderSystems);
            ConstructEntityGraph();
            Gia.BeingConstructed = null;
        }

        public virtual void ConstructSystemGraph(FastList<ISystem<GiaScene>> updateSystems, FastList<ISystem<GiaScene>> renderSystems)
        {
            AddDefaultSystems();
            Debug.Warn("Added default systems. Though nice, they are likely not suited for your game! Override ConstructSystemGraph in " + GetType().Name);
        }
        public virtual void ConstructEntityGraph()
        {
            string message = "Welcome to Gia!\nYou'll need to override ConstructSystemGraph and ConstructEntityGraph\nIn order to start your game.";
            var stringMeasure = Gia.Theme.DefaultFont.MeasureString(message);

            var welcome = World.CreateEntity();
            welcome.Set(new AABB(Screen.Width / 2, Screen.Height / 2, stringMeasure.X, stringMeasure.Y));
            Debug.Warn("Added default Entities. This means your scene is basically empty. Override ConstructEntityGraph in " + GetType().Name);
        }

        public void Begin()
        {
            Core.Instance.Window.Title = Core.Instance.Title;
            Resume();
        }

        public void End()
        {
            Pause();
            if (DestroyOnExit)
            {
                Dispose();
            }
        }

        /// <summary>
        /// The update flow of the scene. It is not recommended to override this
        /// unless you have a very specific reason.
        /// </summary>
        public virtual void Update()
        {
#if DEBUG
            if (Gia.Debug.ToggleInput.IsPressed)
                Gia.Debug.Enabled = !Gia.Debug.Enabled;
            Analytics();
#endif
            CalculateFinalRenderTargetDestination();
            InternalInputUpdate();
            for (int i = 0; i < UpdateSystems.Length; i++)
            {
                UpdateSystems[i].Update(this);
            }
        }

        /// <summary>
        /// The render flow of the scene. It is not recommended to override this
        /// unless you have a very specific reason.
        /// </summary>
        public virtual void Render()
        {
            Core.GraphicsDevice.SetRenderTarget(SceneTarget);
            Core.GraphicsDevice.Clear(Gia.Theme.BackgroundColor);

            for (int i = 0; i < RenderSystems.Length; i++)
            {
                RenderSystems[i].Update(this);
            }

            Graphics.Instance.Batcher.Begin();
            Graphics.Instance.Batcher.DrawPixel(MousePosition, Color.Red, 5);
            Graphics.Instance.Batcher.DrawString(Gia.Theme.DeveloperFont, MousePosition.ToPoint().ToString(), MousePosition, Color.Red);
            Graphics.Instance.Batcher.End();

            // Then Finalize to screen.
            Core.GraphicsDevice.SetRenderTarget(null);
            Core.GraphicsDevice.Clear(Gia.Theme.ApplicationNullColor);

            Graphics.Instance.Batcher.Begin(BlendState.AlphaBlend, SamplerState, null, null);

            Graphics.Instance.Batcher.Draw(SceneTarget, FinalScreenOutput, Color.White);

            if (Gia.Debug.Enabled)
                Gia.Debug.Flush(Graphics.Instance.Batcher);
            else
                Gia.Debug.Cancel();

            Graphics.Instance.Batcher.DrawPixel(Input.MousePosition, Color.Orange, 5);

            Graphics.Instance.Batcher.End();

        }

        void CalculateFinalRenderTargetDestination()
        {
            if (AutoResizeToNativeResolution)
            {
                FinalScreenOutput = new RectangleF(0, 0, Screen.Width, Screen.Height);
                return;
            }

            switch (AspectRatio)
            {
                case AspectRatioResolution.Stretch:
                    {
                        FinalScreenOutput = new RectangleF(0, 0, Screen.Width, Screen.Height);
                        break;
                    }
                case AspectRatioResolution.MaintainRatio:
                    {
                        ScreenScale = Math.Min((float)Screen.Width / (float)SceneTarget.Width, (float)Screen.Height / (float)SceneTarget.Height);
                        FinalScreenOutput = new RectangleF(Vector2.Zero, new Vector2(SceneTarget.Width * ScreenScale, SceneTarget.Height * ScreenScale));

                        var remainingWidth = Screen.Width - FinalScreenOutput.Width;
                        var remainingHeight = Screen.Height - FinalScreenOutput.Height;

                        var offset = new Vector2(remainingWidth, remainingHeight) * AspectOrigin;

                        FinalScreenOutput.Location = offset;

                        break;
                    }
                case AspectRatioResolution.MaintainRatioFill:
                    {
                        ScreenScale = Math.Max((float)Screen.Width / (float)SceneTarget.Width, (float)Screen.Height / (float)SceneTarget.Height);
                        FinalScreenOutput = new RectangleF(Vector2.Zero, new Vector2(SceneTarget.Width * ScreenScale, SceneTarget.Height * ScreenScale));

                        var remainingWidth = Screen.Width - FinalScreenOutput.Width;
                        var remainingHeight = Screen.Height - FinalScreenOutput.Height;

                        var offset = new Vector2(remainingWidth, remainingHeight) * AspectOrigin;

                        FinalScreenOutput.Location = offset;

                        break;
                    }
            }
        }

        float analyticsTimer = 0;
        StringBuilder analyticsBuilder = new StringBuilder();
        void Analytics()
        {
            analyticsTimer += Time.UnscaledDeltaTime;
            if(analyticsTimer > 1f)
            {
                analyticsTimer -= 1f;
                analyticsBuilder.Clear();
                analyticsBuilder.Append(Core.Instance.Title);
                if (!string.IsNullOrEmpty(SceneName))
                {
                    analyticsBuilder.Append(" | Scene: " + SceneName);
                }
                for(int i = 0; i < Nubs.Length; i++)
                {
                    analyticsBuilder.Append(" | "+Nubs[i](this));
                }
                Core.Instance.Window.Title = analyticsBuilder.ToString();
            }
        }

        void InternalInputUpdate()
        {
            var st_size = new Vector2(SceneTarget.Width, SceneTarget.Height);
            var delta = (Input.MousePosition - FinalScreenOutput.Location) / FinalScreenOutput.Size;
            delta = Vector2.Clamp(delta, Vector2.Zero, st_size);
            var x = Mathf.Lerp(0f, st_size.X, delta.X);
            var y = Mathf.Lerp(0f, st_size.Y, delta.Y);
            MousePosition = new Vector2(x, y);
            MouseDelta = Input.MousePositionDelta.ToVector2() / View.Zoom / ScreenScale;

            if(View.Rotation != 0f)
            {
                LateralWorldMouseDelta = View.ToLateral(MouseDelta);
            }
            else
            {
                LateralWorldMouseDelta = MouseDelta;
            }

            WorldMousePosition = View.ScreenToWorldPoint(MousePosition);
        }

        protected void AddDefaultSystems()
        {
            UpdateSystems.Add(DefaultUpdateStack());
            RenderSystems.Add(DefaultRenderStack());
        }

        public ISystem<GiaScene> DefaultRenderStack()
        {
            return new SequentialSystem<GiaScene>
            (
                new SpriteRenderer(World, screenSpace: false),
                new UIRenderer(World, screenSpace: true)
            );
        }
        public ISystem<GiaScene> DefaultUpdateStack()
        {
            return new SequentialSystem<GiaScene>
            (
                new UIUpdater(World)
            );
        }

        /// <summary>
        /// Called when this scene is set as the current scene. Can happen multiple times.
        /// </summary>
        public virtual void Resume()
        {

        }

        /// <summary>
        /// Called when this scene is unset as the current scene. Can happen multiple times, unless
        /// <c>GiaScene.DestroyOnExit = true</c>.
        /// </summary>
        public virtual void Pause()
        {

        }

        /// <summary>
        /// Adds 2 Nubs to the window title that inspects the fps and draw calls.
        /// </summary>
        public void Inspect()
        {
            Nubs.Add(FpsNub);
            Nubs.Add(DrawCallNub);
        }
        public void Uninspect()
        {
            Nubs.Remove(FpsNub);
            Nubs.Remove(DrawCallNub);
        }
        string FpsNub(GiaScene context)
        {
            return ((int)(1f / Time.UnscaledDeltaTime)).ToString() + " FPS";
        }
        string DrawCallNub(GiaScene context)
        {
            
            return $"{Core.DrawCalls} Draws";
        }

        public void Dispose()
        {
            Content.Dispose();
            World.Dispose();
        }

        void SetSceneSize(int width, int height)
        {
            if (SceneTarget != null)
                SceneTarget.Dispose();
            SceneTarget = new RenderTarget2D(Core.GraphicsDevice, width, height);
            ScreenBounds = new RectangleF(0, 0, width, height);
        }
    }
}
