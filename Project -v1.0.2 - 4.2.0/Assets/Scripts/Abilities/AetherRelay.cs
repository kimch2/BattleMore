using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.SoundManagerNamespace;
public class AetherRelay : Ability, AllySighted,EnemySighted{

	List<DayexaShield> shieldList = new List<DayexaShield> ();
	List<UnitStats> enemyStats = new List<UnitStats> ();
	public float energyChargeRate;



	public MultiShotParticle myEffect;
	public GameObject chargeEffect;

	public float damageRate;
	bool turnedOn;
    // Use this for initialization
    public override void Start()
    { 
        myType = type.activated;
		myManager.AddAllySighted (this);
		myManager.AddEnemySighted (this);

		InvokeRepeating ("UpdateAether", 1, 1);


		if (GetComponent<DayexaShield>().maxDamagePerSec  < 1000) {
			Descripton += " Maximum of " + GetComponent<DayexaShield>().maxDamagePerSec +" damage per second can be taken while field is active.";
		}
	}
	
	// Update is called once per frame
	void UpdateAether () {

			if (turnedOn) {
				if (myManager.myStats.currentEnergy <= 35) {
					turnedOn = !turnedOn;
					autocast = false;
					myEffect.stopEffect ();
				updateAutocastCommandCard ();


				}
				else{
				myManager.getUnitStats ().TakeDamage (.1f, this.gameObject,DamageTypes.DamageType.Regular);
				myManager.myStats.changeEnergy (-34.9f);
					if (soundEffect) {
						SoundManager.PlayOneShotSound(audioSrc, soundEffect);
					}


					foreach (UnitStats us in enemyStats) {
						if (us) {
							if (Vector3.Distance (us.transform.position, this.transform.position) < 40) {
								float actual = us.TakeDamage (damageRate, this.gameObject, DamageTypes.DamageType.Regular);
							myManager.myStats.veternStat.UpdamageDone (actual);
							}
						}
					}
				}

			} else {

			float total = 0;
				foreach (DayexaShield ds in shieldList) {
					if (ds) {
						if (ds.myStats.currentEnergy < ds.myStats.MaxEnergy) {
							float actual = ds.myStats.changeEnergy (energyChargeRate);
							Instantiate (chargeEffect, ds.transform.position, Quaternion.identity);
						total += actual;
							
						}
					}
				}
			myManager.myStats.veternStat.UpEnergy(total);
			}

	}


	public void AllySpotted (UnitManager otherManager)
	{
		DayexaShield s = otherManager.gameObject.GetComponent<DayexaShield> ();
		if (s) {
			shieldList.Add (s);
		}
	}

	public void EnemySpotted (UnitManager otherManager)
	{
		
		enemyStats.Add (otherManager.getUnitStats ());
	}

	public void enemyLeft (UnitManager otherManager)
	{
		enemyStats.Remove(otherManager.getUnitStats ());
	}

	public void allyLeft (UnitManager otherManager)
	{
		if (otherManager) {
			DayexaShield s = otherManager.gameObject.GetComponent<DayexaShield> ();
			if (s) {
				shieldList.Remove (s);
			}
		}
	}
		

	public override void setAutoCast(bool offOn){
	}


	override
	public continueOrder canActivate (bool showError)
	{

		continueOrder order = new continueOrder ();


		if (myManager.myStats.currentEnergy < 20) {
			order.canCast = false;
			return order;}
		
		return order;
	}

	override
	public void Activate()
	{
		turnedOn = !turnedOn;
		autocast = turnedOn;
		if (turnedOn) {
			myManager.getUnitStats ().TakeDamage (.1f, this.gameObject, DamageTypes.DamageType.Regular);
			myManager.getUnitStats ().changeEnergy (.1f);
			myEffect.continueEffect ();
			myCost.payCost ();

		} else {
			
			myEffect.stopEffect ();
		
		}

		updateAutocastCommandCard ();

		//return true;//next unit should also do this.
	}




}
