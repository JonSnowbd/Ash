using Microsoft.Xna.Framework;

namespace Coga
{
	public class ComputedRectangle
	{
		public ComputedVector Position;
		public ComputedVector Size;

		public ComputedRectangle()
		{
			Position = new ComputedVector();
			Size = new ComputedVector();
		}

		public bool IsResolved()
		{
			return Position.IsResolved() && Size.IsResolved();
		}

		public bool WidthResolved { get { return Size.X.Resolved; } }
		public bool HeightResolved { get { return Size.Y.Resolved; } }
		public float Width { get { return Size.X.Value; } }
		public float Height { get { return Size.Y.Value; } }

		public bool XResolved { get { return Position.X.Resolved; } }
		public bool YResolved { get { return Position.Y.Resolved; } }
		public float X { get { return Position.X.Value; } }
		public float Y { get { return Position.Y.Value; } }

		public static implicit operator Rectangle(ComputedRectangle rect)
		{
			return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
		}
	}
}
