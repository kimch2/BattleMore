using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarbotOverlord : MonoBehaviour, LethalDamageinterface
{
	
	public List<PlayerProgress> PlayerList;

	public float supplyPerLevel = 8;
	public float PassiveExpPerSec = 1;

	public List<float> levelThresholds;
	public int VictoryLevel = 12;

	public enum RewardType {UnlockUnits, UnlockUlts, Upgrades, Supply, VictoryPoints }

	public List<RewardType> FirstNexus;
	public List<RewardType> SecondNexus;
	public List<RewardType> ThirdNexus;
	public List<RewardType> EnemyTowers;
	public List<RewardType> KillEnemies;
	public List<RewardType> Time;


	public CarbotPointApplier pointsApplier;
	public List<Button> BuyUnitButtons;
	public List<Button> UltButtons;

	public static CarbotOverlord main;


	private void Start()
	{
		main = this;
		GameManager.main.playerList[0].addActualDeathWatcher(this);
		GameManager.main.playerList[1].addActualDeathWatcher(this);
		InvokeRepeating("TimePassed", 1,.5f);
	}

	public bool lethalDamageTrigger(UnitManager Unit, GameObject deathSource)
	{
		if (Unit.PlayerOwner == 1)
		{
			KillEnemy(2, Unit.myStats.supply);
		}
		else
		{
			KillEnemy(1, Unit.myStats.supply);
		}
		return true;
	}

	public void purchaseUnit(int i)
	{
		PlayerList[0].purchaseUnit(i);
	}

	public void purchaseUlt(int i)
	{
		PlayerList[0].purchaseUlt(i);
	}

	void ParsePoints(List<RewardType> rewards,int playerNumber, float number)
	{
		foreach (RewardType reward in rewards)
		{
			if
				(reward == RewardType.Supply) {
				PlayerList[playerNumber - 1].unlockSupplyPoints(number);
			}
			else if (reward == RewardType.UnlockUlts)
			{
				PlayerList[playerNumber - 1].unlockUltPoints(number);
			}
			else if (reward == RewardType.UnlockUnits) {
				PlayerList[playerNumber - 1].unlockUnitPoints(number);
			}
			else if (reward == RewardType.Upgrades) {
				PlayerList[playerNumber - 1].unlockUpgradesPoints(number);
			}
			else if (reward == RewardType.VictoryPoints) { 
					PlayerList[playerNumber - 1].unlockVictoryPoints(number);
			}
		}
	}

	void KillEnemy(int playerNumber, float supply)
	{
		ParsePoints(KillEnemies, playerNumber, supply);
	}

	public void TimePassed()
	{
		ParsePoints(Time, 1, PassiveExpPerSec/2);
		ParsePoints(Time, 2, PassiveExpPerSec/2);
	}


	public void TakeFirstNexus(int playerNumber, float points)
	{
		ParsePoints(FirstNexus, playerNumber, points);
	}

	public void TakeSecondNexus(int playerNumber, float points)
	{
		ParsePoints(SecondNexus, playerNumber, points);
	}

	public void TakeThirdNexus(int playerNumber, float points)
	{
		ParsePoints(ThirdNexus, playerNumber, points);
	}

	public void TakeEnemyTowers(int playerNumber)
	{
		Debug.Log("Here");
		ParsePoints(EnemyTowers, playerNumber, levelThresholds[PlayerList[playerNumber - 1].SupplyPlayerLevel]);

	}

}

[System.Serializable]
public class PlayerProgress
{
	public CarbotOverlord overlord;
	public int ResearchAvailable;
	public UnitManager ProductionStructure;
	public RaceManager racer;
	public Slider experienceBar;
	public Text levelNumber;
	public List<GameObject> UpgradeRacks;
	public List<Text> unitsAvailableText;
	public List<Text> ultsAvailableText;
	public List<Text> upgradesAvailableText;
	public CarbotAI myAI;
	public List<ResearchUpgrade> UpgradeList;

	public int UnitPlayerLevel;
	public float Unitexperience;
	public int UnitsAvailable;
	public Slider UnitBar;

	public int UltPlayerLevel;
	public float UltExperience;
	public int UltsAvailable;
	public Slider UltBar;

	public int UpgradesPlayerLevel;
	public float UpgradesExperience;
	public int UpgradesAvailable;
	public Slider UpgradeBar;

	public int SupplyPlayerLevel = 1;
	public float Supplyexperience;

	public int VictoryPlayerLevel = 1;
	public float VictoryExperience;

	public AudioTub OnePointAway;
	public AudioTub newUnitAudio;
	public AudioTub newUltAudio;
	public AudioTub newUpgradeAudio;
	public AudioTub supplyAudio;

	public int gainExperience(float amount, ref float targetExp, ref int targetLevel, Text UIdisplay, Slider UISlider)
	{
		targetExp += amount;
		int levelIncrease = 0;
		while (targetExp > overlord.levelThresholds[targetLevel])
		{
			targetExp -= overlord.levelThresholds[targetLevel];
			targetLevel++;
			levelIncrease++;
			
			if (UIdisplay)
			{
				UIdisplay.text = "" + targetLevel;
			}
		}

		if (UISlider)
		{
			UISlider.value = targetExp / overlord.levelThresholds[targetLevel];
		}
		return levelIncrease;
	}

	void gainFullPoint(ref float targetExp, ref int targetLevel, Text UIdisplay, Slider UISlider)
	{
		gainExperience(overlord.levelThresholds[targetLevel], ref targetExp, ref targetLevel, UIdisplay, UISlider);
	}


	//====================================
	public void unlockUnitPoints(float m)
	{ int i = UnitsAvailable;
		UnitsAvailable += gainExperience(m, ref Unitexperience, ref UnitPlayerLevel, null, UnitBar);
		if (i != UnitsAvailable)
		{
			foreach (Text t in unitsAvailableText) {
				t.text = "" + UnitsAvailable;
			}
			if (myAI)
			{
				myAI.checkNewUnits();
			}
			else
			{
				AudioClip clip = newUnitAudio.getRandomClip();
				ExpositionDisplayer.instance.displayText("", clip.length, clip, .93f, null, 3);
			}
		}
	}
	public void unlockUnitFull()
	{
		UnitsAvailable++;
		gainFullPoint(ref Unitexperience, ref UnitPlayerLevel, null, UnitBar);
		foreach (Text t in unitsAvailableText)
		{
			t.text = "" + UnitsAvailable;
		}
		if (myAI)
		{
			myAI.checkNewUnits();
		}
		else
		{
			AudioClip clip = newUnitAudio.getRandomClip();
			ExpositionDisplayer.instance.displayText("", clip.length, clip, .93f, null, 3);
		}
	}


	//================================

	public void unlockUltPoints(float m)
	{
		int i = UltsAvailable;
		UltsAvailable += gainExperience(m, ref UltExperience, ref UltPlayerLevel, null, UltBar);
		if (i != UltsAvailable)
		{
			foreach (Text t in ultsAvailableText)
			{
				t.text = "" + UltsAvailable;
			}
			if (myAI)
			{
				myAI.checkNewUlt();
			}
			else
			{
				AudioClip clip = newUltAudio.getRandomClip();
				ExpositionDisplayer.instance.displayText("", clip.length, clip, .93f, null, 3);
			}
		}
	}
	public void unlockUltFull()
	{
		gainFullPoint(ref UltExperience, ref UltPlayerLevel, null, UltBar);
		UltsAvailable++;
		foreach (Text t in ultsAvailableText)
		{
			t.text = "" + UltsAvailable;
		}
		if (myAI)
		{
			myAI.checkNewUlt();
		}
		else
		{
			AudioClip clip = newUltAudio.getRandomClip();
			ExpositionDisplayer.instance.displayText("", clip.length, clip, .93f, null, 3);
		}
	}
	//================================

	public void unlockUpgradesPoints(float m)
	{
		int i = UpgradesAvailable;
		UpgradesAvailable += gainExperience(m, ref UpgradesExperience, ref UpgradesPlayerLevel, null, UpgradeBar);
		if (i != UpgradesAvailable)
		{
			foreach (Text t in upgradesAvailableText)
			{
				t.text = "" + UpgradesAvailable;
			}
			if (myAI)
			{
				myAI.checkForUpgrades();
			}
			else
			{
				AudioClip clip = newUpgradeAudio.getRandomClip();
				ExpositionDisplayer.instance.displayText("", clip.length, clip, .93f, null, 3);
			}
		}

	}
	public void unlockUpgradeFull()
	{
		gainFullPoint(ref UpgradesExperience, ref UpgradesPlayerLevel, null, UpgradeBar);
		UpgradesAvailable++;
		foreach (Text t in upgradesAvailableText)
		{
			t.text = "" + UpgradesAvailable;
		}
		if (myAI)
		{
			myAI.checkForUpgrades();
		}
		else
		{
			AudioClip clip = newUpgradeAudio.getRandomClip();
			ExpositionDisplayer.instance.displayText("", clip.length, clip, .93f, null, 3);
		}
	}
	//================================
	public void unlockSupplyPoints(float m)
	{
		int i=	gainExperience(m, ref Supplyexperience, ref SupplyPlayerLevel, null, null);
		if (i > 0)
		{
			racer.UnitCreated(overlord.supplyPerLevel * i * -1);
			if (!myAI)
			{
				AudioClip clip = supplyAudio.getRandomClip();
				ExpositionDisplayer.instance.displayText("", clip.length, clip, .93f, null, 3);
			}
		}
	
	}
	public void unlockSupplyFull()
	{
		gainFullPoint(ref Supplyexperience, ref SupplyPlayerLevel, null, null);
		racer.UnitCreated(overlord.supplyPerLevel);
	}
	//================================
	public void unlockVictoryPoints(float m)
	{
		int i = gainExperience(m, ref VictoryExperience, ref VictoryPlayerLevel, levelNumber, experienceBar);

		if (VictoryPlayerLevel >= overlord.VictoryLevel)
		{
			if (racer.playerNumber == 1)
			{
				VictoryTrigger.instance.Win();
			}
			else
			{
				VictoryTrigger.instance.Lose();
			}
		}
		else if (i >0 && VictoryPlayerLevel == overlord.VictoryLevel - 1)
		{
			if (!myAI)
			{ 
				AudioClip clip =OnePointAway.getRandomClip();
				ExpositionDisplayer.instance.displayText("", clip.length, clip, .93f, null, 3);
			}
		}

	}
	public void unlockVictoyFull()
	{
		gainFullPoint(ref VictoryExperience, ref VictoryPlayerLevel, null, null);

	}


	public void purchaseUnit(int i)
	{
		if (UnitsAvailable > 0)
		{
			
			UnitsAvailable--;
			ProductionStructure.abilityList[i].active = true;
			if (racer.playerNumber == 1)
			{
				overlord.BuyUnitButtons[i - 1].interactable = false;
				RaceManager.updateActivity();
			}
		}
		foreach (Text t in unitsAvailableText)
		{
			t.text = "" + UnitsAvailable;
		}
	}

	public void purchaseUlt(int i)
	{
		if (UltsAvailable > 0)
		{
			if (racer.playerNumber == 1)
			{
				overlord.UltButtons[i].interactable = false;
				UISetter.main.EnableUlt(i, true);
			}
				UltsAvailable--;
			
		}
		foreach (Text t in ultsAvailableText)
		{
			t.text = "" + UltsAvailable;
		}
	}

	public void purchaseUpgrade(int i)
	{
		if (UpgradesAvailable > 0)
		{
			if (racer.playerNumber == 1)
			{
				overlord.pointsApplier.UpgradeTemplate.transform.parent.GetComponentsInChildren<Button>()[i].interactable = false;
			}

			Debug.Log("Buying " + i);
			racer.addUpgrade(UpgradeList[i].upgrades[0], "");
			
			UpgradesAvailable--;

		}

		foreach (Text t in upgradesAvailableText)
		{
			t.text = "" + UpgradesAvailable;
		}
	}
}
