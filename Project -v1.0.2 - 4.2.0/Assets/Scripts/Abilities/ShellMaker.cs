using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellMaker : Ability{


	ArtilleryBastion bastion;


	bool turnedOn;
	// Use this for initialization
	new void Start () {
		myType = type.activated;
		bastion= GameObject.FindObjectOfType<ArtilleryBastion> ();
	}


	public override void setAutoCast(bool offOn){
	}



	override
	public continueOrder canActivate (bool showError)
	{
		continueOrder order = new continueOrder ();
		return order;
	}


	override
	public void Activate()
	{


		turnedOn = !turnedOn;
		autocast = turnedOn;

		updateAutocastCommandCard();

		if (!makingShell) {
			makeShell ();
		}


	}

	bool makingShell;

	void makeShell()
	{
		bastion.changeCharge (1);

		/*

		if (turnedOn) {
			if (racer.ResourceOne >= myCost.ResourceOne) {
				myCost.payCost ();

				Invoke ("makeShell", myCost.cooldown + .01f);
				makingShell = true;
				return;
			} else {
				Invoke ("makeShell",1);
				makingShell = true;
				return;
			}
		}*/
		makingShell = false;

	}











}
