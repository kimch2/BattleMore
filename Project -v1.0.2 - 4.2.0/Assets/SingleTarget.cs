using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SingleTarget:  TargetAbility {

	protected UnitManager manage;
	int playerOwner;

	public GameObject missile;
	protected Selected mySelect;
	public List<UnitTypes.UnitTypeTag> cantTarget = new List<UnitTypes.UnitTypeTag>();
	public List<UnitTypes.UnitTypeTag> mustTarget = new List<UnitTypes.UnitTypeTag> ();
	public bool canTargetSelf;
	public enum sideTarget{ally, enemy, all}
	public sideTarget who;
	public int maxChargeCount;
	public IEffect AttributeEffect;

	Coroutine currentCharger;
	// Use this for initialization
	public void Start () {
		base.Start();
		myType = type.target;
		mySelect = GetComponent<Selected> ();
		manage = this.gameObject.GetComponent<UnitManager> ();
		if (manage)
		{
			playerOwner = manage.PlayerOwner;
		}
		else
		{
			playerOwner = 1; // Assuming the only things without managers are Ults
		}

		if (chargeCount >-1) {
			if(chargeCount < maxChargeCount){
				if (currentCharger != null)
				{
					StopCoroutine(currentCharger);
				}
				currentCharger = StartCoroutine (increaseCharges ());
			}
		}
	}


	public void UpMaxCharge()
	{
		maxChargeCount = 3;

		if (currentCharger == null) {
			currentCharger = StartCoroutine (increaseCharges ());

		}
	}


	override
	public continueOrder canActivate(bool showError){

		continueOrder order = new continueOrder ();

		if (myCost && !myCost.canActivate (this)) {
			order.canCast = false;
			if (myCost.energy == 0 && myCost.resourceCosts.MyResources.Count == 0 && chargeCount > 0) {
				order.canCast = true;
				order.nextUnitCast = false;
			}
		} else {
			order.nextUnitCast = false;
		}
		return order;
	}

	override
	public void Activate()
	{
	}  // returns whether or not the next unit in the same group should also cast it


	override
	public  void setAutoCast( bool offOn){}

	public override bool isValidTarget (GameObject target, Vector3 location){

		if (target == null) {
			return false;
		}

		UnitManager m = target.GetComponent<UnitManager> ();
		if (m == null) {
			return false;}
		switch (who) {

		case sideTarget.ally:
			if (playerOwner != m.PlayerOwner) {
				return false;
			}
			break;

		case sideTarget.enemy:
			if (playerOwner == m.PlayerOwner) {
				return false;
			}
			break;

		}
		if (!canTargetSelf && target == this.gameObject) {
			return false;

		}

		foreach (UnitTypes.UnitTypeTag t in cantTarget) {
			
			if (m.myStats.isUnitType (t)) {
				return false;}
		}

		if (mustTarget.Count == 0) {
			return true;
		}
		foreach (UnitTypes.UnitTypeTag t in mustTarget) {
				if (m.myStats.isUnitType (t)) {
					return true;}
			}
	
		return false;

	}


	override
	public  bool Cast(GameObject tar, Vector3 location)
	{target = tar;
		
		if (target) {


			if (chargeCount >-1) {
				changeCharge (-1);
				if (currentCharger == null) {
					currentCharger = StartCoroutine (increaseCharges ());
				}
			}
			if (myCost) {
				myCost.payCost ();
			}
			GameObject proj = null;

			if (missile) {
				Vector3 pos = this.gameObject.transform.position;
				pos.y += this.gameObject.GetComponent<CharacterController> ().radius;
				proj = (GameObject)Instantiate (missile, pos, Quaternion.identity);

				Projectile script = proj.GetComponent<Projectile> ();

				if (script) {
					script.target = target.GetComponent<UnitManager> ();
					script.Source = this.gameObject;
					script.Initialize (target.GetComponent<UnitManager>(),0,manage);
				} else {
					proj.SendMessage ("setSource", this.gameObject,SendMessageOptions.DontRequireReceiver);
					proj.SendMessage ("setTarget", tar,SendMessageOptions.DontRequireReceiver);
				}

			} else {

				AttributeEffect.apply (this.gameObject, tar);
			}
		}



		return false;

	}
	override
	public void Cast(){


	//	Debug.Log ("Casting in other");
		if (target) {
			if (chargeCount >-1) {
				changeCharge (-1);
				if (currentCharger == null) {
					currentCharger = StartCoroutine (increaseCharges ());
				}
			}
			if (myCost) {
				myCost.payCost ();
			}
			GameObject proj = null;

			if (missile) {
				Vector3 pos = this.gameObject.transform.position;
				pos.y += this.gameObject.GetComponent<CharacterController> ().radius;
				proj = (GameObject)Instantiate (missile, pos, Quaternion.identity);

				Projectile script = proj.GetComponent<Projectile> ();
				if (script) {
					script.Initialize (target.GetComponent<UnitManager>(),0, manage);
					script.setup ();
				} else {

					proj.SendMessage ("setSource", this.gameObject, SendMessageOptions.DontRequireReceiver);
					proj.SendMessage ("setTarget", target, SendMessageOptions.DontRequireReceiver);
					proj.SendMessage ("setDamage", 0, SendMessageOptions.DontRequireReceiver);
				}
			

			} else {

				AttributeEffect.apply (this.gameObject, target);
			}
		}

	}




	IEnumerator increaseCharges()
	{

		if (chargeCount == 0) {
			active = false;
		}

		myCost.startCooldown ();
	//	Debug.Log("Waiting for " + myCost.cooldown);
		yield return new WaitForSeconds (myCost.cooldown-.15f);
		//Debug.Log("Done Waiting");
		active = true;
		changeCharge (1);

		if (chargeCount < maxChargeCount) {
			currentCharger = StartCoroutine (increaseCharges ());
		} else {
			currentCharger = null;
		}
	}
	public void changeCharge(int n)
	{
		chargeCount += n;
		if (chargeCount == 0) {
			active = false;

		}
		if (chargeCount > maxChargeCount) {
			chargeCount = maxChargeCount;
		}
		if (mySelect && mySelect.IsSelected) {
			RaceManager.upDateUI ();
			RaceManager.updateActivity ();

		}
	}

}
