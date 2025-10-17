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
        
        private float _masterVolumeBase;
        private float _musicVolumeBase;
        private float _sfxVolumeBase;
        
        //for lerping volumes but not yet implemented
        // private float _masterVolumeModifier = 1f; 
        // private float _musicVolumeModifier = 1f;
        // private float _sfxVolumeModifier = 1f;
        
        private void OnEnable()
        {
            EventBus.Subscribe<MasterVolumeChangedEvent>(OnMasterVolumeChanged);
            EventBus.Subscribe<MusicVolumeChangedEvent>(OnMusicVolumeChanged);
            EventBus.Subscribe<SfxVolumeChangedEvent>(OnSfxVolumeChanged);
        }
        
        private void OnDisable()
        {
            EventBus.Unsubscribe<MasterVolumeChangedEvent>(OnMasterVolumeChanged);
            EventBus.Unsubscribe<MusicVolumeChangedEvent>(OnMusicVolumeChanged);
            EventBus.Unsubscribe<SfxVolumeChangedEvent>(OnSfxVolumeChanged);
        }

        private void OnMasterVolumeChanged(MasterVolumeChangedEvent e)
        {
            _masterVolumeBase = FloatToDecibel(e.NewVolume);
            _mixer.SetFloat(MASTER_VOLUME, _masterVolumeBase);
        }
        
        private void OnMusicVolumeChanged(MusicVolumeChangedEvent e)
        {
            _musicVolumeBase = FloatToDecibel(e.NewVolume);
            _mixer.SetFloat(MUSIC_VOLUME, _musicVolumeBase);
        }
        
        private void OnSfxVolumeChanged(SfxVolumeChangedEvent e)
        {
            _sfxVolumeBase = FloatToDecibel(e.NewVolume);
            _mixer.SetFloat(SFX_VOLUME, _sfxVolumeBase);
        }
        
        private static float FloatToDecibel(float value)
        {
            return Mathf.Log10(Mathf.Pow(Mathf.Clamp(value, 0.0001f, 1f), 2f)) * 20f;
        }
    }
}