using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Embomb : DamagerIeffect, Modifier
{
    public float radius;
    [HideInInspector]
    public GameObject Source;

    public override void applyTo(GameObject source, UnitManager target)
    {
        CopyIEffect(target, true, out bool alreadyOnIt);
    }

    public override void BeginEffect()
    {
        GetComponent<UnitStats>().addDeathTrigger(this);
    }

    public override void EndEffect()
    {
        base.EndEffect();
        GetComponent<UnitStats>().removeDeathTrigger(this);
    }


    public float modify(float damage, GameObject source, OnHitContainer hitSource, DamageTypes.DamageType theType)
    {
        foreach (UnitManager man in GameManager.GetUnitsInRange(transform.position, GetComponent<UnitManager>().PlayerOwner, radius)){
            
            applyTo(null, man);
            man.myStats.TakeDamage(DamageAmount, Source, DamageTypes.DamageType.Energy, myHitContainer);
        }
        return damage;
    }



    public override bool validTarget(GameObject target)
    {
        return true;
    }
}
