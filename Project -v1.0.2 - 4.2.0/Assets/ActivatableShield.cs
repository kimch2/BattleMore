using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatableShield : Ability,Modifier {



	public float EnergyBurnRate;
	public UnitStats myStats;

	public GameObject shieldEffect;
	ParticleSystem myEffect;
	public ComboTag.AbilType TagType;

	public List<ComboTag.AbilType> Combination;

	new void Awake()
	{
		base.Awake();
		audioSrc = GetComponent<AudioSource> ();
		myType = type.activated;
	}


	bool turnedOn;
    // Use this for initialization
    public override void Start()
    {
        myStats = GetComponent<UnitStats> ();
		myStats.addModifier (this);
	}




	float lastShieldEffect;

	public float modify(float amount, GameObject src, DamageTypes.DamageType theType)
	{
		if (turnedOn) {
			if (myStats.currentEnergy > 0) {

				float energyLost = Mathf.Min (amount, myStats.currentEnergy);


				myStats.changeEnergy (-energyLost);



				if (shieldEffect && energyLost > 0 && lastShieldEffect < Time.time - .6f) {
					lastShieldEffect = Time.time;

					if (myEffect == null) {
						myEffect =	Instantiate (shieldEffect, this.gameObject.transform.position, this.gameObject.transform.rotation, this.transform).GetComponent<ParticleSystem> ();
					} else {
						myEffect.Play ();
					}
				}

				return (amount - energyLost);
			} else {
				return amount;
			}
		} else {
			return amount;
		}
	}


	public override void setAutoCast(bool offOn){
	}
		


	override
	public continueOrder canActivate (bool showError)
	{

		continueOrder order = new continueOrder ();
		order.canCast = (myCost.canActivate (this));



		return order;
	}

	override
	public void Activate()
	{
		turnedOn = !turnedOn;
		autocast = turnedOn;
		if (turnedOn) {
			myCost.payCost ();

			if (ComboTag.CastTag (this.gameObject, TagType, Combination)) {
				myStats.changeEnergy (50);
			}
			StartCoroutine (UseEnergy());
		} 

		updateAutocastCommandCard ();

		//return true;//next unit should also do this.
	}

	IEnumerator UseEnergy()
	{
		while (turnedOn) {
			yield return new WaitForSeconds (1);
			if (myStats.currentEnergy > 5) {
				myStats.changeEnergy (-EnergyBurnRate);
			} else {
				Activate ();
				break;
			}
	
		}

	}

}
