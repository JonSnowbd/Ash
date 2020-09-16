using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Ash
{
	public class SpriteEffect : Effect
	{
		public static byte[] EffectBytes => EffectResource.GetMonoGameEmbeddedResourceBytes("Microsoft.Xna.Framework.Graphics.Effect.Resources.SpriteEffect.ogl.mgfxo");
		EffectParameter _matrixTransformParam;


		public SpriteEffect() : base(Core.GraphicsDevice, EffectBytes)
		{
			_matrixTransformParam = Parameters["MatrixTransform"];
		}


		public void SetMatrixTransform(ref Matrix matrixTransform)
		{
			_matrixTransformParam.SetValue(matrixTransform);
		}
	}
}