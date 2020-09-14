using DefaultEcs.System;
using System;

namespace Nez
{
    public class SharedRenderPhase : ISystem<GiaScene>
    {
        RenderSystem[] Systems;

        public Material Material;
        public bool IsScreenSpace;

        bool isEnabled = true;
        public bool IsEnabled { get => isEnabled; set => isEnabled = value; }

        public SharedRenderPhase(bool screenSpace, params RenderSystem[] systems)
        {
            for (int i = 0; i < systems.Length; i++)
            {
                systems[i].IsPartOfSharedSystem = true;
                systems[i].IsScreenSpace = screenSpace;
            }

            Material = Material.DefaultMaterial;
            IsScreenSpace = screenSpace;
            Systems = systems;
        }

        public void Dispose()
        {
            for(int i = 0; i < Systems.Length; i++)
            {
                Systems[i].Dispose();
            }
        }

        public void Update(GiaScene state)
        {
            if (!IsEnabled)
                return;

            if (IsScreenSpace)
                state.Batcher.Begin(Material);
            else
                state.Batcher.Begin(Material, state.View.TransformMatrix);

            for(int i = 0; i < Systems.Length; i++)
            {
                Systems[i].Update(state);
            }

            state.Batcher.End();

        }

        public void Inspect()
        {
            for(int i = 0; i < Systems.Length; i++)
            {
                Systems[i].Inspect();
            }
        }
        public void Uninspect()
        {
            for (int i = 0; i < Systems.Length; i++)
            {
                Systems[i].Uninspect();
            }
        }
    }
}
