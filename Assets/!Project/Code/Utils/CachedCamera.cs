using UnityEngine;

namespace UnityTemplate
{
	public static class CachedCamera
	{
		private static Camera _camera;

		public static Camera Main
		{
			get
			{
				if (!_camera)
				{
					_camera = Camera.main;
				}

				return _camera;	
			}
		}
	}
}