using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Nez
{
	public class SepiaEffect : Effect
	{
		public static readonly byte[] EffectBytes = EffectResource.GetFileResourceBytes("Content/nez/effects/Sepia.mgfxo");

		/// <summary>
		/// multiplied by the grayscale value for the final output. Defaults to 1.2f, 1.0f, 0.8f
		/// </summary>
		/// <value>The sepia tone.</value>
		public Vector3 SepiaTone
		{
			get => _sepiaTone;
			set
			{
				_sepiaTone = value;
				_sepiaToneParam.SetValue(_sepiaTone);
			}
		}


		Vector3 _sepiaTone = new Vector3(1.2f, 1.0f, 0.8f);
		EffectParameter _sepiaToneParam;


		public SepiaEffect() : base(Core.GraphicsDevice, EffectBytes)
		{
			_sepiaToneParam = Parameters["_sepiaTone"];
			_sepiaToneParam.SetValue(_sepiaTone);
		}
	}
}