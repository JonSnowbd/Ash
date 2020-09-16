namespace Coga.StandardLayout
{
	internal static class StandardLayoutAbsolute
	{
		internal static void CalculateAbsolutePositioning(this CogaNode node)
		{
			if (node.Parent == null)
				return;

			// Calculate the left and right absolute positioning.
			if (node.NodeConfiguration.Left.ValueType != CogaValueMode.Undefined)
			{
				node.Compute.Position.X.Complete(node.Parent.Compute.X + node.NodeConfiguration.Left.Value);
				if (node.NodeConfiguration.Right.ValueType != CogaValueMode.Undefined)
				{
					node.Compute.Size.X.Complete((node.Parent.Compute.Width - node.NodeConfiguration.Right.Value) - node.Compute.X);
				}
			}
			else if (node.NodeConfiguration.Right.ValueType != CogaValueMode.Undefined)
			{
				if (node.Compute.Size.X.Resolved != false)
				{
					var right = node.Parent.Compute.X + node.Parent.Compute.Width - node.NodeConfiguration.Right.Value;
					node.Compute.Position.X.Complete(right - node.Compute.Width);
				}
			}

			// Then the up and down absolute positioning values.
			if (node.NodeConfiguration.Top.ValueType != CogaValueMode.Undefined)
			{
				node.Compute.Position.Y.Complete(node.Parent.Compute.Y + node.NodeConfiguration.Top.Value);
				if (node.NodeConfiguration.Bottom.ValueType != CogaValueMode.Undefined)
				{
					node.Compute.Size.Y.Complete((node.Parent.Compute.Height - node.NodeConfiguration.Bottom.Value) - node.Compute.Y);
				}
			}
			else if (node.NodeConfiguration.Bottom.ValueType != CogaValueMode.Undefined)
			{
				if (node.Compute.Size.Y.Resolved != false)
				{
					var bottom = node.Parent.Compute.Y + node.Parent.Compute.Height - node.NodeConfiguration.Bottom.Value;
					node.Compute.Position.Y.Complete(bottom - node.Compute.Height);
				}
			}
		}
	}
}
