using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldExplosion : Buff,Modifier  {

	public GameObject ExplosionObj;
	bool applied;
	void OnEnable()
	{
		if (!applied) {
			applied = true;
			myManager.myStats.addDeathTrigger (this);
			applyBuff ();
		}
	}


	public float modify(float damage, GameObject source, OnHitContainer hitSource, DamageTypes.DamageType theType)
	{
		UnitStats stats = GetComponent<UnitStats> ();

		if (stats && stats.currentEnergy > 0) {
			
			GameObject obj = Instantiate (ExplosionObj, this.transform.position, this.transform.rotation);
			obj.GetComponent<explosion> ().setSource (this.gameObject);
			obj.GetComponent<explosion> ().damageAmount = stats.currentEnergy;
		}
		return damage;
	}
}
