using Microsoft.Xna.Framework.Graphics;


namespace Ash
{
	public class NoiseEffect : Effect
	{
		public static readonly byte[] EffectBytes = EffectResource.GetFileResourceBytes("Content/nez/effects/Noise.mgfxo");
		/// <summary>
		/// Intensity of the noise. Defaults to 1.
		/// </summary>
		[Range(0, 10)]
		public float Noise
		{
			get => _noise;
			set
			{
				if (_noise != value)
				{
					_noise = value;
					_noiseParam.SetValue(_noise);
				}
			}
		}

		float _noise = 1f;
		EffectParameter _noiseParam;


		public NoiseEffect() : base(Core.GraphicsDevice, EffectBytes)
		{
			_noiseParam = Parameters["noise"];
			_noiseParam.SetValue(_noise);
		}
	}
}