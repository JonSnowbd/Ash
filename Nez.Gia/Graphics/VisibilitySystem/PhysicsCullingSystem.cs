using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using System;
using System.Collections.Generic;

namespace Nez.VisibilitySystem
{
    public class PhysicsCullingSystem : AEntitySystem<GiaScene>
    {
        EntitySet NewEnts;
        EntitySet OldEnts;

        SpatialHash Hash;

        public HashSet<AABB> Potentials;

        public PhysicsCullingSystem(World world) : base(world)
        {
            Hash = new SpatialHash(100);

            NewEnts = world.GetEntities().With<AABB>().WhenAdded<Transform>().AsSet();
            OldEnts = world.GetEntities().With<AABB>().WhenRemoved<Transform>().AsSet();
        }
        public PhysicsCullingSystem(World world, IParallelRunner runner) : base(world, runner)
        {
            Hash = new SpatialHash(100);

            NewEnts = world.GetEntities().With<AABB>().WhenAdded<Transform>().AsSet();
            OldEnts = world.GetEntities().With<AABB>().WhenRemoved<Transform>().AsSet();
        }

        protected override void PreUpdate(GiaScene state)
        {
            // Manage the Hash State.
            foreach (ref readonly Entity newEnt in NewEnts.GetEntities())
            {
                ref var aa = ref newEnt.Get<AABB>();
                Hash.Register(aa);
            }
            foreach (ref readonly Entity oldEnt in OldEnts.GetEntities())
            {
                ref var aa = ref oldEnt.Get<AABB>();
                Hash.Remove(aa);
            }

            // Dump off VisibleEntities.
            //Potentials = Hash.AabbBroadphase(ref state.View.Bounds);

            Gia.Debug.VisibleEntities = Potentials.Count;
        }

        protected override void Update(GiaScene state, ReadOnlySpan<Entity> entities)
        {
            foreach(ref readonly Entity entity in entities)
            {
                ref var aa = ref entity.Get<AABB>();
                if (aa.Hidden)
                {
                    if (Gia.Debug.Enabled)
                    {
                        Gia.Debug.DeferHollowRectangle(aa.Bounds, Gia.Theme.PrimaryThemeColor);
                        Gia.Debug.DeferPixel(aa.Bounds.Center, Gia.Theme.HighlightColor, 2);
                    }
                }
                else // Entities set to invisible are never on screen.
                { 
                }
                entity.Set(aa);
            }
        }

        protected override void PostUpdate(GiaScene state)
        {
            base.PostUpdate(state);
            if (Gia.Debug.Enabled)
            {
                Gia.Debug.DeferStringMessage($"{Potentials.Count} Visible Entities");
            }
        }
    }
}
