﻿using UnityEngine;
using System.Collections;
using Pathfinding;

public class BuildingInteractor : MonoBehaviour, Iinteract {

	private UnitManager myManager;

	public Vector3 rallyPoint = Vector3.zero;
	public GameObject rallyUnit;
	public bool AttackMoveSpawn;
	public GameObject RallyPointObj;
	public LineRenderer myLine;
	private float underConstruction;
	private bool doneConstruction = true;

	private GameObject sourceObj;
	public Animator myAnim;

	float animSpeed = 1;

	//private float buildTime;
	// Last time someone did a construction action, for animation tracking
	private float lastBuildInput;

	private Coroutine currentCoRoutine;
	// Use this for initialization

	void Awake()
	{
		myManager = GetComponent<UnitManager> ();
		myManager.setInteractor (this);
	}

	bool initialized = false;

	void Start () {
		if (!initialized) {
			initialized = true;
			GraphUpdateObject b = new GraphUpdateObject (GetComponent<CharacterController> ().bounds); 
			StartCoroutine (DeathRescan (b));

			if (Clock.main.getTotalSecond () < 3) {
				doneConstruction = true;
				underConstruction = 1;

			} else {
				myManager.metaStatus.Stun(myManager, this, true);
			}
			if (!myAnim) {
				myAnim = GetComponentInChildren<Animator> ();
			}
			if (GameManager.main.playerList [myManager.PlayerOwner - 1].upgradeBall) {
				AbilityDisabler disa = GameManager.main.playerList [myManager.PlayerOwner - 1].upgradeBall.GetComponent<AbilityDisabler> ();
				if (disa) {
					disa.applyUpgrade (this.gameObject);
				}
			}
		}
	}

	public void cancelBuilding()
	{
		myManager.myStats.kill (null);

	}


    public void initializeInteractor()
    {
		Start ();
	}


	public void startConstruction(GameObject obj, float animationRate)
	{sourceObj = obj;
		doneConstruction = false;
		//buildTime = buildtime;

		//GameManager.main.playerList [myManager.PlayerOwner - 1].addUnit (myManager);
		foreach (Ability ab in  GetComponent<UnitManager>().abilityList) {
			if (ab) {
				ab.active = false;
			}
			//ab.enabled = false;
		}
		if(myAnim)
		{myAnim.Play ("Construct");
			animSpeed = animationRate;
			myAnim.speed = animationRate;
		}
	}

	public void startSelfConstruction(GameObject obj, float constructionTime)
	{
		//GameManager.main.playerList [myManager.PlayerOwner - 1].addUnit (myManager);

		sourceObj = obj;
		doneConstruction = false;
		//buildTime = buildtime;
	
		foreach (Ability ab in  GetComponent<UnitManager>().abilityList) {
			if (ab) {
				ab.active = false;
			}
			//ab.enabled = false;
		}
		if(myAnim)
		{myAnim.Play ("Construct");
			animSpeed = 1;
			myAnim.speed = 1;
		}
		StartCoroutine (selfBuild (constructionTime));
	}

	IEnumerator selfBuild(float constructionTime)
	{
		if (myManager.myStats.supply > 0) {
			GameManager.main.playerList [myManager.PlayerOwner - 1].UnitCreated (myManager.myStats.supply);
		}

		while (!doneConstruction) {
			yield return null;
			construct (Time.deltaTime/constructionTime);
		}
		if (myManager.myStats.supply < 0) {
			GameManager.main.playerList [myManager.PlayerOwner - 1].UnitCreated (myManager.myStats.supply);
		}
		//GameManager.main.playerList[myManager.PlayerOwner - 1].addUnit(myManager);
		//myManager.setStun (false, this, false);
		if (GetComponent<Selected> ().IsSelected) {
			SelectedManager.main.updateUI ();
		}
	}



	public bool ConstructDone()
	{return doneConstruction;
	}
	public float getProgess()
	{
		return underConstruction;
	}

	public float construct(float m)
	{
		if (doneConstruction) {
			return 1;}

		if (myAnim) {
			lastBuildInput = Time.time;
			myAnim.speed = animSpeed;
			if (currentCoRoutine != null) {
				StopCoroutine (currentCoRoutine);
			}
			currentCoRoutine =  StartCoroutine (checkBuildAnim ());
		}
		
		underConstruction += m;
		myManager.myStats.heal (myManager.myStats.Maxhealth * m, DamageTypes.DamageType.True);
		if (underConstruction >= 1) {
			
			GameManager.main.playerList [myManager.PlayerOwner - 1].addUnit (myManager);
			doneConstruction = true;
			//Debug.Log ("Finished up");
			myManager.metaStatus.UnStun(this);
			if (myAnim) {
				myAnim.SetInteger ("State", 1);
			}
			if (currentCoRoutine != null) {
				StopCoroutine (currentCoRoutine);
			}
			if (myAnim) {
				myAnim.speed = animSpeed;
			}
		

			ErrorPrompt.instance.BuildingDone(myManager.UnitName, this.transform.position);

			UnitManager template = sourceObj.GetComponent<UnitManager> ();
			for (int i = 0; i < myManager.abilityList.Count; i++) {

				if (template.abilityList [i].active && myManager.abilityList [i]) {
					myManager.abilityList [i].active = true;
				}
				if (template.abilityList [i].enabled && myManager.abilityList [i]) {

		
					myManager.abilityList [i].enabled = true;
				}
			}
				
		

			foreach (ResearchUpgrade ru in GetComponents<ResearchUpgrade>()) {
				ru.UpdateAvailable ();
			}

			return 1;
		}
		return underConstruction;
	}

	public void Dying()
	{
		
		//GraphUpdateObject b = new GraphUpdateObject (GetComponent<CharacterController> ().bounds); 
		//AstarPath.active.UpdateGraphs (b);
	}

	IEnumerator DeathRescan(GraphUpdateObject b)
	{	
		yield return new WaitForSeconds (.2f);
		AstarPath.active.UpdateGraphs (b);
	}


	IEnumerator checkBuildAnim()
	{
		yield return new WaitForSeconds (.75f);
		if (lastBuildInput < Time.time - .75) {
			myAnim.speed = 0;
		}

	}

	public virtual void computeInteractions (Order order)
	{

		switch (order.OrderType) {
		//Stop Order----------------------------------------
		case Const.ORDER_STOP:
			
		//	myManager.changeState (new DefaultState (myManager, myManager.cMover, myManager.myWeapon));
			break;

			//Move Order ---------------------------------------------
		case Const.ORDER_MOVE_TO:
			AttackMoveSpawn = false;
			if (RallyPointObj) {
				if (myLine) {
					myLine.SetPositions (new Vector3[]{ this.gameObject.transform.position + Vector3.up *5, order.OrderLocation + Vector3.up *3 });
				}
				RallyPointObj.transform.position = order.OrderLocation;
			}
			GetComponent<Selected> ().RallyUnit = null;
			rallyPoint = order.OrderLocation;
			rallyUnit = null;
			break;



		case Const.ORDER_AttackMove:
			AttackMoveSpawn = true;
			GetComponent<Selected> ().RallyUnit =null;
			if (RallyPointObj) {
				if (myLine) {
					myLine.SetPositions (new Vector3[]{ this.gameObject.transform.position + Vector3.up *5, order.OrderLocation + Vector3.up *3});
				}
				RallyPointObj.transform.position = order.OrderLocation;
			}
			rallyPoint = order.OrderLocation;
			rallyUnit = null;

			break;
		case Const.ORDER_Interact:
			AttackMoveSpawn = false;
			rallyUnit = order.Target.gameObject;
			GetComponent<Selected> ().RallyUnit = order.Target.gameObject;
			if (RallyPointObj) {
				
				RallyPointObj.transform.position = order.Target.gameObject.transform.position;
				if (myLine) {
					myLine.SetPositions (new Vector3[]{ this.gameObject.transform.position + Vector3.up*5, order.Target.gameObject.transform.position+ Vector3.up *3 });
				}

			}
			rallyPoint= Vector3.zero ;
			break;

		case Const.ORDER_Follow:
			AttackMoveSpawn = false;
			rallyUnit = order.Target.gameObject;
			if (RallyPointObj) {

				RallyPointObj.transform.position = order.Target.gameObject.transform.position;
				if (myLine) {
					myLine.SetPositions (new Vector3[] {
						this.gameObject.transform.position+ Vector3.up,
						order.Target.gameObject.transform.position
					});
				}

			}
			GetComponent<Selected> ().RallyUnit = order.Target.gameObject;
			rallyPoint= Vector3.zero ;
			break;



		}





	}

	public UnitState computeState(UnitState s)
	{
		if(s is MoveState)
		{
			return new DefaultState();
		}
		return s;
	}


}
