using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class RaceInfo : MonoBehaviour {


	public enum raceType{SteelCrest, Coalition, Polygons, Animals , None }

    public enum unitType
	{
		ConstructionYard,Armory,AetherCore,Augmentor,Aviatrix,FluxArray,EngineeringBay,Academy,Manticore,SteelCraftor,Zephyr,Vulcan,Triton,Chimera,Minigun,ImperioCannon,RepairBay,MortarPod
	}

	public raceType race;
	

	public List<GameObject> unitList = new List<GameObject>();
	public List<GameObject> buildingList = new List<GameObject>();
	public List<GameObject> attachmentsList = new List<GameObject>();

    

}
