using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerIeffect : IEffect
{
    public OnHitContainer myHitContainer;
    public float DamageAmount;

    public override void applyTo(GameObject source, UnitManager target)
    {
        target.myStats.TakeDamage( DamageAmount, this.gameObject, DamageTypes.DamageType.Regular, myHitContainer);
    }

    public override void BeginEffect()
    {

    }

    public override bool validTarget(GameObject target)
    {
        return true;
    }
}
