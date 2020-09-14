using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Nez.VisibilitySystem;
using System;

namespace Nez.SpriteSystem
{
    [With(typeof(AABB))]
    [With(typeof(SpriteC))]
    public sealed class SpriteRenderSystem : AEntitySystem<GiaScene>
    {
        public Material Material = Material.DefaultMaterial;
        int DrawnSprites = 0;
        int SkippedSprites = 0;
        public SpriteRenderSystem(World world) : base(world)
        {

        }
        public SpriteRenderSystem(World world, IParallelRunner runner) : base(world, runner)
        {

        }

        protected override void PreUpdate(GiaScene state)
        {
            DrawnSprites = 0;
            state.Batcher.Begin(Material, state.View.TransformMatrix);
        }
        protected override void Update(GiaScene state, ReadOnlySpan<Entity> entities)
        {
            if (entities.Length == 0)
                return;

            for (int i = 0; i < entities.Length; i++)
            {
                ref AABB aa = ref entities[i].Get<AABB>();

                if (aa.Hidden || !state.View.Bounds.Intersects(aa.Bounds))
                {
                    SkippedSprites++;
                    continue;
                }

                ref SpriteC sprite = ref entities[i].Get<SpriteC>();
                
                state.Batcher.DrawRect(aa.Bounds, sprite.Color);
                
                if (Gia.Debug.Enabled)
                {
                    DrawnSprites++;
                    Gia.Debug.DeferHollowRectangle(aa.Bounds, Gia.Theme.HighlightColor);
                    Gia.Debug.DeferPixel(aa.Bounds.Center, Gia.Theme.HighlightColor, 2);
                }
            }
        }

        protected override void PostUpdate(GiaScene state)
        {
            state.Batcher.End();

            if (Gia.Debug.Enabled)
            {
                Gia.Debug.DeferStringMessage($"SpriteRenderSystem Report: {DrawnSprites} Drawn, {SkippedSprites} Skipped", true);
            }
        }

        public void Inspect()
        {
            Gia.Current.Nubs.Add(InspectionNub);
        }

        public string InspectionNub(GiaScene context)
        {
            return $"{DrawnSprites} Sprites";
        }
    }
}
