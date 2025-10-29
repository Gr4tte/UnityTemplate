using System.Threading.Tasks;
using UnityEngine;

namespace UnityTemplate
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField, Scene] private string _persistentScene;
        #if ODIN_INSPECTOR
        [SerializeField, Sirenix.OdinInspector.InlineEditor(Sirenix.OdinInspector.InlineEditorObjectFieldModes.Foldout)] private SceneCollection _sceneCollection;
        #else
        [SerializeField] private SceneCollection _sceneCollection;
        #endif
        
        private void Start()
        {
            SceneSystem.LoadCollection(_sceneCollection, null, _persistentScene);
        }
    }
}