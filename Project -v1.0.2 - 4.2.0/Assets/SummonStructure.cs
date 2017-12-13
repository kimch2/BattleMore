using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonStructure :  UnitProduction{


	protected Selected mySelect;

	Coroutine currentCharger;
	UnitManager myManager;
	Vector3 targetLocation;

	public Animator myAnim;
	public float Range;
	// Use this for initialization
	void Start () {
		myType = type.building;
		mySelect = GetComponent<Selected> ();
		myManager = GetComponent<UnitManager> ();

	}
		


	override
	public continueOrder canActivate(bool showError){

		continueOrder order = new continueOrder ();
		/*
		if (UIManager.main.m_ObjectBeingPlaced) {
			if (Vector3.Distance (UIManager.main.m_ObjectBeingPlaced.transform.position, this.transform.position) < Range) {
				order.canCast = false;
			}
		}
*/
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
		myCost.payCost ();
		GameObject inConstruction = null;

		Vector3 pos = targetLocation;

		if (this.gameObject.name.Contains( unitToBuild.name)) {
			//Debug.Log ("Loading unit :" + unitToBuild.name);
			// this is so it can construct itself as a prefab and not a copy of itself
			inConstruction = (GameObject)Instantiate (Resources.Load<GameObject> (unitToBuild.GetComponent<UnitManager>().UnitName), pos, Quaternion.identity);
		} else {
			//Debug.Log ("Second unit " + unitToBuild);
			inConstruction = (GameObject)Instantiate (unitToBuild, pos, Quaternion.identity);
		}

		myAnim.CrossFade ("Summon",3);
		UnitManager buildingManager = inConstruction.GetComponent<UnitManager> ();
		BuildingInteractor builder = inConstruction.GetComponent<BuildingInteractor> ();

		buildingManager.PlayerOwner = myManager.PlayerOwner;
		builder.startConstruction (unitToBuild, 1);
		buildingManager.setInteractor();
		buildingManager.interactor.initialize ();
		inConstruction.GetComponent<Selected> ().Initialize ();
		buildingManager.myStats.SetHealth (.02f);
		builder.startSelfConstruction (unitToBuild,buildTime);

	}  // returns whether or not the next unit in the same group should also cast it


	override
	public  void setAutoCast(bool offOn){}

	public bool isValidTarget (GameObject target, Vector3 location){


		return true;

	}

	public void setBuildSpot(Vector3 buildSpot, GameObject ghostPlacer)
	{targetLocation = buildSpot;
		Destroy (ghostPlacer);
	}
		
	public  bool Cast(GameObject target, Vector3 location)
	{

		Cast ();

		return false;

	}

	public void Cast(){
		myCost.payCost ();
		GameObject inConstruction = null;

		Vector3 pos = targetLocation;
		inConstruction = (GameObject)Instantiate (unitToBuild, pos, Quaternion.identity);
		UnitManager buildingManager = inConstruction.GetComponent<UnitManager> ();
		BuildingInteractor builder = inConstruction.GetComponent<BuildingInteractor> ();
	
		buildingManager.PlayerOwner = myManager.PlayerOwner;
		builder.startConstruction (unitToBuild, 1);
		buildingManager.setInteractor();
		buildingManager.interactor.initialize ();
		inConstruction.GetComponent<Selected> ().Initialize ();
		buildingManager.myStats.SetHealth (.02f);
		builder.startSelfConstruction (unitToBuild,buildTime);


	}


	public override void startBuilding(){}

	public override void DeQueueUnit()
	{

	}


	public override void cancelBuilding ()
	{	

	}

	public override float getProgress ()
	{
		return 0;
	}


}
