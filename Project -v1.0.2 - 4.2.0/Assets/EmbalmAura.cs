using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmbalmAura : DamagerIeffect
{

    public float initialDPS = 1;
    public float perSecIncrease = 1;
    public GameObject effect;
    public float MaxDuration = 15;
    GameObject CurrentEffect;
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
        EmbalmAura Copy = (EmbalmAura)CopyIEffect(target, true);
        Copy.BeginToPoison();
    }

    public void BeginToPoison()
    {
        if (currentPoison == null)
        {
            currentPoison = StartCoroutine(Poison());
            if (effect)
            {
                CurrentEffect = Instantiate<GameObject>(effect, transform);
                CurrentEffect.transform.localPosition = Vector3.zero;
            }
        }
    }

    IEnumerator Poison()
    {
        yield return null;
        float damage = initialDPS;
        for(int i = 0; i <= MaxDuration; i++)
        {
            OnTargetManager.getUnitStats().TakeDamage(damage, null, DamageTypes.DamageType.True, myHitContainer);
            damage += perSecIncrease;
            yield return new WaitForSeconds(1);
        }
        currentPoison = null;
    }

    public override void RemoveEffect(UnitManager target)
    {

        StopAllCoroutines();
        currentPoison = null;
        Destroy(CurrentEffect);
        Destroy(this);
    }
}
