namespace Coga.StandardLayout
{

	internal static class StandardLayoutJustification
	{

		internal static void JustifyChildren(CogaNode node)
		{
			switch (node.NodeConfiguration.Justification)
			{
				case CogaJustify.Default:
					JustifyFlexStart(node);
					break;
				case CogaJustify.FlexStart:
					JustifyFlexStart(node);
					break;
				case CogaJustify.FlexEnd:
					JustifyFlexEnd(node);
					break;
				case CogaJustify.Center:
					JustifyCenter(node);
					break;
			}
		}

		static void JustifyFlexStart(CogaNode node)
		{
			for(int y = 0; y < node.RowLayout.Rows.Count; y++)
			{
				var sum = 0f;
				var row = node.RowLayout.Rows[y];
				for(int x = 0; x < row.Children.Count; x++)
				{
					var child = row.Children[x];
					if (child.NodeConfiguration.Layout == CogaLayout.Absolute)
						continue;
					child.Compute.Position.X.Complete(node.Compute.X + node.ComputedPadding[Edge.Left].Value + sum);
					sum += child.Compute.Width + node.NodeConfiguration.Spacing;
				}
			}
		}
		static void JustifyFlexEnd(CogaNode node)
		{
			JustifyFlexStart(node);
			foreach(var row in node.RowLayout.Rows)
			{
				if(row.RemainingWidth > 0f)
				{
					foreach(var child in row.Children)
					{
						child.Compute.Position.X.Complete(child.Compute.X + row.RemainingWidth);
					}
				}
			}
		}

		static void JustifyCenter(CogaNode node)
		{
			for(int y = 0; y < node.RowLayout.Rows.Count; y++)
			{
				var row = node.RowLayout.Rows[y];
				var sum = row.RemainingWidth * 0.5f;
				for(int x = 0; x < row.Children.Count; x++)
				{
					var child = row.Children[x];
					if (child.NodeConfiguration.Layout == CogaLayout.Absolute)
						continue;

					child.Compute.Position.X.Complete(node.Compute.X + sum + node.ComputedPadding[Edge.Left].Value);
					sum += child.Compute.Width + node.NodeConfiguration.Spacing;
				}
			}
		}
	}
}
