using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace UnityTemplate
{
	public partial class AudioSystem
	{
		[SerializeField] private int _poolSize = 50;
		private List<AudioSource> _sourcePool;
		private GameObject _audioSourceContainer;

		private AudioSource _sourceA;
		private AudioSource _sourceB;
		
		private AudioSource _musicSource;
		private AudioSource _fadeSource;

		protected override void Awake()
		{
			_audioSourceContainer = new("AudioSourcePool")
			{
				transform =
				{
					parent = transform
				}
			};

			_sourceA = _audioSourceContainer.AddComponent<AudioSource>();
			_sourceB = _audioSourceContainer.AddComponent<AudioSource>();
			_musicSource = _sourceA;
			_fadeSource = _sourceB;

			_sourcePool = new List<AudioSource>(_poolSize);

			for (int i = 0; i < _poolSize; i++)
			{
				AudioSource src = _audioSourceContainer.AddComponent<AudioSource>();
				src.playOnAwake = false;
				_sourcePool.Add(src);
			}
		}
		
		public void PlayMusic(AudioClip clip, float volume = 1f, float pitch = 1f)
		{
			_musicSource.clip = clip;
			_musicSource.volume = volume;
			_musicSource.pitch = pitch;
			_musicSource.loop = true;
			_musicSource.Play();
		}
		
		public void StopMusic()
		{
			_musicSource.Stop();
			_musicSource.clip = null;
		}
		
		public void CrossfadeMusic(AudioClip clip, float duration = 1f, Ease easing = Ease.InOutSine, float volume = 1f, float pitch = 1f)
		{
			_fadeSource.clip = clip;
			_fadeSource.volume = 0f;
			_fadeSource.pitch = pitch;
			_fadeSource.loop = true;
			_fadeSource.Play();

			_fadeSource.DOKill();
			_musicSource.DOKill();
			
			_fadeSource.DOFade(volume, duration)
				.SetEase(easing);

			_musicSource.DOFade(0f, duration)
				.SetEase(easing)
				.OnComplete(() =>
				{
					_musicSource.Stop();
					_musicSource.clip = null;
					
					(_musicSource, _fadeSource) = (_fadeSource, _musicSource);
				});
		}
		
		public void FadeOutMusic(float duration = 1f, Ease easing = Ease.InOutSine)
		{
			_fadeSource.DOKill();
			_musicSource.DOKill();
			
			_fadeSource.DOFade(0f, duration)
				.SetEase(easing);
			
			_musicSource.DOFade(0f, duration)
				.SetEase(easing)
				.OnComplete(() =>
				{
					_musicSource.Stop();
					_musicSource.clip = null;

					_fadeSource.Stop();
					_fadeSource.clip = null;
				});
		}
		
		public void FadeMusic(float volume, float duration = 1f, Ease easing = Ease.InOutSine)
		{
			_musicSource.DOKill();
			
			_musicSource.DOFade(volume, duration)
				.SetEase(easing);
		}

		public void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f)
		{
			AudioSource src = GetAvailableSource();
			if (!src)
			{
				Debug.LogWarning("No available AudioSource in pool");
				return;
			}

			src.clip = clip;
			src.volume = volume;
			src.pitch = pitch;
			src.Play();
			
			StartCoroutine(ReleaseWhenDone(src));
		}

		private AudioSource GetAvailableSource()
		{
			return _sourcePool.FirstOrDefault(src => !src.isPlaying);
		}

		private static IEnumerator ReleaseWhenDone(AudioSource src)
		{
			yield return new WaitUntil(() => !src.isPlaying);
			src.clip = null;
		}
	}
}