﻿using UnityEngine;
using System.Collections;

public class SlowDebuff : Behavior, Notify {

	public bool OnTarget = false;

	public float speedDecrease;
	public float speedPercent;

	public float attackSpeedDecrease;
	public float attackSpeedPercent;

	public float duration;
	public bool stackable;
	
    UnitManager TargetManager;

	public void initialize(float dur, float speed, float percentdec, float attackSpeedDec, float attackSpeedPerc, bool stacks)
	{
        TargetManager = GetComponent<UnitManager>();
		stackable = stacks;
		OnTarget = true;
		duration = dur;
		speedDecrease = speed;
		speedPercent = percentdec;
		attackSpeedPercent = attackSpeedPerc;
		attackSpeedDecrease = attackSpeedDec;


		if (speedDecrease != 0 || speedPercent != 0)
		{
            TargetManager.myStats.statChanger.changeMoveSpeed(speedPercent, speedDecrease, this, stackable);
		}

		if (attackSpeedDecrease != 0 || attackSpeedPercent != 0)
		{
            TargetManager.myStats.statChanger.changeAttackSpeed(attackSpeedPercent, attackSpeedDecrease, this, stackable);
		}
	
		StopAllCoroutines ();
		StartCoroutine (waitForTime());
			

	}

	IEnumerator waitForTime()
	{
		yield return new WaitForSeconds (duration);
		if (speedDecrease != 0 || speedPercent != 0)
		{
            TargetManager.myStats.statChanger.removeMoveSpeed(this);
		}
		if (attackSpeedDecrease != 0 || attackSpeedPercent != 0)
		{
            TargetManager.myStats.statChanger.removeAttackSpeed(this);
		}
		Destroy (this);
	}



	public float trigger(GameObject source,GameObject proj, UnitManager target, float damage)
	{
		SlowDebuff debuff = target.gameObject.GetComponent<SlowDebuff>();
		if (!debuff)
		{
			debuff = target.gameObject.AddComponent<SlowDebuff>();
		}
		debuff.initialize(duration, speedDecrease, speedPercent, attackSpeedDecrease, attackSpeedPercent, stackable);

		return damage;
	}
}
