﻿using UnityEngine;
using System.Collections;

public class BuildStructure:  UnitProduction {

	//private BuildingInteractor myInteractor;

	private RaceManager racer;


	private bool Morphing = false;
	private HealthDisplay HD;
	private BuildManager buildMan;
	Vector3 targetLocation;

	private UnitManager inConstruction;
	private BuildingInteractor builder;
	public float animationRate =1;

	public ParticleSystem myEffect;
	new void Awake()
	{base.Awake ();
		audioSrc = GetComponent<AudioSource> ();
		myType = type.building;
		buildMan = GetComponent<BuildManager> ();
	}

	bool hasPaid;

	// Use this for initialization
	new void Start () {

	
		racer = GameObject.FindObjectOfType<GameManager>().activePlayer;
		HD = GetComponentInChildren<HealthDisplay>(true);
	}

	// Update is called once per frame
	void Update () {
		if (Morphing) {

			if (!builder) {
				cancelBuilding ();
				return;
			}

			float percent = builder.construct (Time.deltaTime / buildTime);
			if (percent >= 1) {
				select.updateCoolDown (0);
				HD.stopBuilding ();
				Morphing = false;
				createUnit ();
				myEffect.Stop ();

				if (myManager.getStateCount () == 0 && myManager.getState() is DefaultState) {
					RaycastHit hit;		
					if (Physics.Raycast ((this.gameObject.transform.position + Vector3.right * 14), Vector3.down, out hit, Mathf.Infinity, ~(1 << 16))) {
					
						Vector3 attackMovePoint = hit.point;
						myManager.GiveOrder (Orders.CreateMoveOrder (attackMovePoint));
					}
				}
			} else {
                select.updateCoolDown (percent);

			}


		}

	} 
	// this only halts construction
	public void cancel()
	{
		myEffect.Stop ();
        select.updateCoolDown (0);
		HD.stopBuilding ();
		Morphing = false;
		//myManager.setStun (false, this, false);

		myManager.changeState(new DefaultState());

		//builder.cancelBuilding ();
		if (select.IsSelected) {
			SelectedManager.main.updateUI ();
		}
	}

	public override void setAutoCast(bool offOn){}

	public void setBuildSpot(Vector3 buildSpot)
	{//Debug.Log ("Set build spot " + buildSpot ) ;
		targetLocation = buildSpot;
	}


	public override void DeQueueUnit()
	{
		
		myCost.refundCost ();
		myCost.showCostPopUp(true);
	}


	public override void cancelBuilding ()
	{	
		myEffect.Stop ();
		racer.stopBuildingUnit (this);
		HD.stopBuilding ();
        select.updateCoolDown (0);

		Morphing = false;

		racer.stopBuildingUnit (this);
	//	myManager.setStun (false, this, false);
		myManager.changeState(new DefaultState());

		if (inConstruction) {
			Destroy (inConstruction.gameObject);
		}
		if (select.IsSelected) {
			SelectedManager.main.updateUI ();
		}
	}

	public override float getProgress ()
	{return builder.getProgess();}

	override
	public continueOrder canActivate (bool showError)
	{

		continueOrder order = new continueOrder();


		if (myManager.getState() is ChannelState) {
			
		//	order.canCast = false;
		}

		if (!myCost.canActivate (this, order,showError)) {

			order.canCast = false;
		}
		if (!active) {
			order.reasonList.Add (continueOrder.reason.requirement);
		}
		if (order.canCast) {
			order.nextUnitCast = false;
		}
		return order;
	}

	public void Dying()
	{

		if (myManager.getState () is PlaceBuildingState) {
			((PlaceBuildingState)myManager.getState ()).cancel ();
			myManager.GiveOrder (Orders.CreateStopOrder ());
		}
	
	}

	override
	public void Activate()
	{

		if (!Morphing) {
			HD.loadIMage (iconPic);


			//myCost.payCost ();
			buildMan.buildUnit (this);
			myManager.cMover.stop ();
			myEffect.Play ();

			Morphing = true;
			racer.buildingUnit (this);

			myManager.changeState (new ChannelState (),true,false);
			//myManager.setStun (true, this, false);
			if (select.IsSelected) {
				SelectedManager.main.updateUI ();
			}
			inConstruction = ((GameObject)Instantiate(unitToBuild, targetLocation, Quaternion.identity)).GetComponent<UnitManager>();

			builder = inConstruction.GetComponent<BuildingInteractor> ();
			if (!builder) {
				builder = (BuildingInteractor)inConstruction.GetComponent<ArmoryInteractor> ();

			} 
			builder.startConstruction (unitToBuild, animationRate);
			inConstruction.setInteractor();
			inConstruction.interactor.initializeInteractor();
			inConstruction.GetComponent<Selected> ().Initialize ();
			inConstruction.myStats.SetHealth (.02f);
		} 
	}



	public override void startBuilding(){}

	public void createUnit()
	{HD.stopBuilding ();


        select.updateCoolDown (0);

		GameManager.main.playerList[myManager.PlayerOwner-1].UnitCreated(unitToBuild.GetComponent<UnitStats> ().supply);
		//myManager.setStun (false, this, false);
		myManager.changeState(new DefaultState());
		racer.stopBuildingUnit (this);
	

		buildMan.unitFinished (this);
		Morphing = false;
		if (inConstruction.GetComponent<Selected>().IsSelected || GetComponent<Selected>().IsSelected){
			SelectedManager.main.updateUI ();
		}
		inConstruction = null;

	}

	public void resumeBuilding(GameObject obj)
	{
		
		HD.loadIMage (iconPic);
		myEffect.Play ();

		//Debug.Log ("Activating");

		buildMan.buildUnit (this);
		myManager.cMover.stop ();

	
		Morphing = true;

		myManager.changeState (new ChannelState (), false, false);
		//myManager.setStun (true, this, false);
		if (select.IsSelected) {
			SelectedManager.main.updateUI ();
		}
		inConstruction = obj.GetComponent<UnitManager>();
		builder = inConstruction.GetComponent<BuildingInteractor> ();
	
	}

    public override void InitializeGhostPlacer(GameObject ghost)
    { }

}
