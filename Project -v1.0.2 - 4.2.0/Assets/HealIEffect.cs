using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealIEffect : IEffect
{
    public float HealAmount = 10;
    public override void applyTo(GameObject source, UnitManager target)
    {
        target.myStats.heal(HealAmount);
    }

    public override void BeginEffect()
    {
       
    }

    public override bool validTarget(GameObject target)
    {
        return true;
    }
}
