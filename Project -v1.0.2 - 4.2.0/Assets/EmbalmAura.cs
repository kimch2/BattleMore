using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmbalmAura : DamagerIeffect, Modifier
{
    // Causes the attached guy to explode on death
    
    public GameObject Exploder;
    public GameObject AuraFX;
    public float Duration = 3;

    private void Start()
    {
        if (onTarget)
        {
            OnTargetManager.myStats.addDeathTrigger(this);
            Invoke("EndEffect", Duration);
            GameObject CurrentEffect = Instantiate<GameObject>(AuraFX, transform);
            CurrentEffect.transform.localPosition = Vector3.zero;
        }
    }


    public override bool validTarget(GameObject target)
    {
        return true;
    }

    public override void applyTo(GameObject source, UnitManager target)
    {
        EmbalmAura Copy = (EmbalmAura)CopyIEffect(target, true);       
    }

    void EndEffect()
    {
        OnTargetManager.myStats.removeDeathTrigger(this);
    }

    public float modify(float damage, GameObject source, DamageTypes.DamageType theType)
    {
        GameObject obj = Instantiate<GameObject>(Exploder, OnTargetManager.transform.position, Quaternion.identity);
        myHitContainer.SetOnHitContainer(obj, DamageAmount, null);
        return damage;
    }
}
