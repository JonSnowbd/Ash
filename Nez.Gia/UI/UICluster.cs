using Coga;
using Microsoft.Xna.Framework;
using Nez.UIComponents;
using System;

namespace Nez.UI
{
    public class UICluster : ICogaManager
    {
        public class TransactionalValue
        {
        }
        public class TransactionalValue<T> : TransactionalValue
        {
            public T Value;
            /// <summary>
            /// Used by controls to push an value outward to subscribers.
            /// </summary>
            protected event Action<T> InternalPush;
            /// <summary>
            /// Used by external subscribers to push a value inward to the control.
            /// </summary>
            public event Action<T> ExternalPush;
        }

        public FastList<TransactionalValue> Transactions;
        public UIComponent Focus;
        public bool FocusLocked;

        public int Width;
        public int Height;

        public UIComponent Root;


        public UICluster(int width, int height)
        {
            Transactions = new FastList<TransactionalValue>();
            Focus = null;
            FocusLocked = false;

            Width = width;
            Height = height;

            Root = new Panel(width, height);
            Root.SetWidthInPixels(width);
            Root.SetHeightInPixels(height);
            Root.SetRoot(width, height, this);
        }

        public TransactionalValue<T> CreateTransaction<T>()
        {
            var transaction = new TransactionalValue<T>();
            Transactions.Add(transaction);
            return transaction;
        }

        public void LockFocus()
        {
            FocusLocked = true;
        }

        public void UnlockFocus()
        {
            FocusLocked = false;
        }

        public void SetFocus(CogaNode node)
        {
            Focus = node as UIComponent;
        }

        public CogaNode GetFocus() => Focus;

        public CogaNode GetRoot() => Root;

        public bool IsClickDown => Input.LeftMouseButtonDown;

        public bool IsClickPressed => Input.LeftMouseButtonPressed;

        public bool IsClickReleased => Input.LeftMouseButtonReleased;

        // TODO: This is inaccurate. Do it in the render system.
        public Vector2 MousePosition => ForwardedMousePosition;

        public Vector2 ForwardedMousePosition;
    }
}
