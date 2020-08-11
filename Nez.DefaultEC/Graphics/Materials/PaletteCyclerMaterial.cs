using Microsoft.Xna.Framework;

namespace Nez
{
	public class PaletteCyclerMaterial : Material<PaletteCyclerEffect>
	{
		public PaletteCyclerMaterial()
		{
			Effect = new PaletteCyclerEffect();
		}

		public override void OnPreRender(Matrix viewProj)
		{
			Effect.UpdateTime();
		}
	}
}