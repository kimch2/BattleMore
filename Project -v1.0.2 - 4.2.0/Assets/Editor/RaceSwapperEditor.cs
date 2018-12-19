using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RaceSwapper))]
public class RaceSwapperEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		if (GUILayout.Button("Delete All GameObjects (but not spots)"))
		{
			((RaceSwapper)target).DeleteGameObjects();
		}

		if (GUILayout.Button("Populate"))
		{
			((RaceSwapper)target).SwapUnits(((RaceSwapper)target).POneUnits, RaceInfo.raceType.SteelCrest);
			((RaceSwapper)target).SwapUnits(((RaceSwapper)target).PTwoUnits, RaceInfo.raceType.Coalition);
			((RaceSwapper)target).SwapUnits(((RaceSwapper)target).PThreeUnits, RaceInfo.raceType.SteelCrest);
		}
	}
}
