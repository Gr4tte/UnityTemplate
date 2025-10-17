using UnityEngine;

namespace UnityTemplate
{
	public class MasterVolumeChangedEvent : IGameEvent
	{
		public float NewVolume { get; }
		public MasterVolumeChangedEvent(float newVolume) => NewVolume = newVolume;
	}
	
	public class MusicVolumeChangedEvent : IGameEvent
	{
		public float NewVolume { get; }
		public MusicVolumeChangedEvent(float newVolume) => NewVolume = newVolume;
	}
	
	public class SfxVolumeChangedEvent : IGameEvent
	{
		public float NewVolume { get; }
		public SfxVolumeChangedEvent(float newVolume) => NewVolume = newVolume;
	}
}