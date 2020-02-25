using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stunZone : VisionTrigger {

	public float rootTime = 2;

	public override void UnitEnterTrigger(UnitManager manager)
	{
            manager.metaStatus.Stun(null, this, false, rootTime);
	}


	public override void UnitExitTrigger(UnitManager manager)
	{
		if (manager) {
            manager.metaStatus.UnStun(this);
        }
	}

	void OnDestroy()
	{
		foreach (UnitManager manage in InVision) {
			if (manage) {

                manage.metaStatus.UnStun(this);
            }
		}
	}

}
