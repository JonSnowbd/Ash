using Microsoft.Xna.Framework;


namespace Ash.UI
{
	public interface ICullable
	{
		void SetCullingArea(Rectangle cullingArea);
	}
}