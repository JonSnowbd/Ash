using DefaultEcs.System;

namespace Nez.Editor
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
