namespace Coga.StandardLayout
{
	internal static class StandardLayout
	{
		internal static void LayoutChildren(this CogaNode node)
		{
			if (node.NeedsLayout == false || node.GetIsActive() == false || node.Children == null || node.Children.Count == 0)
			{
				node.TriggerLayout();
				return;
			}
				

			node.RowLayout.Refresh();
			StandardLayoutJustification.JustifyChildren(node);
			StandardLayoutAlignment.AlignChildren(node);

			foreach (var c in node.Children)
			{
				if (c.GetIsActive() == false)
					continue;
				if (c.NodeConfiguration.Layout == CogaLayout.Absolute)
					c.CalculateAbsolutePositioning();
				c.LayoutChildren();
				c.TriggerLayout();
			}

			node.NeedsLayout = false;
		}
	}
}
