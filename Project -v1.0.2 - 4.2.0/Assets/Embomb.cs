﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Embomb : IEffect, Modifier
{
    public float DamageAmount;
    public float radius;
    [HideInInspector]
    public GameObject Source;

    public override void applyTo(GameObject source, GameObject target)
    {
        
        Embomb targetEmbomb = target.AddComponent<Embomb>();
        targetEmbomb.Source = source;
        targetEmbomb.DamageAmount = DamageAmount;
        targetEmbomb.gameObject.GetComponent<UnitStats>().addDeathTrigger(targetEmbomb);

    }

    public override bool canCast()
    {
        return true;
    }

    public float modify(float damage, GameObject source, DamageTypes.DamageType theType)
    {
        foreach (UnitManager man in GameManager.GetUnitsInRange(transform.position, GetComponent<UnitManager>().PlayerOwner, radius)){
            
            applyTo(null, man.gameObject);
            man.myStats.TakeDamage(DamageAmount, Source, DamageTypes.DamageType.Energy);
        }
        return damage;
    }

    public override bool validTarget(GameObject target)
    {
        return true;
    }
}
