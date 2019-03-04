using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbstractCost))]
public class AbstractCostEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		GUILayout.BeginHorizontal();
		AbstractCost info = (AbstractCost)target;

		typ = (ResourceType)EditorGUILayout.EnumPopup("Res type: ", typ);

		if (GUILayout.Button("Add Resource Cost"))
		{
			info.resourceCosts.MyResources.Add(new ResourceTank(typ, 0));
			EditorUtility.SetDirty(target);
		}

		GUILayout.EndHorizontal();
		foreach (ResourceTank tank in info.resourceCosts.MyResources)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(tank.resType + "");
			float f = tank.currentAmount;
			tank.currentAmount = EditorGUILayout.FloatField(tank.currentAmount);
			if (f != tank.currentAmount)
			{
				EditorUtility.SetDirty(target);
			}

			if (GUILayout.Button("Remove"))
			{
				info.resourceCosts.MyResources.Remove(tank);
				EditorUtility.SetDirty(target);
			}
			

			GUILayout.EndHorizontal();
		}

	}

	

	ResourceType typ;


	
}
