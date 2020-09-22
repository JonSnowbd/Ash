using Microsoft.Xna.Framework;

namespace Ash
{
    public struct AABB
    {
        public bool Hidden;
        public RectangleF Bounds;
        /// <summary>
        /// A non-normalized origin. This is in pixels, and is local.
        /// For example if you had an AABB{30,30,50,50} and wanted a centered
        /// origin, you supply Vector2{25,25}
        /// </summary>
        public Vector2 Origin;

        public AABB(float x = 0, float y = 0, float width = 0, float height = 0, float originX = 0, float originY = 0)
        {
            Bounds = new RectangleF(x, y, width, height);
            Hidden = false;
            Origin = new Vector2(originX, originY);
        }
        public AABB(Vector2 pos, Vector2 size, Vector2 origin)
        {
            Bounds = new RectangleF(pos, size);
            Hidden = false;
            Origin = origin;
        }
        public AABB(Vector2 pos, Vector2 size) : this(pos, size, Vector2.Zero)
        { }

        public override bool Equals(object obj)
        {
            return obj is AABB aa && aa.Bounds == Bounds && aa.Hidden == Hidden;
        }

        public override int GetHashCode()
        {
            return Bounds.GetHashCode();
        }

        public override string ToString()
        {
            return $"<ABBC [{Bounds.X}, {Bounds.Y}, {Bounds.Width}, {Bounds.Height} Hidden={Hidden}]>";
        }

        public static bool operator ==(AABB lhs, AABB rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(AABB lhs, AABB rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Returns a new AABB that perfectly contains both first and second
        /// </summary>
        public static AABB Encompass(AABB first, AABB second)
        {
            var lower = Vector2.Min(first.Bounds.Location, second.Bounds.Location);
            var upper = Vector2.Max(first.Bounds.Location + first.Bounds.Size, second.Bounds.Location + second.Bounds.Size);

            var aabb = new AABB();
            aabb.Bounds = new RectangleF(lower, upper - lower);
            return aabb;
        }
        /// <summary>
        /// Mutates the inner bounds to perfectly contain the first and second bounds.
        /// </summary>
        public void EncompassMutate(AABB first, AABB second)
        {
            var bounds = Encompass(first, second).Bounds;
            Bounds = bounds;
        }
    }
}
