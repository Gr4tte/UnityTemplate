using UnityEngine;

namespace UnityTemplate
{
	public static class CachedCamera
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
}