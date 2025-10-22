using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityTemplate
{
    public static class SceneSystem
    {
        private static readonly List<UnloadTask> _syncUnloadTasks = new();
        private static readonly List<UnloadTask> _asyncUnloadTasks = new();

        private static bool _isTransitioning;
        private static string _persistentScene;
        
        public static void RegisterSyncUnloadTask(GameObject gameObject, Func<Task> task)
        {
            UnloadTask unloadTask = new(gameObject.scene.path, task);
            if (!_syncUnloadTasks.Contains(unloadTask))
                _syncUnloadTasks.Add(unloadTask);
        }

        public static void RegisterAsyncUnloadTask(GameObject gameObject, Func<Task> task)
        {
            UnloadTask unloadTask = new(gameObject.scene.path, task);
            if (!_asyncUnloadTasks.Contains(unloadTask))
                _asyncUnloadTasks.Add(unloadTask);
        }

        public static void UnregisterSyncUnloadTask(Func<Task> task) => _syncUnloadTasks.RemoveAll(x => x.Func == task);
        public static void UnregisterAsyncUnloadTask(Func<Task> task) => _asyncUnloadTasks.RemoveAll(x => x.Func == task);
        
        public static async void LoadCollection(SceneCollection collection, string persistentScene = "")
        {
            if (_isTransitioning)
            {
                Debug.LogWarning("Scene transition already in progress.");
                return;
            }

            _isTransitioning = true;

            List<Func<Task>> syncTasks = new();
            List<Func<Task>> asyncTasks = new();
            
            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (collection.Scenes.Contains(scene.path)) continue;
                
                syncTasks.AddRange(_syncUnloadTasks.Where(x => x.Scene == scene.path).Select(x => x.Func));
                asyncTasks.AddRange(_asyncUnloadTasks.Where(x => x.Scene == scene.path).Select(x => x.Func));
                _syncUnloadTasks.RemoveAll(x => x.Scene == scene.path);
                _asyncUnloadTasks.RemoveAll(x => x.Scene == scene.path);
            }
            
            var startedAsyncTasks = asyncTasks.Select(t => t.Invoke()).ToList();
            
            await Task.WhenAll(syncTasks.Select(t => t.Invoke()));

            #region Loading Scenes
            if(!string.IsNullOrEmpty(persistentScene) && !SceneManager.GetSceneByPath(persistentScene).isLoaded)
            {
                _persistentScene = persistentScene;
                await SceneManager.LoadSceneAsync(_persistentScene, LoadSceneMode.Additive);
                SceneManager.SetActiveScene(SceneManager.GetSceneByPath(_persistentScene));
            }
            
            List<string> scenesToLoad = new(collection.Scenes);
            
            sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (!collection.Scenes.Contains(scene.path)) continue;

                scenesToLoad.Remove(scene.path);
            }

            var loadOperations = scenesToLoad.Select(sceneName => SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive)).ToList();

            while (loadOperations.Any(x => !x.isDone))
            {
                await Task.Yield();
            }
            #endregion
            
            EventBus.Publish(new Events.SceneCollectionLoaded(collection));
            
            await Task.WhenAll(startedAsyncTasks);

            #region Unloading Scenes
            var unloadOperations = new List<AsyncOperation>();
            
            sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (collection.Scenes.Contains(scene.path)) continue;
                if (scene.path == _persistentScene) continue;
                
                unloadOperations.Add(SceneManager.UnloadSceneAsync(scene.path));
            }

            while (unloadOperations.Any(op => !op.isDone))
            {
                await Task.Yield();
            }
            #endregion
            
            _isTransitioning = false;
            _isTransitioning = false;
        }
        
        private readonly struct UnloadTask : IEquatable<UnloadTask>
        {
            public UnloadTask(string scene, Func<Task> func)
            {
                Scene = scene;
                Func = func;
            }
            
            public readonly string Scene;
            public readonly Func<Task> Func;
            
            public bool Equals(UnloadTask other)
            {
                return Scene == other.Scene && Equals(Func, other.Func);
            }

            public override bool Equals(object obj)
            {
                return obj is UnloadTask other && Equals(other);
            }
            
            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Scene != null ? Scene.GetHashCode() : 0) * 397) ^ (Func != null ? Func.GetHashCode() : 0);
                }
            }
        }
    }
}