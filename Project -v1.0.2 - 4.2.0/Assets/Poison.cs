using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : DamagerIeffect
{
    public float perSecIncrease = 1;
    public float HPPercent;
    public GameObject effect;
    public float MaxDuration = 15;
    Coroutine currentPoison;

    public override bool validTarget(GameObject target)
    {
        return true;
    }

    public override void applyTo(GameObject source, UnitManager target)
    {
        Poison Copy = (Poison)CopyIEffect(target, true);
    }

    public override void BeginEffect()
    {
        if (currentPoison == null)
        {
            currentPoison = StartCoroutine(Poisoned());
        }
    }

    public override void EndEffect()
    {
        base.RemoveVisualFX();
        StopAllCoroutines();
        currentPoison = null;
        Destroy(this);
    }

    IEnumerator Poisoned()
    {
        yield return null;
        float damage = DamageAmount;
        for (int i = 0; i <= MaxDuration; i++)
        {
            OnTargetManager.getUnitStats().TakeDamage(OnTargetManager.getUnitStats().Maxhealth * HPPercent + damage, null, DamageTypes.DamageType.True, myHitContainer);
            damage += perSecIncrease;
            yield return new WaitForSeconds(1);
        }
        currentPoison = null;
    }

   
}