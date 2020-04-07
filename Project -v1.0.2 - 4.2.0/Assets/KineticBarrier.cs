using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticBarrier : DamagerMonoBehavior, Modifier{
	// Ability used by Codan's ult. applies a shield that restores energy and then if combo'ed, at the end releases an explosion

	public UnitStats myStats;
	public float HP = 500;

	public Selected mySelected;
	public float duration = 6;
	public GameObject EndExplosion;
	float initialHP;

	void Start () {
		
		initialHP = HP;
		myStats =	GetComponentInParent<UnitStats> ();
		myStats.addModifier (this, 0);
		mySelected = GetComponentInParent<Selected> ();
		mySelected.outsideAttchement = true;
        myStats.SetShield(HP, this);

        Invoke ("TurnOff",duration);

	}

	public void Reset()
	{
		HP = initialHP;
		CancelInvoke ("TurnOff");
		Invoke ("TurnOff",duration);
	}

	public float modify(float amount, GameObject src, OnHitContainer hitSource, DamageTypes.DamageType theType)
	{	
		float AmountReduced = Mathf.Min (amount, HP);
		HP -= AmountReduced;
		myStats.changeEnergy (AmountReduced);
        myStats.SetShield(HP, this);
		if (HP <= 0) {
			Invoke ("TurnOff",.1f);
		}
		return amount - AmountReduced;
	}


	void TurnOff(){

		myStats.removeModifier (this);
        myStats.SetShield(0, this);
        mySelected.outsideAttchement = false;
		if (EndExplosion) {
			GameObject explode = Instantiate<GameObject> (EndExplosion, transform.position,Quaternion.identity);
            explosion explody = explode.GetComponent<explosion>();
            explody.Initialize(initialHP - HP, myHitContainer);
		}
		Destroy (this.gameObject);

	}

}
