using Microsoft.Xna.Framework;

namespace Coga
{
	/// <summary>
	/// The interface required to act as a house for the root of a coga node.
	/// </summary>
	public interface ICogaManager
	{
		/// <summary>
		/// Locks the currently hovered node, making sure it is no longer
		/// cleared or changed.
		/// </summary>
		void LockFocus();
		/// <summary>
		/// Unlocks the hover, letting it get cleared and changed per frame.
		/// </summary>
		void UnlockFocus();
		/// <summary>
		/// Sets the currently hovered Coga Node.
		/// </summary>
		void SetFocus(CogaNode node);

		/// <summary>
		/// Returns the currently hovered Coga Node.
		/// </summary>
		CogaNode GetFocus();
		/// <summary>
		/// Returns the Coga Node being used as the root of everything.
		/// </summary>
		CogaNode GetRoot();

        /// <summary>
        /// Gets whether the click button is currently down
        /// </summary>
		bool IsClickDown { get; }
        /// <summary>
        /// Gets whether the click input is currently down, where it was up last frame.
        /// </summary>
        bool IsClickPressed { get; }
        /// <summary>
        /// Gets whether the click input is currently up, where it was down last frame.
        /// </summary>
        bool IsClickReleased { get; }
        /// <summary>
        /// Returns the current mouse position, taking into account world space vs screen space.
        /// </summary>
        Vector2 MousePosition { get; }
	}
}
