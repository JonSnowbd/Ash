namespace Ash.ImGuiTools.ObjectInspectors
{
	public interface IComponentInspector
	{
		ECEntity Entity { get; }
		ECComponent Component { get; }

		void Draw();
	}
}