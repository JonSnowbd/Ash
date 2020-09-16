using DefaultEcs;
using DefaultEcs.System;
using Ash.UIComponents;

namespace Ash.UI
{
    /// <summary>
    /// Loops through all entities of AABB and UserInterface. Sets up and updates
    /// various UI input phases including hovers, focus inputs and clearing focus.
    /// </summary>
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

            // If the hover isnt locked, we gotta find what its currently hovering.
            if (ui.HoverLocked == false)
            {

                // Send a goodbye interactivity update
                var old_focus = ui.GetHover();
                ui.SetHover(null);
                if (old_focus != null && old_focus.Interactivity != null)
                    old_focus.Interactivity.Update(Time.UnscaledDeltaTime);

                // Find the next hover with recursion
                ui.Root.DetermineDeepestHover(ui.ForwardedMousePosition);

                // Unfocus logic.
                if(ui.GetHover() == ui.Root && ui.IsClickPressed)
                {
                    if (ui.DisruptFocusOnExternalClicks)
                        ui.SetFocus(null);
                    else if (bounds.Bounds.Contains(ui.ForwardedMousePosition))
                        ui.SetFocus(null);
                }
            }

            if (ui.Hover != null && ui.Hover.Interactivity != null)
                ui.Hover.Interactivity.Update(Time.UnscaledDeltaTime);

            if(ui.GetFocus() is UIComponent uic)
                uic.FocusUpdate();
        }
    }
}
