using Microsoft.Xna.Framework;

namespace Nez.UIComponents
{
    public class Button : UIComponent
    {
        public Color FontColor;
        public Color IdleColor;
        public Color HoverColor;
        public Color ActiveColor;
        public IFont LabelFont;
        public string Message;
        public int Padding;
        Vector2 _bounds;

        Color realColor;
        Color target;

        public Button(IFont buttonLabelFont, string message, int padding = 3)
        {
            LabelFont = buttonLabelFont;
            Message = message;
            Padding = padding;

            // Colors
            IdleColor = Gia.Theme.MainThemeColor;
            HoverColor = Gia.Theme.SecondaryThemeColor;
            FontColor = Gia.Theme.BackgroundColor;
            ActiveColor = Gia.Theme.ForegroundColor;

            // Meta
            DrawMethod = DefaultDraw;
            CalculateBounds();

            // Defaults
            realColor = IdleColor;
            target = IdleColor;

            // Interactivity
            SetInteractive();
            Interactivity.OnHover += (node) => target = HoverColor;
            Interactivity.OnClick += (node) => realColor = ActiveColor;
            Interactivity.OnUnhover += (node) => target = IdleColor;
        }

        public override Vector2 MinimumNodeSize()
        {
            return _bounds;
        }

        public override Vector2? PreferredNodeSize()
        {
            return _bounds;
        }

        void CalculateBounds()
        {
            var size = LabelFont.MeasureString(Message);
            _bounds = size + (new Vector2(Padding, Padding) * 2);
        }

        public void DefaultDraw(Batcher batcher, Rectangle finalBounds)
        {
            realColor = Color.Lerp(realColor, target, 5f * Time.UnscaledDeltaTime);
            batcher.DrawRect(finalBounds, realColor);
            batcher.DrawString(LabelFont, Message, finalBounds.Location.ToVector2() + new Vector2(Padding, Padding), FontColor);
        }
    }
}
