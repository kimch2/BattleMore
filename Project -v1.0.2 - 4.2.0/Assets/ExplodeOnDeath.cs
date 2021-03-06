﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnDeath : IEffect, Modifier {


    // BATTLEMORE SPECIFIC SCRIPT

	public GameObject explosion;
	[Tooltip("If this unit has less than this amount of energy it wont explode")]
	public float mininumEnergy = 7;
	public float myDamage = 0;
	// Use this for initialization
	public float speedboost = .2f;

	public GameObject Dynamite;


    public override void BeginEffect()
    {
        OnTargetManager.myStats.addDeathTrigger(this);
    }

    public override void EndEffect()
    {
        base.EndEffect();
        OnTargetManager.myStats.removeDeathTrigger(this);
    }

    public float modify(float damage, GameObject source, OnHitContainer hitSource, DamageTypes.DamageType theType) {

		
		if (myDamage == 0)
		{
			DayexaShield dayexaShield = GetComponent<DayexaShield>();
			if (dayexaShield)
			{
				float energy = OnTargetManager.myStats.currentEnergy;
				if (energy < mininumEnergy)
				{
					return damage;
				}
				//explosion.explode (energy * 45);
				if (explosion)
				{
					GameObject explode = (GameObject)Instantiate(explosion, this.gameObject.transform.position + Vector3.up*.5f, Quaternion.identity);
					//Debug.Log ("INstantiating explosion");

					explosion Escript = explode.GetComponent<explosion>();
					if (Escript)
					{
						Escript.setSource(this.gameObject);
						Escript.damageAmount = energy;
					}
				}
			}
		}
		else {
			if (explosion)
			{
				GameObject explode = (GameObject)Instantiate(explosion, this.gameObject.transform.position, Quaternion.identity);
				//Debug.Log ("INstantiating explosion");

				explosion Escript = explode.GetComponent<explosion>();
				if (Escript)
				{
					Escript.setSource(this.gameObject);
					Escript.damageAmount = myDamage;
				}
			}
		}
		return damage;
	}


	public override void applyTo(GameObject source, UnitManager target)
	{
		ExplodeOnDeath deather = target.GetComponent<ExplodeOnDeath>();
		if (!deather)
		{
			deather = target.gameObject.AddComponent<ExplodeOnDeath>();
		}

		deather.onTarget = true;
		deather.myDamage += myDamage;
		GameObject dyno = Instantiate<GameObject>(Dynamite, target.transform);
		float number = (deather.myDamage - myDamage) / myDamage;
		dyno.transform.localPosition = Vector3.back * ((number) % 4) * 1.5f + Vector3.right * ((int)number / 4);
		deather.explosion = explosion;

		selfDestructTimer destruct = target.GetComponent<selfDestructTimer>();
		if (!destruct)
		{
			destruct = target.gameObject.AddComponent<selfDestructTimer>();
			destruct.timer = 40;
			destruct.showTimer = true;
		}
		else
		{
			destruct.modifyRemainingByPercent(-.33f);
		}
		
	

		UnitManager manager = target.GetComponent<UnitManager>();
		if (manager.cMover)
		{
			manager.myStats.statChanger.changeMoveSpeed(speedboost, 0, null, true);
		}
	}


	public override bool validTarget(GameObject target) {
		return true;
	}


}
