using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityTemplate
{
    public class SceneSystem : PersistentSingleton<SceneSystem>
    {
        public void LoadScene(string scene)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            loadOperation.completed += _ =>
            {
                int sceneCount = SceneManager.sceneCount;

                for (int i = sceneCount - 1; i >= 0; i--)
                {
                    if (SceneManager.GetSceneAt(i).name == scene)
                    {
                        SceneManager.SetActiveScene(SceneManager.GetSceneAt(i));
                        continue;
                    }

                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
                }
            };
        }
        
        public void LoadSceneAdditively(string scene)
        {
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        }
        
        public void UnloadScene(string scene)
        {
            SceneManager.UnloadSceneAsync(scene);
        }
    }
}