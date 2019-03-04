using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class missileSalvo :  Ability, Iinteract, Validator, Notify{


	private IWeapon myweapon;
	public int  maxRockets = 2;

	public List<GameObject> MissileModels = new List<GameObject> ();
	Vector3 padSpot;
	private float nextCheckTime;

	float fillHerUp = 1;
	float flierheight;
	public HarpyLandingPad home;

	float lastDistance;
	bool inLanding;
	Vector3 homeLocation;
	float landingTime = 4.5f;

	Coroutine ReFill;
	// Use this for initialization
	void Start () {
		flierheight = GetComponent<airmover> ().flyerHeight;

		myweapon = GetComponent<IWeapon> ();
		myweapon.triggers.Add (this);
		myweapon.validators.Add (this);
		myType = type.activated;
		StartCoroutine (delayedUpdate());


	}



	IEnumerator delayedUpdate()
	{
		yield return new WaitForSeconds (.1f);
		select.updateCoolDown (chargeCount / maxRockets);
		fillHerUp = 1;
	}


	public void upRockets ()
	{if(chargeCount < maxRockets){
		chargeCount++;
		StartCoroutine (fillUpBar(.5f));
		
		
		if (select.IsSelected) {
			
			RaceManager.upDateUI ();
		}
			if(MissileModels.Count > chargeCount-1 &&chargeCount-1 >=0 ){
			MissileModels [chargeCount-1].SetActive (true);
	}}
	}

	public override void setAutoCast(bool offOn){
		autocast = offOn;

	}


	IEnumerator fillUpBar(float amount)
	{
		for (int i = 0; i < 12; i++) {
			yield return new WaitForSeconds (.07f);
			fillHerUp += amount/12;
			select.updateCoolDown (fillHerUp + .05f);
		
		}
		if (fillHerUp > .9f) {
			fillHerUp = 1;
			select.updateCoolDown (1);
		}
	}

		

	public bool validate(GameObject source, GameObject target)
	{
		if (chargeCount > 0) {
			return true;
		}
		if (autocast && chargeCount <= 0) {
			//Activate ();
		}
		return false;
	}




	public float trigger(GameObject source, GameObject projectile,UnitManager target, float damage)	{
		
		chargeCount--;

		StartCoroutine (fillUpBar(-.5f));
	
		if (select.IsSelected ) {
			RaceManager.upDateUI ();
		}
		if (autocast && chargeCount <= 0) {
			if (ReFill == null) {
				
				ReFill =	StartCoroutine (checkForLandingPad ());
			}
			Activate ();
		}
		if(MissileModels.Count > chargeCount && chargeCount >= 0){
			MissileModels [chargeCount].SetActive (false);
		}
		return damage;
	}

	override
	public continueOrder canActivate(bool showError)
	{

		continueOrder newOrder = new continueOrder ();
		if (chargeCount < maxRockets) {
			newOrder.canCast = true;
		} else {
			newOrder.canCast = false;
		}
		return newOrder;

	}


	override
	public void Activate()
	{

		if (chargeCount < maxRockets) {
			if (home) {
				home.finished (this.gameObject);}

			if (ReFill == null) {
				ReFill =	StartCoroutine (checkForLandingPad ());
			}

			home = null;
			padSpot = Vector3.zero;
			float distance = 100000;

			List<UnitManager> landingPads = myManager.myRacer.getUnitType("Aviatrix");
			// NEED TO CHECK FOR PLAYER OWNER
			foreach (UnitManager man in landingPads)
			{
				HarpyLandingPad arm = man.GetComponent<HarpyLandingPad>();

				if (!arm.hasAvailable() || !arm.GetComponent<BuildingInteractor>().ConstructDone()) {
					continue;
				}

					float temp = Vector3.Distance (arm.gameObject.transform.position, this.gameObject.transform.position);
					if (temp < distance) {
						distance = temp;
						home = arm;
					}

			}

			if (!home) { // we didn't find one the first time because they are all in use, go and wait in line
				foreach (UnitManager man in landingPads)
				{
					HarpyLandingPad arm = man.GetComponent<HarpyLandingPad>();
					if (!arm.GetComponent<BuildingInteractor>().ConstructDone()) {
						continue;
					}
					float temp = Vector3.Distance (arm.gameObject.transform.position, this.gameObject.transform.position);
					if (temp < distance) {
						distance = temp;
						home = arm;
					}

				}
			
			}

			if (home) {

				if (home.hasAvailable ()) {

					homeLocation = home.transform.position;
					myManager.GiveOrder (Orders.CreateMoveOrder (home.transform.position));
				} else {
					myManager.GiveOrder (Orders.CreateMoveOrder 
						(new Vector3(home.transform.position.x + Random.Range(0,5),home.transform.position.y + Random.Range(0,5),
							home.transform.position.z)));
				}
			}
		}
	}


	public void Dying()
	{
		if (home != null) {
			home.finished (this.gameObject);
		}

	}


	IEnumerator Descend()
	{
		GetComponent<airmover> ().flyerHeight = 4;
		inLanding = true;
		float t = 0;
		Vector3 startPosition = this.gameObject.transform.position;
		while (t <= 1) {
			t += Time.deltaTime;
			transform.position = Vector3.Lerp (startPosition, padSpot, t);
			yield return null;
		}

		//Loading missiles
		landingTime = home.startLanding (this.gameObject);
		yield return new WaitForSeconds (.001f);

		GetComponent<CharacterController> ().radius = 2.1f;

		myManager.StunForTime (this, landingTime);
		StopCoroutine (ReFill);

		yield return new WaitForSeconds (1.5f);
		upRockets ();
		yield return new WaitForSeconds (2f);
		upRockets ();
		yield return new WaitForSeconds (1.2f);

		ReFill = null;
		myManager.setStun (false, this, false);

		inLanding = false;
		if (home) {
			home.finished (this.gameObject);
		}


		Vector3 homePos = transform.position;

		if(home)
		{homePos = home.transform.position;}

	
		Vector3 MoveLocation = (padSpot - homePos);
		MoveLocation.x += Random.Range (-6, 6);
		MoveLocation.Normalize ();
		home = null;
		padSpot = Vector3.zero;
		GetComponent<airmover> ().flyerHeight = flierheight;
	
		myManager.GiveOrder (Orders.CreateMoveOrder (this.transform.position+ MoveLocation * 17) );

	}




	IEnumerator checkForLandingPad()
	{	
		yield return new WaitForSeconds (.1f);

		while (true) {

			if (home && homeLocation != Vector3.zero && Vector3.Distance(transform.position,homeLocation) < 30  && !inLanding) {

				Vector3 temp = home.requestLanding (this.gameObject);

				if (temp != Vector3.zero) {
					padSpot = temp;
					StartCoroutine (Descend ());
	
				} else {
					Activate ();
				}
			}
		
			yield return new WaitForSeconds (1f);
		}
	}



	public virtual UnitState computeState(UnitState s)
	{
		if (inLanding || this == null) {
			return null;}

		airmover air = GetComponent<airmover> ();
		if (air && air.flyerHeight == 4 && inLanding) {
			return new MoveState (padSpot + transform.forward * .25f, myManager);
		}
		return s;
	}


		List<HarpyLandingPad> nearbyPads = new List<HarpyLandingPad> ();

		void OnTriggerEnter(Collider other)
		{
		//Fix this if the enemy ever has harpies
		HarpyLandingPad pad = other.GetComponent<HarpyLandingPad>();
		if (pad) {
			nearbyPads.Add (pad);
		}
	}

	void OnTriggerExit(Collider other)
	{
		//Fix this if the enemy ever has harpies
		HarpyLandingPad pad = other.GetComponent<HarpyLandingPad>();
		if (pad) {
			nearbyPads.Remove(pad);

		}
	}


	
	public bool attackWhileMoving;

	// Use this for initialization
	new void Awake()
	{
		base.Awake();
		myManager.setInteractor (this);
	}


	public void initialize(){
		Awake ();
	}


	// When creating other interactor classes, make sure to pass all relevant information into whatever new state is being created (IMover, IWeapon, UnitManager)
	public virtual void computeInteractions (Order order)
	{
		//Debug.Log ("Queued " + order.queued);
		switch (order.OrderType) {
		case Const.Order_HoldGround:

			HoldGround (order);
			break;


		case Const.Order_Patrol:
			Patrol (order);
			break;


		case Const.ORDER_STOP:
			Stop (order);
			break;

		case Const.ORDER_MOVE_TO:
			Move (order);
			break;

		case Const.ORDER_Interact:
			Interact (order);
			break;

			// ATTACK MOVE - Move towards a location and attack enemies on the way.
		case Const.ORDER_AttackMove:
			//	Debug.Log ("Setting to attack move");
			AttackMove (order);

			break;


			// Right click on a allied unit
		case Const.ORDER_Follow:
			Follow (order);
			break;



		}


	}

	// Attack move towards a ground location (Tab - ground)
	public virtual void  AttackMove(Order order)
	{
		if (myManager.myWeapon.Count > 0) {

			myManager.changeState (new AttackMoveState (null, order.OrderLocation, AttackMoveState.MoveType.command, myManager, myManager.gameObject.transform.position),false,order.queued);
		}else {
			myManager.changeState (new MoveState (order.OrderLocation, myManager, true),false,order.queued);
		}
	}

	// Right click on a obj/unit
	public virtual void Interact(Order order)
	{//Debug.Log ("First Intereact");
		UnitManager manage = order.Target.GetComponent<UnitManager> ();
		if (!manage) {
			manage = order.Target.GetComponentInParent<UnitManager> ();
		}

		if (manage != null) {

			if (manage ==myManager) {
				return;
			}
			if (manage.PlayerOwner != myManager.PlayerOwner) {
				if (this.gameObject.GetComponent<UnitManager> ().myWeapon.Count == 0) {
					myManager.changeState (new FollowState (order.Target.gameObject, myManager), false, order.queued);
				} else {
					//Debug.Log ("Ordering to interact " + manage.gameObject);
					myManager.changeState (new InteractState (manage.gameObject, myManager), false, order.queued);
				}
			} else if (manage.UnitName == "Aviatrix") {
				Activate ();
			}
			else{
				myManager.changeState (new FollowState (order.Target.gameObject,  myManager),false,order.queued);
			}
		}
	}
	//Right click on the ground
	public void Move(Order order)
	{
		if (attackWhileMoving &&  myManager.myWeapon.Count >0) {

			myManager.changeState (new AttckWhileMoveState (order.OrderLocation, myManager),false,order.queued);
		} else {
			myManager.changeState (new MoveState (order.OrderLocation, myManager),false,order.queued);
		}
	}

	//Stop, caps lock
	public void Stop(Order order)
	{myManager.changeState (new DefaultState ());

	}

	//Shift-Tab 
	public void Patrol(Order order)
	{
		myManager.changeState (new AttackMoveState (null, order.OrderLocation, AttackMoveState.MoveType.patrol, myManager, myManager.gameObject.transform.position),false,order.queued);
	}

	//Shift-Caps
	public void HoldGround(Order order)
	{
		myManager.changeState (new HoldState(myManager));
	}

	//Right click on a unit/object. how is this different than interact? is it only on allied units?
	public virtual void Follow(Order order){

		if (order.Target == this.gameObject) {
			return;
		}
		//	Debug.Log ("First ORder");
		if (myManager.myWeapon.Count > 0) {

			myManager.changeState (new AttackMoveState (null, order.OrderLocation, AttackMoveState.MoveType.command, myManager, myManager.gameObject.transform.position),false,order.queued);
		}else {
			myManager.changeState (new MoveState (order.OrderLocation, myManager),false,order.queued);
		}
	}

}
