using System;
using System.Collections.Generic;
using System.Text;

namespace Ash.Editor
{
    /// <summary>
    /// This system hijacks the inner edited scene to redirect screen space to the in-world editor.
    /// </summary>
    public class EditorSpoofSystem : StaticUpdateSystem
    {
        EditorScene Editor;
        public EditorSpoofSystem(EditorScene scene)
        {
            Editor = scene;
        }
        public override void Update(GiaScene state)
        {
            base.Update(state);
        }
    }
}
