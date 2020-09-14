using DefaultEcs;
using DefaultEcs.System;

namespace Nez
{
    public sealed class WorldEntityDebugSystem : AEntitySystem<GiaScene>
    {
        public WorldEntityDebugSystem(World world) : base(world)
        {

        }
    }
}
