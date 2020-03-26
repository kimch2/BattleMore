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

    private void Start()
    {
        if (onTarget)
        {
            OnTargetManager = myHitContainer.myManager;
            BeginToPoison();
        }
    }


    public override bool validTarget(GameObject target)
    {
        return true;
    }

    public override void applyTo(GameObject source, UnitManager target)
    {
        Poison Copy = (Poison)CopyIEffect(target, true);
       // Copy.BeginToPoison();
    }

    public void BeginToPoison()
    {
        if (currentPoison == null)
        {
            currentPoison = StartCoroutine(Poisoned());
        }
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

    public override void RemoveEffect(UnitManager target)
    {
        base.RemoveVisualFX();
        StopAllCoroutines();
        currentPoison = null; 
        Destroy(this);
    }
}
