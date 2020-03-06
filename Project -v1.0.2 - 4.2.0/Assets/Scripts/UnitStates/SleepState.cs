using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepState : UnitState
{

    // Use this for initialization

    public SleepState(UnitManager myMan)
    {
        myManager = myMan;
    }

    // Update is called once per frame
    override
    public void Update()
    {

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
        if (amount > 0)
        {
            myManager.metaStatus.UnSleep();
        }
    }
}