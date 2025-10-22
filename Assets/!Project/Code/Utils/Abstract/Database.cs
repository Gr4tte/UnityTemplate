using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace UnityTemplate
{
	public abstract class Database<TKey, TValue> : ScriptableObject
	{
		[System.Serializable]
		public struct Entry
		{
			public TKey Key;
			public TValue Value;
		}

		[SerializeField] private Entry[] _entries;

		private Dictionary<TKey, TValue> _dictionary;

		protected virtual void OnEnable()
		{
			BuildDictionary();
		}

		private void BuildDictionary()
		{
			_dictionary = new Dictionary<TKey, TValue>();

			if (_entries == null) return;

			foreach (Entry entry in _entries.Where(entry => entry.Key != null))
			{
				_dictionary[entry.Key] = entry.Value;
			}
		}
		
		public TValue Get(TKey key)
		{
			if (_dictionary == null)
				BuildDictionary();

			_dictionary.TryGetValue(key, out var value);
			return value;
		}
		
		public bool TryGet(TKey key, out TValue value)
		{
			if (_dictionary == null)
				BuildDictionary();

			return _dictionary.TryGetValue(key, out value);
		}
	}
}