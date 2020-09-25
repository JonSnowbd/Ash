namespace Ash
{
    public struct Text
    {
        public enum Alignment
        {
            Left,
            Center,
            Right
        }
        public enum Overflow
        {
            Hidden,
            Truncated,
            Visible
        }

        public IFont Font;
        public string Message;
        public bool Wrapped;
        public Alignment Align;
        public Overflow OverflowBehaviour;

        public Text(string message) : this(message, Gia.Theme.DefaultFont)
        {
        }
        public Text(string message, IFont font)
        {
            Font = font;
            Message = message;
            Wrapped = true;
            Align = Alignment.Left;
            OverflowBehaviour = Overflow.Visible;
        }
    }
}
