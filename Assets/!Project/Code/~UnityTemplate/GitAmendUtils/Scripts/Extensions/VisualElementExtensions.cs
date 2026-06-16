using UnityEngine;
using UnityEngine.UIElements;

namespace UnityUtils {
    public static class VisualElementExtensions {
        /// <summary>
        /// Creates a new child VisualElement and adds it to the parent.
        /// </summary>
        /// <param name="parent">The parent VisualElement to add the child to.</param>
        /// <param name="classes">The CSS classes to add to the child.</param>
        /// <returns>The created child VisualElement.</returns>
        public static VisualElement CreateChild(this VisualElement parent, params string[] classes) {
            var child = new VisualElement();
            child.AddClass(classes).AddTo(parent);
            return child;
        }

        /// <summary>
        /// Creates a new child of type T and adds it to the parent.
        /// </summary>
        /// <typeparam name="T">The type of the child VisualElement.</typeparam>
        /// <param name="parent">The parent VisualElement to add the child to.</param>
        /// <param name="classes">The CSS classes to add to the child.</param>
        /// <returns>The created child VisualElement of type T.</returns>
        public static T CreateChild<T>(this VisualElement parent, params string[] classes)
            where T : VisualElement, new() {
            var child = new T();
            child.AddClass(classes).AddTo(parent);
            return child;
        }

        /// <summary>
        /// Adds the child VisualElement to the parent and returns the child.
        /// </summary>
        /// <typeparam name="T">The type of the child VisualElement.</typeparam>
        /// <param name="child">The child VisualElement to add.</param>
        /// <param name="parent">The parent VisualElement to add the child to.</param>
        /// <returns>The added child VisualElement.</returns>
        public static T AddTo<T>(this T child, VisualElement parent) where T : VisualElement {
            parent.Add(child);
            return child;
        }
        
        /// <remarks>
        /// See <see cref="AddTo{T}(T, VisualElement)"/> for adding a child to a parent.
        /// </remarks>
        public static void RemoveFrom<T>(this T child, VisualElement parent)
            where T : VisualElement => parent.Remove(child);

        /// <summary>
        /// Adds the specified CSS classes to the VisualElement.
        /// </summary>
        /// <typeparam name="T">The type of the VisualElement.</typeparam>
        /// <param name="visualElement">The VisualElement to add the classes to.</param>
        /// <param name="classes">The CSS classes to add.</param>
        /// <returns>The VisualElement with the added classes.</returns>
        public static T AddClass<T>(this T visualElement, params string[] classes) where T : VisualElement {
            foreach (string cls in classes) {
                if (!string.IsNullOrEmpty(cls)) {
                    visualElement.AddToClassList(cls);
                }
            }
            return visualElement;
        }
        
        /// <remarks>
        /// See <see cref="AddClass{T}(T, string[])"/> for adding classes.
        /// </remarks>
        public static void RemoveClass<T>(this T visualElement, params string[] classes) where T : VisualElement {
            foreach (string cls in classes) {
                if (!string.IsNullOrEmpty(cls)) {
                    visualElement.RemoveFromClassList(cls);
                }
            }
        }

        /// <summary>
        /// Adds a manipulator to the VisualElement.
        /// </summary>
        /// <typeparam name="T">The type of the VisualElement.</typeparam>
        /// <param name="visualElement">The VisualElement to add the manipulator to.</param>
        /// <param name="manipulator">The manipulator to add.</param>
        /// <returns>The VisualElement with the added manipulator.</returns>
        public static T WithManipulator<T>(this T visualElement, IManipulator manipulator) where T : VisualElement {
            visualElement.AddManipulator(manipulator);
            return visualElement;
        }
        
        /// <summary>
        /// Sets the background image of a VisualElement using a given Sprite.
        /// </summary>
        /// <param name="imageContainer">The VisualElement whose background image will be set.</param>
        /// <param name="sprite">The Sprite to use as the background image.</param>
        public static void SetImageFromSprite(this VisualElement imageContainer, Sprite sprite) {
            var texture = sprite.texture;
            if (texture) {
                imageContainer.style.backgroundImage = new StyleBackground(texture);
            }
        }
        
        /// <summary>
        /// Finds an element recursively using a predicate function.
        /// Unity's Query().Where() only searches descendants, so this method checks the element itself first.
        /// </summary>
        /// <param name="element">The VisualElement to search in (including itself).</param>
        /// <param name="predicate">The predicate function to match elements.</param>
        /// <returns>The found VisualElement, or null if not found.</returns>
        static VisualElement FindElement(this VisualElement element, System.Func<VisualElement, bool> predicate) {
            if (predicate(element)) {
                return element;
            }
            return element.Query<VisualElement>().Where(predicate).First();
        }

        /// <summary>
        /// Finds an element by name recursively.
        /// </summary>
        /// <param name="element">The VisualElement to search in (including itself).</param>
        /// <param name="name">The name of the element to find.</param>
        /// <returns>The found VisualElement, or null if not found.</returns>
        public static VisualElement FindElementByName(this VisualElement element, string name) {
            return element.FindElement(e => e.name == name);
        }

        /// <summary>
        /// Finds an element by tooltip recursively.
        /// Unity's Query API doesn't provide tooltip search, so this uses Query().Where() with a predicate.
        /// </summary>
        /// <param name="element">The VisualElement to search in (including itself).</param>
        /// <param name="tooltip">The tooltip text of the element to find.</param>
        /// <returns>The found VisualElement, or null if not found.</returns>
        public static VisualElement FindElementByTooltip(this VisualElement element, string tooltip) {
            return element.FindElement(e => e.tooltip == tooltip);
        }

        /// <summary>
        /// Finds an element by CSS class recursively.
        /// </summary>
        /// <param name="element">The VisualElement to search in (including itself).</param>
        /// <param name="className">The CSS class name to find.</param>
        /// <param name="filter">Optional filter function to further refine the search.</param>
        /// <returns>The found VisualElement, or null if not found.</returns>
        public static VisualElement FindElementByClass(this VisualElement element, string className, System.Func<VisualElement, bool> filter = null) {
            return element.FindElement(e => e.ClassListContains(className) && (filter == null || filter(e)));
        }
    }
}