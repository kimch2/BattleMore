using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDamageBoost :  Ability,Notify {


	public IWeapon myWeapon;

	public GameObject chargingLightning;
	public GameObject lightningEffect;
	public float damagePerDistance = 1;
	public float MaxDamage = 30;
	Vector3 lastLocation;
	float currentDamage;
	Coroutine currentRevUp;


	public Animator myAnim;


	// Use this for initialization
	new void Start () {
		if (myWeapon == null) {

			myWeapon = this.gameObject.GetComponent<IWeapon> ();
		}
		lastLocation = transform.position;

		myWeapon.triggers.Add (this);
		currentRevUp = StartCoroutine (RevUp ());
	}


	public float trigger(GameObject source, GameObject proj,UnitManager target,float damage)
	{

		damage += currentDamage;
		currentDamage = 0;
		lastLocation = transform.position;
		if (currentRevUp == null) {
			currentRevUp = StartCoroutine (RevUp ());
		}
		lightningEffect.SetActive (false);
		return damage;

	}



	IEnumerator RevUp()
	{float moreDamage;
		while (true) {
		
			yield return new WaitForSeconds (.5f);
			moreDamage = Vector3.Distance (transform.position, lastLocation) * damagePerDistance;
			lastLocation = transform.position;
			if (moreDamage > 0) {
				currentDamage += moreDamage;
				chargingLightning.SetActive (moreDamage > 4);
	
				if (currentDamage > MaxDamage) {
					currentDamage = MaxDamage;
					break;
				}
			}
		}
		chargingLightning.SetActive (false);
		lightningEffect.SetActive (true);
		currentRevUp = null;
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

}