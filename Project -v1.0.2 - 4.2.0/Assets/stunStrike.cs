using UnityEngine;
using System.Collections;

public class stunStrike : MonoBehaviour, Notify {


	public float stunTime;
	public UnitTypes.UnitTypeTag mustTarget;

    
	public float trigger(GameObject source,GameObject proj, UnitManager target, float damage)
	{
        
		if (target && source != target) {

			if (mustTarget != UnitTypes.UnitTypeTag.Dead) {
				if (!target.myStats.isUnitType (mustTarget)) {
					return damage;}
			}
		
			target.metaStatus.Stun (null, source, false, stunTime);

		}
		return damage;
	}


}
