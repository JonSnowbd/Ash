using Coga;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Ash.UIComponents
{
    public class TextInput : UIComponent
    {
        public delegate string Binding();

        IFont _labelFont;
        Vector2 _Bounds;
        int _padding;
        float caretTimer;
        bool caretVisible;

        public string GhostValue = "Type Here";
        public Color FontColor;
        public Color PanelColor;
        public Color FontGhostColor;
        public float CaretBlinkTime;
        public IFont LabelFont
        {
            get { return _labelFont; }
            set { _labelFont = value; CalculateBounds(); SetDirty(); }
        }
        public string TextValue;
        public int Padding
        {
            get { return _padding; }
            set { _padding = value; CalculateBounds(); SetDirty(); }
        }
        

        public TextInput(string defaultValue = "", int padding = 3, IFont fontFace = null) : this(defaultValue, padding, fontFace ?? Gia.Theme.DefaultFont, Gia.Theme.ForegroundColor, Gia.Theme.FaintBackgroundColor) { }
        public TextInput(string defaultValue, int padding, IFont fontFace, Color fontCol, Color backColor)
        {
            caretVisible = false;
            CaretBlinkTime = 0.33f;
            DrawMethod = DefaultDraw;
            caretTimer = 0f;
            _padding = padding;
            TextValue = defaultValue;
            _labelFont = fontFace;
            FontColor = fontCol;
            PanelColor = backColor;
            FontGhostColor = Color.FromNonPremultiplied(FontColor.R, FontColor.G, FontColor.B, 100);

            SetWidthToGrow();

            CalculateBounds();

            SetInteractive();

            Interactivity.OnClick += (node) => Manager?.SetFocus(this);
            Interactivity.OnDragStart += StartDrag;
            Interactivity.OnDrag += Drag;
        }

        public override Vector2 MinimumNodeSize()
        {
            return _Bounds;
        }

        public override Vector2? PreferredNodeSize()
        {
            return _Bounds;
        }

        Dictionary<int, float> KeyTimeout = new Dictionary<int, float>();
        const float keyTimer = 0.2f;
        public override void FocusUpdate()
        {
            caretTimer += Time.UnscaledDeltaTime;
            if (caretTimer >= CaretBlinkTime)
            {
                caretTimer -= CaretBlinkTime;
                caretVisible = !caretVisible;
            }
            var keys = Input.CurrentKeyboardState.GetPressedKeys();
            var count = Input.CurrentKeyboardState.GetPressedKeyCount();
            var upper = Input.IsKeyDown(Keys.LeftShift) || Input.IsKeyDown(Keys.RightShift);

            for (int i = 0; i < count; i++)
            {
                var keycode = (int)keys[i];
                
                if (!KeyTimeout.ContainsKey(keycode))
                {
                    KeyTimeout.Add(keycode, keyTimer);
                }
                else
                {
                    if(KeyTimeout[keycode] > 0f)
                    {
                        continue;
                    }
                    else
                    {
                        KeyTimeout[keycode] = keyTimer;
                    }
                }

                switch (keys[i])
                {
                    case Keys.Back:
                        if (TextValue.Length > 0)
                            TextValue = TextValue.Substring(0, TextValue.Length - 1);
                        continue;
                    case Keys.Space:
                        TextValue += " ";
                        continue;
                }

                if (keycode > 64 && keycode < 91)
                {
                    var val = upper ? keys[i].ToString().ToUpper() : keys[i].ToString().ToLower();
                    TextValue += val;
                }
            }

            foreach(var pair in KeyTimeout)
            {
                KeyTimeout[pair.Key] -= Time.UnscaledDeltaTime;
            }
        }

        void CalculateBounds()
        {
            var height = LabelFont.MeasureString("A").Y;
            _Bounds = new Vector2(Padding*2,height+Padding*2);
        }

        public void DefaultDraw(Batcher batcher, Rectangle finalBounds)
        {
            var isEmpty = string.IsNullOrEmpty(TextValue);
            var start = finalBounds.Location.ToVector2() + new Vector2(Padding);
            batcher.DrawRect(finalBounds, PanelColor);

            // Ghost or normal
            if (isEmpty)
            {
                batcher.DrawString(LabelFont, GhostValue, start, FontGhostColor);
            }
            else
            {
                batcher.DrawString(LabelFont, TextValue, start, FontColor);
            }
            
            if(Manager?.GetFocus() == this)
            {
                if(caretVisible)
                {
                    var end = LabelFont.MeasureString(isEmpty ? "." : TextValue);
                    batcher.DrawLine(start.X + end.X, start.Y, start.X + end.X, start.Y + end.Y, FontColor);
                }
            }
        }

        void Drag(CogaNode node, Vector2 vec)
        {

        }
        void StartDrag(CogaNode node)
        {

        }
    }
}
