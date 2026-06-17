
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UnityTemplate
{
    public static class SceneCollectionDatabase
    {
        public static SceneCollection[] AllCollections = new SceneCollection[]
        {
            AssetDatabase.LoadAssetAtPath<SceneCollection>("Assets/!Demo/Scriptable Objects/DemoMenu.asset"),
            AssetDatabase.LoadAssetAtPath<SceneCollection>("Assets/!Demo/Scriptable Objects/DemoWave1.asset"),
            AssetDatabase.LoadAssetAtPath<SceneCollection>("Assets/!Demo/Scriptable Objects/DemoWave2.asset"),
            AssetDatabase.LoadAssetAtPath<SceneCollection>("Assets/!Project/Design/!Scenes/!Scene Collections/StartingScenes.asset")
        };
    }
}
#endif
