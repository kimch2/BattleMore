using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmbalmAura : MonoBehaviour, ModularAura.ICustomAura
{

    public float initialDPS = 1;
    public float perSecIncrease = 1;
    public GameObject effect;
    public float MaxDuration = 15;
   // GameObject instantiatedEffect;
    UnitManager myManager;


    public void Apply(UnitManager manage)
    {
       myManager = manage;
       currentPoison = StartCoroutine(Poison());
        effect.SetActive(true);
      // instantiatedEffect = Instantiate<GameObject>(effect, manage.transform);

    }

    public void Remove(UnitManager manage)
    {
        StopCoroutine(currentPoison);
        effect.SetActive(false);
       // Destroy(instantiatedEffect);
    }

    Coroutine currentPoison;

    IEnumerator Poison()
    {
        yield return null;
        float damage = initialDPS;
        for(int i = 0; i <= MaxDuration; i++)
        {
            myManager.getUnitStats().TakeDamage(damage, null, DamageTypes.DamageType.True, null);
            damage += perSecIncrease;
            yield return new WaitForSeconds(1);
        }
    }

  
}
