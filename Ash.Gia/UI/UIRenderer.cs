using DefaultEcs;
using Microsoft.Xna.Framework;
using Ash.UIComponents;

namespace Ash.UI
{
    public class UIRenderer : RenderSystem
    {

        public UIRenderer(World world, bool screenSpace) : base(world, screenSpace, typeof(AABB), typeof(UserInterface))
        {

        }

        protected override void DebugDraw(GiaScene context, Batcher batcher, Entity entity, ref AABB bounds)
        {
            ref var cluster = ref entity.Get<UserInterface>();

            if (!cluster.IsScreenSpace)
            {
                var screenPos = context.View.WorldToScreenPoint(bounds.Bounds.Location);
                var nom = cluster.Hover != null ? cluster.Hover.ToString() : "N/A";
                Gia.Debug.DeferStringMessage($"{nom}", screenPos - new Vector2(0, 28), Color.White, true);
                Gia.Debug.DeferStringMessage($"{cluster.MousePosition}", screenPos - new Vector2(0, 15), Color.White, true);
                Gia.Debug.DeferWorldHollowRect(bounds.Bounds, Gia.Theme.SecondaryThemeColor, 1);
                Gia.Debug.DeferWorldPixel(bounds.Bounds.Location, Gia.Theme.SecondaryThemeColor, 4);
            }
            else
            {
                Gia.Debug.DeferHollowRectangle(bounds.Bounds, Gia.Theme.SecondaryThemeColor);
                Gia.Debug.DeferPixel(bounds.Bounds.Location, Gia.Theme.SecondaryThemeColor, 4);
            }
        }

        protected override void Draw(GiaScene context, Batcher batcher, Entity entity, ref AABB bounds)
        {
            ref var ui = ref entity.Get<UserInterface>();
            ui.IsScreenSpace = IsScreenSpace;
            ui.Root.PerformLayout();
            DrawElement(context, batcher, ref bounds, ui.Root);
        }

        protected void DrawElement(GiaScene context, Batcher batcher, ref AABB bounds, UIComponent component)
        {
            var finalBounds = new RectangleF(bounds.Bounds.Location + component.Compute.Position, component.Compute.Size);
            if (Gia.Debug.Enabled && !IsScreenSpace)
            {
                component.DebugUpdateValue = Mathf.Lerp(component.DebugUpdateValue, 0f, 10 * Time.UnscaledDeltaTime);
                var col = Color.Lerp(Gia.Theme.HighlightColor, Gia.Theme.SecondaryThemeColor, component.DebugUpdateValue);
                Gia.Debug.DeferWorldHollowRect(finalBounds, col);
                Gia.Debug.DeferWorldPixel(finalBounds.Location, col, 3);
            }
            component.DrawMethod(batcher, finalBounds);
            foreach(var child in component.Children)
            {
                if(child is UIComponent uic)
                {
                    DrawElement(context, batcher, ref bounds, uic);
                }
            }
        }
    }
}
