using Microsoft.Xna.Framework;

namespace Nez.UIComponents
{
    public class Panel : UIComponent
    {
        public Color Color;
        Vector2 _Bounds;

        public int Width;
        public int Height;

        public Panel(int width, int height)
        {
            Width = width;
            Height = height;
            Color = Gia.Theme.FaintBackgroundColor;
            DrawMethod = DefaultDraw;
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
            _Bounds = new Vector2(Width, Height);
        }

        public void DefaultDraw(Batcher batcher, Rectangle finalBounds)
        {
            batcher.DrawRect(finalBounds, Color);
        }
    }
}
