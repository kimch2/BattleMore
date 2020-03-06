using UnityEngine;
using System.Collections;

public class StunState : UnitState {

    // Use this for initialization

    public StunState(UnitManager myMan)
    {
        myManager = myMan;
    }
	
	// Update is called once per frame
	override
	public void Update () {
       // Debug.Log("Updating");

	}

	public override void initialize()
	{
        //myManager.cMover.resetMoveLocation(myManager.transform.position);
        myManager.cMover.stop();
        myManager.animStop();
        myManager.myAnim.Play("Idle");
    }


	override
	public void endState()
	{
	}

	override
	public void attackResponse(UnitManager src, float amount)
	{
	}
}
