using System.Collections.Generic;

namespace Ash.Tiled
{
	public interface ITmxLayer : ITmxElement
	{
		float OffsetX { get; }
		float OffsetY { get; }
		float Opacity { get; }
		bool Visible { get; }
		Dictionary<string, string> Properties { get; }
	}
}