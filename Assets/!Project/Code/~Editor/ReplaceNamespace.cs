using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class ReplaceNamespace : EditorWindow
{
	private string _newNamespace = "";
	
	[MenuItem("Tools/Replace Namespace")]
	public static void ShowWindow()
	{
		GetWindow<ReplaceNamespace>("Replace Namespace");
	}

	private void OnGUI()
	{
		_newNamespace = EditorGUILayout.TextField("New Namespace:", _newNamespace);
		
		if (!GUILayout.Button("Replace")) return;
		if (string.IsNullOrEmpty(_newNamespace))
		{
			Debug.Log(_newNamespace);
			EditorUtility.DisplayDialog("Error", "Please enter a namespace.", "OK");
			return;
		}

		NamespaceSetter.ReplaceNamespace(_newNamespace);
	}
}