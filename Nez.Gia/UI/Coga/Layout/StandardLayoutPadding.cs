namespace Coga.StandardLayout
{
	internal static class StandardLayoutPadding
	{
		internal static void CalculatePadding(this CogaNode node)
		{
			// Conveniently compute the node edges without repeating code over and over.
			void AttemptCalculatePadding(Edge e)
			{
				switch (node.NodeConfiguration.Padding[e].ValueType)
				{
					case CogaValueMode.Pixel:
						node.ComputedPadding[e].Complete(node.NodeConfiguration.Padding[e].Value);
						break;
				}
			}
			AttemptCalculatePadding(Edge.Left);
			AttemptCalculatePadding(Edge.Top);
			AttemptCalculatePadding(Edge.Right);
			AttemptCalculatePadding(Edge.Bottom);

			// Resolve convenience edges.
			if (node.ComputedPadding[Edge.Top].Resolved || node.ComputedPadding[Edge.Bottom].Resolved)
			{
				var top = node.ComputedPadding.ContainsKey(Edge.Top) ? node.ComputedPadding[Edge.Bottom].Value : 0f;
				var bottom = node.ComputedPadding.ContainsKey(Edge.Right) ? node.ComputedPadding[Edge.Right].Value : 0f;
				node.ComputedPadding[Edge.Vertical].Complete(top+bottom);
			}
			if (node.ComputedPadding[Edge.Left].Resolved || node.ComputedPadding[Edge.Right].Resolved)
			{
				var left = node.ComputedPadding.ContainsKey(Edge.Left) ? node.ComputedPadding[Edge.Left].Value : 0f;
				var right = node.ComputedPadding.ContainsKey(Edge.Right) ? node.ComputedPadding[Edge.Right].Value : 0f;
				node.ComputedPadding[Edge.Horizontal].Complete(left+right);
			}
		}

	}
}
