#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UnityTemplate
{
	[ExecuteAlways]
	[CreateAssetMenu(fileName = "Scene Collection", menuName = "Scriptable Objects/Scene Collection")]
	public class SceneCollection : ScriptableBase
	{
		[field: SerializeField, Scene] public List<string> Scenes { get; private set; }

		#if UNITY_EDITOR 
		#if ODIN_INSPECTOR
		[Button]
		#endif
		public void OpenScenes()
		{
			if (Scenes == null || Scenes.Count == 0) return;
			if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
			
			EditorSceneManager.OpenScene(Scenes[0], OpenSceneMode.Single);
			for (int i = 1; i < Scenes.Count; i++)
			{
				EditorSceneManager.OpenScene(Scenes[i], OpenSceneMode.Additive);
			}
		}
		#endif
	}
}