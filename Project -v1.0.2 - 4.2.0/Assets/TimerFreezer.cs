using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerFreezer : MonoBehaviour, Modifier
{

	Coroutine bleederCo;
	UnitStats myStats;
	public TimedObjective myObjective;
	float lastAttackTime;

	// Use this for initialization
	void Start()
	{
		myStats = GetComponent<UnitStats>();
		myStats.addModifier(this);
	}



	public float modify(float amount, GameObject src, OnHitContainer hitSource, DamageTypes.DamageType theType)
	{
		lastAttackTime = Time.time + 6;
		if (bleederCo == null)
		{
			bleederCo = StartCoroutine(delayTurnOff());
		}
		return amount;
	}


	IEnumerator delayTurnOff()
	{
		myObjective.Pause(true);
		while (Time.time < lastAttackTime)
		{
			yield return new WaitForSeconds(1);
		}
		myObjective.Pause(false);
		bleederCo = null;
	}

}
