using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stunZone : VisionTrigger {

	public float rootTime = 2;

	public override void UnitEnterTrigger(UnitManager manager)
	{	if (manager.cMover) {
			StartCoroutine (timedRoot(manager));
		}
		//manager.StunForTime (this, timer.timer);
	}

	IEnumerator timedRoot(UnitManager manager)
	{
		manager.cMover.changeSpeed (0, -1000, false, this);
		yield return new WaitForSeconds (rootTime);
		if (manager) {
			manager.cMover.removeSpeedBuff (this);
		}
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
			if (manage && manage.cMover) {
				
				manage.cMover.removeSpeedBuff ( this);
			}
		}
	}

}
