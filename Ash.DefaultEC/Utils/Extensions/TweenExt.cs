﻿using Ash.Tweens;
using Microsoft.Xna.Framework;


namespace Ash
{
	public static class TweenExt
	{
		#region Transform tweens

		/// <summary>
		/// transform.position tween
		/// </summary>
		/// <returns>The kposition to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector2> TweenPositionTo(this Transform self, Vector2 to, float duration = 0.3f)
		{
			var tween = Pool<TransformVector2Tween>.Obtain();
			tween.SetTargetAndType(self, TransformTargetType.Position);
			tween.Initialize(tween, to, duration);

			return tween;
		}


		/// <summary>
		/// transform.localPosition tween
		/// </summary>
		/// <returns>The klocal position to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector2> TweenLocalPositionTo(this Transform self, Vector2 to, float duration = 0.3f)
		{
			var tween = Pool<TransformVector2Tween>.Obtain();
			tween.SetTargetAndType(self, TransformTargetType.LocalPosition);
			tween.Initialize(tween, to, duration);

			return tween;
		}


		/// <summary>
		/// transform.scale tween
		/// </summary>
		/// <returns>The scale to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector2> TweenScaleTo(this Transform self, float to, float duration = 0.3f)
		{
			return self.TweenScaleTo(new Vector2(to), duration);
		}


		/// <summary>
		/// transform.scale tween
		/// </summary>
		/// <returns>The scale to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector2> TweenScaleTo(this Transform self, Vector2 to, float duration = 0.3f)
		{
			var tween = Pool<TransformVector2Tween>.Obtain();
			tween.SetTargetAndType(self, TransformTargetType.Scale);
			tween.Initialize(tween, to, duration);

			return tween;
		}


		/// <summary>
		/// transform.localScale tween
		/// </summary>
		/// <returns>The klocal scale to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector2> TweenLocalScaleTo(this Transform self, float to, float duration = 0.3f)
		{
			return self.TweenLocalScaleTo(new Vector2(to), duration);
		}


		/// <summary>
		/// transform.localScale tween
		/// </summary>
		/// <returns>The klocal scale to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector2> TweenLocalScaleTo(this Transform self, Vector2 to, float duration = 0.3f)
		{
			var tween = Pool<TransformVector2Tween>.Obtain();
			tween.SetTargetAndType(self, TransformTargetType.LocalScale);
			tween.Initialize(tween, to, duration);

			return tween;
		}


		/// <summary>
		/// transform.rotation tween
		/// </summary>
		/// <returns>The rotation to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector2> TweenRotationDegreesTo(this Transform self, float to, float duration = 0.3f)
		{
			var tween = Pool<TransformVector2Tween>.Obtain();
			tween.SetTargetAndType(self, TransformTargetType.RotationDegrees);
			tween.Initialize(tween, new Vector2(to), duration);

			return tween;
		}


		/// <summary>
		/// transform.localEulers tween
		/// </summary>
		/// <returns>The klocal eulers to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector2> TweenLocalRotationDegreesTo(this Transform self, float to, float duration = 0.3f)
		{
			var tween = Pool<TransformVector2Tween>.Obtain();
			tween.SetTargetAndType(self, TransformTargetType.LocalRotationDegrees);
			tween.Initialize(tween, new Vector2(to), duration);

			return tween;
		}

		#endregion


		#region Entity tweens

		/// <summary>
		/// transform.position tween
		/// </summary>
		/// <returns>The kposition to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector2> TweenPositionTo(this ECEntity self, Vector2 to, float duration = 0.3f)
		{
			var tween = Pool<TransformVector2Tween>.Obtain();
			tween.SetTargetAndType(self.Transform, TransformTargetType.Position);
			tween.Initialize(tween, to, duration);

			return tween;
		}


		/// <summary>
		/// transform.localPosition tween
		/// </summary>
		/// <returns>The klocal position to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector2> TweenLocalPositionTo(this ECEntity self, Vector2 to, float duration = 0.3f)
		{
			var tween = Pool<TransformVector2Tween>.Obtain();
			tween.SetTargetAndType(self.Transform, TransformTargetType.LocalPosition);
			tween.Initialize(tween, to, duration);

			return tween;
		}


		/// <summary>
		/// transform.scale tween
		/// </summary>
		/// <returns>The scale to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector2> TweenScaleTo(this ECEntity self, float to, float duration = 0.3f)
		{
			return self.TweenScaleTo(new Vector2(to), duration);
		}


		/// <summary>
		/// transform.scale tween
		/// </summary>
		/// <returns>The scale to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector2> TweenScaleTo(this ECEntity self, Vector2 to, float duration = 0.3f)
		{
			var tween = Pool<TransformVector2Tween>.Obtain();
			tween.SetTargetAndType(self.Transform, TransformTargetType.Scale);
			tween.Initialize(tween, to, duration);

			return tween;
		}


		/// <summary>
		/// transform.localScale tween
		/// </summary>
		/// <returns>The klocal scale to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector2> TweenLocalScaleTo(this ECEntity self, float to, float duration = 0.3f)
		{
			return self.TweenLocalScaleTo(new Vector2(to), duration);
		}


		/// <summary>
		/// transform.localScale tween
		/// </summary>
		/// <returns>The klocal scale to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector2> TweenLocalScaleTo(this ECEntity self, Vector2 to, float duration = 0.3f)
		{
			var tween = Pool<TransformVector2Tween>.Obtain();
			tween.SetTargetAndType(self.Transform, TransformTargetType.LocalScale);
			tween.Initialize(tween, to, duration);

			return tween;
		}


		/// <summary>
		/// transform.rotation tween
		/// </summary>
		/// <returns>The rotation to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector2> TweenRotationDegreesTo(this ECEntity self, float to, float duration = 0.3f)
		{
			var tween = Pool<TransformVector2Tween>.Obtain();
			tween.SetTargetAndType(self.Transform, TransformTargetType.RotationDegrees);
			tween.Initialize(tween, new Vector2(to), duration);

			return tween;
		}


		/// <summary>
		/// transform.localEulers tween
		/// </summary>
		/// <returns>The klocal eulers to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Vector2> TweenLocalRotationDegreesTo(this ECEntity self, float to, float duration = 0.3f)
		{
			var tween = Pool<TransformVector2Tween>.Obtain();
			tween.SetTargetAndType(self.Transform, TransformTargetType.LocalRotationDegrees);
			tween.Initialize(tween, new Vector2(to), duration);

			return tween;
		}

		#endregion


		#region RenderableComponent tweens

		/// <summary>
		/// RenderableComponent.color tween
		/// </summary>
		/// <returns>The color to.</returns>
		/// <param name="self">Self.</param>
		/// <param name="to">To.</param>
		/// <param name="duration">Duration.</param>
		public static ITween<Color> TweenColorTo(this ECRenderable self, Color to, float duration = 0.3f)
		{
			var tween = Pool<RenderableColorTween>.Obtain();
			tween.SetTarget(self);
			tween.Initialize(tween, to, duration);
			return tween;
		}

		#endregion
	}
}