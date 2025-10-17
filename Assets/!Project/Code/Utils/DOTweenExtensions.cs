using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace UnityTemplate
{
	public static class DOTweenExtensions
	{
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveInTargetLocalSpace(
			this Transform transform,
			Transform target,
			Vector3 targetLocalEndPosition,
			float duration)
		{
			var t = DOTween.To(
				() => transform.position - target.position,
				x => transform.position = x + target.position,
				targetLocalEndPosition,
				duration
			);

			t.SetLink(target.gameObject); // Automatically kills the tween if target is destroyed
			return t;
		}
	}
}