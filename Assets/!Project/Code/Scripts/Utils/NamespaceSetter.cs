#if UNITY_EDITOR
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class NamespaceSetter : AssetPostprocessor
{
	private static void OnPostprocessAllAssets(string[] importedAssets, string[] _, string[] __, string[] ___)
	{
		foreach (string path in importedAssets)
		{
			if (!path.EndsWith(".cs")) continue;

			if (path.Contains("NamespaceSetter.cs")) continue;

			string fullPath = Path.Combine(Directory.GetCurrentDirectory(), path);
			string content = File.ReadAllText(fullPath);

			if (!content.Contains("#NAMESPACE#")) continue;
			
			string rawProjectName = Application.productName;
			string safeNamespace = Regex.Replace(rawProjectName, @"[^a-zA-Z0-9_]", "");

			content = content.Replace("#NAMESPACE#", safeNamespace);
			File.WriteAllText(fullPath, content);
		}
		
		AssetDatabase.Refresh();
	}
}
#endif