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
	{
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

    bool Channeling = false;
    float CastTime;
	// Update is called once per frame
	override
	public void Update () {

        if (Channeling)
        {
            if (Time.time > CastTime)
            {
                myAbility.Cast();
                WorldRecharger.main.SpellWasCast(myManager.PlayerOwner, myManager.gameObject);
                myManager.changeState(new DefaultState());
            }
            return;
        }

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

	
		if (!myAbility.inRange (location))
        {
			myManager.cMover.move ();
		}
        else
        {
            myManager.cMover.stop();
            myAbility.setTarget(location, target);
			if (myAbility.canActivate (false).canCast) {
                Channeling = true;
                CastTime = Time.time + myAbility.ChannelTime;
                
                myAbility.SpellCastShowIndicator(location);
                //myAbility.Cast ();
                //WorldRecharger.main.SpellWasCast(myManager.PlayerOwner, myManager.gameObject);
            }
		
			//myManager.changeState(new DefaultState());

		}
	}

	override
	public void endState()
	{
        if (Channeling)
        {
            myAbility.DisableSkillShotIndicator();

            if (AbilityHeatMap.main)
            {
                AbilityHeatMap.main.RemoveCircleArea(myAbility); // Need to add in projectile travel time if thats a thing.
            }
        }
	}
}
