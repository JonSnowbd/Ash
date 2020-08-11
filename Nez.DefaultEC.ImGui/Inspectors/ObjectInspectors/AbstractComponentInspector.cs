using System.Collections.Generic;
using Nez.ImGuiTools.TypeInspectors;


namespace Nez.ImGuiTools.ObjectInspectors
{
	public abstract class AbstractComponentInspector : IComponentInspector
	{
		public abstract ECEntity Entity { get; }
		public abstract ECComponent Component { get; }

		protected List<AbstractTypeInspector> _inspectors;
		protected int _scopeId = NezImGui.GetScopeId();

		public abstract void Draw();
	}
}