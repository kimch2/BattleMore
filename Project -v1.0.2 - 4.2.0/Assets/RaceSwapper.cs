using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class RaceSwapper : MonoBehaviour {


    public List<UnitSpot> POneUnits;
    public List<UnitSpot> PTwoUnits;
    public List<UnitSpot> PThreeUnits;

	public UnitEquivalance RacePacket;

	public GameObject PlayerRoot;
	public GameObject EnemyRoot;
	public GameObject NuetralRoot;
	LevelChoice choice;

	[Tooltip("Set at runtime")]
	public UltObject Ulty;

	public static RaceSwapper main;

	private void Awake()
	{
		main = this;
		choice = LevelSelectmanager.BattleModeChoice;
		LevelSelectmanager.BattleModeChoice = null;
		if (RacePacket == null)
		{
			RacePacket = Resources.Load<GameObject>("RaceInfoPacket").GetComponent<UnitEquivalance>();
		}

		if (choice != null)
		{
			Ulty = Instantiate<GameObject>(RacePacket.getRace(choice.PlayingAs).UltimatePrefab).GetComponent<UltObject>();

			swap(1, choice.PlayingAs);
			Debug.Log("Swapping enemy to " + choice.PlayingAgainst);
			swap(2, choice.PlayingAgainst);
		}
		else
		{
			Ulty = Instantiate<GameObject>( RacePacket.getRace(GameObject.FindObjectOfType<GameManager>().playerList[0].myRace).UltimatePrefab).GetComponent<UltObject>();
		}
	}


	void Start()
	{
		if(choice != null)
		{
			GameManager.main.activePlayer.SetUltimates(Ulty);
		}
	}

	public RaceInfo getPlayerRaceInfo()
	{

		if (RacePacket == null)
		{
			RacePacket = Resources.Load<GameObject>("RaceInfoPacket").GetComponent<UnitEquivalance>();
		}
		return RacePacket.getRace(choice == null ? RaceInfo.raceType.SteelCrest : choice.PlayingAs);
	}

	public void swap(int playerNumber, RaceInfo.raceType race)
	{
		GameManager gamer = FindObjectOfType<GameManager>(); // this gets called during edit time so .main doesn't exist 

		if (playerNumber == 1)
		{
			gamer.activePlayer.changeRace(race);
			if (choice != null)
			{
				addColoror(gamer.playerList[0], choice.PlayerHueShift, choice.PlayerSaturation);
			}
			SwapUnits(POneUnits, race);
		}
		else if (playerNumber == 2)
		{
			gamer.playerList[1].myRace = race;
			if (choice != null)
			{
				addColoror(gamer.playerList[1], choice.EnemyHueShift, choice.EnemySaturation);
			}
			SwapUnits(PTwoUnits, race);
		}
		else if (playerNumber == 3)
		{
			gamer.playerList[2].myRace = race;
			if (choice != null)
			{
				addColoror(gamer.playerList[2], 0, 1);
			}
			SwapUnits(PThreeUnits, race);
		}
	}


	void addColoror(RaceManager racer, float HueShift, float Saturation)
	{
		Debug.Log("Making Swapper " + racer.playerNumber);
		SkinColorManager colorer = GameManager.main.gameObject.AddComponent<SkinColorManager>();
		racer.colorManager = colorer;
		colorer.resetHue(HueShift);
		colorer.resetSaturation(Saturation);
		colorer.useColoredSkins = true;
	}

	public void DeleteGameObjects()
	{
		foreach (UnitSpot spot in POneUnits)
		{
			foreach (GameObject obj in spot.CurrentGuys)
			{
				DestroyImmediate(obj);
			}
		}
		foreach (UnitSpot spot in PTwoUnits)
		{
			foreach (GameObject obj in spot.CurrentGuys)
			{
				DestroyImmediate(obj);
			}
		}
		foreach (UnitSpot spot in PThreeUnits)
		{
			foreach (GameObject obj in spot.CurrentGuys)
			{
				DestroyImmediate(obj);
			}
		}
	}

	public void SwapUnits(List<UnitSpot> spots, RaceInfo.raceType race)
	{
		int difficulty = 3;
		if (Application.isPlaying)
		{
			difficulty = LevelData.getDifficulty();
		}

		foreach (UnitSpot spot in spots)
		{
			foreach (GameObject obj in spot.CurrentGuys) { 
				DestroyImmediate(obj);
			}
			spot.CurrentGuys.Clear();
			if (RacePacket == null)
			{
				RacePacket = Resources.Load<GameObject>("RaceInfoPacket").GetComponent<UnitEquivalance>();
			}

			if (Application.isPlaying && spots == PTwoUnits)
			{
				if (spot.MaxDifficulty > difficulty )
				{
					continue;
				}
			}

			Composition comp = RacePacket.myComps.Find(item => item.ID == spot.CompositionID);
			UnitPile pile = comp.RacePiles.Find(item => item.myRace == race);

			List<GameObject> allUnits = SpawnUnit(comp, race, spot.PlayerOwner, spot.position, spot.Rotation);
			spot.CurrentGuys = allUnits;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		foreach (UnitSpot spot in POneUnits)
		{
			Gizmos.DrawSphere(spot.position, 1);
		}
		Gizmos.color = Color.red;
		foreach (UnitSpot spot in PTwoUnits)
		{
			Gizmos.DrawSphere(spot.position, 1);
		}
		Gizmos.color = Color.yellow;
		foreach (UnitSpot spot in PThreeUnits)
		{
			Gizmos.DrawSphere(spot.position, 1);
		}
	}

	public void CreateUnits(Composition currentComp, RaceInfo.raceType currentType, int playerNumber, Vector3 position, Vector3 direction, int difficulty)
	{
		List<GameObject> allUnits = SpawnUnit(currentComp, currentType, playerNumber, position, direction);
		addUnit(currentComp, allUnits, position, direction, difficulty , playerNumber);
		
	}

	List<GameObject>  SpawnUnit(Composition currentComp, RaceInfo.raceType currentType, int playerNumber, Vector3 position, Vector3 direction) { 

		List<GameObject> prefab = currentComp.RacePiles.Find(item => item.myRace == currentType).units;

		direction.y = 0;
		List<GameObject> allUnits = new List<GameObject>();
		int count = -1;
		foreach (GameObject obj in prefab)
		{
			count++;

			Vector3 newPosition = position;
			newPosition.x += Mathf.Sin(Mathf.Deg2Rad * 60 * count) * 12 * ((count + 5) / 6);
			newPosition.z += Mathf.Cos(Mathf.Deg2Rad * 60 * count) * 12 * ((count + 5) / 6);

			GameObject newUnit = null;
			if (Application.isPlaying) {
				newUnit = (GameObject)Instantiate<GameObject>(obj, newPosition, Quaternion.identity, null);
			//	Debug.Log("Is Playing");
			}

#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
			//	Debug.Log("Is Playing");
				newUnit = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(obj);
				newUnit.transform.position = newPosition;
				newUnit.transform.rotation = Quaternion.identity;
				UnityEditor.Undo.RegisterCreatedObjectUndo(newUnit, "Create " + newUnit.name);
				UnityEditor.Undo.RecordObject(this, "Create " + newUnit.name);
			}
#endif
			newUnit.transform.LookAt(newUnit.transform.position + direction);
			if (newUnit.GetComponent<airmover>())
			{
				newUnit.transform.position += Vector3.up * newUnit.GetComponent<airmover>().flyerHeight;
			}
			allUnits.Add(newUnit);

		}

		if (playerNumber == 1)
		{
			makeFriendly(allUnits);
			setOwner(allUnits, 1, position, direction);
		}
		else if (playerNumber == 2)
		{
			makeEnemy(allUnits);
			setOwner(allUnits, 2, position, direction);
		}
		else if (playerNumber == 3)
		{
			makeNuetral(allUnits);
			setOwner(allUnits, 3, position, direction);
		}
		return allUnits;
	}

	void makeFriendly(List<GameObject> obj)
	{
		if (PlayerRoot == null)
		{
			PlayerRoot = GameObject.Find("PlayerUnitRoot");
			if (PlayerRoot == null)
			{
				PlayerRoot = new GameObject("PlayerUnitRoot");
			}
		}

		foreach (GameObject unit in obj)
		{
			unit.transform.SetParent(PlayerRoot.transform);
			FogOfWarUnit fogger = unit.GetComponent<FogOfWarUnit>();
			if (!fogger)
			{ fogger = unit.AddComponent<FogOfWarUnit>(); }
			fogger.enabled = true;
		}
	}

	void makeEnemy(List<GameObject> obj)
	{
		if (EnemyRoot == null)
		{
			EnemyRoot = GameObject.Find("EnemyUnitRoot");
			if (EnemyRoot == null)
			{
				EnemyRoot = new GameObject("EnemyUnitRoot");
			}
		}


		foreach (GameObject unit in obj)
		{
			unit.transform.SetParent(EnemyRoot.transform);
			FogOfWarUnit fogger = unit.GetComponent<FogOfWarUnit>();
			if (fogger)
			{ DestroyImmediate(fogger); }
		}
	}

	void makeNuetral(List<GameObject> obj)
	{
		if (NuetralRoot == null)
		{
			NuetralRoot = GameObject.Find("NuetralUnitRoot");
			if (NuetralRoot == null)
			{
				NuetralRoot = new GameObject("NuetralUnitRoot");
			}
		}

		foreach (GameObject unit in obj)
		{
			unit.transform.SetParent(NuetralRoot.transform);
			FogOfWarUnit fogger = unit.GetComponent<FogOfWarUnit>();
			if (!fogger)
			{ fogger = unit.AddComponent<FogOfWarUnit>(); }
			fogger.enabled = false;
		}
	}

	void setOwner(List<GameObject> obj, int num, Vector3 position, Vector3 direction)
	{
		foreach (GameObject unit in obj)
		{
			
			UnitManager manage = unit.GetComponent<UnitManager>();
			manage.initializeVision();
			foreach (UnitManager man in unit.GetComponentsInChildren<UnitManager>(true))
			{
				man.initializeVision();
				man.PlayerOwner = num;
			}
			manage.PlayerOwner = num;
		}
	}


	public void DeleteSpot(Vector3 position)
	{
		UnitSpot deleteMe = null;
		foreach (UnitSpot spot in POneUnits)
		{
			if (Vector3.Distance(position, spot.position) < 5)
			{
				deleteMe = spot;
				break;
			}
		}
		if (deleteMe != null)
		{
			POneUnits.Remove(deleteMe);
			foreach (GameObject obj in deleteMe.CurrentGuys)
			{
				DestroyImmediate(obj);
			}
		}


		foreach (UnitSpot spot in PTwoUnits)
		{
			if (Vector3.Distance(position, spot.position) < 5)
			{
				deleteMe = spot;
				break;
			}
		}
		if (deleteMe != null)
		{
			PTwoUnits.Remove(deleteMe);
			foreach (GameObject obj in deleteMe.CurrentGuys)
			{
				DestroyImmediate(obj);
			}
		}

		foreach (UnitSpot spot in PThreeUnits)
		{
			if (Vector3.Distance(position, spot.position) < 5)
			{
				deleteMe = spot;
				break;
			}
		}
		if (deleteMe != null)
		{
			PThreeUnits.Remove(deleteMe);
			foreach (GameObject obj in deleteMe.CurrentGuys)
			{
				DestroyImmediate(obj);
			}
		}
	}



	public void addUnit(Composition comp, List<GameObject> obj, Vector3 position, Vector3 rotation,int difficulty, int playerNumber = 0)
    {
        if (playerNumber == 0)
        {
			
            playerNumber = obj[0].GetComponent<UnitManager>().PlayerOwner;
        }

        UnitSpot spot = new UnitSpot(comp.ID, position, rotation, obj, playerNumber);
		spot.MaxDifficulty = difficulty;

        if (playerNumber == 1)
        {
            POneUnits.Add(spot);
        }
        if (playerNumber == 2)
        {
            PTwoUnits.Add(spot);
        }
        if (playerNumber == 3)
        {
            PThreeUnits.Add(spot);
        }
    }



	//public void

}

[System.Serializable]
public class UnitSpot{

    public Vector3 position;
    public Vector3 Rotation;
    public List<GameObject> CurrentGuys;
    public int PlayerOwner;
    public int MaxDifficulty = 1;
	public int CompositionID;

    public UnitSpot(int compID, Vector3 pos, Vector3 Rot, List <GameObject> Guy, int Owner)
    {
        position = pos;
        Rotation = Rot;
        CurrentGuys = Guy;
        PlayerOwner = Owner;
		CompositionID = compID;
    }

}
