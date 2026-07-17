#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditor.Build.Profile;

public class CustomBuild : EditorWindow
{
	static readonly int BUILD_PROFILE_COLUMNS = 3;
	private enum VersionPart { Major, Minor, Patch }
	private static string[] labels = new string[] { "None", "Alpha", "Beta", "Release Candidate" };
	private static string[] labelsSuffix = new string[] { "", "a", "b", "rc" };
	private static int selectedProfileIndex = 0;
	
	private static int selectedLabelIndex
	{
		get => EditorPrefs.HasKey("buildLabel") ? EditorPrefs.GetInt("buildLabel") : 0;
		set => EditorPrefs.SetInt("buildLabel", value);
	}
	
	private static VersionPart incrementPart
	{
		get => EditorPrefs.HasKey("incrementPart") ? (VersionPart)EditorPrefs.GetInt("incrementPart") : VersionPart.Patch;
		set => EditorPrefs.SetInt("incrementPart", (int)value);
	}

	
	[MenuItem("Tools/Build")]
	public static void ShowWindow()
	{
		GetWindow<CustomBuild>("Build Game");
		var profiles = GetBuildProfiles();
		selectedProfileIndex = profiles.IndexOf(BuildProfile.GetActiveBuildProfile());
	}
	
	private void OnGUI()
	{
		GUILayout.Label("Current Version: " + PlayerSettings.bundleVersion, EditorStyles.boldLabel, GUILayout.Height(20));
		GUILayout.Space(5);
		incrementPart = (VersionPart)EditorGUILayout.EnumPopup("Increment Version Part", incrementPart, GUILayout.Height(20));
		selectedLabelIndex = EditorGUILayout.Popup("Pre-Release Label", selectedLabelIndex, labels, GUILayout.Height(20));
		
		DrawBuildProfiles();
		DrawBuildButtons();
	}
	
	private static void DrawBuildProfiles()
	{
		GUILayout.Space(10);
		
		var profiles = GetBuildProfiles();
		if (profiles.Count == 0)
		{
			EditorGUILayout.HelpBox(
				"No build profiles were found!",
				MessageType.Warning
			);
			return;
		}
		
		List<string> profileNames = profiles.Select(p => p.name).ToList();
		profileNames.Add("All Profiles");
		
		selectedProfileIndex = GUILayout.SelectionGrid(
			Math.Max(selectedProfileIndex, 0),
			profileNames.ToArray(),
			BUILD_PROFILE_COLUMNS
		);
	}

	private static void DrawBuildButtons()
	{
		GUILayout.Space(10);
		GUILayout.BeginHorizontal();
		
		if (GUILayout.Button("Build", GUILayout.Height(30)))
		{
			if (selectedProfileIndex == GetBuildProfiles().Count)
			{
				EditorApplication.delayCall += () => BuildAllProfiles();
			}
			else
			{
				var profiles = GetBuildProfiles();
				int oldIndex = profiles.IndexOf(BuildProfile.GetActiveBuildProfile());
				if (selectedProfileIndex != oldIndex && selectedProfileIndex >= 0 && selectedProfileIndex < profiles.Count)
				{
					BuildProfile.SetActiveBuildProfile(profiles[selectedProfileIndex]);
				}
				EditorApplication.delayCall += () => BuildGame(incrementPart, labelsSuffix[selectedLabelIndex]);
			}
		}

		if (GUILayout.Button("Build Without Increment", GUILayout.Height(30)))
		{
			if (selectedProfileIndex == GetBuildProfiles().Count)
			{
				EditorApplication.delayCall += () => BuildAllProfiles(false);
			}
			else
			{
				var profiles = GetBuildProfiles();
				int oldIndex = profiles.IndexOf(BuildProfile.GetActiveBuildProfile());
				if (selectedProfileIndex != oldIndex && selectedProfileIndex >= 0 && selectedProfileIndex < profiles.Count)
				{
					BuildProfile.SetActiveBuildProfile(profiles[selectedProfileIndex]);
				}
				EditorApplication.delayCall += () => BuildGame(incrementPart, labelsSuffix[selectedLabelIndex], false);
			}
		}
		
		GUILayout.EndHorizontal();
	}

	private static void BuildAllProfiles(bool incrementVersion = true)
	{
		int profileCount = GetBuildProfiles().Count;
		for (int i = 0; i < profileCount; i++)
		{
			BuildProfile.SetActiveBuildProfile(GetBuildProfiles()[i]);
			BuildGame(incrementPart, labelsSuffix[selectedLabelIndex], incrementVersion);
			incrementVersion = false;
		}
	}
	private static void BuildGame(VersionPart incrementPart, string label, bool incrementVersion = true)
	{
		string version = PlayerSettings.bundleVersion.Split('-')[0];
		string[] parts = version.Split('.');
		int major = int.Parse(parts[0]);
		int minor = parts.Length > 1 ? int.Parse(parts[1]) : 0;
		int patch = parts.Length > 2 ? int.Parse(parts[2]) : 0;

		if (incrementVersion)
		{
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
	
	private static List<BuildProfile> GetBuildProfiles()
	{
		string[] guids = AssetDatabase.FindAssets("t:BuildProfile");
		List<BuildProfile> profiles = new();
		foreach (string guid in guids)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			BuildProfile profile = AssetDatabase.LoadAssetAtPath<BuildProfile>(path);
			if (profile)
			{
				profiles.Add(profile);
			}
		}
		return profiles;
	}
}
#endif