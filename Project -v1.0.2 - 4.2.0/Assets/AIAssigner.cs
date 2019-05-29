using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAssigner :  Upgrade
{
	public CarbotAI myAI;

	public override void applyUpgrade(GameObject obj)
	{
		
		myAI.addNewUnit(obj.GetComponent<UnitManager>());
	}

	public override void unApplyUpgrade(GameObject obj)
	{
		throw new System.NotImplementedException();
	}

}
