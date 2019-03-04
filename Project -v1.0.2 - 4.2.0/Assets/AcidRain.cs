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
					manage.myStats.statChanger.changeArmor(0, -1, this, true);
				}
				if (manage.cMover)
				{
					manage.myStats.statChanger.changeMoveSpeed(-.05f, 0, this, true);
				}
			}
		}
	}


	public override void UnitEnterTrigger(UnitManager manager) {

	}
	public override void UnitExitTrigger(UnitManager manager) {

	}

}
