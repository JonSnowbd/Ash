namespace Coga.StandardLayout
{
	internal static class StandardLayoutAlignment
	{
		internal static void AlignChildren(this CogaNode node)
		{
			if (node.Compute.Position.IsResolved() == false)
			{
				return;
			}
			switch (node.NodeConfiguration.Alignment)
			{
				case CogaAlign.Default:
					AlignFlexStart(node);
					break;
				case CogaAlign.FlexStart:
					AlignFlexStart(node);
					break;
				case CogaAlign.FlexEnd:
					AlignFlexEnd(node);
					break;
				case CogaAlign.Center:
					AlignCenter(node);
					break;
				default:
					AlignFlexStart(node);
					break;
			}
		}
		static void AlignFlexStart(CogaNode node)
		{
			for(int y = 0; y < node.RowLayout.Rows.Count; y++)
			{
				var real_index = (node.RowLayout.Rows.Count - 1) - y; // Reverse accessor index.
				var row = node.RowLayout.Rows[y]; 
				var totalChildrenInRow = row.Children.Count;
				var cumulative_height = node.RowLayout.CumulativeHeights[y];
				var spacing = y * node.NodeConfiguration.Spacing;
				for (int x = 0; x < totalChildrenInRow; x++)
				{
					var child = row.Children[x];
					if (child.NodeConfiguration.Layout == CogaLayout.Absolute)
						continue;

					var offset = 0f;
					if (node.ComputedPadding.ContainsKey(Edge.Top))
						offset = node.ComputedPadding[Edge.Top].Value;
					child.Compute.Position.Y.Complete(offset+node.Compute.Y + cumulative_height+spacing);
				}
			}
		}
		static void AlignFlexEnd(CogaNode node)
		{
			AlignFlexStart(node);
			var offset = node.RowLayout.RemainingHeight;
			for (int y = 0; y < node.RowLayout.Rows.Count; y++)
			{
				var row = node.RowLayout.Rows[y];
				for (int x = 0; x < row.Children.Count; x++)
				{
					var child = row.Children[x];
					if (child.NodeConfiguration.Layout == CogaLayout.Absolute)
						continue;
					child.Compute.Position.Y.Complete(child.Compute.Y + offset);
				}
			}
		}
		static void AlignCenter(CogaNode node)
		{
			AlignFlexStart(node);
			var offset = node.RowLayout.RemainingHeight * 0.5f;
			for (int y = 0; y < node.RowLayout.Rows.Count; y++)
			{
				var row = node.RowLayout.Rows[y];
				for (int x = 0; x < row.Children.Count; x++)
				{
					var child = row.Children[x];
					if (child.NodeConfiguration.Layout == CogaLayout.Absolute)
						continue;
					child.Compute.Position.Y.Complete(child.Compute.Y + offset);
				}
			}
		}
	}
}
