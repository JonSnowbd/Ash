namespace Coga.StandardLayout
{
	/// <summary>
	/// This is the class responsible for adding extension methods for
	/// calculating the Sizes of a node as best it can.
	/// </summary>
	internal static class StandardLayoutSizing
	{
		internal static void CalculateSize(this CogaNode node)
		{
			// Now calculate the actual sizes.
			CalculateWidth(node);
			CalculateHeight(node);
		}

		static void CalculateWidth(CogaNode node)
		{
			if (node.GetIsRoot())
			{
				node.Compute.Size.X.Complete(node.NodeConfiguration.Width.Value);
				return;
			}
			switch (node.NodeConfiguration.Width.ValueType)
			{
				// Flat pixel amounts are just passed through.
				case CogaValueMode.Pixel:
					node.Compute.Size.X.Complete(node.NodeConfiguration.Width.Value);
					break;

				case CogaValueMode.Default:
					var pref = node.PreferredNodeSize();
					if (pref.HasValue)
					{
						node.Compute.Size.X.Complete(pref.Value.X);
					}
					else
					{
						node.Compute.Size.X.Complete(node.MinimumNodeSize().X);
					}
					break;
				case CogaValueMode.Fill:
					node.Compute.Size.X.Complete(node.MinimumNodeSize().X);
					break;
			}
		}

		static void CalculateHeight(CogaNode node)
		{
			if (node.GetIsRoot())
			{
				node.Compute.Size.Y.Complete(node.NodeConfiguration.Height.Value);
				return;
			}
			switch (node.NodeConfiguration.Height.ValueType)
			{
				// Flat pixel amounts are just passed through.
				case CogaValueMode.Pixel:
					node.Compute.Size.Y.Complete(node.NodeConfiguration.Height.Value);
					break;

				case CogaValueMode.Default:
					var pref = node.PreferredNodeSize();
					if (pref.HasValue)
					{
						node.Compute.Size.Y.Complete(pref.Value.Y);
					}
					else
					{
						node.Compute.Size.Y.Complete(node.MinimumNodeSize().Y);
					}
					break;

				case CogaValueMode.Fill:
					node.Compute.Size.Y.Complete(node.MinimumNodeSize().Y);
					break;
			}
		}
	}
}
