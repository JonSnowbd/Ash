using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ash.Impl.DefaultEC.Utils.Extensions
{
	public static class CompatExt
	{
		public static bool RayIntersects(this RectangleF rect, ref Ray2D ray, out float distance)
		{
			distance = 0f;
			var maxValue = float.MaxValue;

			if (Math.Abs(ray.Direction.X) < 1E-06f)
			{
				if ((ray.Start.X < rect.X) || (ray.Start.X > rect.X + rect.Width))
					return false;
			}
			else
			{
				var num11 = 1f / ray.Direction.X;
				var num8 = (rect.X - ray.Start.X) * num11;
				var num7 = (rect.X + rect.Width - ray.Start.X) * num11;
				if (num8 > num7)
				{
					var num14 = num8;
					num8 = num7;
					num7 = num14;
				}

				distance = MathHelper.Max(num8, distance);
				maxValue = MathHelper.Min(num7, maxValue);
				if (distance > maxValue)
					return false;
			}

			if (Math.Abs(ray.Direction.Y) < 1E-06f)
			{
				if ((ray.Start.Y < rect.Y) || (ray.Start.Y > rect.Y + rect.Height))
				{
					return false;
				}
			}
			else
			{
				var num10 = 1f / ray.Direction.Y;
				var num6 = (rect.Y - ray.Start.Y) * num10;
				var num5 = (rect.Y + rect.Height - ray.Start.Y) * num10;
				if (num6 > num5)
				{
					var num13 = num6;
					num6 = num5;
					num5 = num13;
				}

				distance = MathHelper.Max(num6, distance);
				maxValue = MathHelper.Min(num5, maxValue);
				if (distance > maxValue)
					return false;
			}

			return true;
		}
		public static bool RayIntersects(ref Rectangle rect, ref Ray2D ray, out float distance)
		{
			distance = 0f;
			var maxValue = float.MaxValue;

			if (Math.Abs(ray.Direction.X) < 1E-06f)
			{
				if ((ray.Start.X < rect.X) || (ray.Start.X > rect.X + rect.Width))
					return false;
			}
			else
			{
				var num11 = 1f / ray.Direction.X;
				var num8 = (rect.X - ray.Start.X) * num11;
				var num7 = (rect.X + rect.Width - ray.Start.X) * num11;
				if (num8 > num7)
				{
					var num14 = num8;
					num8 = num7;
					num7 = num14;
				}

				distance = MathHelper.Max(num8, distance);
				maxValue = MathHelper.Min(num7, maxValue);
				if (distance > maxValue)
					return false;
			}

			if (Math.Abs(ray.Direction.Y) < 1E-06f)
			{
				if ((ray.Start.Y < rect.Y) || (ray.Start.Y > rect.Y + rect.Height))
				{
					return false;
				}
			}
			else
			{
				var num10 = 1f / ray.Direction.Y;
				var num6 = (rect.Y - ray.Start.Y) * num10;
				var num5 = (rect.Y + rect.Height - ray.Start.Y) * num10;
				if (num6 > num5)
				{
					var num13 = num6;
					num6 = num5;
					num5 = num13;
				}

				distance = MathHelper.Max(num6, distance);
				maxValue = MathHelper.Min(num5, maxValue);
				if (distance > maxValue)
					return false;
			}

			return true;
		}
	}
}
