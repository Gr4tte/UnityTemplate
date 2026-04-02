using System;
using System.Globalization;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

namespace UnityTemplate
{
	public static class DOTweenExtensions
	{
		/// <summary>
		/// Creates a tween that moves a transform to a target local position relative to another transform's space.
		/// The movement is performed in the local space of the target transform.
		/// </summary>
		/// <param name="transform">The transform to move.</param>
		/// <param name="target">The target transform whose local space is used.</param>
		/// <param name="targetLocalEndPosition">The desired local end position relative to the target transform.</param>
		/// <param name="duration">The duration of the tween.</param>
		/// <returns>A TweenerCore controlling the movement tween.</returns>
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
		
		/// <summary>
		/// Animates the numeric value of a TextMeshProUGUI component from a starting value to an ending value over a specified duration.
		/// </summary>
		/// <param name="text">The TextMeshProUGUI component whose text will be updated.</param>
		/// <param name="fromValue">The starting integer value for the count animation.</param>
		/// <param name="toValue">The ending integer value for the count animation.</param>
		/// <param name="duration">The duration of the tween in seconds.</param>
		/// <param name="format">The numeric format string (default is "#,#").</param>
		/// <param name="separator">The character used as a thousands separator (default is space).</param>
		/// <returns>A DOCountHandle containing the tween and a getter for the current value.</returns>
		public static DOCountHandle DOCount(
			this TextMeshProUGUI text,
			int fromValue,
			int toValue,
			float duration,
			string format = "#,#",
			char separator = ' ')
		{
			int current = fromValue;

			var tween = DOTween.To(
				() => current,
				x =>
				{
					current = x;
					text.text = current.ToString(format, CultureInfo.InvariantCulture).Replace(',', separator);
				},
				toValue,
				duration
			);

			return new DOCountHandle(tween, () => current);
		}
		
		public class DOCountHandle
		{
			public Tweener Tween { get; }
			public int CurrentValue => _getter();
			private readonly Func<int> _getter;

			internal DOCountHandle(Tweener tween, Func<int> getter)
			{
				Tween = tween;
				_getter = getter;
			}
		}
	}
}