using System;
using Ash.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ash.SimpleScene
{
    /// <summary>
    /// A super simple barebones scene with minimum features.
    /// With this you only get a simple camera, basic mouse position, and
    /// final render target aspect ratio/internal resolution.
    /// </summary>
    public abstract class SimpleScene : IScene
    {
        public enum AspectRatioStyle
        {
            /// <summary>
            /// The final image will always be native window resolution.
            /// </summary>
            Native,
            
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
            MaintainRatioOverfill
        }
        
        public SimpleCamera Camera;
        public Vector2 MousePosition { get; private set; }
        public Vector2 WorldMousePosition { get; private set; }
        
        public Vector2 ScreenScale { get; private set; }

        public AspectRatioStyle Aspect;

        protected RectangleF FinalScreenOutput;

        public RenderTarget2D SceneRenderTarget;

        public Color BackColor;
        public Color ClearColor;

        public SimpleScene()
        {
            Camera = new SimpleCamera();
            MousePosition = Vector2.Zero;
            WorldMousePosition = Vector2.Zero;
            Aspect = AspectRatioStyle.Native;

            ScreenScale = Vector2.One;
            FinalScreenOutput = new RectangleF(0,0,Screen.Width,Screen.Height);
            
            SceneRenderTarget = new RenderTarget2D(Core.GraphicsDevice, Screen.Width, Screen.Height);

            BackColor = Color.Black;
            ClearColor = Color.CornflowerBlue;
            
            
        }

        private bool _initialized = false;
        public void Begin()
        {
            if (!_initialized)
            {
                SceneInit();
                _initialized = true;
            }
        }

        public void End()
        {
        }

        public void Update()
        {
            // This will automatically handle the Screen render target scaling to the window.
            UpdateRenderTargetSize();
            // And then handle the new mouse positions.
            InternalInputUpdate();
            
            // Then your scene updates.
            SceneUpdate();
        }

        public virtual void Render()
        {
            Core.GraphicsDevice.SetRenderTarget(SceneRenderTarget);
            Core.GraphicsDevice.Clear(ClearColor);
            SceneRender(Graphics.Instance.Batcher);
            
            // Then we finalize.
            SceneFinalRender(Graphics.Instance.Batcher, SceneRenderTarget, FinalScreenOutput);
        }

        private static readonly Vector2 HalfVec = new Vector2(0.5f,0.5f);
        void UpdateRenderTargetSize()
        {
            switch (Aspect)
            {
                case AspectRatioStyle.Native:
                {
                    if (Screen.Width != SceneRenderTarget.Width || Screen.Height != SceneRenderTarget.Height)
                    {
                        SetSceneRenderSize(Screen.Width, Screen.Height);
                    }
                    ScreenScale = Vector2.One;
                    FinalScreenOutput = new RectangleF(0, 0, Screen.Width, Screen.Height);
                    break;
                }
                case AspectRatioStyle.Stretch:
                {
                    ScreenScale = new Vector2((float) SceneRenderTarget.Width / (float) Screen.Width,
                        (float) SceneRenderTarget.Height / (float) Screen.Height);
                    
                    FinalScreenOutput = new RectangleF(0, 0, Screen.Width, Screen.Height);
                    
                    break;
                }
                case AspectRatioStyle.MaintainRatio:
                {
                    var scale = Math.Min((float) Screen.Width / (float) SceneRenderTarget.Width,
                        (float) Screen.Height / (float) SceneRenderTarget.Height);
                    ScreenScale = new Vector2(scale);
                    
                    FinalScreenOutput = new RectangleF(Vector2.Zero,
                        new Vector2(SceneRenderTarget.Width, SceneRenderTarget.Height) * ScreenScale);

                    var remainingWidth = Screen.Width - FinalScreenOutput.Width;
                    var remainingHeight = Screen.Height - FinalScreenOutput.Height;

                    var offset = new Vector2(remainingWidth, remainingHeight) * HalfVec;

                    FinalScreenOutput.Location = offset;

                    break;
                }
                case AspectRatioStyle.MaintainRatioOverfill:
                {
                    var scale = Math.Max((float) Screen.Width / (float) SceneRenderTarget.Width,
                        (float) Screen.Height / (float) SceneRenderTarget.Height);
                    ScreenScale = new Vector2(scale);

                    FinalScreenOutput = new RectangleF(Vector2.Zero,
                        new Vector2(SceneRenderTarget.Width, SceneRenderTarget.Height) * ScreenScale);

                    var remainingWidth = Screen.Width - FinalScreenOutput.Width;
                    var remainingHeight = Screen.Height - FinalScreenOutput.Height;

                    var offset = new Vector2(remainingWidth, remainingHeight) * HalfVec;

                    FinalScreenOutput.Location = offset;

                    break;
                }
            }
        }
        void InternalInputUpdate()
        {
            var sceneSize = new Vector2(SceneRenderTarget.Width, SceneRenderTarget.Height);
            var delta = (Input.MousePosition - FinalScreenOutput.Location) / FinalScreenOutput.Size;
            MousePosition = delta * sceneSize;
            WorldMousePosition = Camera.ScreenToWorldPoint(MousePosition);
        }

        private void SetSceneRenderSize(int width, int height)
        {
            SceneRenderTarget?.Dispose();
            SceneRenderTarget = new RenderTarget2D(Core.GraphicsDevice, width, height);
        }

        public void SetSceneRenderSize(AspectRatioStyle style, int width, int height)
        {
            Aspect = style;
            SetSceneRenderSize(width,height);
            Camera.ViewportSize = new Vector2(width, height);
        }

        public void AutoSizeSceneRenderSize()
        {
            Aspect = AspectRatioStyle.Native;
        }

        protected abstract void SceneInit();
        protected abstract void SceneRender(Batcher batch);

        protected virtual void SceneFinalRender(Batcher batch, RenderTarget2D frame, RectangleF finalDestination)
        {            
            Core.GraphicsDevice.SetRenderTarget(null);
            Core.GraphicsDevice.Clear(BackColor);
            batch.Begin(BlendState.Opaque, SamplerState.PointClamp, null, null);
            batch.Draw(frame, finalDestination);
            batch.End();
        }
        protected abstract void SceneUpdate();
    }
}