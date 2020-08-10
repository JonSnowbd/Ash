using System;
using System.IO;
using Microsoft.Xna.Framework;


namespace Nez
{
	public static class EffectResource
	{
		// sprite effects
		public static byte[] SpriteBlinkEffectBytes => GetFileResourceBytes("Content/nez/effects/SpriteBlinkEffect.mgfxo");

		public static byte[] SpriteLinesEffectBytes => GetFileResourceBytes("Content/nez/effects/SpriteLines.mgfxo");

		public static byte[] SpriteAlphaTestBytes => GetFileResourceBytes("Content/nez/effects/SpriteAlphaTest.mgfxo");

		public static byte[] CrosshatchBytes => GetFileResourceBytes("Content/nez/effects/Crosshatch.mgfxo");

		public static byte[] NoiseBytes => GetFileResourceBytes("Content/nez/effects/Noise.mgfxo");

		public static byte[] TwistBytes => GetFileResourceBytes("Content/nez/effects/Twist.mgfxo");

		public static byte[] DotsBytes => GetFileResourceBytes("Content/nez/effects/Dots.mgfxo");

		public static byte[] DissolveBytes => GetFileResourceBytes("Content/nez/effects/Dissolve.mgfxo");

		// post processor effects
		public static byte[] BloomCombineBytes => GetFileResourceBytes("Content/nez/effects/BloomCombine.mgfxo");

		public static byte[] BloomExtractBytes => GetFileResourceBytes("Content/nez/effects/BloomExtract.mgfxo");

		public static byte[] GaussianBlurBytes => GetFileResourceBytes("Content/nez/effects/GaussianBlur.mgfxo");

		public static byte[] VignetteBytes => GetFileResourceBytes("Content/nez/effects/Vignette.mgfxo");

		public static byte[] LetterboxBytes => GetFileResourceBytes("Content/nez/effects/Letterbox.mgfxo");

		public static byte[] HeatDistortionBytes => GetFileResourceBytes("Content/nez/effects/HeatDistortion.mgfxo");

		public static byte[] SpriteLightMultiplyBytes => GetFileResourceBytes("Content/nez/effects/SpriteLightMultiply.mgfxo");

		public static byte[] PixelGlitchBytes => GetFileResourceBytes("Content/nez/effects/PixelGlitch.mgfxo");

		public static byte[] StencilLightBytes => GetFileResourceBytes("Content/nez/effects/StencilLight.mgfxo");

		// deferred lighting
		public static byte[] DeferredSpriteBytes => GetFileResourceBytes("Content/nez/effects/DeferredSprite.mgfxo");

		public static byte[] DeferredLightBytes => GetFileResourceBytes("Content/nez/effects/DeferredLighting.mgfxo");

		// forward lighting
		public static byte[] ForwardLightingBytes => GetFileResourceBytes("Content/nez/effects/ForwardLighting.mgfxo");

		public static byte[] PolygonLightBytes => GetFileResourceBytes("Content/nez/effects/PolygonLight.mgfxo");

		// scene transitions
		public static byte[] SquaresTransitionBytes => GetFileResourceBytes("Content/nez/effects/transitions/Squares.mgfxo");

		// sprite or post processor effects
		public static byte[] SpriteEffectBytes => GetMonoGameEmbeddedResourceBytes("Microsoft.Xna.Framework.Graphics.Effect.Resources.SpriteEffect.ogl.mgfxo");

		public static byte[] MultiTextureOverlayBytes => GetFileResourceBytes("Content/nez/effects/MultiTextureOverlay.mgfxo");

		public static byte[] ScanlinesBytes => GetFileResourceBytes("Content/nez/effects/Scanlines.mgfxo");

		public static byte[] ReflectionBytes => GetFileResourceBytes("Content/nez/effects/Reflection.mgfxo");

		public static byte[] GrayscaleBytes => GetFileResourceBytes("Content/nez/effects/Grayscale.mgfxo");

		public static byte[] SepiaBytes => GetFileResourceBytes("Content/nez/effects/Sepia.mgfxo");

		public static byte[] PaletteCyclerBytes => GetFileResourceBytes("Content/nez/effects/PaletteCycler.mgfxo");


		/// <summary>
		/// gets the raw byte[] from an EmbeddedResource
		/// </summary>
		/// <returns>The embedded resource bytes.</returns>
		/// <param name="name">Name.</param>
		static byte[] GetEmbeddedResourceBytes(string name)
		{
			var assembly = typeof(EffectResource).Assembly;
			using (var stream = assembly.GetManifestResourceStream(name))
			{
				using (var ms = new MemoryStream())
				{
					stream.CopyTo(ms);
					return ms.ToArray();
				}
			}
		}


		internal static byte[] GetMonoGameEmbeddedResourceBytes(string name)
		{
			var assembly = typeof(MathHelper).Assembly;
#if FNA
			name = name.Replace( ".ogl.mgfxo", ".fxb" );
#else
			// MG 3.8 decided to change the location of Effecs...sigh.
			if (!assembly.GetManifestResourceNames().Contains(name))
				name = name.Replace(".Framework", ".Framework.Platform");
#endif

			using (var stream = assembly.GetManifestResourceStream(name))
			{
				using (var ms = new MemoryStream())
				{
					stream.CopyTo(ms);
					return ms.ToArray();
				}
			}
		}


		/// <summary>
		/// fetches the raw byte data of a file from the Content folder. Used to keep the Effect subclass code simple and clean due to the Effect
		/// constructor requiring the byte[].
		/// </summary>
		/// <returns>The file resource bytes.</returns>
		/// <param name="path">Path.</param>
		public static byte[] GetFileResourceBytes(string path)
		{
#if FNA
			path = path.Replace( ".mgfxo", ".fxb" );
#endif

			byte[] bytes;
			try
			{
				using (var stream = TitleContainer.OpenStream(path))
				{
					if (stream.CanSeek)
					{
						bytes = new byte[stream.Length];
						stream.Read(bytes, 0, bytes.Length);
					}
					else
					{
						using (var ms = new MemoryStream())
						{
							stream.CopyTo(ms);
							bytes = ms.ToArray();
						}
					}
				}
			}
			catch (Exception e)
			{
				var txt = string.Format(
					"OpenStream failed to find file at path: {0}. Did you add it to the Content folder and set its properties to copy to output directory?",
					path);
				throw new Exception(txt, e);
			}

			return bytes;
		}
	}
}