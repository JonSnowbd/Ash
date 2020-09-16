using DefaultEcs.System;

namespace Ash.Editor
{
    public class EditorScene : GiaScene
    {
        GiaScene EditedScene;
        public EditorScene(GiaScene sceneToEdit)
        {
            EditedScene = sceneToEdit;
        }

        public override void ConstructSystemGraph(FastList<ISystem<GiaScene>> updateSystems, FastList<ISystem<GiaScene>> renderSystems)
        {
            
        }
    }
}
