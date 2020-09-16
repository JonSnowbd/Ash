using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;


namespace Ash
{
	public static class TouchLocationExt
	{
		public static Vector2 ScaledPosition(this TouchLocation touchLocation)
		{
			return Input.ScaledPosition(touchLocation.Position);
		}
	}
}