using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulcanModule : MonoBehaviour
{
	VulcanAI myAI = new VulcanAI();


	private void Start()
	{
		myAI.apply(GetComponent<UnitManager>());
	}

}


public class VulcanAI : UniqUnitAI
{
	public VulcanAI()
	{
		priority = 50;
		myUnitType = RaceInfo.unitType.Vulcan;

	}


	public override void apply(UnitManager man)
	{
	
	}

	public override int UseAPM(UnitManager manager, AIPhase phase)
	{
		if (phase == AIPhase.inCombat)
		{ // Deploy Turret
			if (manager.abilityList[2].canActivate(false).canCast)
			{
				Vector3 targetZone = UniqUnitAI.getRandomTargetZone(manager.transform.position +  manager.transform.forward * 30, 15);
				((TargetAbility)manager.abilityList[2]).Cast(null, targetZone);
				return 1;
			}
			else if (manager.abilityList[1].canActivate(false).canCast)
			{ // Kinetic Barrier
				foreach (UnitManager man in manager.allies)
				{
					if (!man)
					{
						continue;
					}
					if (!man.myStats.isUnitType(UnitTypes.UnitTypeTag.Structure))
					{
						if (!(man.myStats.currentEnergy < man.myStats.MaxEnergy -50))
						{
							((TargetAbility)manager.abilityList[1]).Cast(man.gameObject, Vector3.zero);
							return 1;
						}
					}
				}
			}
		}

		else if (phase == AIPhase.OutOfCombat)
		{
			// FIll in for landmines later in some way that wont make AIs spam thousands of mines across a whole game.
		}


		return 0;
	}

}
