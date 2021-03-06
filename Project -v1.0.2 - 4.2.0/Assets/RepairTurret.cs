﻿using UnityEngine;
using System.Collections;


public class RepairTurret : Ability, Modifier, Addon{


	private UnitManager mymanager;


	public GameObject target;

	public MultiShotParticle particleEff;
	public int repairRate = 8;

	private repairReturn returner;

	public GameObject drone;
	RepairDrone droneScript;
	public bool DroneAway;
	public bool commandRepair;

    // Use this for initialization
    public override void Start()
    {
        mymanager = gameObject.transform.parent.GetComponentInParent<UnitManager> ();
		//chargeCount = maxRepair;

		mymanager.myStats.addDeathTrigger (this);
		droneScript = drone.GetComponent<RepairDrone> ();
		droneScript.repairRate = repairRate;

		InvokeRepeating ("Updater", .5f,1f);
	
	}
	public UnitManager getManager()
	{
		return mymanager;
	}

	public override void setAutoCast(bool offOn){
	}


	void Updater () {

		if (!DroneAway) {

			if (mymanager.getState () is MoveState) {

				if (!((MoveState)mymanager.getState ()).assumedMove) {

					return;
				}
			}
		
			if(target &&  Vector3.Distance (target.transform.position, this.gameObject.transform.position) < 40)
			{
				if (mymanager.myWeapon.Count == 0) {
					if (mymanager.cMover) {
						mymanager.cMover.stop ();
					}

					mymanager.changeState (new DefaultState ());
				}

				else if ( !(mymanager.getState () is AttackMoveState)) {

					if (mymanager.cMover) {
						mymanager.cMover.stop ();
					}
				}
		
				droneScript.setTarget (target);
				DroneAway = true;
				drone.transform.SetParent (null);
				
			}

			else if (!target || (Vector3.Distance (target.transform.position, this.gameObject.transform.position) > 65 && !commandRepair)) {
				
				
				if (mymanager.allies.Count > 0) {
					
					target = findHurtAlly ();
				
					commandRepair = false;
					if (target && target != mymanager.gameObject && !(mymanager.getState() is AttackMoveState)) {

						mymanager.changeState (new FollowState (target, null));
					
					} 
				} 
			} else if (target && target != mymanager.gameObject && !(mymanager.getState() is AttackMoveState)){

					mymanager.changeState (new FollowState (target, null));

				}
			

		}

		if (target && !DroneAway) {

			if (Vector3.Distance (this.gameObject.transform.position, target.transform.position) < 40) {
				if (mymanager.myWeapon.Count == 0) {
					if (mymanager.cMover) {
						mymanager.cMover.stop ();
					}

					mymanager.changeState (new DefaultState ());
				} else if (!(mymanager.getState () is AttackMoveState)) {

					if (mymanager.cMover) {

						mymanager.cMover.stop ();
					}
				}
				droneScript.setTarget (target);
				DroneAway = true;
				drone.transform.SetParent (null);
			}
		} else if (target && DroneAway) {

			if (Vector3.Distance (this.gameObject.transform.position, target.transform.position) < 40) {

				// Fix this line of code if tritons have a hard time moving while holding a repait bay
				if (mymanager.myWeapon.Count == 0 && (mymanager.getState () is AttackMoveState)) {
					//if ((mymanager.myWeapon.Count == 0 && !(mymanager.getState () is MoveState) )|| !(mymanager.getState () is AttackMoveState)&& !(mymanager.getState () is MoveState)) {
						
					if (mymanager.cMover) {
						mymanager.cMover.stop ();
					}
				
					mymanager.changeState (new DefaultState ());
				} 
			}
		}

	}

	public void possibleStop()
	{

		if (mymanager.getState () is MoveState) {
			if (!((MoveState)mymanager.getState ()).assumedMove) {

				return;
			}
		}
		if (mymanager.myWeapon.Count == 0) {
			mymanager.changeState (new DefaultState ());
		}
	}


	public bool setTarget(GameObject obj)
	{
		if (obj.GetComponent<UnitStats> ().atFullHealth ()) {
			return false;}
		
			try{
				if (!obj.GetComponent<BuildingInteractor> ().ConstructDone()) {
					return false;
				}}catch{
			};
		commandRepair = true;

		target = obj;
		DroneAway = false;
		if (Vector3.Distance (target.transform.position, this.gameObject.transform.position) < 40) {
			if (mymanager.myWeapon.Count == 0) {
				mymanager.cMover.stop ();

				mymanager.changeState (new DefaultState ());
			}
			droneScript.setTarget (target);
			DroneAway = true;
			drone.transform.SetParent (null);

		} else {

			mymanager.changeState (new FollowState (target, null));
		}
		return true;
	
	}

	public void doneRepairing(GameObject t)
	{if (t == target) {
			target = null;
			commandRepair = false;
		}
	
		DroneAway = false;

	}


	private GameObject findHurtAlly()
	{

			GameObject best = null;
			float distance = 1000000;

		for (int i = 0; i <mymanager.allies.Count; i ++) {
			if (mymanager.allies[i] != null) {

				if(!mymanager.allies[i].myStats.atFullHealth()){
					try{
						if (!mymanager.allies [i].GetComponent<BuildingInteractor> ().ConstructDone()) {
						continue;
						}}catch{
					};
					
					

					float currDistance = Vector3.Distance(mymanager.allies[i].transform.position, this.gameObject.transform.position);
					if(currDistance < distance)
					{best = mymanager.allies[i].gameObject;

						distance = currDistance;
						}
					}
				}
			}
		if (!best && !mymanager.myStats.atFullHealth()) {
			best = mymanager.gameObject;
		
		}
			return best;

		}




	public bool validate(GameObject source, GameObject target)
	{if (!active) {
			return false;}

		return false;
	}




	public void trigger(GameObject source, GameObject projectile,GameObject target)	{



	}

	override
	public continueOrder canActivate(bool showError)
	{continueOrder order = new continueOrder();

		return order;


	}


	override
	public void Activate()
	{
		

	}




	public float modify(float damage, GameObject source, OnHitContainer hitSource, DamageTypes.DamageType theType)
	{ 

		Destroy (drone);

		return 0 ;

	}



}


public interface Addon {
}