using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moneyObjective : Objective, ManagerWatcher {

	public Slider moneySlide;
	public RaceManager myRace;
	public Text myText;
	public List<int> halfwayVoiceLines;

	public ResourceType resourceType = ResourceType.Ore;
	public float moneyVictory;

	bool playedHalfWay;

	public new void Start()
	{
		base.Start();

		GameManager.main.activePlayer.addWatcher(this);
	}

	public void updateResources(ResourceManager manager) {
		float currentMoneyAmount = manager.getResource(resourceType);
		if (myText)
		{
			myText.text = (int)currentMoneyAmount + "/" + moneyVictory;
		}
		if (moneySlide)
		{
			moneySlide.value = currentMoneyAmount / moneyVictory;
		}

		if (!playedHalfWay && currentMoneyAmount > moneyVictory / 2)
		{
			playedHalfWay = true;
			if (halfwayVoiceLines.Count > 0)
			{
				dialogManager.instance.playLine(UnityEngine.Random.Range(0, halfwayVoiceLines.Count));
			}
		}

		if (currentMoneyAmount >= moneyVictory)
		{
			complete();
		}
	}


	public void updateSupply(float current, float max)
	{

	}

	public void updateUpgrades() {

	}

}
