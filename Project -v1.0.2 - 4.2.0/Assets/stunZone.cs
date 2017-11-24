using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stunZone : VisionTrigger {

	//Currently meant to be used on the SquareTrons squareZone; Not Actually a stun zone, its a root zone
	selfDestructTimer timer ; 

	void Start()
	{
		timer = GetComponentInParent<selfDestructTimer> ();
	}



	public override void UnitEnterTrigger(UnitManager manager)
	{	if (manager.cMover) {
			manager.cMover.changeSpeed (0, -1000, false, this);
		}
		//manager.StunForTime (this, timer.timer);
	}


	public override void UnitExitTrigger(UnitManager manager)
	{

	}

}
