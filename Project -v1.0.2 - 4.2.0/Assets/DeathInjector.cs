using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathInjector : DamagerIeffect
{

	public float DamageTime =15;
	public GameObject effect;
    [Tooltip("only use this if the thing that is spawning is the thign that this is on, which causing reference errors, because it will refer to itself and not the prefab")]
	public string toSpawn;
    public GameObject toSpawnObject;
	GameObject myEffect;


	void Start ()
    {
		if (onTarget)
        {            
            if (myEffect)
            {
                myEffect = Instantiate<GameObject>(effect, this.transform.position, Quaternion.identity, transform);
            }
            DamageOverTime(DamageAmount, DamageTime);
        }
	}


	public void DamageOverTime(float damagePerSecond, float duration)
	{
		DamageTime = duration;
		DamageAmount = damagePerSecond;
		onTarget = true;
		if (currentDamager != null) {
			StopCoroutine (currentDamager);}
		currentDamager = StartCoroutine (OverTime());
	}

	Coroutine currentDamager;

	IEnumerator OverTime()
	{
        for (float i = 0; i < DamageTime; i++)
        {
            yield return new WaitForSeconds(1);
            if (DamageAmount > 0)
            {
               OnTargetManager.myStats.TakeDamage(DamageAmount, null, DamageTypes.DamageType.True, myHitContainer);
            }
        }
		Destroy (myEffect);
		Destroy (this);
	}

	public void Dying()
	{
        if (onTarget)
        {
            if (toSpawnObject)
            {
                Instantiate(toSpawnObject, this.transform.position + Vector3.up * 3, Quaternion.identity);
            }
            else
            {
                Instantiate(Resources.Load<GameObject>(toSpawn), this.transform.position + Vector3.up * 3, Quaternion.identity);
            }
        }
	}

    public override bool validTarget(GameObject target)
    {
        return true;
    }


    public override void applyTo(GameObject source, UnitManager target)
    {
        UnitManager manag = source.GetComponent<UnitManager>();
        if (manag && manag.PlayerOwner == target.PlayerOwner) // Gives this ability to friends
        {
            CopyIEffect(target, false);           
        }
        else
        {
            CopyIEffect(target, true);
        }
    }

    public override void RemoveEffect(UnitManager target)
    {
        DeathInjector inject = target.GetComponent<DeathInjector>();
        if (inject &&  !inject.onTarget)
        {
            inject.EndEffect();
        }
    }

    public void EndEffect()
    {
        Destroy(myEffect);
        Destroy(this);
    }
}
