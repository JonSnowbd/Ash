using Microsoft.Xna.Framework;
using System;

namespace Nez
{
    public static partial class Gia
    {
        public static class Debug
        {
            class DeferredDebugEvent
            {
                // 1: String debug log
                // 2: Rectangle draw
                // 3: Hollow Rectangle draw
                // 4: Pixel draw
                // 5: Circle draw
                // 6: Line draw
                internal int Type = 0;
                internal string Message = "";
                internal Rectangle Rect = new Rectangle(0, 0, 0, 0);
                internal Vector2 Position = Vector2.Zero;
                internal Vector2 End = Vector2.Zero;
                internal Color Color = Theme.ForegroundColor;
                internal int Size = 1;
                internal float Radius = 0f;
            }

            static int messagePadding;
            static Vector2 stringSplat;
            static Vector2 panelPad;

            public static bool Enabled;
            public static bool SceneSpace;
            public static int VisibleEntities;

            public static FastList<IInspectable> InspectedItems;

            static FastList<DeferredDebugEvent> DeferredEvents;
            static int DeferredCount;

            public static VirtualButton ToggleInput;

            static Debug()
            {
#if DEBUG
                Enabled = true;
#else
                Enabled = false;
#endif
                SceneSpace = true;
                InspectedItems = new FastList<IInspectable>();
                SetMessagePadding(4);

                VisibleEntities = 0;
                DeferredEvents = new FastList<DeferredDebugEvent>();
                DeferredCount = 0;

                stringSplat = new Vector2(messagePadding, messagePadding);

                ToggleInput = new VirtualButton()
                    .AddKeyboardKey(Microsoft.Xna.Framework.Input.Keys.F1);
            }

            internal static void SetMessagePadding(int pad)
            {
                messagePadding = pad;
                panelPad = new Vector2(Math.Max(0, pad * 0.33f), Math.Max(0, pad * 0.33f));
            }

            internal static void Flush(Batcher batcher)
            {
                for (int i = 0; i < DeferredCount; i++)
                {
                    var evt = DeferredEvents[i];
                    switch (evt.Type)
                    {
                        case 1:
                            batcher.DrawString(Theme.DeveloperFont, evt.Message, evt.Position, evt.Color);
                            break;
                        case 2:
                            batcher.DrawRect(evt.Rect, evt.Color);
                            break;
                        case 3:
                            batcher.DrawHollowRect(evt.Rect, evt.Color, evt.Size);
                            break;
                        case 4:
                            batcher.DrawPixel(evt.Position, evt.Color, evt.Size);
                            break;
                        case 5:
                            batcher.DrawCircle(evt.Position, evt.Radius, evt.Color, evt.Size);
                            break;
                        case 6:
                            batcher.DrawLine(evt.Position, evt.End, evt.Color, evt.Size);
                            break;
                    }
                }
                stringSplat.X = messagePadding;
                stringSplat.Y = messagePadding;
                DeferredCount = 0;
            }
            internal static void Cancel()
            {
                DeferredCount = 0;
            }

            internal static void Check()
            {
                if (DeferredCount + 1 >= DeferredEvents.Length)
                {
                    for (int i = 0; i < 128; i++)
                    {
                        DeferredEvents.Add(new DeferredDebugEvent());
                    }
                }
            }

            public static void DeferStringMessage(string text, bool panel = false)
            {
                var offset = Theme.DeveloperFont.MeasureString(text);
                DeferStringMessage(text, stringSplat, Theme.ForegroundColor, panel);
                stringSplat.Y += messagePadding + offset.Y;
            }
            public static void DeferStringMessage(string text, Vector2 pos, Color c, bool panel = false)
            {
                if (panel)
                {
                    var offset = Theme.DeveloperFont.MeasureString(text);
                    DeferRectangle(new Rectangle((pos - panelPad).ToPoint(), (offset + panelPad * 2).ToPoint()), Theme.FaintBackgroundColor);
                }
                Check();
                DeferredEvents[DeferredCount].Type = 1;
                DeferredEvents[DeferredCount].Message = text;
                DeferredEvents[DeferredCount].Position = pos;
                DeferredEvents[DeferredCount].Color = c;
                DeferredCount++;
            }

            public static void DeferRectangle(Rectangle r, Color c, string label, Color labelCol)
            {
                DeferRectangle(r, c);
                DeferStringMessage(label, r.Location.ToVector2() + new Vector2(messagePadding, messagePadding), labelCol);
            }
            public static void DeferRectangle(Rectangle r, Color c)
            {
                Check();
                DeferredEvents[DeferredCount].Type = 2;
                DeferredEvents[DeferredCount].Rect = r;
                DeferredEvents[DeferredCount].Color = c;
                DeferredCount++;
            }

            public static void DeferHollowRectangle(Rectangle r, Color c, string label, Color labelCol, int thickness = 1)
            {
                DeferHollowRectangle(r, c, thickness);
                DeferStringMessage(label, r.Location.ToVector2() + new Vector2(messagePadding, messagePadding), labelCol);
            }
            public static void DeferHollowRectangle(Rectangle r, Color c, int thickness = 1)
            {
                Check();
                DeferredEvents[DeferredCount].Type = 3;
                DeferredEvents[DeferredCount].Rect = r;
                DeferredEvents[DeferredCount].Color = c;
                DeferredEvents[DeferredCount].Size = thickness;
                DeferredCount++;
            }

            public static void DeferPixel(Vector2 pos, Color c, int thickness = 1)
            {
                Check();
                DeferredEvents[DeferredCount].Type = 4;
                DeferredEvents[DeferredCount].Position = pos;
                DeferredEvents[DeferredCount].Color = c;
                DeferredEvents[DeferredCount].Size = thickness;
                DeferredCount++;
            }

            public static void DeferCircle(Vector2 pos, int radius, Color c, int thickness = 1)
            {
                Check();
                DeferredEvents[DeferredCount].Type = 5;
                DeferredEvents[DeferredCount].Position = pos;
                DeferredEvents[DeferredCount].Color = c;
                DeferredEvents[DeferredCount].Size = thickness;
                DeferredEvents[DeferredCount].Radius = radius;
                DeferredCount++;
            }

            public static void DeferLine(Vector2 from, Vector2 to, Color c, int thickness = 1)
            {
                Check();
                DeferredEvents[DeferredCount].Type = 5;
                DeferredEvents[DeferredCount].Position = from;
                DeferredEvents[DeferredCount].Color = c;
                DeferredEvents[DeferredCount].Size = thickness;
                DeferredEvents[DeferredCount].End = to;
                DeferredCount++;
            }
        }
    }
}
