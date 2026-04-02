namespace UnityTemplate.Events
{
	public class MasterVolumeChanged : IGameEvent
	{
		public float NewVolume { get; }
		public MasterVolumeChanged(float newVolume) => NewVolume = newVolume;
	}
	
	public class MusicVolumeChanged : IGameEvent
	{
		public float NewVolume { get; }
		public MusicVolumeChanged(float newVolume) => NewVolume = newVolume;
	}
	
	public class SfxVolumeChanged : IGameEvent
	{
		public float NewVolume { get; }
		public SfxVolumeChanged(float newVolume) => NewVolume = newVolume;
	}
}