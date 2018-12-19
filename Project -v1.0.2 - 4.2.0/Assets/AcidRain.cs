using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidRain : VisionTrigger {

	public float minimumArmor = -10;

	// Use this for initialization
	void Start () {

		InvokeRepeating("AcidPour", 1, 1);
	}

	public void AcidPour()
	{
		foreach (UnitManager manage in InVision)
		{
			if (manage)
			{
				if (manage.myStats.armor > minimumArmor)
				{
					manage.myStats.changeArmor(-1);
				}
				if (manage.cMover)
				{
					manage.cMover.changeSpeed(.95f, 0, true, this);
				}
			}
		}
	}


	public override void UnitEnterTrigger(UnitManager manager) {

	}
	public override void UnitExitTrigger(UnitManager manager) {

	}

}
