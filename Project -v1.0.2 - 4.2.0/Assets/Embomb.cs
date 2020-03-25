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
        
        Embomb targetEmbomb = target.gameObject.AddComponent<Embomb>();
        targetEmbomb.Source = source;
        targetEmbomb.DamageAmount = DamageAmount;
        targetEmbomb.gameObject.GetComponent<UnitStats>().addDeathTrigger(targetEmbomb);

    }



    public float modify(float damage, GameObject source, DamageTypes.DamageType theType)
    {
        foreach (UnitManager man in GameManager.GetUnitsInRange(transform.position, GetComponent<UnitManager>().PlayerOwner, radius)){
            
            applyTo(null, man);
            man.myStats.TakeDamage(DamageAmount, Source, DamageTypes.DamageType.Energy, myHitContainer);
        }
        return damage;
    }

    public override void RemoveEffect(UnitManager target)
    {
        throw new System.NotImplementedException();
    }

    public override bool validTarget(GameObject target)
    {
        return true;
    }
}
