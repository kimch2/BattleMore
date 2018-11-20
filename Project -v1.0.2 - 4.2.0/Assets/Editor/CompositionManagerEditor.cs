using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System;

public class CompositionManagerEditor : EditorWindow
{

    UnitEquivalance UnitHolder;
  
    [MenuItem("Window/Race Composition Manager ")]
    public static void showWindow()
    {
        EditorWindow.GetWindow(typeof(CompositionManagerEditor));
    }



    void OnGUI()
    {
        if (UnitHolder == null)
        {
            UnitHolder = Resources.Load<GameObject>("RaceInfoPacket").GetComponent<UnitEquivalance>();
        }

 

        foreach (Composition comp in UnitHolder.myComps)
        {
           
            comp.exposed = EditorGUILayout.Foldout(comp.exposed,  comp.CompositionName);

            if (!comp.exposed)
            {
                continue;
            }
         
            comp.CompositionName = GUILayout.TextField(comp.CompositionName);
            comp.GizmoColor = EditorGUILayout.ColorField(comp.GizmoColor, GUILayout.Width(60));
            
            EditorGUI.indentLevel++;

            foreach (UnitPile pile in comp.RacePiles)
            {

                if (pile.myRace == RaceInfo.raceType.None)
                {
                    pile.myRace = selectRace();
                }
                else if (GUILayout.Button(pile.myRace.ToString() + " (Press Clear All and Reselect)", GUILayout.Width(300)))
                {
                    pile.myRace = RaceInfo.raceType.None;
                    pile.units.Clear();
                }
              
                
                GUILayout.BeginHorizontal();
                for (int i = 0; i < pile.units.Count; i ++)
                {
                    if (pile.units[i] == null) {
                        GUILayout.Label("Select a Unit");
                        pile.units[i] = selectUnit(pile.myRace);
                        if (GUILayout.Button("Remove", GUILayout.Width(65)))
                        {
                            pile.units.RemoveAt(i);
                        }
                    }
                    else { 
                    UnitManager manage = pile.units[i].GetComponent<UnitManager>();
                        if (GUILayout.Button(manage.GetComponent<UnitStats>().Icon.texture, GUILayout.Width(50), GUILayout.Height(50)))
                        {
                            pile.units[i] = null;
                        }
                    }
                }
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Add Race Specific Unit", GUILayout.Width(200)))
                {
                    pile.units.Add(null);
                 
                }
                GUILayout.Label(" ");

            }
   
            if (GUILayout.Button("Add Race", GUILayout.Width(100)))
            {
               comp.RacePiles.Add(new UnitPile());
            }
            EditorGUI.indentLevel--;
            GUILayout.Label(" ");
        }

		if (GUILayout.Button("Add New Unit Equivelance", GUILayout.Width(300)))
		{
			UnitHolder.myComps.Add(new Composition(UnityEngine.Random.Range(0, 100000)));
		}
    }


   GameObject selectUnit(RaceInfo.raceType type)
    {
        RaceInfo info = UnitHolder.getRace(type);
        GUILayout.BeginVertical();
       
        foreach (GameObject obj in info.unitList)
        {
            if (GUILayout.Button(obj.GetComponent<UnitStats>().Icon.texture, GUILayout.Width(50), GUILayout.Height(50)))
            {
                return obj;
            }
        }
        GUILayout.EndVertical();
        GUILayout.BeginVertical();

        foreach (GameObject obj in info.buildingList)
        {
            if (GUILayout.Button(obj.GetComponent<UnitStats>().Icon.texture, GUILayout.Width(50), GUILayout.Height(50)))
            {
                return obj; 
            }
        }

        GUILayout.EndVertical();
        return null;
    }

    RaceInfo.raceType selectRace()
    {
        foreach (RaceInfo obj in UnitHolder.raceInfos)
        {
            if (GUILayout.Button(obj.race.ToString()))
            {
                return obj.race;
            }
        }
        return RaceInfo.raceType.None;
    }

}
