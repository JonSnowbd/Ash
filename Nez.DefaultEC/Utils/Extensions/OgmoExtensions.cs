using Microsoft.Xna.Framework;

namespace Nez.Ogmo
{
	public static class OgmoExtensions
    {
		public static Point WorldToTile(this OgmoTileLayer layer, Vector2 world)
		{
			var x = Mathf.Clamp(Mathf.Floor((float)world.X / (float)layer.CellSize.X), 0f, layer.CellCount.X);
			var y = Mathf.Clamp(Mathf.Floor((float)world.Y / (float)layer.CellSize.Y), 0f, layer.CellCount.Y);
			var point = new Point((int)x, (int)y);
			return point;
		}
	}
}
