using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ComboTag))]
public class comboTagEditor: Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();

		if (GUILayout.Button ("Add Magic")) {
			Debug.Log(	((ComboTag)target).addTag (ComboTag.AbilType.Magic, new List<ComboTag.AbilType>(){ComboTag.AbilType.Life}));

				
		
		}
		if (GUILayout.Button ("Add Life")) {
			Debug.Log(	((ComboTag)target).addTag (ComboTag.AbilType.Life, new List<ComboTag.AbilType>()));



		}
	}
}
