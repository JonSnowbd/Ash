using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nez
{
    public class PixelPlusFinalDelegate : IFinalRenderDelegate
    {
        bool initializedSize = false;
        public PixelPlusEffect InternalEffect;
        public void HandleFinalRender(RenderTarget2D finalRenderTarget, Color letterboxColor, RenderTarget2D source, Rectangle finalRenderDestinationRect, SamplerState samplerState)
        {
            if (!initializedSize)
            {
                InternalEffect.RenderTargetSize = new Vector2(source.Width, source.Height);
                initializedSize = true;
            }
            
            Core.GraphicsDevice.SetRenderTarget(finalRenderTarget);
            Core.GraphicsDevice.Clear(letterboxColor);

            Graphics.Instance.Batcher.Begin(BlendState.Opaque, SamplerState.LinearClamp, null, null, InternalEffect);
            Graphics.Instance.Batcher.Draw(source, finalRenderDestinationRect, Color.White);
            Graphics.Instance.Batcher.End();
        }

        public void OnAddedToScene(ECScene scene)
        {
            InternalEffect = new PixelPlusEffect();
        }

        public void OnSceneBackBufferSizeChanged(int newWidth, int newHeight)
        {
            InternalEffect.RenderTargetSize = new Vector2(newWidth, newHeight);
        }

        public void Unload()
        {
            InternalEffect.Dispose();
        }
    }
}
