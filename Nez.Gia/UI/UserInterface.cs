using Coga;
using Microsoft.Xna.Framework;
using Nez.UIComponents;
using System;

namespace Nez.UI
{
    public class UserInterface : ICogaManager
    {
        public class TransactionalBinding
        {
        }
        public class TransactionalBinding<T> : TransactionalBinding
        {
            public T Value;
            /// <summary>
            /// Used by controls to push an value outward to subscribers.
            /// </summary>
            public event Action<T> InternalPush;
            /// <summary>
            /// Used by external subscribers to push a value inward to the control.
            /// </summary>
            public event Action<T> ExternalPush;

            public void PushFromControl(T value)
            {
                Value = value;
                InternalPush(value);
            }

            public void PushFromExternal(T value)
            {
                Value = value;
                ExternalPush(value);
            }
        }

        public FastList<TransactionalBinding> Transactions;
        public UIComponent Hover;
        public bool HoverLocked;

        public int Width;
        public int Height;

        public UIComponent Root;
        public UIComponent Focus;

        public bool IsScreenSpace;

        public UserInterface(int width, int height)
        {
            Transactions = new FastList<TransactionalBinding>();
            Hover = null;
            HoverLocked = false;

            Width = width;
            Height = height;

            Root = new Panel(width, height);
            Root.SetWidthInPixels(width);
            Root.SetHeightInPixels(height);
            Root.SetRoot(width, height, this);
        }

        public void LockHover()
        {
            HoverLocked = true;
        }

        public void UnlockHover()
        {
            HoverLocked = false;
        }

        public void SetHover(CogaNode node)
        {
            Hover = node as UIComponent;
        }

        public CogaNode GetHover() => Hover;

        public CogaNode GetRoot() => Root;

        public CogaNode GetFocus() => Focus;

        public CogaNode SetFocus(CogaNode newFocus) => Focus = newFocus as UIComponent;

        public bool IsClickDown => Input.LeftMouseButtonDown;

        public bool IsClickPressed => Input.LeftMouseButtonPressed;

        public bool IsClickReleased => Input.LeftMouseButtonReleased;

        public Vector2 MousePosition => ForwardedMousePosition;
        public Vector2 ForwardedMousePosition;
    }
}
