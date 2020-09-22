using DefaultEcs;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Ash
{
    public class TextRenderSystem : RenderSystem
    {
        /// <summary>
        /// Use this constructor to draw all types of text. Be warned if you use this constructor
        /// you lock yourself into only using this text renderer unless you're fine with duplicate rendering. 
        /// For fine tuning the layer order please use the alternate constructor.
        /// </summary>
        /// <param name="w"></param>
        public TextRenderSystem(World w, bool screenSpace) : base(w, screenSpace, typeof(AABB), typeof(Text))
        {
        }

        /// <summary>
        /// Use this constructor to limit the draws to a specific set of entities by specifying extra
        /// required components.
        /// </summary>
        public TextRenderSystem(World w, bool screenSpace, params Type[] extraPredicates) : base(w, screenSpace, new[] { typeof(AABB), typeof(Text) }.Concat(extraPredicates).ToArray())
        {
        }
        protected override void DebugDraw(GiaScene context, Batcher batcher, Entity entity, ref AABB bounds)
        {
            Gia.Debug.DeferWorldHollowRect(bounds.Bounds, Color.Orange);
        }

        protected override void Draw(GiaScene context, Batcher batcher, Entity entity, ref AABB bounds)
        {
            ref var text = ref entity.Get<Text>();

            batcher.DrawString(Gia.Theme.DefaultFont, text.Message, bounds.Bounds.Location, Color.White);
        }
    }
}
