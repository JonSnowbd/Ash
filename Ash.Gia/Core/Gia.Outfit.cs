using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ash.SpriteSystem;
using Ash.UI;
using Ash.UIComponents;
using Ash.VisibilitySystem;

namespace Ash
{
    public static partial class Gia
    {
        /// <summary>
        /// Outfit uses <c>Gia.Current</c> to create and bootstrap basic entities to save time.
        /// What you should keep in mind is that this will use <c>Core.Scene as GiaScene</c>
        /// to operate off, and if that is not a GiaScene, then it will use the GiaScene
        /// you are currently constructing.
        /// </summary>
        public static class Outfit
        {
            /// <summary>
            /// Create an entity that represents a sprite that will not move.
            /// </summary>
            public static Entity StaticSprite(Vector2 position, Texture2D texture, Rectangle source = default)
            {
                var entity = Gia.Current.World.CreateEntity();
                StaticSprite(entity, position, texture, source);
                return entity;
            }
            /// <summary>
            /// Add to an entity to make it represent a sprite that will not move.
            /// </summary>
            public static void StaticSprite(Entity entity, Vector2 position, Texture2D texture, Rectangle source = default)
            {
                if(source == default)
                {
                    var aa = new AABB(position.X, position.Y, texture.Width, texture.Height);
                    entity.Set(aa);
                    entity.Set(new SpriteC(texture));
                    entity.Set(new Transform(position.X, position.Y));
                }
                else
                {
                    var aa = new AABB(position.X, position.Y, source.Width, source.Height);
                    entity.Set(aa);
                    entity.Set(new SpriteC(texture, Color.White, source));
                    entity.Set(new Transform(position.X, position.Y));
                }
            }

            /// <returns>A tuple containing the final entity, and the root UI node to build off.</returns>
            public static (Entity, UIComponent) UI(Vector2 position, Vector2 size, bool screenSpace = true)
            {
                var entity = Gia.Current.World.CreateEntity();

                var ui = new UserInterface((int)size.X, (int)size.Y);
                var aa = new AABB(position.X, position.Y, size.X, size.Y);

                entity.Set(ui);
                entity.Set(aa);

                return (entity, ui.Root);
            }


            /// <summary>
            /// Create and return a simple AABB+Text entity to display text.
            /// </summary>
            public static Entity Text(string text, Vector2 position, IFont font = null)
            {
                var entity = Gia.Current.World.CreateEntity();
                Text(entity, text, position, new Vector2(0.5f, 0.5f), font);
                return entity;
            }
            /// <summary>
            /// Create and return a simple AABB+Text entity to display text. Initial Origin is the offset of the overall
            /// size of the text, with 0f being centered on the top left, and 1f being centered on the bottom right.
            /// </summary>
            public static Entity Text(string text, Vector2 position, Vector2 initialOrigin, IFont font = null)
            {
                var entity = Gia.Current.World.CreateEntity();
                Text(entity, text, position, initialOrigin, font);
                return entity;
            }
            /// <summary>
            /// Adds to an entity to make a simple AABB+Text entity to display text. Initial Origin is the offset of the overall
            /// size of the text, with 0f being centered on the top left, and 1f being centered on the bottom right.
            /// </summary>
            public static void Text(Entity entity, string text, Vector2 position, IFont font = null)
            {
                Text(entity, text, position, Vector2.Zero, font);
            }
            /// <summary>
            /// Adds to an entity to make a simple AABB+Text entity to display text. Initial Origin is the offset of the overall
            /// size of the text, with 0f being centered on the top left, and 1f being centered on the bottom right.
            /// </summary>
            public static void Text(Entity entity, string text, Vector2 position, Vector2 initialOrigin, IFont font = null)
            {
                var realFont = font ?? Theme.DefaultFont;
                var stringMeasure = realFont.MeasureString(text);

                entity.Set(new AABB(position.X - stringMeasure.X * initialOrigin.X, position.Y, stringMeasure.X - stringMeasure.Y * initialOrigin.Y, stringMeasure.Y));
                entity.Set(new Text(text, realFont));
            }
        }
    }
}
