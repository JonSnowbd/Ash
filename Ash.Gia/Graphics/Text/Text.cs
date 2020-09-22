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

        public string Message;
        public bool Wrapped;
        public Alignment Align;
        public Overflow OverflowBehaviour;

        public Text(string message)
        {
            Message = message;
            Wrapped = true;
            Align = Alignment.Left;
            OverflowBehaviour = Overflow.Visible;
        }
    }
}
