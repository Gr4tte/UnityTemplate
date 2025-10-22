using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityTemplate
{
	// Class by YinXiaozhou from https://discussions.unity.com/t/editor-script-to-make-play-button-always-jump-to-a-start-scene/68990/3
	public static class PlayFromBootstrap
	{      
		const string playFromFirstMenuStr = "Edit/Always play from Bootstrap scene &p";

		static bool playFromFirstScene
		{
			get{return EditorPrefs.HasKey(playFromFirstMenuStr) && EditorPrefs.GetBool(playFromFirstMenuStr);}
			set{EditorPrefs.SetBool(playFromFirstMenuStr, value);}
		}

		[MenuItem(playFromFirstMenuStr, false, 150)]
		static void PlayFromFirstSceneCheckMenu() 
		{
			playFromFirstScene = !playFromFirstScene;
			Menu.SetChecked(playFromFirstMenuStr, playFromFirstScene);

			ShowNotifyOrLog(playFromFirstScene ? "Play from Bootstrap" : "Play from current scene");
		}

		// The menu won't be gray out, we use this validate method for update check state
		[MenuItem(playFromFirstMenuStr, true)]
		static bool PlayFromFirstSceneCheckMenuValidate()
		{
			Menu.SetChecked(playFromFirstMenuStr, playFromFirstScene);
			return true;
		}

		// This method is called before any Awake. It's the perfect callback for this feature
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] 
		static void LoadFirstSceneAtGameBegins()
		{
			if(!playFromFirstScene)
				return;

			if(EditorBuildSettings.scenes.Length  == 0)
			{
				Debug.LogWarning("The scene build list is empty. Can't play from first scene.");
				return;
			}

			foreach (GameObject go in Object.FindObjectsByType<GameObject>(
						 FindObjectsInactive.Include,
						 FindObjectsSortMode.None))
			{
				go.SetActive(false);
			}
        
			SceneManager.LoadScene(0);
		}

		static void ShowNotifyOrLog(string msg)
		{
			if(Resources.FindObjectsOfTypeAll<SceneView>().Length > 0)
				EditorWindow.GetWindow<SceneView>().ShowNotification(new GUIContent(msg));
			else
				Debug.Log(msg); // When there's no scene view opened, we just print a log
		}
	}
}