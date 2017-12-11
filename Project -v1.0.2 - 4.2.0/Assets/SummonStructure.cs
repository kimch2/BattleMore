using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonStructure : TargetAbility {


	protected Selected mySelect;
	public GameObject ToSummon;
	Coroutine currentCharger;
	UnitManager myManager;
	public bool OnlyOnPathable;

	// Use this for initialization
	void Start () {
		myType = type.target;
		mySelect = GetComponent<Selected> ();
		myManager = GetComponent<UnitManager> ();

	}
		


	override
	public continueOrder canActivate(bool showError){

		continueOrder order = new continueOrder ();

		if (!myCost.canActivate (this)) {
			order.canCast = false;

			// FIX THIS LINE IN THE FUTURE IF IT BREAKS! its currently in here to allow guys with multiple charges to use them even though the cooldown timer is shown.
			if (myCost.energy == 0 && myCost.ResourceOne == 0 && chargeCount > 0) {
				order.canCast = true;
			}
		} else {
			order.nextUnitCast = false;
		}
		if (order.canCast) {
			order.nextUnitCast = false;
		}
		return order;
	}

	override
	public void Activate()
	{
	}  // returns whether or not the next unit in the same group should also cast it


	override
	public  void setAutoCast(bool offOn){}

	public override bool isValidTarget (GameObject target, Vector3 location){

		if (OnlyOnPathable) {
			return onPathableGround (location);
		}
		return true;

	}


	override
	public  bool Cast(GameObject target, Vector3 location)
	{

		Cast ();

		return false;

	}
	override
	public void Cast(){
		myCost.payCost ();
		GameObject inConstruction = null;

		Vector3 pos = location;
		inConstruction = (GameObject)Instantiate (ToSummon, pos, Quaternion.identity);
		UnitManager buildingManager = inConstruction.GetComponent<UnitManager> ();
		BuildingInteractor builder = inConstruction.GetComponent<BuildingInteractor> ();
	
		buildingManager.PlayerOwner = myManager.PlayerOwner;
		builder.startConstruction (ToSummon, 1);
		buildingManager.setInteractor();
		buildingManager.interactor.initialize ();
		inConstruction.GetComponent<Selected> ().Initialize ();
		buildingManager.myStats.SetHealth (.02f);
		while (!builder.ConstructDone ()) {
			builder.construct (10);
		}


	}


}
