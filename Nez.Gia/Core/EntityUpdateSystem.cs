using DefaultEcs;
using DefaultEcs.System;
using System;

namespace Nez
{
    
    public class EntityUpdateSystem : AEntitySystem<GiaScene>
    {
        protected World CurrentWorld;

        static EntitySet Compute(World world, Type[] types)
        {
            var build = world.GetEntities();
            for (int i = 0; i < types.Length; i++)
            {
                Type t = types[i];
                typeof(EntityRuleBuilder)
                    .GetMethod("With", new Type[0])
                    .MakeGenericMethod(t)
                    .Invoke(build, new object[0]);
            }
            return build.AsSet();
        }

        /// <summary>
        /// If you use this constructor be sure to have applied [With] [Without] and [Either]
        /// attributes to the class definition. The World is drawn from <c>Gia.Current</c>
        /// </summary>
        public EntityUpdateSystem() : base(Gia.Current.World)
        {
            CurrentWorld = Gia.Current.World;
        }

        /// <summary>
        /// If you use this constructor be sure to have applied [With] [Without] and [Either]
        /// attributes to the class definition.
        /// </summary>
        public EntityUpdateSystem(World world) : base(world)
        {
            CurrentWorld = world;
        }

        /// <summary>
        /// If you use this constructor, you must pass in a list of components(of any type) that
        /// the entities must have. If you need fine tuned control over the set restrictions use attribute
        /// constructor.
        /// </summary>
        public EntityUpdateSystem(World world, params Type[] requirements) : base(Compute(world, requirements))
        {
            CurrentWorld = world;
        }
    }
}
