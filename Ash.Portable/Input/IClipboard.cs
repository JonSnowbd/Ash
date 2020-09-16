namespace Ash
{
	public interface IClipboard
	{
		string GetContents();
		void SetContents(string text);
	}
}