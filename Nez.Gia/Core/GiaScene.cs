using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez.SpriteSystem;
using Nez.Systems;
using System;
using System.Text;

namespace Nez
{
    public class GiaScene : IScene, IDisposable
    {
        public delegate string WindowTitleNub(GiaScene scene);
        public FastList<WindowTitleNub> Nubs;

        public string SceneName;
        public World World;
        public IParallelRunner Runner;
        public NezContentManager Content;

        FastList<ISystem<GiaScene>> UpdateSystems;
        FastList<ISystem<GiaScene>> RenderSystems;

        public Batcher Batcher;

        public RenderTarget2D SceneTarget;
        public SamplerState SamplerState;

        public Camera View;
        public RectangleF ScreenBounds;

        public bool DestroyOnExit;

        public Vector2 MouseDelta;
        public Vector2 WorldMousePosition;
        public Vector2 MousePosition;
        public Vector2 LateralWorldMouseDelta;

        public GiaScene()
        {
            Gia.InProgress = this;
            DestroyOnExit = true;
            Nubs = new FastList<WindowTitleNub>();
            Runner = new DefaultParallelRunner(Environment.ProcessorCount);
            World = new World();
            Content = new NezContentManager();
            UpdateSystems = new FastList<ISystem<GiaScene>>();
            RenderSystems = new FastList<ISystem<GiaScene>>();
            Batcher = Graphics.Instance.Batcher;
            SceneTarget = new RenderTarget2D(Core.GraphicsDevice, Screen.Width, Screen.Height);

            SamplerState = SamplerState.PointClamp;

            View = new Camera();
            View.Origin = new Vector2(Screen.Width * 0.5f, Screen.Height * 0.5f);
            ScreenBounds = new RectangleF(0, 0, Screen.Width, Screen.Height);

            ConstructSystemGraph(UpdateSystems, RenderSystems);
            ConstructEntityGraph();
            Gia.InProgress = null;
        }

        public virtual void ConstructSystemGraph(FastList<ISystem<GiaScene>> updateSystems, FastList<ISystem<GiaScene>> renderSystems)
        {

        }
        public virtual void ConstructEntityGraph()
        {

        }

        public void Begin()
        {
            Core.Instance.Window.Title = Core.Instance.Title;
            Resume();
        }

        public void End()
        {
            if (DestroyOnExit)
            {
                Dispose();
            }
        }

        public void Render()
        {
            Core.GraphicsDevice.SetRenderTarget(SceneTarget);
            Core.GraphicsDevice.Clear(Gia.Theme.BackgroundColor);

            for (int i = 0; i < RenderSystems.Length; i++)
            {
                RenderSystems[i].Update(this);
            }

            // Then Finalize to screen.
            Core.GraphicsDevice.SetRenderTarget(null);
            Core.GraphicsDevice.Clear(Gia.Theme.BackgroundColor);

            Graphics.Instance.Batcher.Begin(BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            Graphics.Instance.Batcher.Draw(SceneTarget, new Rectangle(0, 0, Screen.Width, Screen.Height), Color.White);

            if (Gia.Debug.Enabled)
                Gia.Debug.Flush(Graphics.Instance.Batcher);
            else
                Gia.Debug.Cancel();

            Graphics.Instance.Batcher.End();

        }

        public void Update()
        {
#if DEBUG
            if (Gia.Debug.ToggleInput.IsPressed)
                Gia.Debug.Enabled = !Gia.Debug.Enabled;
            Analytics();
#endif
            InternalInputUpdate();
            PreUpdate();
            for(int i = 0; i < UpdateSystems.Length; i++)
            {
                UpdateSystems[i].Update(this);
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
            MousePosition = Input.MousePosition;
            MouseDelta = Input.MousePositionDelta.ToVector2() / View.Zoom;

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
                new SpriteRenderSystem(World)
            );
        }
        public ISystem<GiaScene> DefaultUpdateStack()
        {
            return new SequentialSystem<GiaScene>
            (
            );
        }

        /// <summary>
        /// Called before any Update or Render systems are ran.
        /// </summary>
        public virtual void PreUpdate()
        {

        }
        /// <summary>
        /// Called when this scene is set as the current scene. Can happen multiple times.
        /// </summary>
        public virtual void Resume()
        {

        }

        /// <summary>
        /// Called when this scene is unset as the current scene. Can happen multiple times.
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
        }
    }
}
