using System.Collections.Generic;

namespace Coga
{
	internal class InternalRowLayout
	{
		internal class Row
		{
			public List<CogaNode> Children;
			public List<CogaNode> DynamicWidthChildren;
			public List<CogaNode> DynamicHeightChildren;
			public float RemainingWidth;
			public float Height;
			public Row(float remaining_width)
			{
				RemainingWidth = remaining_width;
				Children = new List<CogaNode>();
				DynamicWidthChildren = new List<CogaNode>();
				DynamicHeightChildren = new List<CogaNode>();
				Height = 0f;
			}
			public void Add(CogaNode child, float spacing = 0f)
			{
				if (Children.Count > 0)
					RemainingWidth -= spacing;

				Children.Add(child);
				if (child.Compute.HeightResolved && child.Compute.Height > Height)
				{
					Height = child.Compute.Height;
				}
				if (child.NodeConfiguration.Width.ValueType == CogaValueMode.Fill)
				{
					DynamicWidthChildren.Add(child);
				}
				if (child.NodeConfiguration.Height.ValueType == CogaValueMode.Fill)
				{
					DynamicHeightChildren.Add(child);
				}
				RemainingWidth -= (child.Compute.Width);
			}
			public void Reset()
			{
				Children.Clear();
				DynamicHeightChildren.Clear();
				DynamicWidthChildren.Clear();
				Height = 0f;
				RemainingWidth = 0f;
			}
		}

		public CogaNode Parent;
		public List<Row> Rows;
		public List<float> CumulativeHeights;
		List<int> HeightFillRows;
		public float RemainingHeight;
		int current_row = 0;

		public InternalRowLayout(CogaNode parent)
		{
			// Manage relationships.
			Parent = parent;

			// Create and set the index at 0.
			Rows = new List<Row>();
			Rows.Add(new Row(Parent.Compute.Width - Parent.ComputedPadding[Edge.Horizontal].Value));
			CumulativeHeights = new List<float>();
			CumulativeHeights.Add(0f);
			current_row = 0;
			HeightFillRows = new List<int>();
		}

		// Adds a child to the current row if it can fit, if not start a new one.
		public void AddChild(CogaNode node)
		{
			switch (Parent.NodeConfiguration.Direction)
			{
				case CogaDirection.Horizontal:
					if (node.Compute.Width > Rows[current_row].RemainingWidth)
					{
						current_row++;
						Rows.Add(new Row(Parent.Compute.Width - Parent.ComputedPadding[Edge.Horizontal].Value));
						CumulativeHeights.Add(0f);
					}
					Rows[current_row].Add(node, Parent.NodeConfiguration.Spacing);
					break;
				case CogaDirection.Vertical:
					Rows[current_row].Add(node, Parent.NodeConfiguration.Spacing);
					current_row++;
					Rows.Add(new Row(Parent.Compute.Width - Parent.ComputedPadding[Edge.Horizontal].Value));
					CumulativeHeights.Add(0f);
					break;
			}

		}
		public Row GetCurrentRow()
		{
			return Rows[current_row];
		}

		public void Refresh()
		{
			Reset();
			foreach (var c in Parent.Children)
			{
				if(c.GetIsActive() && c.NodeConfiguration.Layout != CogaLayout.Absolute)
					AddChild(c);
			}
			
			var parentheight = Parent.Compute.Height - Parent.ComputedPadding[Edge.Vertical].Value;
			for(int i = 0; i < Rows.Count; i++)
			{
				var r = Rows[i];
				if (i == Rows.Count - 1)
					parentheight -= r.Height;
				else
					parentheight -= (r.Height + Parent.NodeConfiguration.Spacing);
			}
			RemainingHeight = parentheight;

			HorizontalCalculateFills();
			CalculateCumulatives();
		}

		void Reset()
		{
			CumulativeHeights.Clear();
			Rows.Clear();
			Rows.Add(new Row(Parent.Compute.Width - Parent.ComputedPadding[Edge.Horizontal].Value));
			CumulativeHeights.Add(0f);
			current_row = 0;
			HeightFillRows.Clear();
		}
		void CalculateCumulatives()
		{
			var cumulative = 0f;
			for (int i = 0; i < Rows.Count; i++)
			{
				CumulativeHeights[i] = cumulative;
				var r = Rows[i];
				cumulative += r.Height;
			}
		}
		void HorizontalCalculateFills()
		{
			// Expand the horizontal items and mark the verticals for later.
			for(int y = 0; y < Rows.Count; y++)
			{
				var row = Rows[y];
				if (row.DynamicHeightChildren.Count > 0)
					HeightFillRows.Add(y);
				if (row.RemainingWidth == 0f || row.DynamicWidthChildren.Count == 0)
					continue;

				var divvy = (row.RemainingWidth / row.DynamicWidthChildren.Count);
				
				foreach(var widthchild in row.DynamicWidthChildren)
				{
					var current_width = widthchild.Compute.WidthResolved ? widthchild.Compute.Width : 0f;
					widthchild.Compute.Size.X.Complete(current_width + divvy);
					var index_of_widthchild = row.Children.IndexOf(widthchild);
					for(int x = index_of_widthchild+1; x < row.Children.Count; x++)
					{
						var next = row.Children[x];
						next.Compute.Position.X.Complete(next.Compute.Position.X.Value + divvy);
					}
				}
				row.RemainingWidth = 0f;
			}

			// Expand the vertical items.
			var vertical_divvy = RemainingHeight / HeightFillRows.Count;
			if (vertical_divvy <= 0f || HeightFillRows.Count == 0)
				return;
			
			foreach(var index in HeightFillRows)
			{
				var row = Rows[index];
				row.Height += vertical_divvy;
				foreach(var child in row.DynamicHeightChildren)
				{
					child.Compute.Size.Y.Complete(row.Height);
				}
			}
			RemainingHeight = 0f;
		}
	}
}
