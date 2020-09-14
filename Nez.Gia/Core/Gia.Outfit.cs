using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez.SpriteSystem;
using Nez.VisibilitySystem;

namespace Nez
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
            /// Create an entity that represents a sprite that will not move. This is faster than a dynamic sprite as it will
            /// not be iterated over to dynamically update the render volume.
            /// </summary>
            public static Entity StaticSprite(Vector2 position, Texture2D texture, Rectangle source = default)
            {
                var entity = Gia.Current.World.CreateEntity();
                StaticSprite(entity, position, texture, source);
                return entity;
            }
            /// <summary>
            /// Add to an entity to make it represent a sprite that will not move. This is faster than a dynamic sprite as it will
            /// not be iterated over to dynamically update the render volume.
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
        }
    }
}
