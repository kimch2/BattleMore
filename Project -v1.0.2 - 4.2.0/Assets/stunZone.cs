using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stunZone : VisionTrigger {



	public override void UnitEnterTrigger(UnitManager manager)
	{	if (manager.cMover) {
			manager.cMover.changeSpeed (0, -1000, false, this);
		}
		//manager.StunForTime (this, timer.timer);
	}


	public override void UnitExitTrigger(UnitManager manager)
	{
		if (manager.cMover) {
			manager.cMover.removeSpeedBuff (this);
		}
	}

	void OnDestroy()
	{
		foreach (UnitManager manage in InVision) {
			if (manage) {
				
				manage.cMover.removeSpeedBuff ( this);
			}
		}
	}

}
