﻿namespace Ash
{
	public class DefaultRenderer : Renderer
	{
		/// <summary>
		/// renders all renderLayers
		/// </summary>
		/// <param name="renderOrder">Render order.</param>
		/// <param name="camera">Camera.</param>
		public DefaultRenderer(int renderOrder = 0, Camera camera = null) : base(renderOrder, camera)
		{
		}

		public override void Render(ECScene scene)
		{
			var cam = Camera ?? scene.Camera;
			BeginRender(cam);

			for (var i = 0; i < scene.RenderableComponents.Count; i++)
			{
				var renderable = scene.RenderableComponents[i];
				if (renderable.Enabled && renderable.IsVisibleFromCamera(cam))
					RenderAfterStateCheck(renderable, cam);
			}

			if (ShouldDebugRender && ECScene.DebugRenderEnabled)
				DebugRender(scene, cam);

			EndRender();
		}
	}
}