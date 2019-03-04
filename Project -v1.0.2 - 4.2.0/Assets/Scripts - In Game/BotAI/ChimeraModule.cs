using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChimeraModule : MonoBehaviour
{
	ChimeraAI myAI = new ChimeraAI();


	private void Start()
	{ 
		myAI.apply(GetComponent<UnitManager>());
	}

}


public class ChimeraAI : UniqUnitAI , Notify
{
	public ChimeraAI()
	{
		priority = 5;
		myUnitType = RaceInfo.unitType.Chimera;

	}
	
	public override void apply(UnitManager man)
	{
		man.myWeapon[0].addNotifyTrigger(this);
	}

	public override int UseAPM(UnitManager manager , AIPhase phase)
	{
		return 0; // fill this in later and store what the last attacked guy was so it can be switched or changed to the default when out of combat
	}

	public float trigger(GameObject source, GameObject projectile, UnitManager target, float damage)
	{

		UnitManager sourceMan = source.GetComponent<UnitManager>();

		if (target.myStats.isUnitType(UnitTypes.UnitTypeTag.Structure))
		{
			if (!sourceMan.abilityList[1].autocast)
			{
				sourceMan.abilityList[1].Activate();
				return ((ChangeAmmo)sourceMan.abilityList[1]).attackDamage;
			}
		}
		else
		{
			if (!sourceMan.abilityList[0].autocast)
			{
				sourceMan.abilityList[0].Activate();
				return ((ChangeAmmo)sourceMan.abilityList[0]).attackDamage;
			}
		}
			return damage;
	}
}
