using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
	#endif
}