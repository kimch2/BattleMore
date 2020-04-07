using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Augmentor : TargetAbility, Iinteract, Modifier {



	GameObject attached;
	DetachAugment detacher;
	IMover myMover;
	public Tweener Swirly;
	public rotater myRotate;
	public float SpeedPlus = 1.35f;


    // Use this for initialization
    public override void Start()
    {
		
		myType = type.target;
		myManager.myWeapon.Clear ();
		detacher = GetComponent<DetachAugment> ();
		if (target) {
			StartCoroutine (delayCast());
			//manager.changeState (new CastAbilityState (this),false,false);
		}
       myManager.setInteractor(this);
    }
	

	IEnumerator delayCast()
	{
		yield return new WaitForSeconds (1);
		Cast ();
	}


	override
	public void Cast(){


		Unattach ();
		if (!target) {
            myManager.changeState (new DefaultState ());
			return;
		}

		AugmentAttachPoint AAP = target.GetComponent<AugmentAttachPoint> ();
		if (AAP.myAugment) {
		
			return;}
		//Make sure its not under construction
		BuildingInteractor BI = target.GetComponent<BuildingInteractor> ();

		if (!BI.ConstructDone ()) {

			StartCoroutine (delayCast());
			return;
		}

		myRotate.enabled = true;


        //ComboTag.CastTag(target, ComboTag.AbilType.Hot, new List<ComboTag.AbilType>(){ComboTag.AbilType.Hot, ComboTag.AbilType.Hot}); //REMOVE ME
        myManager.myStats.myHeight = UnitTypes.HeightType.Ground;
        myManager.myStats.otherTags.Add(UnitTypes.UnitTypeTag.Structure);
        myManager.myStats.SetTags ();
		detacher.allowDetach (true);
		attached = target;
		target.GetComponent<UnitManager> ().myStats.addDeathTrigger (this);

		AAP.myAugment = this.gameObject;
		BuildManager bm = AAP.gameObject.GetComponent<BuildManager> ();
		if (bm) {
			Swirly.GetComponent<SpriteRenderer> ().color = Color.yellow;
			if (bm.buildOrder.Count > 0) {
				startBuilding ();
			} else {
				stopBuilding ();
			}
		} else {
			Swirly.GetComponent<SpriteRenderer> ().color = Color.blue;
			myRotate.speed = -250;
			stopBuilding ();
		}


		this.gameObject.transform.position = target.transform.position+  target.transform.rotation * AAP.attachPoint;


		ShieldBattery armer = attached.GetComponent<ShieldBattery> ();

		if (armer) {
			armer.stopRecharging ();
			foreach (IWeapon w in GetComponents<IWeapon>()) {
				if (!myManager.myWeapon.Contains (w)) {

                    myManager.myWeapon.Add (w);
                    myManager.getUnitStats ().attackPriority = 3;
				}

			}
		} else {
            myManager.getUnitStats ().attackPriority = 3;
		}
		UnitManager unitMan = target.GetComponent<UnitManager> ();



		OreDispenser OD = target.GetComponent<OreDispenser> ();
		if (OD) {
			OD.efficiency = 1.3f;
		} 
		else if (unitMan.UnitName.Contains("Yard") ||unitMan.UnitName == "Armory" ||unitMan.UnitName.Contains("Avi") || unitMan.UnitName.Contains("Bay") || unitMan.UnitName.Contains("Academy")|| unitMan.UnitName.Contains("Flux")  ) {
			unitMan.GetComponent<Selected> ().setCooldownColor (Color.yellow);
			int xxx = 0;
			foreach (Ability bu in unitMan.abilityList){
				if (bu) {
					if (bu is UnitProduction) {
						((UnitProduction)bu).setBuildRate (SpeedPlus);
						if (xxx > 1 && !unitMan.UnitName.Contains ("Yard") && !unitMan.UnitName.Contains ("Armory")) 
						{
							if (bu is ResearchUpgrade)
							{
								if (!((ResearchUpgrade)bu).researchingElsewhere)
								{
									
									((UnitProduction)bu).active = true;
									bu.SendMessage("attachAddon", SendMessageOptions.DontRequireReceiver);
								}
							}
							else
							{
								((UnitProduction)bu).active = true;
								bu.SendMessage("attachAddon", SendMessageOptions.DontRequireReceiver);
							}

							
						}

					}
				}
				xxx++;
			}

		}

        myManager.changeState( new HoldState (myManager));
		if (target.GetComponent<Selected> ().IsSelected) {
			RaceManager.updateActivity ();
		}


	}

	//Triggers if the attached building dies
	public float modify(float d, GameObject src, OnHitContainer hitSource, DamageTypes.DamageType theType )
	{
		Unattach ();
        myManager.myStats.kill(null);
		return d;
	}

	void OnDestroy()
	{Unattach ();
		
	}



	public void changeSpeed(float n)
	{
		SpeedPlus += n;
		if (!target) {
			return;}
		UnitManager unitMan = attached.GetComponent<UnitManager> ();

		OreDispenser OD = attached.GetComponent<OreDispenser> ();
		if (OD) {
			OD.returnRate = 1.3f;
		} 
		else if (unitMan.UnitName.Contains("Yard") ||unitMan.UnitName == "Armory" ||unitMan.UnitName.Contains("Avi") ||unitMan.UnitName.Contains("Engin") || unitMan.UnitName.Contains("Academy") || unitMan.UnitName.Contains("Flux") ) {

			int xxx = 0;
			foreach (Ability bu in unitMan.abilityList){
				if (bu) {
					if (bu is UnitProduction) {
						((UnitProduction)bu).setBuildRate (1);
						if (xxx > 1 && ! unitMan.UnitName.Contains ("Yard") && !unitMan.UnitName.Contains ("Armory")) 
						{
							((UnitProduction)bu).active = false;
						
						}

					}
				}
				xxx++;
			}

		}
	}

	public void startBuilding()
	{
		myRotate.speed = -200;
		Swirly.GoToPose ("On");
	}

	public void stopBuilding()
	{
		myRotate.speed = -100;
		Swirly.GoToPose ("Middle");
	}

	public void Unattach()
	{if (!attached) {
			return;}
        myManager.getUnitStats ().attackPriority = 2;
		myRotate.enabled = false;
		Swirly.GoToPose ("Off");
        myManager.myStats.myHeight = UnitTypes.HeightType.Air;
		attached.GetComponent<UnitManager> ().myStats.removeDeathTrigger (this);
		ShieldBattery armer = attached.GetComponent<ShieldBattery> ();
		UnitManager man = attached.GetComponent<UnitManager> ();
        myManager.myStats.otherTags.Remove(UnitTypes.UnitTypeTag.Structure);
        myManager.myStats.SetTags ();

		if (armer) {

            myManager.myWeapon.Clear ();
		}

		OreDispenser OD = attached.GetComponent<OreDispenser> ();
		if (OD) {
			OD.efficiency = 1;
		} else if (man.UnitName.Contains ("Yard") || man.UnitName == "Armory" || man.UnitName.Contains ("Avi") || man.UnitName.Contains ("Engin") || man.UnitName.Contains ("Academy")|| man.UnitName.Contains("Flux")) {
			man.GetComponent<Selected> ().setCooldownColor (Color.white);
			int xxx = 0;
		
			foreach (Ability bu in man.abilityList) {
				if (bu) {
					if (bu is UnitProduction) {

						((UnitProduction)bu).setBuildRate (1);
						if (xxx > 1 && !man.UnitName.Contains ("Yard") && !man.UnitName.Contains ("Armory")) {
							bu.SendMessage ("removeAddon", this, SendMessageOptions.DontRequireReceiver);
							((UnitProduction)bu).active = false;
						}

					}
				}
				xxx++;
			}

		}
		
	
		if (target && target.GetComponent<Selected> ().IsSelected) {
			RaceManager.updateActivity ();
		}

		attached.GetComponent<AugmentAttachPoint> ().myAugment = null;
		attached = null;
		detacher.allowDetach (false);

		RaycastHit objecthit;

		Vector3 down = this.gameObject.transform.TransformDirection (Vector3.down);

		if (Physics.Raycast (this.gameObject.transform.position, down, out objecthit, 1000, (~8))) {

			down =objecthit.point;
            myManager.changeState (new MoveState (down, myManager, true));
		
		}
	}

	public void Dying()
	{
		if (attached) {
			UnitManager man = attached.GetComponent<UnitManager> ();
			if (man && (man.UnitName.Contains ("Yard") || man.UnitName == "Armory" || man.UnitName.Contains ("Avi") || man.UnitName.Contains ("Engin") || man.UnitName.Contains ("Academy") || man.UnitName.Contains ("Flux"))) {
				
				attached.GetComponent<Selected> ().setCooldownColor (Color.white);
			} else if (man && man.UnitName.Contains ("Ore")) {
				OreDispenser OD = attached.GetComponent<OreDispenser> ();
				if (OD) {
					OD.returnRate = 1;
				} 
			}
		}
	}

	override public void setAutoCast(bool offOn)
	{
	}

	override
	public  bool Cast(GameObject tar, Vector3 location){
		
		target = tar;
		attached = tar;

		Vector3 attachSpot = target.transform.position;

		this.gameObject.transform.position =  target.transform.rotation * attachSpot;
        myManager.myStats.otherTags.Add(UnitTypes.UnitTypeTag.Structure);
		///myMover = manager.cMover;
		/// 
		//ComboTag.CastTag(target, ComboTag.AbilType.Cold, new List<ComboTag.AbilType>(){ComboTag.AbilType.Cold, ComboTag.AbilType.Cold}); //REMOVE ME

		return false;

	}

	override
	public  bool isValidTarget (GameObject target, Vector3 location){

		if (target == null) {
			return false;
		}
		AugmentAttachPoint AAP =target.GetComponent<AugmentAttachPoint> ();
		if (!AAP) {
			return false;}
		if (AAP.myAugment) {
			return false;}

		if(target.GetComponent<OreDispenser> () )
		{return true;
		}

		UnitManager m = target.GetComponent<UnitManager> ();
		if (m == null) {
			return false;}

		if (m.PlayerOwner == 3) {
			return true;}

		if (m.PlayerOwner != myManager.PlayerOwner) {
			return false;}



	

		return true;
	}

	override
	public continueOrder canActivate(bool showError){

		continueOrder order = new continueOrder ();


		order.nextUnitCast = false;
		return order;
	}

	override
	public void Activate()
	{

	}  // returns whether or not the next unit in the same group should also cast it



	public void initializeInteractor(){
		Start ();
	}




	// When creating other interactor classes, make sure to pass all relevant information into whatever new state is being created (IMover, IWeapon, UnitManager)
	public void computeInteractions (Order order )
	{

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

	public UnitState computeState(UnitState s)
	{
		if (s is AbilityFollowState) {
			Unattach ();
		}

	//	Debug.Log ("attached " + attached);
		if (attached) {
			if (s is DefaultState || s is AttackMoveState) {
				//Debug.Log ("Setting to hold state");
				return new HoldState (myManager);
			}
		}
		return s;
	}

	// Attack move towards a ground location (Tab - ground)
	public void  AttackMove(Order order)
	{
		if (!attached) {
            myManager.changeState (new MoveState (order.OrderLocation, myManager, true),false,order.queued);

		}
	}

	// Right click on a objt/unit
	public void Interact(Order order)
	{

		if (!attached) {
			if (!attached && isValidTarget (order.Target, Vector3.zero)) {
                myManager.UseTargetAbility (order.Target, Vector3.zero, 0, false);

				return;
			}
		} else {
			if (isValidTarget (order.Target, Vector3.zero)) {

                myManager.UseTargetAbility (order.Target, Vector3.zero, 0, false);
			}
		}

		UnitManager manage = order.Target.GetComponent<UnitManager> ();
		if (!manage) {
			manage = order.Target.GetComponentInParent<UnitManager> ();
		}

		if (manage != null) {

			if (!attached && manage.PlayerOwner != this.gameObject.GetComponent<UnitManager>().PlayerOwner ) {

                myManager.changeState (new FollowState (order.Target.gameObject, myManager),false,order.queued);
			} else if(!attached && isValidTarget(order.Target, Vector3.zero)){
                myManager.UseTargetAbility (order.Target, Vector3.zero, 0, false);

				}
			} else {

            myManager.changeState (new FollowState (order.Target.gameObject, myManager),false,order.queued);
			}

	}
	//Right click on the ground
	public void Move(Order order)
	{
		if (!attached) {
            //Debug.Log ("Im moving");
            myManager.changeState (new MoveState (order.OrderLocation, myManager),false,order.queued);

		}

		if (target) {
			target = null;}
	}

	//Stop, caps lock
	public void Stop(Order order)
	{
        myManager.changeState (new DefaultState ());
		if (target) {
			target = null;}
	}

	//Shift-Tab 
	public void Patrol(Order order)
	{if (target) {
			target = null;}
		if(!attached)
            myManager.changeState (new AttackMoveState (null, order.OrderLocation, AttackMoveState.MoveType.patrol, myManager, myManager.gameObject.transform.position),false,order.queued);
	}

	//Shift-Caps
	public void HoldGround(Order order)
	{if (target) {
		//	target = null;
		}
		//manager.changeState (new HoldState(manager));
	}


	//Right click on a unit/object. how is this different than interact? is it only on allied units?
	public void Follow(Order order){
		if (order.Target == this.gameObject) {
			return;
		}

		if (!order.Target) {
		
			return;}

		if (!attached) {
			if (isValidTarget (order.Target, Vector3.zero)) {
                myManager.UseTargetAbility (order.Target, Vector3.zero, 0, order.queued);

			} else {
                myManager.changeState (new MoveState (order.OrderLocation, myManager, true), false, order.queued);
	
				if (target) {
					target = null;
				}
			}
		} else {
		
			int count = 0; // The uagmentor will only target a thing if it is attached and if it is the only uagmentor selected
			foreach (RTSObject obj in SelectedManager.main.ActiveObjectList()) {
				if (obj.getUnitManager ().UnitName == "Augmentor") {
					count++;
					if (count > 1) {
						return;}
				}
			}

				if (isValidTarget (order.Target, Vector3.zero)) {

                myManager.UseTargetAbility (order.Target, Vector3.zero, 0, order.queued);
					//Debug.Log ("Ordered to follow");
				}
			}

		}






}
