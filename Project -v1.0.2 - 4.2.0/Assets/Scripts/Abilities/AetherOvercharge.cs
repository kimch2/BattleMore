﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AetherOvercharge : Buff, Notify{

	[Tooltip("Perecent - between 0 and 1")]
	public float rechargeAmount;
	public float duration;
	private bool onTarget;
	private UnitManager myman;

	[Tooltip("Perecent - between 0 and 1")]
	public float attackSpeed;
	[Tooltip("Perecent - between 0 and 1")]
	public float attackDamage;

	private float nextActionTime;
	private float startTime;
	bool spellHasBegun;
	public GameObject AetherEffect;

	
	// Update is called once per frame
	void Update () {
		if (onTarget) {
			if (Time.time > startTime + duration) {
				endSpell ();
				Destroy (this);
				return;
			}

			if (spellHasBegun && Time.time > nextActionTime) {
	
				nextActionTime += 1;

				myman.myStats.setEnergyRegen (0);
				GetComponent<DayexaShield> ().stopRecharge ();
				myman.myStats.changeEnergy (-myman.myStats.MaxEnergy / duration);
				if(myman.myStats.currentEnergy <= 0)
				{
					endSpell();
				}
			}
		}
	}

	public void initialize(UnitManager unitman,float chargeAmount, float dur, float AS, float dam, GameObject effect, string descrip, Sprite toolIcon)
	{nextActionTime = Time.time + 1;

		toolDescription = descrip;
		HelpIcon = toolIcon;
		spellHasBegun = true;
		attackSpeed = AS;
		attackDamage = dam;
		onTarget = true;
		myman = unitman;
		rechargeAmount = chargeAmount;
		startTime = Time.time;
		duration = dur;
		unitman.myStats.changeEnergy (unitman.myStats.MaxEnergy * rechargeAmount);
		applyBuff ();
		if (rechargeAmount > 0) {
			PopUpMaker.CreateGlobalPopUp ("+" + (int)(unitman.myStats.MaxEnergy * rechargeAmount), Color.blue, unitman.gameObject.transform.position);
		}
		AetherEffect = (GameObject)Instantiate (effect, this.gameObject.transform.position, this.gameObject.transform.rotation);
		AetherEffect.transform.SetParent (this.gameObject.transform);


		unitman.myStats.statChanger.changeAttackSpeed(attackSpeed, 0, this,true);
		unitman.myStats.statChanger.changeWeaponDamage(attackDamage, 0, this,true);

		foreach (IWeapon weap in unitman.myWeapon)
		{
			weap.triggers.Add(this);


		}

		//Debug.Log ("Final speed is " + unitman.myWeapon[0].attackPeriod);

	}

	public float trigger(GameObject src, GameObject proj, UnitManager target, float damage)
	{
		if (!spellHasBegun) {
			nextActionTime = Time.time + 1;
			spellHasBegun = true;
			startTime = Time.time;
			StartCoroutine (delayRemove());
		}
		return damage;
	}

	IEnumerator delayRemove()
	{
		yield return new WaitForSeconds (.01f);
		foreach (IWeapon weap in myman.myWeapon) {
			weap.triggers.Remove (this);
		}
	}



	public void endSpell()
	{
		//Debug.Log ("Ending spell");
		GetComponent<DayexaShield> ().startRecharge (1);
		removeBuff();
		Destroy (AetherEffect);

		myman.myStats.statChanger.removeAttackSpeed( this);
		myman.myStats.statChanger.removeWeaponDamage( this);

		foreach (IWeapon weap in myman.myWeapon)
		{
			if (weap)
			{
				weap.triggers.Remove(this);
			}

		}

	}

	//Used in the casting object
	public void OnTriggerEnter(Collider other)
	{
		if (!onTarget) {
			UnitManager man = other.GetComponent<Collider>().gameObject.GetComponent<UnitManager> ();
			if (man && man.PlayerOwner == 1 && !man.myStats.isUnitType(UnitTypes.UnitTypeTag.Turret)) {
				if (man.gameObject.GetComponent<AetherOvercharge> ()) {
					return;}

				if (man.gameObject.GetComponent<DayexaShield> () && man.myWeapon.Count > 0) {
					AetherOvercharge charge = man.gameObject.AddComponent<AetherOvercharge> ();
					charge.initialize (man, rechargeAmount, duration, attackSpeed, attackDamage,AetherEffect, toolDescription, HelpIcon);
				}
			}
		}
	}
}
