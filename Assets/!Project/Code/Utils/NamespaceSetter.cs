using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityTemplate
{
	public class NamespaceSetter
		#if UNITY_EDITOR
		: AssetPostprocessor
	#endif
	{
		public static string Namespace => Regex.Replace(Application.productName, @"[^a-zA-Z0-9_]", "");
		#if UNITY_EDITOR
		private static void OnPostprocessAllAssets(string[] importedAssets, string[] _, string[] __, string[] ___)
		{
			foreach (string path in importedAssets)
			{
				if (!path.EndsWith(".cs")) continue;

				if (path.Contains("NamespaceSetter.cs")) continue;

				string fullPath = Path.Combine(Directory.GetCurrentDirectory(), path);
				string content = File.ReadAllText(fullPath);

				if (!content.Contains("#NAMESPACE#")) continue;

				content = content.Replace("#NAMESPACE#", Namespace);
				File.WriteAllText(fullPath, content);
			}

			AssetDatabase.Refresh();
		}

		public static void ReplaceNamespace(string newNamespace)
		{
			string projectPath = Application.dataPath;
			string[] csFiles = Directory.GetFiles(projectPath, "*.cs", SearchOption.AllDirectories);

			var matchingFiles = csFiles
				.Where(path =>
				{
					string contents = File.ReadAllText(path);
					return contents.Contains($"namespace {Namespace}") || contents.Contains($"using {Namespace}");
				})
				.ToList();

			if (matchingFiles.Count == 0)
			{
				Debug.Log($"No files found with namespace or using '{Namespace}'.");
				return;
			}

			Debug.Log($"Replacing {Namespace} with {newNamespace} in {matchingFiles.Count} file(s):");
			foreach (var file in matchingFiles)
			{
				string content = File.ReadAllText(file);

				content = content
					.Replace($"namespace {Namespace}", $"namespace {newNamespace}")
					.Replace($"using {Namespace}", $"using {newNamespace}");

				File.WriteAllText(file, content);
				Debug.Log($"• {file.Replace(projectPath, "Assets")}");
			}
		}
		#endif
	}
}