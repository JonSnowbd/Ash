﻿namespace Ash
{
    public static partial class Gia
    {
        internal static GiaScene BeingConstructed;

        /// <summary>
        /// Returns the current Core.Scene as a GiaScene. If it is not found, instead it will look for a GiaScene currently being constructed.
        /// </summary>
        public static GiaScene Current 
        { 
            get 
            {
                var current = Core.Scene as GiaScene;
                if (current == null)
                    return BeingConstructed;
                return current; 
            } 
        }

        public static bool IsConsumingKeyboard = false;
        public static bool KeyboardGuard()
        {
            return !IsConsumingKeyboard;
        }

        public static bool IsConsumingMouse = false;
        public static bool MouseGuard()
        {
            return !IsConsumingMouse;
        }
    }
}
