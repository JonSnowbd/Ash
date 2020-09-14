using Coga;
using Microsoft.Xna.Framework;

namespace Nez.UIComponents
{
    public abstract class UIComponent : CogaNode
    {
        public delegate void DrawDelegate(Batcher batcher, Rectangle finalBounds);

        /// <summary>
        /// Required to make this component drawable. Coded as a delegate
        /// to allow easy re-skinning and per-component flexibility.
        /// </summary>
        public DrawDelegate DrawMethod;

        public override void Destroy()
        {
        }
    }
}
