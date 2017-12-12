using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreExtractor :Ability {
	public float amountPerLoad;

	// Use this for initialization
	void Start () {
		myType = type.passive;
		InvokeRepeating ("extractOre",20,3);
	}

	void extractOre()
	{
		if (this.enabled) {
			PopUpMaker.CreateGlobalPopUp ("+" + amountPerLoad, Color.white, transform.position);
			GameManager.main.playerList [0].updateResources (amountPerLoad, 0, true);
		}
	}

	public override continueOrder canActivate(bool error){

		return new continueOrder ();
	}
	public  override void Activate(){}  // returns whether or not the next unit in the same group should also cast it
	public  override void setAutoCast(bool offOn){}

}
