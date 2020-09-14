using DefaultEcs;
using Microsoft.Xna.Framework;
using Nez.UIComponents;

namespace Nez.UI
{
    public class UIRenderSystem : RenderSystem
    {

        public UIRenderSystem(World world, bool screenSpace) : base(world, screenSpace, typeof(AABB), typeof(UICluster))
        {

        }

        protected override void DebugDraw(GiaScene context, Batcher batcher, Entity entity, ref AABB bounds)
        {
            ref var cluster = ref entity.Get<UICluster>();
            if (IsScreenSpace)
            {
                Gia.Debug.DeferStringMessage($"{cluster.ForwardedMousePosition}", bounds.Bounds.Location - new Vector2(0, 10), Color.White, true);
            }
            else
            {
                var screenPos = context.View.WorldToScreenPoint(bounds.Bounds.Location);
                var nom = cluster.Focus != null ? cluster.Focus.ToString() : "N/A";
                Gia.Debug.DeferStringMessage($"{nom}", screenPos - new Vector2(0, 28), Color.White, true);
                Gia.Debug.DeferStringMessage($"{cluster.MousePosition}", screenPos - new Vector2(0, 15), Color.White, true);
            }
        }

        protected override void Draw(GiaScene context, Batcher batcher, Entity entity, ref AABB bounds)
        {
            ref var cluster = ref entity.Get<UICluster>();
            cluster.Root.PerformLayout();

            cluster.ForwardedMousePosition = IsScreenSpace ? context.MousePosition : context.WorldMousePosition-bounds.Bounds.Location;

            if (cluster.FocusLocked == false)
            {
                var old_focus = cluster.GetFocus();
                cluster.SetFocus(null);
                if(old_focus != null && old_focus.Interactivity != null)
                {
                    old_focus.Interactivity.Update(Time.UnscaledDeltaTime);
                }
                cluster.Root.PropagateMouseHover(cluster.ForwardedMousePosition);
            }

            if (cluster.Focus != null && cluster.Focus.Interactivity != null)
            {
                cluster.Focus.Interactivity.Update(Time.UnscaledDeltaTime);
            }

            DrawElement(context, batcher, ref bounds, cluster.Root);
        }

        protected void DrawElement(GiaScene context, Batcher batcher, ref AABB bounds, UIComponent component)
        {
            component.DrawMethod(batcher, new Rectangle(bounds.Bounds.Location.ToPoint()+component.Compute.Position.ToPoint(), component.Compute.Size.ToPoint()));
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
