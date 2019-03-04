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
		manager.myStats.statChanger.changeMoveSpeed (0, -1000,  this);
		yield return new WaitForSeconds (rootTime);
		if (manager) {
			manager.myStats.statChanger.removeMoveSpeed (this);
		}
	}




	public override void UnitExitTrigger(UnitManager manager)
	{
		if (manager.cMover) {
			manager.myStats.statChanger.removeMoveSpeed(this);
		}
	}

	void OnDestroy()
	{
		foreach (UnitManager manage in InVision) {
			if (manage && manage.cMover) {

				manage.myStats.statChanger.removeMoveSpeed( this);
			}
		}
	}

}
