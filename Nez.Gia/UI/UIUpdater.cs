using DefaultEcs;
using DefaultEcs.System;

namespace Nez.UI
{
    [With(typeof(UserInterface))]
    [With(typeof(AABB))]
    public class UIUpdater : AEntitySystem<GiaScene>
    {
        public UIUpdater(World world) : base(world) { }

        protected override void Update(GiaScene state, in Entity entity)
        {
            base.Update(state, entity);
            ref var ui = ref entity.Get<UserInterface>();
            ref var bounds = ref entity.Get<AABB>();

            ui.ForwardedMousePosition = ui.IsScreenSpace ? state.MousePosition - bounds.Bounds.Location : state.WorldMousePosition - bounds.Bounds.Location;

            if (ui.HoverLocked == false)
            {
                var old_focus = ui.GetHover();
                ui.SetHover(null);
                if (old_focus != null && old_focus.Interactivity != null)
                {
                    old_focus.Interactivity.Update(Time.UnscaledDeltaTime);
                }
                ui.Root.DetermineDeepestHover(ui.ForwardedMousePosition);
            }

            if (ui.Hover != null && ui.Hover.Interactivity != null)
            {
                ui.Hover.Interactivity.Update(Time.UnscaledDeltaTime);
            }
        }
    }
}
