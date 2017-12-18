using UnityEngine;
using System.Collections;

public class AbilityFollowState  : UnitState {

	private GameObject target;
	private Vector3 location;

	public TargetAbility myAbility;

	private int refreshTime = 5;
	private int currentFrame = 0;
	private bool Follow;


	public AbilityFollowState(GameObject unit, Vector3 loc, TargetAbility abil)
	{//Debug.Log ("New ABility follow " + loc);
		location = loc;
		myAbility = abil;
		abil.target = unit;
		abil.location = loc;
	
		target = unit;
		myAbility.target = unit;
		if (target != null) {
			Follow = true;
		}
		//Debug.Log ("Target is " + loc);
	
	}

	public override void initialize()
	{
	//	Debug.Log ("I am on " + myManager.gameObject);
		if (myManager.cMover) {
			refreshTime = 30 - (int)myManager.cMover.getMaxSpeed ();
		} else {
			refreshTime = 12;
		}
		if (refreshTime < 5) {
			refreshTime = 8;
		}
		if (myManager.cMover) {
			if (target) {
				myManager.cMover.resetMoveLocation (target.transform.position);
			} else {
				myManager.cMover.resetMoveLocation (location);
			}
		}
	}

	// Update is called once per frame
	override
	public void Update () {
		
		if (!target && Follow) {
			myManager.changeState(new DefaultState());
			return;
		}
		if (Follow) {
			currentFrame++;
			if (currentFrame > refreshTime) {
				currentFrame = 0;
				location = target.transform.position;
				myManager.cMover.resetMoveLocation (location);
			}
		}

	
		if (!myAbility.inRange (location)) {

		
			myManager.cMover.move ();
		} else {
			//Debug.Log ("Casting " + myAbility.location);

			myAbility.location = location;
			myAbility.target = target;
			if (myAbility.canActivate (false).canCast) {
				myAbility.Cast ();
			}
		
			myManager.changeState(new DefaultState());
			return;

		}
		//attack


	}

	override
	public void endState()
	{
	}

	//override
//	public void attackResponse(UnitManager src, float amount)
	//{
	//}


}
