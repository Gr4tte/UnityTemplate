using System;
using UnityEngine;
using UnityEngine.Audio;

namespace UnityTemplate
{
    public partial class AudioSystem : PersistentSingleton<AudioSystem>
    {
        private const string MASTER_VOLUME = "MasterVolume";
        private const string MUSIC_VOLUME = "MusicVolume";
        private const string SFX_VOLUME = "SfxVolume";
        
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private AudioMixerGroup _musicMixerGroup;
        [SerializeField] private AudioMixerGroup _sfxMixerGroup;
        
        private void Start()
        {
            _mixer.SetFloat(MASTER_VOLUME, FloatToDecibel(Settings.MasterVolume));
            _mixer.SetFloat(MUSIC_VOLUME, FloatToDecibel(Settings.MusicVolume));
            _mixer.SetFloat(SFX_VOLUME, FloatToDecibel(Settings.SfxVolume));
        }

        private void OnEnable()
        {
            EventBus.Subscribe<Events.MasterVolumeChanged>(OnMasterVolumeChanged);
            EventBus.Subscribe<Events.MusicVolumeChanged>(OnMusicVolumeChanged);
            EventBus.Subscribe<Events.SfxVolumeChanged>(OnSfxVolumeChanged);
        }
        
        private void OnDisable()
        {
            EventBus.Unsubscribe<Events.MasterVolumeChanged>(OnMasterVolumeChanged);
            EventBus.Unsubscribe<Events.MusicVolumeChanged>(OnMusicVolumeChanged);
            EventBus.Unsubscribe<Events.SfxVolumeChanged>(OnSfxVolumeChanged);
        }

        private void OnMasterVolumeChanged(Events.MasterVolumeChanged e)
        {
            _mixer.SetFloat(MASTER_VOLUME, FloatToDecibel(e.NewVolume));
        }
        
        private void OnMusicVolumeChanged(Events.MusicVolumeChanged e)
        {
            _mixer.SetFloat(MUSIC_VOLUME, FloatToDecibel(e.NewVolume));
        }
        
        private void OnSfxVolumeChanged(Events.SfxVolumeChanged e)
        {
            _mixer.SetFloat(SFX_VOLUME, FloatToDecibel(e.NewVolume));
        }
        
        private static float FloatToDecibel(float value)
        {
            return Mathf.Log10(Mathf.Pow(Mathf.Clamp(value, 0.0001f, 1f), 2f)) * 20f;
        }
    }
}