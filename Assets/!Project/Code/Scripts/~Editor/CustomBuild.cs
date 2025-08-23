using System;
using UnityEditor;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
public class CustomBuild : EditorWindow
{
	private enum VersionPart { Major, Minor, Patch }
	private VersionPart incrementPart = VersionPart.Patch;
	private string[] labels = new string[] { "None", "Alpha", "Beta", "Release Candidate" };
	private string[] labelsSuffix = new string[] { "", "a", "b", "rc" };
	private int selectedLabelIndex = 0;
	
	[MenuItem("Window/Build", priority = 10)]
	public static void ShowWindow()
	{
		GetWindow<CustomBuild>("Build Game");
	}
	
	private void OnGUI()
	{
		GUILayout.Label("Current Version: " + PlayerSettings.bundleVersion, EditorStyles.boldLabel);

		incrementPart = (VersionPart)EditorGUILayout.EnumPopup("Increment Version Part", incrementPart);

		selectedLabelIndex = EditorGUILayout.Popup("Pre-Release Label", selectedLabelIndex, labels);

		if (GUILayout.Button("Build"))
		{
			BuildGame(incrementPart, labelsSuffix[selectedLabelIndex]);
		}
	}
	
	private void BuildGame(VersionPart incrementPart, string label)
	{
		// 1. Increment version
		string version = PlayerSettings.bundleVersion.Split('-')[0];
		string[] parts = version.Split('.');
		int major = int.Parse(parts[0]);
		int minor = parts.Length > 1 ? int.Parse(parts[1]) : 0;
		int patch = parts.Length > 2 ? int.Parse(parts[2]) : 0;

		switch (incrementPart)
		{
			case VersionPart.Major:
				major++;
				minor = 0;
				patch = 0;
				break;
			case VersionPart.Minor:
				minor++;
				patch = 0;
				break;
			case VersionPart.Patch:
				patch++;
				break;
		}

		string newVersion = $"{major}.{minor}.{patch}";
		if (!string.IsNullOrEmpty(label))
		{
			newVersion += "-" + label;
		}

		PlayerSettings.bundleVersion = newVersion;
		Debug.Log("Building Version: " + PlayerSettings.bundleVersion);
		
		string platform = EditorUserBuildSettings.activeBuildTarget.ToString();
		string buildPath = Path.Combine("Build", platform, PlayerSettings.bundleVersion);

		if (!Directory.Exists(buildPath))
			Directory.CreateDirectory(buildPath);
		
		BuildPipeline.BuildPlayer(EditorBuildSettings.scenes,
			Path.Combine(buildPath, PlayerSettings.productName + ".exe"),
			EditorUserBuildSettings.activeBuildTarget,
			BuildOptions.None);

		Debug.Log("Build complete: " + buildPath);
	}
}
#endif