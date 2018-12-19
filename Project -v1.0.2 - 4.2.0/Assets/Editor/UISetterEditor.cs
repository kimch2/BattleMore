using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UISetter))]
public class UISetterEditor : Editor
{


	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		if (GUILayout.Button("Sort!"))
		{
			((UISetter)target).SortImages();

		}

	}

}