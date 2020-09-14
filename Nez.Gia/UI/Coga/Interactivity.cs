using Microsoft.Xna.Framework;
using System;

namespace Coga
{
	public class Interactivity
	{
		public event Action<CogaNode> OnClick;
		public event Action<CogaNode> OnLongClick;
		public event Action<CogaNode> OnDoubleClick;
		public event Action<CogaNode> OnDragStart;
		public event Action<CogaNode, Vector2> OnDrag;
		public event Action<CogaNode> OnDragEnd;
		public event Action<CogaNode> OnHover;
		public event Action<CogaNode> OnUnhover;

		/// <summary>
		/// A unique event used for when it is still deciding on what click is used.
		/// Useful for buttons to have an active trigger that is better used to display
		/// the click state of longclick and doubleclick buttons
		/// </summary>
		public event Action<CogaNode> OnStartClicking;
		public event Action<CogaNode> OnEndClicking;

		bool WasHoveredLastFrame = false;
		bool DoubleClickingEnabled = false;
		bool LongClickingEnabled = false;

		CogaNode Parent;

		public Interactivity(CogaNode parent)
		{
			Parent = parent;
		}

		public void Update(float deltaTime)
		{
			// Residual clicking handling.
			if(isMouseDown && Parent.Manager.IsClickReleased)
			{
				_handle_up(Parent.Manager);
			}
			// Hover management.
			var IsHovered = Parent.Manager.GetHover() == Parent;

			if (IsHovered == false && WasHoveredLastFrame == true)
				OnUnhover?.Invoke(Parent);
			else if (IsHovered == true && WasHoveredLastFrame == false)
				OnHover?.Invoke(Parent);

			WasHoveredLastFrame = IsHovered;

			if (double_click_timer > 0f)
			{
				double_click_timer -= deltaTime;
				if (double_click_timer <= 0f)
				{
					OnClick?.Invoke(Parent);
					OnEndClicking?.Invoke(Parent);
					has_clicked = false;
				}
			}

			// Clicking related input.
			if (Parent.Manager.GetHover() == Parent)
			{
				_update(Parent.Manager, deltaTime);
			}
		}

		#region Input mess
		// Turn back now, you need only know this works...
		const float MouseClickDelay = 0.3f;
		const float MouseDragMinimumDistance = 5f;
		bool isMouseDown = false;
		float mouseDownTime = 0f;
		Vector2 mouseInitial = Vector2.Zero;
		bool isDragging = false;
		bool has_clicked = false;
		float double_click_timer = 0f;
        Vector2 mousepos_lastframe;
		void _update(ICogaManager coga, float deltaTime)
		{
			// Continuation from previous input
			if (isMouseDown)
			{
				if (mouseDownTime == 0f)
				{
					OnStartClicking?.Invoke(Parent);
                    mousepos_lastframe = coga.MousePosition;
				}
				mouseDownTime += deltaTime;
				if (coga.IsClickReleased) // Exit out
				{
					_handle_up(coga);
					return;
				}

				if (isDragging)
				{
					_update_drag(coga);
					return;
				}

                if ((Vector2.Distance(coga.MousePosition, mouseInitial)) >= MouseDragMinimumDistance)
				{
					OnDragStart?.Invoke(Parent);
					Parent.Manager.LockHover();
					OnDrag?.Invoke(Parent, (coga.MousePosition - mouseInitial));
					isDragging = true;
					return;
				}
			}
			else
			{
				if (coga.IsClickPressed)
				{
					isDragging = false;
					isMouseDown = true;
					mouseDownTime = 0f;
					mouseInitial = coga.MousePosition;
				}
			}

            mousepos_lastframe = coga.MousePosition;
		}
		void _handle_up(ICogaManager coga)
		{
			isMouseDown = false;
			if (isDragging)
			{
				OnDragEnd?.Invoke(Parent);
				coga.UnlockHover();
			}
			else
			{
				if (LongClickingEnabled)
				{
					// With long clicking enabled, allow early cancel out and
					// only long click past the const delay.
					if (mouseDownTime < MouseClickDelay)
					{
						_update_click(coga);
					}
					else
					{
						OnLongClick?.Invoke(Parent);
						OnEndClicking?.Invoke(Parent);
					}
				}
				else
				{
					// If we don't need to worry about the long click,
					// just always click.
					_update_click(coga);
				}

			}

		}

		void _update_click(ICogaManager coga)
		{
			if (DoubleClickingEnabled)
			{
				if (has_clicked)
				{
					OnDoubleClick?.Invoke(Parent);
					OnEndClicking?.Invoke(Parent);
					has_clicked = false;
					double_click_timer = -1f;
				}
				else
				{
					has_clicked = true;
					double_click_timer = MouseClickDelay;
				}
			}
			else
			{
				OnClick?.Invoke(Parent);
				OnEndClicking?.Invoke(Parent);
				has_clicked = false;
			}
		}

		void _update_drag(ICogaManager coga)
		{
            var vel = (coga.MousePosition - mousepos_lastframe);
			if (vel != Vector2.Zero)
				OnDrag?.Invoke(Parent, vel);
		}
		#endregion

		public Interactivity SetDoubleClicking(bool double_click = true)
		{
			DoubleClickingEnabled = double_click;
			return this;
		}
		public Interactivity SetLongClicking(bool long_click = true)
		{
			LongClickingEnabled = long_click;
			return this;
		}
	}
}
