using Microsoft.Xna.Framework;

namespace Nez.UIComponents
{
    public class Label : UIComponent
    {
        public delegate string Binding();

        IFont _labelFont;
        public IFont LabelFont
        {
            get { return _labelFont; }
            set { _labelFont = value; CalculateBounds(); SetDirty(); }
        }
        public Color Color;

        string _message = "";
        public string Message
        {
            get { return _message; }
            set { _message = value; CalculateBounds(); SetDirty(); }
        }

        Vector2 _Bounds;

        string bindCache;
        Binding bind;

        public Label(Binding binding, IFont fontFace = null) : this("", fontFace ?? Gia.Theme.DefaultFont, Gia.Theme.ForegroundColor)
        {
            bind = binding;
            CheckCache();
        }
        public Label(string text, IFont fontFace = null) : this(text, fontFace ?? Gia.Theme.DefaultFont, Gia.Theme.ForegroundColor) { }
        public Label(string text, IFont fontFace, Color fontCol)
        {
            bind = null;
            bindCache = "";

            DrawMethod = DefaultDraw;

            _message = text;
            _labelFont = fontFace;
            Color = fontCol;
            
            CalculateBounds();
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
            if (bind != null)
                CheckCache();
            batcher.DrawString(LabelFont, Message, finalBounds.Location.ToVector2(), Color);
        }

        void CheckCache()
        {
            var output = bind();
            if (output != bindCache)
            {
                bindCache = output;
                Message = output;
            }
        }
    }
}
