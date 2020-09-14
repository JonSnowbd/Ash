using Microsoft.Xna.Framework;
using Nez.UI;

namespace Nez.UIComponents
{
    public class Button : UIComponent, ITransactionalComponent<bool>
    {
        UserInterface.TransactionalBinding<bool> ExternalAction;
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

        public Button(string message, int padding = 3, IFont buttonLabelFont = null) 
            :  this(message, padding, buttonLabelFont ?? Gia.Theme.DefaultFont, Gia.Theme.SecondaryThemeColor,
                   Gia.Theme.PrimaryThemeColor, Gia.Theme.HighlightColor, Gia.Theme.ForegroundColor)
        {
        }
        public Button(string message, int padding, IFont buttonLabelFont, Color idle, Color hover, Color clicked, Color fontColor)
        {
            LabelFont = buttonLabelFont ?? Gia.Theme.DefaultFont;
            Message = message;
            Padding = padding;

            // Colors
            IdleColor = idle;
            HoverColor = hover;
            ActiveColor = clicked;
            FontColor = fontColor;

            // Meta
            DrawMethod = DefaultDraw;
            CalculateBounds();

            // Defaults
            realColor = IdleColor;
            target = IdleColor;

            // Interactivity
            SetInteractive();
            Interactivity.OnHover += (node) => target = HoverColor;
            Interactivity.OnClick += (node) =>
            {
                realColor = ActiveColor;
                Manager.SetFocus(this);
                ExternalAction?.PushFromControl(true);
            };
            Interactivity.OnEndClicking += (node) =>
            {
                ExternalAction?.PushFromControl(false);
            };
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

        public void Take(UserInterface.TransactionalBinding<bool> transaction)
        {
            ExternalAction = transaction;
            ExternalAction.PushFromControl(false); // Buttons start out not clicked.
        }
    }
}
