using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CarbotPointApplier : MonoBehaviour
{

	public List<Toggle> Units;
	public List<Toggle> Ults;
	public List<Toggle> Upgrades;
	public List<Toggle> Supply;
	public List<Toggle> VictoryPoints;

	public CarbotOverlord overlord;

	public Canvas IntroScreen;
	public Canvas AdvancedScreen;

	public InputField VicPoints;
	public InputField TimeExp;
	public InputField FirstLevelUp;
	public InputField LevelUpIncrement;

	public GameObject UpgradeTemplate;


	private void Start()
	{
		Time.timeScale = 0;
		foreach (GameObject obj in overlord.PlayerList[0].UpgradeRacks)
		{
			foreach (ResearchUpgrade up in obj.GetComponents<ResearchUpgrade>())
			{
				GameObject newButton = Instantiate(UpgradeTemplate, UpgradeTemplate.transform.parent);
				Button b = newButton.GetComponent<Button>();
				b.image.sprite = up.iconPic;
				b.GetComponentInChildren<Text>().text = up.Descripton;
				b.onClick.AddListener(delegate ()
				{
					Research( newButton.transform.GetSiblingIndex() -1);
				});
				overlord.PlayerList[0].UpgradeList.Add(up);
			}

		}

		foreach (GameObject obj in overlord.PlayerList[1].UpgradeRacks)
		{
			foreach (ResearchUpgrade up in obj.GetComponents<ResearchUpgrade>())
			{
				overlord.PlayerList[1].UpgradeList.Add(up);
			}
		}
		UpgradeTemplate.SetActive(false);
	}

	public void Finish()
	{
		Time.timeScale = 1;
		AdvancedScreen.enabled = false;
		IntroScreen.enabled = false;
		AddPointCategories();
		overlord.VictoryLevel = int.Parse(VicPoints.text);
		overlord.PassiveExpPerSec = float.Parse(TimeExp.text);
		for (int i = 0; i < overlord.VictoryLevel + 10; i++)
		{
			overlord.levelThresholds.Add(float.Parse(FirstLevelUp.text) + i * float.Parse(LevelUpIncrement.text));
		}
	}


	void AddPointCategories()
	{
		AddCategory(CarbotOverlord.RewardType.UnlockUnits, Units);
		AddCategory(CarbotOverlord.RewardType.UnlockUlts, Ults);
		AddCategory(CarbotOverlord.RewardType.Upgrades, Upgrades);
		AddCategory(CarbotOverlord.RewardType.Supply, Supply);
		AddCategory(CarbotOverlord.RewardType.VictoryPoints, VictoryPoints);
	}

	void Research(int i)
	{
		overlord.PlayerList[0].purchaseUpgrade(i);
	}

	void AddCategory(CarbotOverlord.RewardType rewardType, List<Toggle> toApply)
	{

		if (toApply[0].isOn)
		{
			overlord.FirstNexus.Add(rewardType);
		}
		if (toApply[1].isOn)
		{
			overlord.SecondNexus.Add(rewardType);
		}
		if (toApply[2].isOn)
		{
			overlord.ThirdNexus.Add(rewardType);
		}
		if (toApply[3].isOn)
		{
			overlord.EnemyTowers.Add(rewardType);
		}
		if (toApply[4].isOn)
		{
			overlord.KillEnemies.Add(rewardType);
		}
		if (toApply[5].isOn)
		{
			overlord.Time.Add(rewardType);
		}
	}


}
