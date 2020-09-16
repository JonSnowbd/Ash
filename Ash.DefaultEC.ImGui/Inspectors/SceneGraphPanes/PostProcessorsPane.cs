using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using Ash.ImGuiTools.ObjectInspectors;


namespace Ash.ImGuiTools.SceneGraphPanes
{
	/// <summary>
	/// manages displaying the current PostProcessors in the Scene and provides a means to add PostProcessors
	/// </summary>
	public class PostProcessorsPane
	{
		List<PostProcessorInspector> _postProcessorInspectors = new List<PostProcessorInspector>();
		bool _isPostProcessorListInitialized;

		// Used to hold a reference to the Cast scene, if it derives from `Scene`
		ECScene CastScene;

		void UpdatePostProcessorInspectorList()
		{
			// first, we check our list of inspectors and sync it up with the current list of PostProcessors in the Scene.
			// we limit the check to once every 60 fames
			if (!_isPostProcessorListInitialized || Time.FrameCount % 60 == 0)
			{
				_isPostProcessorListInitialized = true;
				for (var i = 0; i < CastScene._postProcessors.Length; i++)
				{
					var postProcessor = CastScene._postProcessors.Buffer[i];
					if (_postProcessorInspectors.Where(inspector => inspector.PostProcessor == postProcessor).Count() ==
					    0)
						_postProcessorInspectors.Add(new PostProcessorInspector(postProcessor));
				}
			}
		}

		public void OnSceneChanged()
		{
			CastScene = Core.Scene as ECScene;
			if (CastScene == null)
				throw new InvalidOperationException("You cannot use Post Processors if you are not inheriting the base Scene implementation.");

			_postProcessorInspectors.Clear();
			_isPostProcessorListInitialized = false;
			UpdatePostProcessorInspectorList();
		}

		public void Draw()
		{
			UpdatePostProcessorInspectorList();

			ImGui.Indent();
			for (var i = 0; i < _postProcessorInspectors.Count; i++)
			{
				if (_postProcessorInspectors[i].PostProcessor._scene != null)
				{
					_postProcessorInspectors[i].Draw();
					NezImGui.SmallVerticalSpace();
				}
			}

			if (_postProcessorInspectors.Count == 0)
				NezImGui.SmallVerticalSpace();

			if (NezImGui.CenteredButton("Add PostProcessor", 0.6f))
			{
				ImGui.OpenPopup("postprocessor-selector");
			}

			ImGui.Unindent();

			NezImGui.MediumVerticalSpace();
			DrawPostProcessorSelectorPopup();
		}

		void DrawPostProcessorSelectorPopup()
		{
			if (ImGui.BeginPopup("postprocessor-selector"))
			{
				foreach (var subclassType in InspectorCache.GetAllPostProcessorSubclassTypes())
				{
					if (ImGui.Selectable(subclassType.Name))
					{
						var postprocessor = (PostProcessor) Activator.CreateInstance(subclassType,
							new object[] {_postProcessorInspectors.Count});
						CastScene.AddPostProcessor(postprocessor);
						_isPostProcessorListInitialized = false;
					}
				}

				ImGui.EndPopup();
			}
		}
	}
}