using UnityEngine;

public static class SystemInitializer
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void Initialize() => Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Systems")));
}
