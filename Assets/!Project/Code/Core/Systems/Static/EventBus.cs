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

        /// <summary>
        /// Subscribes a callback to an event type. The callback will be invoked when the event is published.
        /// </summary>
        /// <typeparam name="T">The event type to subscribe to.</typeparam>
        /// <param name="callback">The callback to invoke when the event is published.</param>
        public static void Subscribe<T>(Action<T> callback) where T : IGameEvent
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
                _subscribers[type] = new List<(Delegate, Action<IGameEvent>)>();

            Action<IGameEvent> wrapper = e => callback((T)e);
            _subscribers[type].Add((callback, wrapper));
        }

        /// <summary>
        /// Unsubscribes a callback from an event type.
        /// </summary>
        /// <typeparam name="T">The event type to unsubscribe from.</typeparam>
        /// <param name="callback">The callback to remove from the subscriber list.</param>
        public static void Unsubscribe<T>(Action<T> callback) where T : IGameEvent
        {
            var type = typeof(T);
            if (!_subscribers.TryGetValue(type, out List<(Delegate original, Action<IGameEvent> wrapper)> list)) return;

            var record = list.FirstOrDefault(x => x.original.Equals(callback));
            if (record.wrapper != null)
                list.Remove(record);
        }

        /// <summary>
        /// Publishes an event to all subscribers of the event type.
        /// </summary>
        /// <typeparam name="T">The event type to publish.</typeparam>
        /// <param name="gameEvent">The event instance to send to subscribers.</param>
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
}