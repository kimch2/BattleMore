using UnityEngine;
using System.Collections;

public class SuicideAttack : MonoBehaviour, Notify,Modifier {

	bool hasFired;

	void Start()
	{
		GetComponent<IWeapon> ().addNotifyTrigger (this);
		GetComponent<UnitStats> ().addDeathTrigger(this);
	}

	public float modify(float damage, GameObject source, DamageTypes.DamageType theType)
	{
		if (!hasFired) {
			GameObject obj =	(GameObject)Instantiate (GetComponent<IWeapon> ().projectile, this.transform.position, Quaternion.identity);
			obj.GetComponent<explosion> ().damageAmount = GetComponent<IWeapon> ().baseDamage;
		}

		return damage;
	}

	public float trigger(GameObject source, GameObject projectile,UnitManager target, float damage)	{

		StartCoroutine (waitForFrame());
		return damage;
	}

	IEnumerator waitForFrame()
	{
		yield return null;
		hasFired = true;
		GetComponent<UnitStats> ().kill (this.gameObject);
	}
}
