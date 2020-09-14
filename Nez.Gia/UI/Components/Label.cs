using Microsoft.Xna.Framework;

namespace Nez.UIComponents
{
    public class Label : UIComponent
    {
        public IFont LabelFont;
        public Color Color;

        string _message = "";
        public string Message
        {
            get { return _message; }
            set { _message = value; CalculateBounds(); }
        }

        Vector2 _Bounds;
        public Label(string text, IFont font)
        {
            _message = text;
            LabelFont = font;
            CalculateBounds();
            DrawMethod = DefaultDraw;
            Color = Gia.Theme.ForegroundColor;
        }

        public override Vector2 MinimumNodeSize()
        {
            return _Bounds;
        }

        public override Vector2? PreferredNodeSize()
        {
            return _Bounds;
        }

        void CalculateBounds()
        {
            _Bounds = LabelFont.MeasureString(_message);
        }

        public void DefaultDraw(Batcher batcher, Rectangle finalBounds)
        {
            batcher.DrawString(LabelFont, Message, finalBounds.Location.ToVector2(), Color);
        }
    }
}
