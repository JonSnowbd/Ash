using DefaultEcs;
using DefaultEcs.System;
using Nez.VisibilitySystem;
using System;

namespace Nez
{
    public abstract class RenderSystem : AEntitySystem<GiaScene>
    {
        public Material Material;
        public bool IsPartOfSharedSystem;
        public bool IsScreenSpace;

        int drawnItems = 0;
        int skippedItems = 0;
        bool inspecting = false;

        protected static EntitySet Compute(World world, Type[] types)
        {
            var build = world.GetEntities();
            for(int i = 0; i < types.Length; i++)
            {
                Type t = types[i];
                typeof(EntityRuleBuilder)
                    .GetMethod("With", new Type[0])
                    .MakeGenericMethod(t)
                    .Invoke(build, new object[0]);
            }
            return build.AsSet();
        }

        public RenderSystem(World world, bool screenSpace, params Type[] types) : base(Compute(world, types))
        {
            IsPartOfSharedSystem = false;
            Material = Material.DefaultMaterial;
            IsScreenSpace = screenSpace;
        }

        protected override void PreUpdate(GiaScene state)
        {
            drawnItems = 0;
            skippedItems = 0;
            if (!IsPartOfSharedSystem)
            {
                if(IsScreenSpace)
                    state.Batcher.Begin(Material);
                else
                    state.Batcher.Begin(Material, state.View.TransformMatrix);
            }
        }

        protected override void Update(GiaScene state, ReadOnlySpan<Entity> entities)
        {
            if (entities.Length == 0)
                return;

            for (int i = 0; i < entities.Length; i++)
            {
                ref AABB aa = ref entities[i].Get<AABB>();

                if (IsScreenSpace)
                {
                    if (aa.Hidden || !state.ScreenBounds.Intersects(aa.Bounds))
                    {
                        skippedItems++;
                        continue;
                    }
                }
                else
                {
                    if (aa.Hidden || !state.View.Bounds.Intersects(aa.Bounds))
                    {
                        skippedItems++;
                        continue;
                    }
                }


                Draw(state, state.Batcher, entities[i], ref aa);
                drawnItems++;

                if (Gia.Debug.Enabled && inspecting)
                {
                    DebugDraw(state, state.Batcher, entities[i], ref aa);
                }
            }
        }

        protected override void PostUpdate(GiaScene state)
        {
            if(!IsPartOfSharedSystem)
                state.Batcher.End();

            if (Gia.Debug.Enabled && inspecting)
            {
                Gia.Debug.DeferStringMessage($"SpriteRenderer: {drawnItems} Drawn, {skippedItems} Skipped", true);
            }
        }

        public void Inspect()
        {
            Gia.Current.Nubs.Add(InspectionNub);
            inspecting = true;
        }
        public void Uninspect()
        {
            Gia.Current.Nubs.Remove(InspectionNub);
            inspecting = false;
        }
        public string InspectionNub(GiaScene context)
        {
            return $"{GetType().Name}: {drawnItems}/{skippedItems} ({drawnItems+skippedItems}) draws";
        }

        protected abstract void Draw(GiaScene context, Batcher batcher, Entity entity, ref AABB bounds);
        protected abstract void DebugDraw(GiaScene context, Batcher batcher, Entity entity, ref AABB bounds);
    }
}
