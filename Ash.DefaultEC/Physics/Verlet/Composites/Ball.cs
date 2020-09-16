using Microsoft.Xna.Framework;


namespace Ash.Verlet
{
	/// <summary>
	/// single Particle composite
	/// </summary>
	public class Ball : Composite
	{
		public Ball(Vector2 position, float radius = 10)
		{
			AddParticle(new Particle(position)).Radius = radius;
		}
	}
}