using UnityEngine;

namespace UnityTemplate
{
    public static class Settings
    {
        private const string MASTER_VOLUME = "MasterVolume";
        private const string MUSIC_VOLUME = "MusicVolume";
        private const string SFX_VOLUME = "SfxVolume";
        
        public static void Save() => PlayerPrefs.Save();
        
        public static float MasterVolume
        {
            get => PlayerPrefs.GetFloat(MASTER_VOLUME, 1f);
            set
            {
                PlayerPrefs.SetFloat(MASTER_VOLUME, value);
                EventBus.Publish(new MasterVolumeChangedEvent(value));
            }
        }

        public static float MusicVolume
        {
            get => PlayerPrefs.GetFloat(MUSIC_VOLUME, 0.8f);
            set
            {
                PlayerPrefs.SetFloat(MUSIC_VOLUME, value);
                EventBus.Publish(new MusicVolumeChangedEvent(value));
            }
        }

        public static float SfxVolume
        {
            get => PlayerPrefs.GetFloat(SFX_VOLUME, 0.8f);
            set
            {
                PlayerPrefs.SetFloat(SFX_VOLUME, value);
                EventBus.Publish(new SfxVolumeChangedEvent(value));
            }
        }
    }
}