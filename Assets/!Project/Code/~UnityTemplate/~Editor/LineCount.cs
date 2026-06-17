using System;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using ZLinq;

#if UNITY_EDITOR
public class LineCount : EditorWindow
{
	private int lineCount = 0;
	
	[MenuItem("Tools/Line Count")]
	public static void ShowWindow()
	{
		GetWindow<LineCount>("Line Count");
	}
	
	private void OnGUI()
	{
		GUILayout.Label($"{lineCount} lines of code", EditorStyles.boldLabel);

		if (GUILayout.Button("Calculate Line Count"))
		{
			var root = "Assets/!Project/Code";
			var excludedFolder = "Assets/!Project/Code\\~UnityTemplate";
			
			//debug folder
			var lineCount1 = Directory
				.EnumerateFiles(root, "*.cs", SearchOption.AllDirectories)
				.AsValueEnumerable()
				.Where(path => !path.StartsWith(excludedFolder));
			
			lineCount = lineCount1.Sum(path => File.ReadLines(path).Count());
			
			foreach (string se in lineCount1)
			{
				Debug.Log(se);
			}
		}
	}
}
#endif