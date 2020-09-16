using DefaultEcs;
using DefaultEcs.System;

namespace Ash
{
    public sealed class WorldEntityDebugSystem : AEntitySystem<GiaScene>
    {
        public WorldEntityDebugSystem(World world) : base(world)
        {

        }
    }
}
