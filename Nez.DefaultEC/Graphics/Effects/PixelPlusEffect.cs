using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nez
{
    public class PixelPlusEffect : Effect
    {
        public static readonly byte[] EffectBytes = EffectResource.GetFileResourceBytes("Content/nez/effects/PixelPlus.mgfxo");
        Vector2 _renderTargetSize;
        EffectParameter _renderTargetSizeParameter;
        public Vector2 RenderTargetSize
        {
            get
            {
                return _renderTargetSize;
            }
            set
            {
                _renderTargetSize = value;
                _renderTargetSizeParameter?.SetValue(value);
            }
        }

        float kValue;
        EffectParameter _kValueParameter;
        public float KValue
        {
            get
            {
                return kValue;
            }
            set
            {
                kValue = value;
                _kValueParameter?.SetValue(value);
            }
        }
        public PixelPlusEffect() : base(Core.GraphicsDevice, EffectBytes)
        {
            _kValueParameter = Parameters["kValue"];
            _renderTargetSizeParameter = Parameters["renderTargetSize"];
            RenderTargetSize = new Vector2(1280, 720); // Default
            KValue = 0.13f; // I find this works nice at .13f
        }

    }
}
