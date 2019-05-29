using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateAccelerator : Ability
{



	public UnityEngine.Events.UnityEvent OnActivate;
	public UnityEngine.Events.UnityEvent Deacactivate;
	public ResourceTank from;
	public bool UltimateOne;
	public bool UltimateTwo;
	public bool UltimateThree;
	public bool UltimateFour;
	public float CovnersionPeriod = 5;
	RaceManager racer;

	[Tooltip("Whats the minimum amount before this will autocast")]
	public float minimumResFromAmount = 200;

	new void Start()
	{
		myType = type.activated;
		InvokeRepeating("Convert", CovnersionPeriod + UnityEngine.Random.value, CovnersionPeriod);

		racer = GameManager.main.playerList[GetComponent<UnitManager>().PlayerOwner - 1];
	}


	void Convert()
	{
		if (active && autocast)
		{
			if (racer.resourceManager.getResource(from.resType) > minimumResFromAmount)
			{
				PopUpMaker.CreateGlobalPopUp("-" + from.currentAmount, UnitEquivalance.getResourceInfo(from.resType).ResourceColor, transform.position + Vector3.up * 3);			
				GameManager.main.playerList[0].PayCost(from);
				if (UltimateOne)
				{
					racer.UltOne.myCost.cooldownTimer -= 2f;
				}
				if (UltimateTwo)
				{
					racer.UltTwo.myCost.cooldownTimer -= 2f;
				}
				if (UltimateThree)
				{
					racer.UltThree.myCost.cooldownTimer -= 2f;
				}
				if (UltimateFour)
				{
					racer.UltFour.myCost.cooldownTimer -= 2f;
				}
			}
		}
	}

	public override continueOrder canActivate(bool error)
	{

		return new continueOrder();
	}
	public void TurnOff()
	{
		autocast = false;
		updateAutocastCommandCard();
	}

	public override void Activate()
	{
		autocast = !autocast;

		if (autocast)
		{
			OnActivate.Invoke();
		}
		else
		{
			Deacactivate.Invoke();
		}
		updateAutocastCommandCard();
	}  
	
	// returns whether or not the next unit in the same group should also cast it
	public override void setAutoCast(bool offOn) {
		autocast = !autocast;
		updateAutocastCommandCard();
	}

}