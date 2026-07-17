#if UNITY_EDITOR
using System.IO;
using System.Linq;
using UnityEditor;

namespace UnityTemplate
{
    public static class SceneCollectionCodeGenerator
    {
        [InitializeOnLoadMethod]
        static void Init()
        {
            EditorApplication.projectChanged += Generate;
            Generate();
        }

        static void Generate()
        {
            var guids = AssetDatabase.FindAssets("t:SceneCollection");

            var collections = guids
                .Select(g => AssetDatabase.GUIDToAssetPath(g))
                .Select(p => AssetDatabase.LoadAssetAtPath<SceneCollection>(p))
                .Where(x => x != null)
                .ToArray();

            string code = GenerateCode(collections);

            Directory.CreateDirectory("Assets/Generated");
            
            File.WriteAllText("Assets/Generated/SceneCollectionDatabase.cs", code);
            
            AssetDatabase.Refresh();
        }

        static string GenerateCode(SceneCollection[] collections)
        {
            string entries = string.Join(",\n            ",
                collections.Select(c =>
                    $"AssetDatabase.LoadAssetAtPath<SceneCollection>(\"{AssetDatabase.GetAssetPath(c)}\")"
                )
            );
            return $@"
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace {typeof(SceneCollectionCodeGenerator).Namespace}
{{
    public static class SceneCollectionDatabase
    {{
        public static SceneCollection[] AllCollections = new SceneCollection[]
        {{
            {entries}
        }};
    }}
}}
#endif
";
        }
    }
}
#endif