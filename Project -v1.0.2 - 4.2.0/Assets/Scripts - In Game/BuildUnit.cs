using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildUnit : UnitProduction {



	private BuildingInteractor myInteractor;

	private float timer =0;
	private bool buildingUnit = false;
	private UnitManager myManage;
	private HealthDisplay HD;
	private BuildManager buildMan;

	public Vector3 SpawnOffset = new Vector3(4, 4, -7);

	private int QueueNum;

	[Tooltip("object that shows up while the unit is building, can be null")]
	public GameObject constObject;
    // Use this for initialization

    new void Awake()
	{base.Awake ();

		myType = type.activated;
		myManage = GetComponent<UnitManager> ();
	}



    new void Start () {
		buildMan = GetComponent<BuildManager> ();
		myInteractor = GetComponent <BuildingInteractor> ();
		HD = GetComponentInChildren<HealthDisplay>();
	}
	
	// Update is called once per frame
	void Update () {
		if (buildingUnit) {

			timer -= Time.deltaTime * buildRate;

			select.updateCoolDown (1- timer/buildTime);
			if(timer <=0)
			{
				select.updateCoolDown (0);
				
				buildingUnit = false;
				createUnit();
			}
		}	
	}

	public bool isBuilding()
	{
		return buildingUnit;
	}

	public override void setAutoCast(bool offOn){}

	public override void DeQueueUnit()
	{myCost.refundCost ();
		myCost.showCostPopUp(true);

	}



	public override float getProgress ()
	{return (1 - timer/buildTime);}


	public override void cancelBuilding ()
	{HD.stopBuilding ();
		select.updateCoolDown (0);
		timer = 0;
		buildingUnit = false;

		if (!buildMan.waitingOnSupply) {
	
			myManager.myRacer.UnitDied (unitToBuild.GetComponent<UnitStats> ().supply, null);
		}

		myManager.myRacer.stopBuildingUnit (this);
		if (constObject) {
			constObject.SetActive (false);
		}
	}



	override
	public continueOrder canActivate (bool showError)
	{
		
		continueOrder order = new continueOrder();
		order.nextUnitCast = false;


		if (myCost && !myCost.canActivate (this, order, showError)) {
			order.canCast = false;
		}
		if (!active) {
			order.reasonList.Add (continueOrder.reason.requirement);
		}

		return order;


	}
	
	override
		public void Activate()
	{

		if (myCost.canActivate (this)) {


			if (buildMan.buildUnit (this)) {
				myCost.payCost();
				myCost.resetCoolDown ();
				myCost.showCostPopUp(false);
			}
		}

	}


	override
	public  void startBuilding()
	{

		if (constObject) {
			constObject.SetActive (true);
		}

		HD.loadIMage(unitToBuild.GetComponent<UnitStats> ().Icon);
		timer = buildTime;
		myManager.myRacer.UnitCreated (unitToBuild.GetComponent<UnitStats> ().supply);

		buildingUnit = true;
		myManager.myRacer.buildingUnit (this);
	}



	public void createUnit()
	{

		if (constObject) {
			constObject.SetActive (false);
		}
		HD.stopBuilding ();
		Vector3 location = this.gameObject.transform.position + SpawnOffset;

		GameObject unit = (GameObject)Instantiate(unitToBuild, location, Quaternion.identity);
		unit.transform.LookAt (location + Vector3.right + Vector3.back);
		UnitManager unitMan = unit.GetComponent<UnitManager> ();
		unitMan.PlayerOwner = myManage.PlayerOwner;

		unitMan.setInteractor();
		unitMan.interactor.initializeInteractor();
        if (myInteractor != null) {

			//Sends units outside of the Construction yard, so it looks like they were built inside.
			unitMan.GiveOrder(Orders.CreateAttackMove(transform.position + SpawnOffset * 1.5f));
			//Debug.Log ("ordering to move " + new Vector3(this.gameObject.transform.position.x +10,this.gameObject.transform.position.y+4,this.gameObject.transform.position.z -16));
			//Queue a command if they have a rally point or unit
			if (myInteractor.rallyUnit != null) {
				
				unitMan.GiveOrder (Orders.CreateFollowCommand(myInteractor.rallyUnit,true));
			} 
			else if (myInteractor.rallyPoint != Vector3.zero) {
				unitMan.GiveOrder (Orders.CreateAttackMove (myInteractor.rallyPoint,true));

			//	Debug.Log ("Giving Rally Command");
			}
			timer = buildTime;
		}

		
		myManager.myRacer.stopBuildingUnit (this);
		//racer.applyUpgrade (unit);
		buildingUnit = false;
		buildMan.unitFinished (this);
	
	//	manage.changeState (new DefaultState ());//.enQueueState (new DefaultState ());
		}



	private void OnDrawGizmos()
	{
		Gizmos.DrawSphere(transform.position + SpawnOffset, .3f);
	}

    public override void InitializeGhostPlacer(GameObject ghost)
    { }
}
