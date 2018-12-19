using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RaceInfo))]
public class RaceInfoEditor : Editor {

	bool setCosts;

	public override void OnInspectorGUI()
	{
		if (!setCosts)
		{
			DrawDefaultInspector();
		}
		else {
			RaceInfo info = (RaceInfo)target;

			DisplayCosts(info.unitList);
			DisplayCosts(info.buildingList);
			DisplayCosts(info.attachmentsList);
		}

		if (GUILayout.Button("Set Costs"))
		{
			setCosts = !setCosts;
		}
	
	}


	void DisplayCosts(List<GameObject> unitList)
	{
		RaceInfo info = (RaceInfo)target;
		foreach (GameObject obj in unitList)
		{
			GUILayout.BeginHorizontal();

			GUILayout.Label(obj.name);
			UnitStats stats = obj.GetComponent<UnitStats>();
			GUILayout.Button(stats.Icon.texture, GUILayout.Width(50), GUILayout.Height(50));

			if (stats.Cost.MyResources.Count != info.ResourceTypes.MyResources.Count)
			{
				stats.Cost.MyResources.Clear();
				foreach (ResourceTank tank in info.ResourceTypes.MyResources)
				{
					stats.Cost.MyResources.Add(new ResourceTank(tank.resType, 0));
				}
			}

			foreach (ResourceTank tank in stats.Cost.MyResources)
			{
				GUILayout.Label(tank.resType + "");
				tank.currentAmount = EditorGUILayout.FloatField(tank.currentAmount);
				GUILayout.Label("" + stats.cost);
			}
			GUILayout.EndHorizontal();

		}


	}



}
