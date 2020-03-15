using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackAttack : MonoBehaviour, Notify
{
	public float knockDistance;
	public UnitTypes.UnitTypeTag mustTarget;

	public float trigger(GameObject source, GameObject proj, UnitManager target, float damage)
	{
        Debug.Log("Knocking back");
		if (target)
		{
            if (target.myStats.isUnitType(mustTarget))
            {
                Vector3 origin;

                if (proj)
                {
                    origin = proj.GetComponent<Projectile>().getOrigin();
                }
                else {
                    origin = source.transform.position;
                }

                PhysicsSimulator.main.KnockBack(origin, target,this, new Vector2(knockDistance, 0), () => { });
			}
		}
		return damage;
	}
}