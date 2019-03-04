using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackAttack : MonoBehaviour, Notify
{
	public float knockDistance;
	public UnitTypes.UnitTypeTag mustTarget;


	// Use this for initialization
	void Start()
	{
		if (GetComponent<Projectile>())
		{
			GetComponent<Projectile>().triggers.Add(this);
		}
	}





	public float trigger(GameObject source, GameObject proj, UnitManager target, float damage)
	{
		if (target)
		{
			if (target.myStats.isUnitType(mustTarget))
			{
				
				Vector3 origin = proj.GetComponent<Projectile>().getOrigin();
				PhysicsSimulator.main.KnockBack(origin, target, new Vector2(knockDistance, 0), () => { });
			}
		}
		return damage;
	}
}