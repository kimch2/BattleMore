using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmbalmAura : DamagerIeffect, Modifier
{
    // Causes the attached guy to explode on death
    
    public GameObject Exploder;
    public float Duration = 3;


    public override bool validTarget(GameObject target)
    {
        return true;
    }

    public override void applyTo(GameObject source, UnitManager target)
    {
        EmbalmAura Copy = (EmbalmAura)CopyIEffect(target, true, out bool alreadyOnIt);       
    }

    public override void BeginEffect()
    {
        Invoke("EndEffect", Duration);
        OnTargetManager.myStats.addDeathTrigger(this);
    }

    public override void EndEffect()
    {
        RemoveVisualFX();
        OnTargetManager.myStats.removeDeathTrigger(this);
    }

    public float modify(float damage, GameObject source, DamageTypes.DamageType theType)
    {
        GameObject obj = Instantiate<GameObject>(Exploder, OnTargetManager.transform.position, Quaternion.identity);
        myHitContainer.SetOnHitContainer(obj, DamageAmount, null);
        return damage;
    }
}
