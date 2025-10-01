using UnityEngine;

public class CachedCamera : MonoBehaviour
{
	private static Camera _camera;

	public static Camera Get()
	{
		if (!_camera)
		{
			_camera = Camera.main;
		}

		return _camera;
	}
}