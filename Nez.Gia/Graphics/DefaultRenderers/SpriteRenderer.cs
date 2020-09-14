using DefaultEcs;
using Nez.SpriteSystem;
using Nez.VisibilitySystem;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Nez
{
    public class SpriteRenderer : RenderSystem
    {
        /// <summary>
        /// Use this constructor to draw all types of sprites. Be warned if you use this constructor
        /// you lock yourself into only using this sprite renderer unless you're fine with duplicate rendering. 
        /// For fine tuning the layer order please use the alternate constructor.
        /// </summary>
        /// <param name="w"></param>
        public SpriteRenderer(World w, bool screenSpace) : base(w, screenSpace, typeof(SpriteC), typeof(Transform))
        {
        }

        /// <summary>
        /// Use this constructor to limit the draws to a specific set of entities by specifying extra
        /// required components.
        /// </summary>
        public SpriteRenderer(World w, bool screenSpace, params Type[] extraPredicates) : base(w, screenSpace, new[] { typeof(SpriteC), typeof(Transform) }.Concat(extraPredicates).ToArray())
        {
        }

        protected override void DebugDraw(GiaScene context, Batcher batcher, Entity entity, ref AABB bounds)
        {
        }

        protected override void Draw(GiaScene context, Batcher batcher, Entity entity, ref AABB bounds)
        {
            ref SpriteC sprite = ref entity.Get<SpriteC>();
            ref Transform transform = ref entity.Get<Transform>();
            if (sprite.UsesSpriteSource)
            {
                batcher.Draw(sprite.Texture, transform.Position, sprite.SpriteSource, sprite.Color, transform.Rotation, Vector2.Zero, transform.Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1f);
            }
            else
            {
                batcher.Draw(sprite.Texture, transform.Position, null, sprite.Color, transform.Rotation, Vector2.Zero, transform.Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1f);
            }
            
        }
    }
}
