namespace Nez
{
	public class PaletteCyclerMaterial : Material<PaletteCyclerEffect>
	{
		public PaletteCyclerMaterial()
		{
			Effect = new PaletteCyclerEffect();
		}

		public override void OnPreRender(object camera)
		{
			Effect.UpdateTime();
		}
	}
}