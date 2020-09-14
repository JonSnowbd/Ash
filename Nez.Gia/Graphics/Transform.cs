using Microsoft.Xna.Framework;

namespace Nez.VisibilitySystem
{
    public struct Transform
    {
        public float Scale;
        public float Rotation;
        public Vector2 Position;
        public Vector2 NormalizedLocalOrigin;
        public Transform(float x, float y)
        {
            Scale = 1f;
            Rotation = 0f;
            Position = new Vector2(x, y);
            NormalizedLocalOrigin = Vector2.Zero;
        }
    }
}
