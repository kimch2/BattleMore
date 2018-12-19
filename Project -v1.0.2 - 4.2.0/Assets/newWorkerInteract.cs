using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class newWorkerInteract :  Ability, Iinteract {

	private UnitManager myManager;
	public float miningTime;

	public float resourceOne;
	public float resourceTwo;

	public OreDispenser myOre;
	private GameObject oreBlock;
	private OreDispenser lastOreDeposit;
	public GameObject PurifierPrefab;
	public ParticleSystem MiningEffect;

	// Use this for initialization
	new void Start () {
		base.Start();
		myManager = GetComponent<UnitManager> ();
		myManager.setInteractor (this);

		if (autocast) {
			StartCoroutine (delayer ());
		}

		myType = type.activated;
	}

	bool firstMove = true;

	public UnitState computeState(UnitState s)
	{
		
		if (autocast && myManager) {
	
			if ((myManager.getState () is ChannelState && s is MoveState) || (myManager.getState () is PlaceBuildingState && s is DefaultState)) {
				if (myManager.PlayerOwner == 1) { // This is a hack to make sure the SUperCraftor in the money level doesn't try to start mining from the start.
					StartCoroutine (autocastReturn ());
				}
			}

			if (firstMove && myManager.getState () is MoveState &&  s is DefaultState) {
				StartCoroutine (delayer ());
				firstMove = false;
			}
		}


		if (!(s is MiningState)) {
			if (myOre) {
				lastOreDeposit = myOre;

				myOre.currentMinor = null;
				myOre = null;
			}
		}

		StartCoroutine (checkIdle());

		return s;
	}

	IEnumerator checkIdle()
	{
		yield return null;
		FButtonManager.main.changeWorkers ();
		
	}

	IEnumerator autocastReturn()
	{
		yield return new WaitForSeconds (.3f);
		Activate ();
	}

	IEnumerator delayer()
	{
		yield return new WaitForSeconds (1.5f);
		//Debug.Log ("Finding Ore");
		if (myManager.getState () is DefaultState) {
			findNearestOre ();
		}
	}

	public void findNearestOre()
	{
		float distance = 300;
		if (myManager.getState () is MiningState) {
			return;}
		OreDispenser closest = null;

		foreach (KeyValuePair<string, List<UnitManager>> pair in  GameManager.main.playerList[2].getUnitList()) {
			foreach (UnitManager obj in pair.Value) {

				if (FogOfWar.current.IsInCompleteFog (obj.transform.position)) {
					continue;
				}
				OreDispenser dis = obj.GetComponent<OreDispenser> ();

				if (!dis || dis.currentMinor) {
				
					continue;
				}

				float temp = Vector3.Distance (obj.transform.position, this.gameObject.transform.position);
				if (temp < distance) {
					//Debug.Log ("Setting " + obj +  "   " + temp + "   " + distance);
					distance = temp;

					closest = dis;
				}

			}
		}
		if (closest != null) {
			myOre = closest;
			goToOre(closest);
		}
	}


	public void Redistribute(GameObject targ)
	{

		float distance = 130;
		if (myManager.getState () is MiningState) {
			return;}
		OreDispenser closest = null;

		foreach (KeyValuePair<string, List<UnitManager>> pair in  GameManager.main.playerList[2].getUnitList()) {
			foreach (UnitManager obj in pair.Value) {

				if (FogOfWar.current.IsInCompleteFog (obj.transform.position)) {
					continue;
				}
				OreDispenser dis = obj.GetComponent<OreDispenser> ();

				if (!dis || dis.currentMinor) {

					continue;
				}

				float temp = Vector3.Distance (obj.transform.position, targ.transform.position);
				if (temp < distance) {
					//Debug.Log ("Setting " + obj +  "   " + temp + "   " + distance);
					distance = temp;

					closest = dis;
			
				}
			}
		}
		if (closest != null) {
			myOre = closest;
			goToOre(closest);
		
		} else {
			//myManager.changeState (new MoveState (targ.gameObject.transform.position, myManager));
		
			ErrorPrompt.instance.showError ("Deposits already occupied");
		}
	}




	public void initialize(){
		Start ();
	}


	public  void computeInteractions (Order order)
	{

		if (myManager.getState() is ChannelState && !order.queued)
		{
			return;
		}

		//Debug.Log ("interacting" + order.OrderType);
		switch (order.OrderType) {
		//Stop Order----------------------------------------
		case Const.ORDER_STOP:
			if (myOre) {
				lastOreDeposit = myOre;
	
				myOre.currentMinor = null;
				myOre = null;

			}
			myManager.changeState (new DefaultState ());

			break;

			//Move Order ---------------------------------------------
		case Const.ORDER_MOVE_TO:
			
			myManager.changeState (new MoveState (order.OrderLocation, myManager),false,order.queued);
			if (myOre) {
				lastOreDeposit = myOre;
				myOre.currentMinor = null;
				myOre = null;
			}

			break;

		case Const.ORDER_Interact:

			if(order.Target.gameObject.GetComponent<OreDispenser> () != null)
			{
				if (!order.Target.gameObject.GetComponent<OreDispenser> ().currentMinor) {
					if (myOre) {
						lastOreDeposit = myOre;
	
						myOre.currentMinor = null;
						myOre = null;
					}
					myOre = order.Target.gameObject.GetComponent<OreDispenser> ();
					goToOre(order.Target.gameObject.GetComponent<OreDispenser>());
						
				} else if (order.Target.gameObject.GetComponent<OreDispenser> ().currentMinor == this.gameObject) {

					}
				else{
					Redistribute (order.Target);
				}
				break;}


			if (order.Target) {
				if (order.Target.GetComponent<UnitManager> () == null) {
					order.Target = order.Target.transform.parent.gameObject;
				}
			}


			myManager.changeState (new FollowState (order.Target.gameObject, myManager),false,order.queued);


			break;


		case Const.ORDER_AttackMove:
			
			if (myOre) {
				lastOreDeposit = myOre;

				myOre.currentMinor = null;
				myOre = null;
			}
			if (myManager.myWeapon.Count >0)
				myManager.changeState (new AttackMoveState (null, order.OrderLocation, AttackMoveState.MoveType.command, myManager,  myManager.gameObject.transform.position),false,order.queued);
			else {
				myManager.changeState (new MoveState (order.OrderLocation, myManager),false,order.queued);
			}

			break;


		case Const.ORDER_Follow:
			
			if (order.Target.gameObject.GetComponent<OreDispenser> () != null) {
				goToOre(order.Target.gameObject.GetComponent<OreDispenser>());
				break;
			}


			if (order.Target) {
				if (order.Target.GetComponent<UnitManager> () == null) {
					order.Target = order.Target.transform.parent.gameObject;
				}
			}
			if (order.Target.GetComponent<BuildingInteractor> ()){
			if (!order.Target.GetComponent<BuildingInteractor> ().ConstructDone()) {
					myManager.changeState (new buildResumeState (order.Target.gameObject),false,order.queued);
				}
			} 
			else {

				myManager.changeState (new FollowState (order.Target.gameObject, myManager),false,order.queued);
			}

			break;
		}
	}


	public override void setAutoCast(bool offOn){
		autocast = offOn;
	}


	override
	public continueOrder canActivate (bool showError)
	{

		continueOrder order = new continueOrder ();
		order.nextUnitCast = true;
			order.canCast = true;
	
		return order;
	}

	void goToOre(OreDispenser target )
	{
		myManager.GiveOrder(Orders.CreateMoveOrder(target.transform.position, false));

		myManager.changeState(new MiningState(lastOreDeposit.gameObject.GetComponent<OreDispenser>(), myManager, miningTime, resourceOne, resourceTwo,MiningEffect),false,true);
	}

	override
	public void Activate()
	{
		if (lastOreDeposit) {
			
			if (lastOreDeposit.currentMinor == null) {
				goToOre(lastOreDeposit.gameObject.GetComponent<OreDispenser>());
			} else {
				Redistribute (lastOreDeposit.gameObject);
			}


		} 

	}




}