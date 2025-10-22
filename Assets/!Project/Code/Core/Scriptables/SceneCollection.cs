using System.Collections.Generic;
using UnityEngine;

namespace UnityTemplate
{
	[CreateAssetMenu(fileName = "SceneCollection", menuName = "Scriptable Objects/Scene Collection")]
	public class SceneCollection : ScriptableBase
	{
		[field: SerializeField, Scene] public List<string> Scenes { get; private set; }
	}
}