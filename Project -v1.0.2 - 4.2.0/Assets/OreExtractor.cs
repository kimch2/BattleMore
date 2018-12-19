using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreExtractor :Ability {


	[Tooltip("CurrentAmount for amount of each load")]
	public ResourceManager toCollect;
	// Use this for initialization
	new void Start () {
		base.Start();
		myType = type.passive;
		InvokeRepeating ("extractOre",3,3);
	}


	void extractOre()
	{
		if (this.enabled) {
			toCollect.showPopups(transform.position, true);
			GameManager.main.playerList[0].collectResources(toCollect.MyResources);
		}
	}

	public override continueOrder canActivate(bool error){

		return new continueOrder ();
	}
	public  override void Activate(){}  // returns whether or not the next unit in the same group should also cast it
	public  override void setAutoCast(bool offOn){}

}
