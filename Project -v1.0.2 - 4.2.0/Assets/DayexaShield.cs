using UnityEngine;
using System.Collections;

public class DayexaShield : Ability,Modifier , Notify{


	public float Absorbtion;
	public UnitStats myStats;

	private float rechargeTime;

	public float RechargeDelay;
	public float rechargeRate;

	public GameObject shieldEffect;
	ParticleSystem myEffect;

	public bool AbsorbRecoil;
	public float maxDamagePerSec = 100000;

	private float damageAbsorbed;

	void Awake()
	{audioSrc = GetComponent<AudioSource> ();
		myType = type.passive;
	}


	// Use this for initialization
	void Start () {
		myStats.addModifier (this);

		//This makes it so all childed turrets get their incoming damage reduced by the tanks shields. 
		foreach (IWeapon obj in GetComponent<UnitManager>().myWeapon) {
			if (obj) {
				obj.gameObject.GetComponent<UnitManager> ().myStats.addModifier (this);
			}

		}
		GetComponent<UnitManager> ().addNotify (this);

		if (maxDamagePerSec < 9999) {
			InvokeRepeating ("UpdateShield", 1, 1);
		}
		changeAbsorption (0);
		myStats.setEnergyRegen (rechargeRate);
	}

	public void ApplyMaxDamageUpgrade(float max)
	{maxDamagePerSec = max;
		InvokeRepeating ("UpdateShield", 1, 1);
	}


	void UpdateShield()
	{
		damageAbsorbed = 0;
	}

	public float trigger(GameObject source,GameObject proj, UnitManager target,float damage)
	{
		if (AbsorbRecoil) {

			myStats.veternStat.UpEnergy( myStats.changeEnergy (damage/10));


		}

		return damage;

	}

	public void startRecharge(float delayTime = .1f)
	{

	
		if (currentCharging != null) {
			beginRechargeTime = Time.time + delayTime;
		} else {
			myStats.EnergyRegenPerSec = rechargeRate;

		}


	}



	public void stopRecharge()
	{
		if (currentCharging != null) {
			StopCoroutine (currentCharging);
		}
	}



	float lastShieldEffect;
	float StartRechargeTime;

	public float modify(float amount, GameObject src, DamageTypes.DamageType theType)
	{
		
		 if (myStats.currentEnergy > 0) {
			
			float energyLost = Mathf.Min (Absorbtion, myStats.currentEnergy);

			if (energyLost > amount) {
				energyLost = amount;
		
			}
			float damageReduction = energyLost;

			//For the triton Max denergy lost per second upgrade
			if (energyLost > 0) {
				damageAbsorbed += energyLost;
				if (damageAbsorbed > maxDamagePerSec) {
					energyLost = 0;
				}

			}

			myStats.changeEnergy (-energyLost);

			beginRechargeTime = Time.time + RechargeDelay;
			if (currentCharging == null) {
				currentCharging = StartCoroutine (chargeShields ());
			}

		
	
			if (shieldEffect && damageReduction > 0 && lastShieldEffect < Time.time - .6f) {
				lastShieldEffect = Time.time;

				if (myEffect == null) {
					myEffect =	Instantiate (shieldEffect, this.gameObject.transform.position, this.gameObject.transform.rotation, this.transform).GetComponent<ParticleSystem> ();
				} else {
					myEffect.Play ();
				}
			}

			return (amount - damageReduction);
		}
		else {
			return amount;
		}
	}

	public void changeAbsorption(float amount)
	{
		Absorbtion += amount;
		Descripton = "[Passive]\nConsumes energy to reduce incominng damage by " + Absorbtion + ".\nRegenerates " + rechargeRate + " energy per second out of combat.";
	
	}
		

	public override void setAutoCast(bool offOn){
	}


	override
	public continueOrder canActivate (bool showError)
	{

		continueOrder order = new continueOrder ();
		return order;
	}

	override
	public void Activate()
	{
		//return true;//next unit should also do this.
	}

	Coroutine currentCharging;
	float beginRechargeTime;

	IEnumerator chargeShields()
	{
		myStats.setEnergyRegen (0);
		while( Time.time < beginRechargeTime){
			yield return new WaitForSeconds (.5f);
			}
		myStats.setEnergyRegen (rechargeRate);
		currentCharging = null;
	}

}
