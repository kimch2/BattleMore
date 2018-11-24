using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class RaceInfo : MonoBehaviour {


	public enum raceType{SteelCrest, Coalition, Polygons, Animals , None }
	public GameObject UltimatePrefab;
    public enum unitType
	{
		ConstructionYard,Armory,AetherCore,Augmentor,Aviatrix,FluxArray,EngineeringBay,Academy,Manticore,SteelCraftor,Zephyr,Vulcan,Triton,Chimera,Minigun,ImperioCannon,RepairBay,MortarPod
	}

	public raceType race;
	

	public List<GameObject> unitList = new List<GameObject>();
	public List<GameObject> buildingList = new List<GameObject>();
	public List<GameObject> attachmentsList = new List<GameObject>();

	public List<UnitManager> DefaultFOne;
	public List<UnitManager> DefaultFTwo;
	public List<UnitManager> DefaultFThree;
	public List<UnitManager> DefaultFFour;


	public string getFHotkeyString()
	{
		string n = "";
		for (int i = 0; i < DefaultFOne.Count; i++)
		{
			n += DefaultFOne[i].UnitName;
			if (i < DefaultFOne.Count - 1)
			{
				n += ",";
			}
		}
		n += ";";

		for (int i = 0; i < DefaultFTwo.Count; i++)
		{
			n += DefaultFTwo[i].UnitName;
			if (i < DefaultFTwo.Count - 1)
			{
				n += ",";
			}
		}
		n += ";";


		for (int i = 0; i < DefaultFThree.Count; i++)
		{
			n += DefaultFThree[i].UnitName;
			if (i < DefaultFThree.Count - 1)
			{
				n += ",";
			}
		}
		n += ";";


		for (int i = 0; i < DefaultFFour.Count; i++)
		{
			n += DefaultFFour[i].UnitName;
			if (i < DefaultFFour.Count - 1)
			{
				n += ",";
			}
		}

		return n;
	}

}
