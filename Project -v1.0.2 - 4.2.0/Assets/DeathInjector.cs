using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathInjector :MonoBehaviour,  Notify {

	// Unit modifier that make them attack faster the less health they have

	public bool onTarget;
	public float DamageTime =15;
	public float DamageAmount =15;
	private UnitStats myStats;
	public GameObject effect;
    [Tooltip("only use this if the thing that is spawning is the thign that this is on, which causing reference errors, because it will refer to itself and not the prefab")]
	public string toSpawn;
    public GameObject toSpawnObject;
	GameObject myEffect;
	// Use this for initialization
	void Start () {

		myStats = GetComponent<UnitStats> ();

		if (!onTarget) {

            if (GetComponents<IWeapon>().Length > 1)
            {
                foreach (IWeapon weap in GetComponents<IWeapon>())
                {
                    weap.addNotifyTrigger(this);
                }
            }
            else
            {
                foreach (IWeapon weap in GetComponentsInParent<IWeapon>())
                {
                    weap.addNotifyTrigger(this);
                }
            }
			
		} else {
            if(myEffect)
			myEffect =	Instantiate<GameObject> (effect, this.transform.position, Quaternion.identity, transform);
		}
	}




	public float trigger(GameObject source, GameObject projectile,UnitManager target, float damage)
	{
		//Need to fix this if this script is on both allied and enemy units
		DeathInjector inject = target.gameObject.GetComponent<DeathInjector> ();
		if (!inject) {
			inject = target.gameObject.AddComponent<DeathInjector> ();

		}
		inject.effect = effect;
		inject.toSpawn = toSpawn;
		inject.DamageOverTime (DamageAmount,DamageTime);
        inject.toSpawnObject = toSpawnObject;

		return damage;
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
		for (float i = 0; i < DamageTime; i++) {
			yield return new WaitForSeconds (1);
			myStats.TakeDamage (DamageAmount, null, DamageTypes.DamageType.True);
		}
		Destroy (myEffect);
		Destroy (this);
	}

	public void Dying()
	{
        //Debug.Log ("Dying " + onTarget + "  -" + toSpawn);
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

}
