using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ash.SpriteSystem
{
    public struct SpriteC
    {
        public bool UsesSpriteSource;
        public Rectangle SpriteSource;
        public Color Color;

        public Texture2D Texture;

        public SpriteC(Texture2D texture) : this(texture, Color.White, null) { }
        public SpriteC(Texture2D texture, Rectangle source) : this(texture, Color.White, source) { }
        public SpriteC(Texture2D texture, Color c) : this(texture, c, null) { }
        public SpriteC(Texture2D sheet, Color c, Rectangle? source)
        {
            Color = c;
            UsesSpriteSource = source.HasValue;
            if (UsesSpriteSource)
                SpriteSource = source.Value;
            else
                SpriteSource = new Rectangle(0, 0, 0, 0);
            Texture = sheet;
        }
    }
}
