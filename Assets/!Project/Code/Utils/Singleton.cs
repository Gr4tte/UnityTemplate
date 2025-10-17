using UnityEngine;

namespace UnityTemplate
{
	public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		public static T Instance { get; private set; }

		protected virtual void Awake()
		{
			if (Instance != null) Destroy(gameObject);
			else Instance = this as T;
		}

		protected virtual void OnApplicationQuit()
		{
			Instance = null;
			Destroy(gameObject);
		}
	}

	public abstract class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		public static T Instance { get; private set; }

		protected virtual void Awake()
		{
			if (Instance != null) Destroy(gameObject);
			else Instance = this as T;
			DontDestroyOnLoad(gameObject);
		}

		protected virtual void OnApplicationQuit()
		{
			Instance = null;
			Destroy(gameObject);
		}
	}
}