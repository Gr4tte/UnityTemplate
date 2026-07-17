#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace UnityTemplate
{
    public class MainToolbar
    {
		private const float MIN_TIMESCALE = 0f;
		private const float MAX_TIMESCALE = 5f;
		
		private static bool playFromBootstrap
		{
			get => EditorPrefs.HasKey("Edit/Always play from Bootstrap scene &p") && EditorPrefs.GetBool("Edit/Always play from Bootstrap scene &p");
			set => EditorPrefs.SetBool("Edit/Always play from Bootstrap scene &p", value);
		}
		
		[MainToolbarElement("Custom/Play From Bootstrap", defaultDockPosition = MainToolbarDockPosition.Middle)]
		public static MainToolbarElement BootstrapToggle()
		{
			var icon = EditorGUIUtility.IconContent("SceneAsset Icon").image as Texture2D;
			var content = new MainToolbarContent("Bootstrap", icon, "Play from the bootstrap scene");
			
			return new MainToolbarToggle(content, playFromBootstrap, OnBootstrapToggleChanged);
		}
		
		[MainToolbarElement("Custom/Timescale/Slider", defaultDockPosition = MainToolbarDockPosition.Middle)]
		public static MainToolbarElement TimescaleSlider()
		{
			var content = new MainToolbarContent("Time Scale", "Time Scale");
			var slider = new MainToolbarSlider(content, Time.timeScale, MIN_TIMESCALE, MAX_TIMESCALE, OnTimeScaleValueChanged);
			
			slider.populateContextMenu = (menu) =>
			{
				menu.AppendAction("Reset", _ => RefreshTimescaleSlider());
			};

			return slider;
		}
		
		[MainToolbarElement("Custom/Timescale/Reset Button", defaultDockPosition = MainToolbarDockPosition.Middle)]
		public static MainToolbarElement TimeScaleResetButton()
		{
			var icon = EditorGUIUtility.IconContent("Refresh").image as Texture2D;
			var content = new MainToolbarContent(icon);
			
			return new MainToolbarButton(content, RefreshTimescaleSlider);
		}
		
		[MainToolbarElement("Custom/Scene Collection Dropdown", defaultDockPosition = MainToolbarDockPosition.Left)]
		public static MainToolbarElement SceneCollectionDopDown()
		{
			var icon = EditorGUIUtility.IconContent("SceneAsset Icon").image as Texture2D;
			var content = new MainToolbarContent("Collection", icon, "Scene Collection Dropdown");
			
			var dropdown = new MainToolbarDropdown(content, (rect) =>
			{
				var menu = new GenericMenu();
				if (SceneCollectionDatabase.AllCollections == null || SceneCollectionDatabase.AllCollections.Length == 0)
				{
					menu.AddDisabledItem(new GUIContent("No Scene Collections in Project"));
				}
				foreach (var collection in SceneCollectionDatabase.AllCollections)
				{
					menu.AddItem(new GUIContent(collection.name), false, () =>
					{
						collection.OpenScenes();
					});

				}

				menu.DropDown(rect);
			});

			return dropdown;
		}
		
		private static void RefreshTimescaleSlider()
		{
			Time.timeScale = 1f;
			UnityEditor.Toolbars.MainToolbar.Refresh("Custom/Timescale/Slider");
		}
		
		private static void OnBootstrapToggleChanged(bool isOn)
		{
			playFromBootstrap = isOn;
		}
		
		private static void OnTimeScaleValueChanged(float newValue)
		{
			Time.timeScale = newValue;
		}
	}
}
#endif