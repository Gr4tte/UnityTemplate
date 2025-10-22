namespace UnityTemplate.Events
{
	public class SceneCollectionLoaded : IGameEvent
	{
		public SceneCollection Collection { get; }
		public SceneCollectionLoaded(SceneCollection collection) => Collection = collection;
	}
}