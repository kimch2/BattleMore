using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class LevelMaker : EditorWindow
{

    bool showInstruction = true;
    bool showUnitPlacer = true;

    UnitEquivalance UnitHolder;
    RaceSwapper swapper;


    [MenuItem("Window/Level Maker")]
    public static void showWindow()
    {
        EditorWindow.GetWindow(typeof(LevelMaker));
    }

    void OnFocus()
    {
        // Remove if already present and register the function
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;

        // This allows us to register an update function for the scene view port
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
       
    }

    void OnDestroy()
    {
       SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }

    Vector3 lastPoint;

    void OnDrawGizmos()
    {

        Gizmos.DrawSphere(lastPoint, 25);
    }

    Vector3 firstPoint;
    bool DrawLine;
    void OnSceneGUI(SceneView scene_view)
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        //Debug.Log(Tools.current);// = Tool.View;
        if (showUnitPlacer) { 
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
         RaycastHit hit;

    
          if (Physics.Raycast(ray, out hit, 10000, 1 << 8))
         {
                lastPoint = hit.point;
              
        
         }
		  /*
			if (DrawLine)
			{
				Debug.Log("Drawing");

				Handles.DrawLine(firstPoint + Vector3.up, lastPoint + Vector3.up);
			}*/
			if (Event.current.button == 0)
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    DrawLine = true;
                    firstPoint = lastPoint;

                }
                if (Event.current.type == EventType.MouseUp )
                {
                    DrawLine = false;

                    swapper.CreateUnits(currentComp, currentType, playerNumber, firstPoint, lastPoint - firstPoint);
                }
            }

			if (Event.current.button == 1)
			{
				if (Event.current.type == EventType.MouseDown)
				{
					swapper.DeleteSpot(lastPoint);
				}
				
			}
		}
    }


  

    void OnGUI()
    {

        if (swapper == null)
        {
            swapper = GameObject.FindObjectOfType<RaceSwapper>();
            if (!swapper)
            {
                GameObject o = new GameObject();
				o.name = "RaceSwapper";
                swapper =  o.AddComponent<RaceSwapper>();

            }
        }

        if (UnitHolder == null)
        {
            UnitHolder = Resources.Load<GameObject>("RaceInfoPacket").GetComponent<UnitEquivalance>();
        }


        GUILayout.BeginHorizontal();


        if (GUILayout.Button("Instructions"))
        {
            showInstruction = !showInstruction;
        }

        if (GUILayout.Button("Unit Placer"))
        {
            showUnitPlacer = !showUnitPlacer;
        }

        GUILayout.EndHorizontal();
        
        if (showInstruction)
        {
            ShowInstructions();
        }

        if (showUnitPlacer)
        {
            PlaceUnits();
        }

    }


    void ShowInstructions()
    {
        GUILayout.Label("1. Save scene as new, delete all old units");
        GUILayout.Label("2. Add new Level to LevelEditor Prefab");
        GUILayout.Label("3. Create new Terrain and set to Terrain Layer");
        GUILayout.Label("4. Set Minimap bounds on MainCamera and take a picture. Put picture into LevelEditor");
        GUILayout.Label("5. Create units Spawn Points");
        GUILayout.Label("6. Create Objectives and triggers");
        GUILayout.Label("7. Add Scene to Build List, Change Level number in Victory Manager to the build list number, and to the LevelEditor Prefab ");
    }

   


    int playerNumber = 1;
    string[] PlayerType = new string[3] { "Player", "Enemy", "Nuetral" };
    bool capturable = true;
    RaceInfo.raceType currentType = RaceInfo.raceType.SteelCrest;
    Composition currentComp;



	void PlaceUnits()
	{
		GUILayout.BeginHorizontal();
		playerNumber = EditorGUILayout.Popup("Player Type", playerNumber - 1, PlayerType, GUILayout.Width(260)) + 1;
		if (playerNumber == 3)
		{
			capturable = GUILayout.Toggle(capturable, "Capturable ");
		}
		GUILayout.EndHorizontal();

		currentType = (RaceInfo.raceType)EditorGUILayout.EnumPopup("Current Race: ", currentType, GUILayout.Width(260));
		GUILayout.Label("Place Unit: ");


		currentComp = selectComp(currentComp);

		if (currentComp != null)
		{
			GUILayout.Label("Current Composition " + currentComp.CompositionName);
			UnitPile currenPile = currentComp.RacePiles.Find(item => item.myRace == currentType);
			GUILayout.BeginHorizontal();
			foreach (GameObject obj in currenPile.units)
			{
				GUILayout.Box(obj.GetComponent<UnitStats>().Icon.texture, GUILayout.Width(65), GUILayout.Height(65));
			}
			GUILayout.EndHorizontal();
		}

		GUILayout.Space(20);
		if (!SwapUnits)
		{
			if (GUILayout.Button("Swap Races"))
			{
				SwapUnits = true;
			}
		}
		else { 
			GUILayout.Label("Switch all units in player " + playerNumber + " to:");
			foreach (RaceInfo info in UnitHolder.raceInfos)
			{
				if (GUILayout.Button(info.race.ToString()))
				{
					swapper.swap(playerNumber, info.race);
					SwapUnits = false;
				}
			}
		}
    }

	bool SwapUnits;

    Composition selectComp( Composition current)
    {
		GUILayout.BeginHorizontal();
		int i = 0;
        foreach (Composition obj in UnitHolder.myComps)
        {
			i++;
			if (i % 5 == 0)
			{
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
			}
            if (current == obj)
            {
                GUI.backgroundColor = Color.green;
            }
            else
            {
                GUI.backgroundColor = Color.gray;
            }

            UnitPile p = obj.RacePiles.Find(item => item.myRace == currentType);
            if (p == null)
            {
                if (GUILayout.Button("Missing Race for this comp", GUILayout.Width(150), GUILayout.Height(50)))
                {
                    return obj;
                }
            }
            else
            {
                if (p.units.Count == 0)
                {
                    if (GUILayout.Button("No Units for this Race Comp", GUILayout.Width(200), GUILayout.Height(50)))
                    {
                        return obj;
                    }
                }
                else if(GUILayout.Button(p.units[0].GetComponent<UnitStats>().Icon.texture, GUILayout.Width(50), GUILayout.Height(50)))
                {
                    return obj;
                }

            }
            
        }
		GUILayout.EndHorizontal();
		return current;
    }


    void Awake()
    {

    }

}
