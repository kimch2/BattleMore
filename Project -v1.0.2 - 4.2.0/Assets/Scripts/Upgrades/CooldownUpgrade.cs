using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownUpgrade : Upgrade
{

	public float reductionTime = 10;


	override
	public void applyUpgrade(GameObject obj)
	{

		UnitManager manager = obj.GetComponent<UnitManager>();

		if (manager.UnitName == "Vulcan")
		{
			manager.GetComponent<BloodMist> ().myCost.cooldown -= reductionTime;
			manager.GetComponent<DeployTurret> ().ReplicationTime -= reductionTime; 
			manager.GetComponent<DeployTurret> ().myCost.cooldown -= reductionTime;
			manager.GetComponent<SingleTarget> ().myCost.cooldown -= reductionTime;
		}

	}
	public override void unApplyUpgrade (GameObject obj){

	}


}
