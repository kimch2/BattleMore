using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEquivalance : MonoBehaviour {


    public  List<Composition> myComps =  new List<Composition>();
    public List<RaceInfo> raceInfos;

	public List<ResourceInfo> resourceInfos = new List<ResourceInfo>();



	public static UnitEquivalance main;

	public static ResourceInfo getResourceInfo(ResourceType resType)
	{
		if (main == null)
		{
			main = Resources.Load<GameObject>("RaceInfoPacket").GetComponent<UnitEquivalance>();
		}

		return main.resourceInfos.Find(item => item.resType == resType);
	}

    public RaceInfo getRace(RaceInfo.raceType type)
    {
        foreach (RaceInfo info in raceInfos)
        {
            if (info.race == type)
            {
                return info;
            }
        }
        return null;
    }


}

[System.Serializable]
public class Composition
{
    public string CompositionName = "Place Holder";
    public bool exposed;
	public int ID;
    public Color GizmoColor = Color.white;
  public  List<UnitPile> RacePiles = new List<UnitPile>();

	public Composition(int id)
	{
		ID = id;
	}
}


[System.Serializable]
public class UnitPile
{
    public RaceInfo.raceType myRace;
    public int ID;
    [Tooltip("Normally only one guy should be in here but exceptions are made for races with lots of swarmy guys")]
    public List<GameObject> units = new List<GameObject>(); 


}

[System.Serializable]
public class ResourceInfo
{

	public ResourceType resType;
	public Color ResourceColor = Color.gray;
	public Sprite icon;
}