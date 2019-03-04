using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TritonModule : MonoBehaviour
{
	TritonAI myAI = new TritonAI();


	private void Start()
	{
		myAI.apply(GetComponent<UnitManager>());
	}

}


public class TritonAI : UniqUnitAI
{
	public TritonAI()
	{
		Debug.Log("Making Triton AI");
		priority = 20;
		myUnitType = RaceInfo.unitType.Vulcan;
	}


	public override void apply(UnitManager man)
	{
		
	}

	public override int UseAPM(UnitManager manager, AIPhase phase)
	{
		if (phase == AIPhase.inCombat)
		{
			if (!manager.abilityList[0].autocast && manager.myStats.currentEnergy > 200 && manager.enemies.Count > 3)
			{
				manager.abilityList[0].Activate();
				manager.changeState(new AttckWhileMoveState(Vector3.Lerp(manager.enemies[0].transform.position, manager.transform.position, .3f), manager), true, false);
				return 1;
			}
			else if (manager.abilityList[0].autocast && manager.myStats.currentEnergy < 100)
			{
				manager.abilityList[0].Activate();
				return 1;
			}

		}

		else if (phase == AIPhase.ExitCombat)
		{
			if (manager.abilityList[0].autocast)
			{
				manager.abilityList[0].Activate();
				return 1;
			}
		}


		return 0;
	}

}
