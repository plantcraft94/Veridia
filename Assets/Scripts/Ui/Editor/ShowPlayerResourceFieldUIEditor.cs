using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

[CustomEditor(typeof(AmountDisplay))]
public class ShowPlayerResourceFieldUIEditor : Editor
{
	string[] fieldNames;
	int selectedIndex = -1;

	void OnEnable()
	{
		var ui = (AmountDisplay)target;
		var prType = ui.sourceComponent.GetType();
		fieldNames = prType
			.GetFields(BindingFlags.Public | BindingFlags.Instance)
			.Select(f => f.Name).ToArray();

		selectedIndex = Array.IndexOf(fieldNames, ui.varAmount);
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (fieldNames.Length == 0)
		{
			EditorGUILayout.HelpBox("No fields found in PlayerResource.", MessageType.Warning);
			return;
		}

		selectedIndex = EditorGUILayout.Popup("Field to Display", selectedIndex, fieldNames);

		var ui = (AmountDisplay)target;
		if (selectedIndex >= 0 && selectedIndex < fieldNames.Length)
		{
			ui.varAmount = fieldNames[selectedIndex];
		}

		if (GUI.changed)
			EditorUtility.SetDirty(target);
	}
}