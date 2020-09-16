using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace Ash
{
	public static class ComponentExt
	{
		#region Entity Component management

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T AddComponent<T>(this ECComponent self, T component) where T : ECComponent
		{
			return self.Entity.AddComponent(component);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T AddComponent<T>(this ECComponent self) where T : ECComponent, new()
		{
			return self.Entity.AddComponent<T>();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T GetComponent<T>(this ECComponent self) where T : ECComponent
		{
			return self.Entity.GetComponent<T>();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasComponent<T>(this ECComponent self) where T : ECComponent => self.Entity.HasComponent<T>();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GetComponents<T>(this ECComponent self, List<T> componentList) where T : class
		{
			self.Entity.GetComponents<T>(componentList);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static List<T> GetComponents<T>(this ECComponent self) where T : ECComponent
		{
			return self.Entity.GetComponents<T>();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool RemoveComponent<T>(this ECComponent self) where T : ECComponent
		{
			return self.Entity.RemoveComponent<T>();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void RemoveComponent(this ECComponent self, ECComponent component)
		{
			self.Entity.RemoveComponent(component);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void RemoveComponent(this ECComponent self)
		{
			self.Entity.RemoveComponent(self);
		}

		#endregion
	}
}