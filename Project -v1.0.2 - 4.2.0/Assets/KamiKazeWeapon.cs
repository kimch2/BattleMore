using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamiKazeWeapon : IWeapon, Modifier
{
    public float modify(float damage, GameObject source, OnHitContainer hitSource, DamageTypes.DamageType theType)
    {

        return 0;
    }

    public override void Start()
    {
        base.Start();
        myManager.myStats.addDeathTrigger(this);
    }

    protected override void DealDamage(float damage, UnitManager target)
    {
        GameObject proj = null;
        if (projectile != null)
        {
            proj = Instantiate<GameObject>(proj);
            damage = fireTriggers(this.gameObject, proj, target, damage);
            myHitContainer.SetOnHitContainer(proj, damage, null);
        }
    }
}