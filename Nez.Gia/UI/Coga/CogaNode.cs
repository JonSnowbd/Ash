using Coga.StandardLayout;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Coga
{
    public abstract class CogaNode
    {
        public class Config
        {
			public CogaLayout Layout;
			public CogaJustify Justification;
			public CogaAlign Alignment;
			public CogaDirection Direction;
			public CogaWrap Wrap;

			public CogaValue X;
			public CogaValue Y;
            public CogaValue Width;
            public CogaValue Height;
			public float Spacing;

            public Dictionary<Edge, CogaValue> Padding;

			public CogaValue Left;
			public CogaValue Top;
			public CogaValue Right;
			public CogaValue Bottom;

			public Config()
            {
                Padding = new Dictionary<Edge, CogaValue>();
				Padding[Edge.Left] = new CogaValue(0);
				Padding[Edge.Top] = new CogaValue(0);
				Padding[Edge.Right] = new CogaValue(0);
				Padding[Edge.Bottom] = new CogaValue(0);
			}

            public static Config Default()
            {
				return new Config()
				{
					Layout = CogaLayout.Default,
					Justification = CogaJustify.Default,
					Alignment = CogaAlign.Default,
					Direction = CogaDirection.Horizontal,
					Wrap = CogaWrap.Wrap,

					X = new CogaValue(0),
					Y = new CogaValue(0),
					Width = new CogaValue(0f, CogaValueMode.Default),
					Height = new CogaValue(0f, CogaValueMode.Default),
					Spacing = 0f,

					Left = new CogaValue(0f, CogaValueMode.Undefined),
					Top = new CogaValue(0f, CogaValueMode.Undefined),
					Right = new CogaValue(0f, CogaValueMode.Undefined),
					Bottom = new CogaValue(0f, CogaValueMode.Undefined)
                };
            }
        }

		internal Config NodeConfiguration;
		

		// State
		public int Precedence = 10;
		public bool IgnoresInput;
        protected bool IsDirty;
        protected bool IsRoot;
		protected bool IsActive;
		public bool NeedsLayout = true;

		// Relationships
		public ICogaManager Manager;
		public CogaNode Parent { get; protected set; }
		public HashSet<CogaNode> Children;
		public Interactivity Interactivity { get; protected set; }


		#region Events
		public delegate void OnChildChanged(CogaNode parent, CogaNode child);
		public delegate void OnSelfChanged(CogaNode changed_node);
		public event OnChildChanged OnChildAdded;
		public event OnChildChanged OnChildRemoved;
		public event OnSelfChanged OnLayoutRecalculated;
		public event OnSelfChanged OnNodeConfigurationChanged;
		public event OnSelfChanged OnActiveChange;
		#endregion

		/// <summary>
		/// You shouldn't modify this yourself unless you REALLY know why youre doing it.
		/// This variable gets re-written each layout.
		/// </summary>
		public ComputedRectangle Compute;
		/// <summary>
		/// There should never be a situation in which to edit this, any change will be
		/// overwritten next relayout. Instead use `.SetPadding`
		/// </summary>
		public Dictionary<Edge, ComputedValue> ComputedPadding;

		internal InternalRowLayout RowLayout;

        public CogaNode()
        {
			ComputedPadding = new Dictionary<Edge, ComputedValue>();
			Children = new HashSet<CogaNode>();
            NodeConfiguration = Config.Default();
            IsDirty = true;
            IsRoot = false;
			IsActive = true;

			Compute = new ComputedRectangle();

			ComputedPadding[Edge.Left] = new ComputedValue();
			ComputedPadding[Edge.Top] = new ComputedValue();
			ComputedPadding[Edge.Right] = new ComputedValue();
			ComputedPadding[Edge.Bottom] = new ComputedValue();
			ComputedPadding[Edge.Horizontal] = new ComputedValue();
			ComputedPadding[Edge.Vertical] = new ComputedValue();

			RowLayout = new InternalRowLayout(this);
		}

		public abstract Vector2 MinimumNodeSize();
		public abstract Vector2? PreferredNodeSize();

		#region Configuration Setters
		/// <summary>
		/// Gives this node a fixed size in Width. Note: Some widgets handle this behaviour for you!
		/// If they do, then you will know through the C# Warnings you will get from calling this on them.
		/// </summary>
		public CogaNode SetWidthInPixels(int pixels)
		{
			NodeConfiguration.Width.ValueType = CogaValueMode.Pixel;
			NodeConfiguration.Width.Value = pixels;

			SetChildrenDirty();
			SetDirty();

			TriggerNodeConfig();
			return this;
		}
		/// <summary>
		/// This sets this node to take up as much space as possible horizontally.
		/// If there is multiple fill-width nodes in the row then the remaining
		/// space after all fixed-width nodes is divvied up to every fill-width node.
		/// </summary>
		public CogaNode SetWidthToGrow()
		{
			NodeConfiguration.Width.ValueType = CogaValueMode.Fill;

			SetChildrenDirty();
			SetDirty();

			TriggerNodeConfig();
			return this;
		}
		/// <summary>
		/// Gives this node a fixed size in Height. Note: Some widgets handle this behaviour for you!
		/// If they do, then you will know through the C# Warnings you will get from calling this on them.
		/// </summary>
		public CogaNode SetHeightInPixels(int pixels)
		{
			NodeConfiguration.Height.ValueType = CogaValueMode.Pixel;
			NodeConfiguration.Height.Value = pixels;

			SetChildrenDirty();
			SetDirty();

			TriggerNodeConfig();
			return this;
		}
		/// <summary>
		/// This sets this node to take up as much space as possible vertically.
		/// If there are multiple fill-height rows in the layout then the remaining
		/// space after all fixed-height rows is divvied up to every fill-height row.
		/// </summary>
		public CogaNode SetHeightToGrow()
		{
			NodeConfiguration.Height.ValueType = CogaValueMode.Fill;

			SetChildrenDirty();
			SetDirty();

			TriggerNodeConfig();
			return this;
		}
		/// <summary>
		/// Padding is the internal space from each edge that will never have a node overlap
		/// in.
		/// This space is ignored by Absolute layout nodes.
		/// </summary>
		public CogaNode SetPadding(float? left, float? top, float? right, float? bottom)
		{
			var lpad = left ?? 0f;
			var tpad = top ?? 0f;
			var rpad = right ?? 0f;
			var bpad = bottom ?? 0f;

			NodeConfiguration.Padding[Edge.Left] = new CogaValue(lpad, CogaValueMode.Pixel);
			NodeConfiguration.Padding[Edge.Top] = new CogaValue(tpad, CogaValueMode.Pixel);
			NodeConfiguration.Padding[Edge.Right] = new CogaValue(rpad, CogaValueMode.Pixel);
			NodeConfiguration.Padding[Edge.Bottom] = new CogaValue(bpad, CogaValueMode.Pixel);

			SetDirty();
			SetChildrenDirty();

			TriggerNodeConfig();
			return this;
		}
		/// <summary>
		/// Padding is the internal space from each edge that will never have a node overlap
		/// in.
		/// This is ignored by Absolute layout nodes.
		/// </summary>
		public CogaNode SetPadding(float padding)
		{
			return SetPadding(padding, padding, padding, padding);
		}
		/// <summary>
		/// Padding is the internal space from each edge that will never have a node overlap
		/// in.
		/// This is ignored by Absolute layout nodes.
		/// </summary>
		public CogaNode SetPadding(float horizontal, float vertical)
		{
			return SetPadding(horizontal, vertical, horizontal, vertical);
		}
		/// <summary>
		/// In most Justifications and Alignments this is used to make sure that
		/// each node is spaced apart by atleast this much.
		/// </summary>
		public CogaNode SetSpacing(float s)
		{
			NodeConfiguration.Spacing = s;

			SetDirty();
			SetChildrenDirty();

			TriggerNodeConfig();
			return this;
		}
		/// <summary>
		/// You shouldn't call this unless you know you need to! 
		/// This is a method that sets this root as the defacto root of all
		/// nodes in the hierarchy. There should only ever be one of these in a manager.
		/// </summary>
		public CogaNode SetRoot(int width, int height, ICogaManager manager)
		{
			IsRoot = true;
			NodeConfiguration.Width = new CogaValue(width);
			NodeConfiguration.Height = new CogaValue(height);

			Manager = manager;

			Compute.Size.X.Complete(width);
			Compute.Size.Y.Complete(height);
			Compute.Position.X.Complete(0f);
			Compute.Position.Y.Complete(0f);

			SetChildrenDirty();
			SetDirty();

			TriggerNodeConfig();
			return this;
		}
		public CogaNode SetActive(bool active = true)
		{
			IsActive = active;

			SetDirty();
			SetChildrenDirty();

			// Turning inactive will stop a layout from being triggered,
			// so manually trigger it now for listeners that need to know its disabled.
			TriggerLayout();
			OnActiveChange?.Invoke(this);
			return this;
		}
		public CogaNode ToggleActive()
		{
			SetActive(!IsActive);
			return this;
		}
		/// <summary>
		/// Justification controls the left to right position of all nodes inside of only this node!
		/// </summary>
		public CogaNode SetContentJustification(CogaJustify just)
		{
			NodeConfiguration.Justification = just;

			SetChildrenDirty();
			SetDirty();

			TriggerNodeConfig();
			return this;
		}
		/// <summary>
		/// Alignment controls the top to bottom position of all nodes inside of only this node!
		/// </summary>
		public CogaNode SetContentAlignment(CogaAlign align)
		{
			NodeConfiguration.Alignment = align;

			SetChildrenDirty();
			SetDirty();

			TriggerNodeConfig();
			return this;
		}
		/// <summary>
		/// Sets the direction that children are laid out in. Note, only Horizontal direction
		/// supports Wrapping.
		/// </summary>
		public CogaNode SetDirection(CogaDirection direction)
		{
			NodeConfiguration.Direction = direction;

			SetDirty();
			SetChildrenDirty();

			TriggerNodeConfig();
			return this;
		}
		public CogaNode SetWrap(CogaWrap wrap)
		{
			NodeConfiguration.Wrap = wrap;

			SetDirty();
			SetChildrenDirty();

			TriggerNodeConfig();
			return this;
		}
		/// <summary>
		/// Changes the node's Parent. This will automatically handle moving the widget
		/// into the new space and handling references such as `.Parent` and children.
		/// </summary>
		public virtual CogaNode SetParent(CogaNode parent)
		{
			if (Parent != null)
			{
				Parent.Children.Remove(this);
				Parent.OnChildRemoved?.Invoke(Parent, this);

				Parent.SetChildrenDirty();
				Parent.SetDirty();

				OnActiveChange = null;
			}
			Parent = parent;
			Parent.OnActiveChange += (node) => { OnActiveChange?.Invoke(this); };
			Parent.SetChildrenDirty();
			if(!Parent.Children.Contains(this))
			{
				Parent.Children.Add(this);
			}

			if(Parent.Manager != null)
			{
				PropagateManager(this, Parent.Manager);
			}
			return this;
		}

		// Local function to recurse the new manager throughout the tree.
		// This is necessary for nodes that are put together before being
		// added to the root.
		void PropagateManager(CogaNode node, ICogaManager manager)
		{
			if (node.Manager == null)
			{
				node.Manager = manager;
				node.OnReceiveManager();
			}

			foreach (var c in node.Children)
			{
				PropagateManager(c, manager);
			}
		}
		/// <summary>
		/// Gives this node interactivity, allowing it to react to clicks and hovers and drags.
		/// </summary>
		/// <returns></returns>
		public virtual CogaNode SetInteractive()
		{
            if(Interactivity == null)
			    Interactivity = new Interactivity(this);
			return this;
		}
		/// <summary>
		/// Adds a child to this node's hierarchy. Handles everything including removing
		/// the child reference from the previous parent if one existed.
		/// </summary>
		public virtual CogaNode AddChild(CogaNode child)
		{
			child.SetParent(this);
			Children.Add(child);
			OnChildAdded?.Invoke(this, child);

			SetDirty();
			SetChildrenDirty();

			if (IsRoot)
			{
				PropagateManager(this, Manager);
			}else if(Manager != null)
			{
				PropagateManager(this, Manager);
			}
			
			return this;
		}
		/// <summary>
		/// Makes this node ignore its parent's layout rules and instead bind itself to
		/// a set distance from each edge. Opposites are not exclusive, setting a left and
		/// right edge distance will override width etc.
		/// </summary>
		public CogaNode SetAbsolute(float? left, float? top, float? right, float? bottom)
		{
			NodeConfiguration.Layout = CogaLayout.Absolute;

			if (left.HasValue)
				NodeConfiguration.Left.Set(left.Value, CogaValueMode.Pixel);
			else
				NodeConfiguration.Left.Clear();
			if (top.HasValue)
				NodeConfiguration.Top.Set(top.Value, CogaValueMode.Pixel);
			else
				NodeConfiguration.Top.Clear();
			if (right.HasValue)
				NodeConfiguration.Right.Set(right.Value, CogaValueMode.Pixel);
			else
				NodeConfiguration.Right.Clear();
			if (bottom.HasValue)
				NodeConfiguration.Bottom.Set(bottom.Value, CogaValueMode.Pixel);
			else
				NodeConfiguration.Bottom.Clear();

			TriggerNodeConfig();
			return this;
		}
		/// <summary>
		/// Same as SetAbsolute but avoids dirtying the parent. Use this for lightning fast absolute
		/// repositioning.
		/// </summary>
		public CogaNode SetAbsolutePosition(float? left, float? top, float? right, float? bottom)
		{
			if (left.HasValue)
				NodeConfiguration.Left.Set(left.Value, CogaValueMode.Pixel);
			else
				NodeConfiguration.Left.Clear();
			if (top.HasValue)
				NodeConfiguration.Top.Set(top.Value, CogaValueMode.Pixel);
			else
				NodeConfiguration.Top.Clear();
			if (right.HasValue)
				NodeConfiguration.Right.Set(right.Value, CogaValueMode.Pixel);
			else
				NodeConfiguration.Right.Clear();
			if (bottom.HasValue)
				NodeConfiguration.Bottom.Set(bottom.Value, CogaValueMode.Pixel);
			else
				NodeConfiguration.Bottom.Clear();

			TriggerNodeConfig();
			return this;
		}
		public void RemoveChild(CogaNode child)
		{
			Children.Remove(child);
			OnChildRemoved?.Invoke(this, child);
		}
        /// <summary>
        /// Removes each child in this hierarchy.
        /// </summary>
		public void Clear()
		{
			foreach(var child in Children)
			{
				child.Destroy();
			}
			Children.Clear();
		}

		public void SetDirty()
        {
			IsDirty = true;
        }
		public void SetChildrenDirty()
        {
			foreach(var child in Children)
            {
				child.SetDirty();
            }
        }
		#endregion

		#region Getters
		/// <summary>
		/// If the position has been calculated, this will return its computed on-screen coordinate.
		/// If this was not calculated it returns null.
		/// </summary>
		public Vector2? GetPosition()
		{
			if (Compute.Position.IsResolved())
				return Compute.Position;
			return null;
		}
		/// <summary>
		/// If the size has been calculated, this will return its computed on-screen size.
		/// If this was not calculated it returns null.
		/// </summary>
		public Vector2? GetSize()
		{
			if(Compute.Size.IsResolved())
				return Compute.Size;
			return null;
		}
		/// <summary>
		/// If the entire node has been calculated, this will return its computed on-screen rectangle.
		/// If this was not calculated it returns null.
		/// </summary>
		public Rectangle? GetRectangle()
		{
			if (Compute.IsResolved())
				return Compute;
			return null;
		}
		/// <summary>
		/// If the entire node has been calculated, this will return its computed on-screen center point.
		/// If this was not calculated it returns null.
		/// </summary>
		public Vector2? GetCenter()
		{
			if (Compute.IsResolved())
			{
				return new Vector2(Compute.X + Compute.Width * 0.5f, Compute.Y + Compute.Height);
			}
			return null;
		}

		public bool GetIsRoot()
		{
			return IsRoot;
		}

		public bool GetIsActive()
		{
			if (Parent != null)
				return Parent.GetIsActive() && IsActive;
			else
				return IsActive;
		}

		public bool GetIsDirty()
		{
			return IsDirty;
		}
		#endregion

		/// <summary>
		/// You shouldn't need to call this. This is used by a manager to query the structure
		/// for any re-layouts it needs to do.
		/// CogaManager does this for you.
		/// </summary>
		public void PerformLayout()
		{
			if (!IsActive)
				return;

			foreach (var c in Children)
			{
				c.PerformLayout(); // Depth first! Recursion is awesome.
			}

			if (IsDirty)
			{
				if (!IsRoot) // The root widget is manually completed, so leave that to the user.
					this.CalculateSize();

				this.CalculatePadding();

				IsDirty = false;

				NeedsLayout = true;
			}

			if (IsRoot)
				this.LayoutChildren();
		}
		internal void TriggerLayout()
		{
			OnLayoutRecalculated?.Invoke(this);
		}
		internal void TriggerNodeConfig()
		{
			OnNodeConfigurationChanged?.Invoke(this);
		}

		private List<CogaNode> _HoverPile = new List<CogaNode>();
		public void PropagateMouseHover(Vector2 mouse)
		{
			_HoverPile.Clear();
			foreach(var c in Children)
			{
				if(!c.GetIsActive() || c.IgnoresInput)
					continue;

				Rectangle box = c.Compute;
				if (box.Contains(mouse))
				{
					if(c.NodeConfiguration.Layout == CogaLayout.Absolute)
					{
						// Absolute layouts get precedence due to their floaty nature.
						c.PropagateMouseHover(mouse);
						return;
					}
					_HoverPile.Add(c);
				}
			}

			if(_HoverPile.Count > 0)
			{
				_HoverPile.Sort((x,y) => { return x.Precedence.CompareTo(y.Precedence); });
				_HoverPile[0].PropagateMouseHover(mouse);
				return;
			}

			// End of the line
			if (!IsRoot && !IgnoresInput)
			{
				Manager.SetFocus(this);
				return;
			}
		}
		public virtual void OnReceiveManager() { }
		public abstract void Destroy();
	}
}
