using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PortalArea))]

public class TriggerAreaEditor : Editor
{
	public override void OnInspectorGUI()
	{
		PortalArea myTarget = (PortalArea)target;
		myTarget.worldNames = (SceneController.WorldScene)EditorGUILayout.EnumFlagsField(myTarget.worldNames);

		myTarget.color = EditorGUILayout.ColorField(myTarget.color);
	}
}
