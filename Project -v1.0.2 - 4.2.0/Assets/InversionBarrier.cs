using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InversionBarrier : MonoBehaviour, Modifier{


	public UnitStats myStats;
	public float HP = 200;

	DayexaShield myShield;
	public float duration = 10;


	// Use this for initialization
	void Start () {
		myStats = GetComponentInParent<UnitStats> ();
		myStats.addModifier (this, 0);
		myShield = GetComponentInParent<DayexaShield> ();
		Invoke ("TurnOff",duration);

	}

	public void Reset()
	{
		HP = 200;
		CancelInvoke ("TurnOff");
		Invoke ("TurnOff",duration);
	}
		
	public float modify(float amount, GameObject src, OnHitContainer hitSource, DamageTypes.DamageType theType)
	{	

		float AmountReduced = Mathf.Min (amount, HP);

		HP -= AmountReduced;

		if (myShield) {
			myStats.changeEnergy (AmountReduced);
		}
		if (HP <= 0) {
			Invoke ("TurnOff",.1f);
		}
		return amount - AmountReduced;
	}


	void TurnOff(){
	
		myStats.removeModifier (this);
	
		Destroy (this.gameObject);

	}

}
