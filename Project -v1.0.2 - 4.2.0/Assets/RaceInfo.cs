using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class RaceInfo : MonoBehaviour {


	public ResourceManager ResourceTypes;

	public enum raceType{SteelCrest, Coalition, Polygons, Animals , None }
	public GameObject UltimatePrefab;
    public enum unitType
	{
		ConstructionYard,Armory,AetherCore,Augmentor,Aviatrix,FluxArray,EngineeringBay,Academy,Manticore,SteelCraftor,Zephyr,Vulcan,Triton,Chimera,Minigun,ImperioCannon,RepairBay,MortarPod,
		ScrapCycle, BlastDrone, Crackhammer, Chembuchet, Desolator, Dreadnaught, Chainflail, ScrapYard, ChopShop, Subterforge, WastePlant, Foundry, Thrasher, FlakCannon
	}

	public raceType race;
    public Sprite factionIcon;
	[TextArea(3,10)]
	public string RaceDescription;

	public string FHotkeyString;

	public List<GameObject> unitList = new List<GameObject>();
	public List<GameObject> buildingList = new List<GameObject>();
	public List<GameObject> attachmentsList = new List<GameObject>();

	public List<UnitManager> DefaultFOne;
	public List<UnitManager> DefaultFTwo;
	public List<UnitManager> DefaultFThree;
	public List<UnitManager> DefaultFFour;

	public UIImages myUIImages;

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


[System.Serializable]
public class UIImages
{
	public List<Sprite> toReplace;
	public Sprite UltBackGround;
	public Sprite FButtonbackgrounds;
	public Sprite BottomLeftPanel;
	public Sprite BottomCenterPanel;
	public Sprite MinimapBackground;
	public Sprite FTwelveImage;

	public Sprite SupplyImage;
	public string supplyUnit; // get both helper in upper left and in the build queue
	public Sprite ControlGroup;
	public Sprite BasicCommands;
	public Sprite PageTab;
	public Sprite TinyButton;
	public Sprite GrayButton;
	public Sprite BlackButton;
	public Sprite SmallButton;
	public Sprite Panel;
	public Sprite SquareButton;
	public Sprite Plus;
	public Sprite Minus;
	public Sprite BorderlessButton;
}