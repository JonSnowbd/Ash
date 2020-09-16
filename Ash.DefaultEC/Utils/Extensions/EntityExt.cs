using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;


namespace Ash
{
	public static class EntityExt
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ECEntity SetParent(this ECEntity self, Transform parent)
		{
			self.Transform.SetParent(parent);
			return self;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ECEntity SetParent(this ECEntity self, ECEntity entity)
		{
			self.Transform.SetParent(entity.Transform);
			return self;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ECEntity SetPosition(this ECEntity self, Vector2 position)
		{
			self.Transform.SetPosition(position);
			return self;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ECEntity SetPosition(this ECEntity self, float x, float y)
		{
			self.Transform.SetPosition(x, y);
			return self;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ECEntity SetLocalPosition(this ECEntity self, Vector2 localPosition)
		{
			self.Transform.SetLocalPosition(localPosition);
			return self;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ECEntity SetRotation(this ECEntity self, float radians)
		{
			self.Transform.SetRotation(radians);
			return self;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ECEntity SetRotationDegrees(this ECEntity self, float degrees)
		{
			self.Transform.SetRotationDegrees(degrees);
			return self;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ECEntity SetLocalRotation(this ECEntity self, float radians)
		{
			self.Transform.SetLocalRotation(radians);
			return self;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ECEntity SetLocalRotationDegrees(this ECEntity self, float degrees)
		{
			self.Transform.SetLocalRotationDegrees(degrees);
			return self;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ECEntity SetScale(this ECEntity self, Vector2 scale)
		{
			self.Transform.SetScale(scale);
			return self;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ECEntity SetScale(this ECEntity self, float scale)
		{
			self.Transform.SetScale(scale);
			return self;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ECEntity SetLocalScale(this ECEntity self, Vector2 scale)
		{
			self.Transform.SetLocalScale(scale);
			return self;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ECEntity SetLocalScale(this ECEntity self, float scale)
		{
			self.Transform.SetLocalScale(scale);
			return self;
		}
	}
}