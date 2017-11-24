using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal: VisionTrigger {

		public Vector3 teleportLocation;
	public GameObject teleportEffect;
		public override void UnitExitTrigger(UnitManager manager)
		{

		}

		public override void UnitEnterTrigger(UnitManager manager)
		{
		if (!manager.myStats.isUnitType (UnitTypes.UnitTypeTag.Turret)) {
			if(teleportEffect)
			Instantiate (teleportEffect, manager.transform.position,Quaternion.identity);
			manager.transform.position = teleportLocation + Vector3.forward * Random.Range (-8, 8) + Vector3.right * Random.Range (-8, 8);
			if(teleportEffect)
			Instantiate (teleportEffect, manager.transform.position,Quaternion.identity);
		}
		}

	void OnDrawGizmos()
	{Gizmos.color = Color.cyan;
		Gizmos.DrawSphere (teleportLocation,3);
	}

		
	}