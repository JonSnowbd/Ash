using Microsoft.Xna.Framework;

namespace Coga
{
	public class ComputedVector
	{
		public ComputedValue X;
		public ComputedValue Y;

		public ComputedVector()
		{
			X = new ComputedValue();
			Y = new ComputedValue();
		}

		public bool IsResolved()
		{
			return X.Resolved && Y.Resolved;
		}

		public static implicit operator Vector2(ComputedVector vec)
		{
			return new Vector2(vec.X.Value, vec.Y.Value);
		}

		public Point ToPoint()
        {
			return new Point((int)X.Value, (int)Y.Value);
        }
	}
}
