using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityTemplate
{
    public interface IGameEvent { }
    
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<(Delegate original, Action<IGameEvent> wrapper)>> _subscribers = new();

        public static void Subscribe<T>(Action<T> callback) where T : IGameEvent
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
                _subscribers[type] = new List<(Delegate, Action<IGameEvent>)>();

            Action<IGameEvent> wrapper = e => callback((T)e);
            _subscribers[type].Add((callback, wrapper));
        }

        public static void Unsubscribe<T>(Action<T> callback) where T : IGameEvent
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type)) return;

            var list = _subscribers[type];
            var record = list.FirstOrDefault(x => x.original.Equals(callback));
            if (record.wrapper != null)
                list.Remove(record);
        }

        public static void Publish<T>(T gameEvent) where T : IGameEvent
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type)) return;

            for (int i = _subscribers[type].Count - 1; i >= 0; i--)
            {
                var (_, wrapper) = _subscribers[type][i];
                wrapper(gameEvent);
            }
        }
    }

    public class TrianglePressedEvent : IGameEvent { }
}