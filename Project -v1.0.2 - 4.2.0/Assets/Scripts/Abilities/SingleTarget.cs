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
	public IEffect AttributeEffect;

	protected Lean.LeanPool myBulletPool;

	Coroutine currentCharger;

    // Use this for initialization
    public override void Start()
    { 

        myType = type.target;
		mySelect = GetComponent<Selected> ();
		manage = this.gameObject.GetComponent<UnitManager> ();

        if (!manage)
        {
            manage = this.gameObject.GetComponentInParent<UnitManager>();
        }
		if (manage)
		{
			playerOwner = manage.PlayerOwner;
		}
		else
		{
			playerOwner = 1; // Assuming the only things without managers are Ults
		}

        InitializeCharges();

		if (missile)
		{
			if (!myBulletPool)
			{
				myBulletPool = Lean.LeanPool.getSpawnPool(missile);
			}
		}
	}



	override
	public continueOrder canActivate(bool showError){

        return BaseCanActivate(showError);
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
		if (!canTargetSelf && target == myManager.gameObject) {
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


            changeCharge(-1);
			if (myCost) {
				myCost.payCost ();
			}
			GameObject proj = null;

			if (missile) {
				Vector3 pos = this.gameObject.transform.position;
				pos.y += this.gameObject.GetComponent<CharacterController> ().radius;
				proj = myBulletPool.FastSpawn(transform.position, Quaternion.identity);
				Projectile script = proj.GetComponent<Projectile> ();

				if (script) {
                    // SET THE ONHIT CONTAINER!
					script.Initialize (target.GetComponent<UnitManager>(),0,manage, null);

				} else {
					proj.SendMessage ("setSource", this.gameObject,SendMessageOptions.DontRequireReceiver);
					proj.SendMessage ("setTarget", tar,SendMessageOptions.DontRequireReceiver);
				}

			} else {

				AttributeEffect.applyTo (this.gameObject, tar.GetComponent<UnitManager>());
			}
		}



		return false;

	}
	override
	public void Cast(){

	//	Debug.Log ("Casting in other");
		if (target) {
            changeCharge(-1);
			if (myCost) {
				myCost.payCost ();
			}
			GameObject proj = null;

			if (missile) {
				Vector3 pos = this.gameObject.transform.position;
				pos.y += this.gameObject.GetComponentInParent<CharacterController> ().radius;
				proj = myBulletPool.FastSpawn(transform.position, Quaternion.identity);


				Projectile script = proj.GetComponent<Projectile> ();
				if (script) {
                    // SET THE ONHITCONTAINER!
					script.Initialize (target.GetComponent<UnitManager>(),0, manage, null);

				} else {

					proj.SendMessage ("setSource", this.gameObject, SendMessageOptions.DontRequireReceiver);
					proj.SendMessage ("setTarget", target, SendMessageOptions.DontRequireReceiver);
					proj.SendMessage ("setDamage", 0, SendMessageOptions.DontRequireReceiver);
				}
			

			} else {

				AttributeEffect.applyTo (this.gameObject, target.GetComponent<UnitManager>());
			}
		}

	}




	

}
