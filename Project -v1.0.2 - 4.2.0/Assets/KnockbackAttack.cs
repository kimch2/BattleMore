using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackAttack : MonoBehaviour, Notify
{
	public float knockDistance;
	public UnitTypes.UnitTypeTag mustTarget;

	public float trigger(GameObject source, GameObject proj, UnitManager target, float damage)
	{
		if (target)
		{
            if (target.myStats.isUnitType(mustTarget))
            {
                Vector3 origin = source.transform.position;

                if (proj)
                {
                    Projectile projectile = proj.GetComponent<Projectile>();
                    if (projectile)
                    {
                        origin = projectile.getOrigin();
                    }
                    else
                    {
                        explosion explos = proj.GetComponent<explosion>();
                        if (explos)
                        {
                            origin = proj.transform.position;
                        }
                    }
                }
                PhysicsSimulator.main.KnockBack(origin, target,this, new Vector2(knockDistance, 0), () => { });
			}
		}
		return damage;
	}
}