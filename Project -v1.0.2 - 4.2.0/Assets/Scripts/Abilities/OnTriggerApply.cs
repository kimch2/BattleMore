using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerApply : MonoBehaviour, Notify
{
    // This is the base class which applys on-hit effects from projectiles and explosions

    public List<UnitTypes.UnitTypeTag> CantTarget;
    public List<IEffect> toApply;

    void Start()
    {
        if (GetComponent<Projectile>())
        {
            GetComponent<Projectile>().triggers.Add(this);
        }

        explosion myexplode = this.gameObject.GetComponent<explosion>();
        if (myexplode)
        {
            myexplode.triggers.Add(this);
        }
    }

    public float trigger(GameObject source, GameObject proj, UnitManager target, float damage)
    {
        if (target && source != target)
        {
            foreach (UnitTypes.UnitTypeTag tag in CantTarget)
            {
                if (target.myStats.isUnitType(tag))
                {
                    return damage;
                }
            }

            foreach (IEffect fect in toApply)
            {
                fect.applyTo(source, target.gameObject);
            }           
        }
        return damage;
    }
}
