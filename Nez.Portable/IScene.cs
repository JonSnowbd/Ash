namespace Nez
{
    public interface IScene
	{
		void Begin();
		void End();
		void Update();
		void Render();
	}
}