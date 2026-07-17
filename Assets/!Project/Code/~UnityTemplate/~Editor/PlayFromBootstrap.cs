#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityUtils;
using Object = UnityEngine.Object;

namespace UnityTemplate
{
	// Class by YinXiaozhou from https://discussions.unity.com/t/editor-script-to-make-play-button-always-jump-to-a-start-scene/68990/3
	public static class PlayFromBootstrap
	{      
		const string playFromBootstrapMenuStr = "Edit/Always play from Bootstrap scene &p";
		const string loadPersistentMenuStr = "Edit/Always load Persistent scene &p";

		static bool playFromBootstrapScene
		{
			get{return EditorPrefs.HasKey(playFromBootstrapMenuStr) && EditorPrefs.GetBool(playFromBootstrapMenuStr);}
			set{EditorPrefs.SetBool(playFromBootstrapMenuStr, value);}
		}
		
		static bool loadPersistentScene
		{
			get{return EditorPrefs.HasKey(loadPersistentMenuStr) && EditorPrefs.GetBool(loadPersistentMenuStr);}
			set{EditorPrefs.SetBool(loadPersistentMenuStr, value);}
		}

		[MenuItem(playFromBootstrapMenuStr, false, 150)]
		static void PlayFromFirstSceneCheckMenu()
		{
			playFromBootstrapScene = !playFromBootstrapScene;
			Menu.SetChecked(playFromBootstrapMenuStr, playFromBootstrapScene);

			ShowNotifyOrLog(playFromBootstrapScene ? "Play from Bootstrap" : "Play from current scene");
		}
		
		[MenuItem(loadPersistentMenuStr, false, 151)]
		static void LoadPersistentSceneCheckMenu()
		{
			loadPersistentScene = !loadPersistentScene;
			Menu.SetChecked(loadPersistentMenuStr, loadPersistentScene);

			ShowNotifyOrLog(loadPersistentScene ? "Load Persistent scene" : "Don't load persistent scene");
		}
		
		[MenuItem(playFromBootstrapMenuStr, true)]
		static bool PlayFromFirstSceneCheckMenuValidate()
		{
			Menu.SetChecked(playFromBootstrapMenuStr, playFromBootstrapScene);
			return true;
		}
		
		[MenuItem(loadPersistentMenuStr, true)]
		static bool LoadPersistentSceneCheckMenuValidate()
		{
			Menu.SetChecked(loadPersistentMenuStr, loadPersistentScene);
			return true;
		}
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] 
		static void LoadFirstSceneAtGameBegins()
		{
			if (playFromBootstrapScene)
			{
				LoadBootstrap();
			}
			else if (loadPersistentScene)
			{
				LoadPersistent();
			}
		}

		private static void LoadPersistent()
		{
			int persistentBuildIndex = SceneUtility.GetBuildIndexByScenePath("Persistent");
			int bootstraoBuildIndex = SceneUtility.GetBuildIndexByScenePath("Bootstrap");
			if (persistentBuildIndex == -1)
			{
				Debug.LogWarning("Persistent scene is not in the build list. Can't load Persistent scene. \r\n Make sure the Persistent scene is added to the build list and that the scene name is correct.");
				return;
			}
			else if (bootstraoBuildIndex == -1)
			{
				Debug.LogWarning("Bootstrap scene is not in the build list. Can't load Bootstrap scene. \r\n Make sure the Bootstrap scene is added to the build list and that the scene name is correct.");
				return;
			}
				
			foreach (GameObject go in Object.FindObjectsByType<GameObject>(
						 FindObjectsInactive.Include,
						 FindObjectsSortMode.None))
			{
				go.SetActive(false);
			}

			SceneCollection collection = ScriptableObject.CreateInstance<SceneCollection>();
			
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene scene = SceneManager.GetSceneAt(i);
				collection.AddScene(scene.path);
			}
			
			SceneManager.LoadScene("Bootstrap", LoadSceneMode.Single);
			SceneSystem.LoadCollection(collection, persistentScene: SceneUtility.GetScenePathByBuildIndex(persistentBuildIndex));
		}
		
		private static void LoadBootstrap()
		{
			int buildIndex = SceneUtility.GetBuildIndexByScenePath("Bootstrap");
			if (buildIndex == -1)
			{
				Debug.LogWarning("Bootstrap scene is not in the build list. Can't load Bootstrap scene. \r\n Make sure the Bootstrap scene is added to the build list and that the scene name is correct.");
				return;
			}

			foreach (GameObject go in Object.FindObjectsByType<GameObject>(
						 FindObjectsInactive.Include,
						 FindObjectsSortMode.None))
			{
				go.SetActive(false);
			}
			
			SceneManager.LoadScene("Bootstrap", LoadSceneMode.Single);
		}

		static void ShowNotifyOrLog(string msg)
		{
			if(Resources.FindObjectsOfTypeAll<SceneView>().Length > 0)
				EditorWindow.GetWindow<SceneView>().ShowNotification(new GUIContent(msg));
			else
				Debug.Log(msg);
		}
	}
}
#endif