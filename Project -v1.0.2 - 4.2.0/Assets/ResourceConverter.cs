using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceConverter : Ability
{

	public UnityEngine.Events.UnityEvent OnActivate;
	public UnityEngine.Events.UnityEvent Deacactivate;
	public ResourceTank from;
	public ResourceTank To;

	[Tooltip("Whats the minimum amount before this will autocast")]
	public float minimumResFromAmount = 200;

	new void Start()
	{
		base.Start();
		myType = type.activated;
		InvokeRepeating("Convert", 4 + UnityEngine.Random.value,5);
	}


	void Convert()
	{
		if (active && autocast)
		{
			if (GameManager.main.playerList[0].resourceManager.getResource(from.resType) > minimumResFromAmount)
			{
				PopUpMaker.CreateGlobalPopUp("" + from.currentAmount, UnitEquivalance.getResourceInfo(from.resType).ResourceColor, transform.position + Vector3.up * 3);
				PopUpMaker.CreateGlobalPopUp("+" + To.currentAmount, UnitEquivalance.getResourceInfo(To.resType).ResourceColor, transform.position);
				GameManager.main.playerList[0].collectOneResource(from, true);
				GameManager.main.playerList[0].collectOneResource(To, true);
			}
		}
	}

	public void TurnOff()
	{
		autocast = false;
		updateAutocastCommandCard();
	}

	public override continueOrder canActivate(bool error)
	{

		return new continueOrder();
	}
	public override void Activate() {
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


	}  // returns whether or not the next unit in the same group should also cast it
	public override void setAutoCast(bool offOn) {
		autocast = !autocast;
		updateAutocastCommandCard();
	}

}
