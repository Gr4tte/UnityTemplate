using UnityEngine;

namespace UnityTemplate
{
    public static class GameObjectExtensions
    {
        // Taken from Unity Utils by Git Amend
        // https://github.com/adammyhre/Unity-Utils
        //--------------------------------------------------
        
        
        /// <summary>
        /// Gets a component of the given type attached to the GameObject. If that type of component does not exist, it adds one.
        /// </summary>
        /// <remarks>
        /// This method is useful when you don't know if a GameObject has a specific type of component,
        /// but you want to work with that component regardless. Instead of checking and adding the component manually,
        /// you can use this method to do both operations in one line.
        /// </remarks>
        /// <typeparam name="T">The type of the component to get or add.</typeparam>
        /// <param name="gameObject">The GameObject to get the component from or add the component to.</param>
        /// <returns>The existing component of the given type, or a new one if no such component exists.</returns>    
        public static T GetOrAdd<T>(this GameObject gameObject) where T : Component {
            T component = gameObject.GetComponent<T>();
            if (!component) component = gameObject.AddComponent<T>();

            return component;
        }
        
        /// <summary>
        /// Activates the GameObject associated with the MonoBehaviour and returns the instance.
        /// </summary>
        /// <typeparam name="T">The type of the MonoBehaviour.</typeparam>
        /// <param name="obj">The MonoBehaviour whose GameObject will be activated.</param>
        /// <returns>The instance of the MonoBehaviour.</returns>
        public static T SetActive<T>(this T obj) where T : MonoBehaviour {
            obj.gameObject.SetActive(true);
            return obj;
        }

        /// <summary>
        /// Deactivates the GameObject associated with the MonoBehaviour and returns the instance.
        /// </summary>
        /// <typeparam name="T">The type of the MonoBehaviour.</typeparam>
        /// <param name="obj">The MonoBehaviour whose GameObject will be deactivated.</param>
        /// <returns>The instance of the MonoBehaviour.</returns>
        public static T SetInactive<T>(this T obj) where T : MonoBehaviour {
            obj.gameObject.SetActive(false);
            return obj;
        }
    }
}