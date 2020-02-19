using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EconomyManager : MonoBehaviour {

	private RaceManager racer;

	public Transform ResourceGrid;

	public static EconomyManager main;

	private Dictionary<ResourceType, Text> ResourceMap = new Dictionary<ResourceType, Text>();
	private Dictionary<ResourceType, Dictionary<float, int>> updateMap = new Dictionary<ResourceType, Dictionary<float, int>>();

	private void Awake()
	{
		main = this;
	}

	// Use this for initialization
	void Start () {
		racer = GameManager.main.playerList [0];
		InvokeRepeating ("updateAverage", 1,4);  // ?? FIX THIS
	}

    public static EconomyManager getInstance()
    {
        if (main == null)
        {
            main = GameObject.FindObjectOfType<EconomyManager>();

        }
        return main;
    }

    public void AddResourceType(ResourceType typ)
	{

		GameObject obj = new GameObject("Income Rate");
		Text text =  obj.AddComponent<Text>();
		((RectTransform)obj.transform).sizeDelta = new Vector2(60,24);
		text.fontSize = 13;
		

		text.font = RaceUIManager.instance.supply.font;
		foreach (Transform t in RaceUIManager.getInstance().ResourceGrid.transform)
		{
			if (t.GetComponentInChildren<Image>().sprite == UnitEquivalance.getResourceInfo(typ).icon)
			{
				obj.transform.SetParent(t);
			}
		}

		obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = new Vector3(38, -21, 0);
		//obj.transform.SetParent(RaceUIManager.instance.ResourceGrid.parent);
		
		ResourceMap.Add(typ, text);
		updateMap.Add(typ, new Dictionary<float, int> ());

	}

	public void updateResource(ResourceType theType, float currentAmount, float changeAmount)
	{
		if (updateMap[theType].ContainsKey(Time.time))
		{
			updateMap[theType][Time.time] += (int)changeAmount;
		}
		else
		{
			updateMap[theType].Add(Time.time, (int)changeAmount);
		}
	}

	void updateAverage()
	{
		List<float> deleteThese = new List<float>();

		

		foreach (KeyValuePair<ResourceType, Dictionary<float, int>> pair in updateMap)
		{
			int totalResOne = 0;
			foreach (KeyValuePair<float, int> entry in pair.Value)
			{
				if (entry.Key + 15.1f > Time.time)
				{
					totalResOne += entry.Value;
				}
				else
				{
					deleteThese.Add(entry.Key);
				}
			}

			foreach (float f in deleteThese)
			{
				pair.Value.Remove(f);
			}
			ResourceMap[pair.Key].text = "+" + (totalResOne * 4);

		}
	}



}
