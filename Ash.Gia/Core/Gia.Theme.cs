using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ash
{
    public static partial class Gia
    {
        public static class Theme
        {
            /// <summary>
            /// This is the font used for debug purposes. Ideally this font is really small.
            /// </summary>
            public static IFont DeveloperFont;
            public static IFont DefaultFont;
            /// <summary>
            /// The color of the application if it was blank. For black text on white background, this is white.
            /// </summary>
            public static Color BackgroundColor;
            /// <summary>
            /// The absolute background color, this is whats behind your final render target. Use this for letterbox coloring.
            /// </summary>
            public static Color ApplicationNullColor;
            /// <summary>
            /// A transparent background.
            /// </summary>
            public static Color FaintBackgroundColor;
            /// <summary>
            /// For UI, the color of background UI Panels.
            /// </summary>
            public static Color PanelBackground;
            /// <summary>
            /// The color of the application's main content like text. For black text on white background, this is black.
            /// </summary>
            public static Color ForegroundColor;
            /// <summary>
            /// The highlight color, for instance if something flashed, or if the top edge of something needed light shading.
            /// </summary>
            public static Color HighlightColor;
            /// <summary>
            /// The Theme color, anything that needs a consistent color across the UI. For example Twitter's Blue
            /// </summary>
            public static Color PrimaryThemeColor;
            /// <summary>
            /// Not quite main, something that compliments MainThemeColor.
            /// </summary>
            public static Color SecondaryThemeColor;
            /// <summary>
            /// The error color, if something went wrong this is the color used to display its background.
            /// </summary>
            public static Color ErrorThemeColor;

            // Default theme
            static Theme()
            {
                DeveloperFont = Graphics.Instance.DevFont;
                DefaultFont = Graphics.Instance.DevFontNarrow;
                ApplicationNullColor = Color.Black;
                PanelBackground = ColorExt.HexToColor("#14101d");
                BackgroundColor = ColorExt.HexToColor("#07090F");
                FaintBackgroundColor = Color.FromNonPremultiplied(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B, 140);
                ForegroundColor = ColorExt.HexToColor("#D8DBE2");
                HighlightColor = ColorExt.HexToColor("#2176AE");
                PrimaryThemeColor = ColorExt.HexToColor("#04F06A");
                SecondaryThemeColor = ColorExt.HexToColor("#E56399");
                ErrorThemeColor = ColorExt.HexToColor("#D8DBE2");
            }
        }
    }
}
