using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangeAmmo : Ability {

	// Used by Chimera

	//UnitManager myManager;
	public GameObject myAmmo;
    public OnHitContainer NewOnHitContainer;
	public IWeapon myWeapon;
	public float range;
	public float attackPeriod;
	public float attackDamage;
	public int numOfShots =1;
	public string AnimationName;
	public List<IWeapon.bonusDamage> bonus = new List<IWeapon.bonusDamage>();
	// Use this for initialization
	public List<UnitTypes.UnitTypeTag> cantAttackTypes = new List<UnitTypes.UnitTypeTag> ();

	Lean.LeanPool myBulletPool;

	//Used by Seer;
	[Tooltip("If 0, will be permanant, used by Seer")]
	public float Duration = 0;
	public GameObject PreviousAmmo;
    OnHitContainer PreviousOnHitContianer;

	new void Awake()
	{
		base.Awake();
		audioSrc = GetComponent<AudioSource> ();

		myType = type.activated;
	}

	new void Start () {
		base.Start();
		if (myAmmo) {
			myBulletPool = Lean.LeanPool.getSpawnPool (myAmmo);
		}



		//myManager = GetComponent<UnitManager> ();
		if (autocast) {
			Activate ();
		}
	}



	public override void setAutoCast(bool offOn){
		Activate ();
	}

	public void resetBulletPool()
	{
		myBulletPool = Lean.LeanPool.getSpawnPool (myAmmo);
	}

	override
	public continueOrder canActivate (bool showError)
	{
		if (!myCost)
		{
			return new continueOrder();
		}

		continueOrder order = new continueOrder
		{
			canCast = myCost.canActivate(this)
		};

		return order;
	
	}

	public void upgrade(string upgradeName)
	{
		if (autocast) {
	//		Debug.Log (attackDamage + "   " + myWeapon.getUpgradeLevel());
			myWeapon.baseDamage = attackDamage + myWeapon.getUpgradeLevel()*5;
		}
	
	}

	override
	public void Activate()
	{


		if (myCost)
		{
			myCost.payCost();
		}

		autocast = true;
		myWeapon.projectile = myAmmo;
        myWeapon.myHitContainer = NewOnHitContainer;
		myWeapon.baseDamage = attackDamage + myWeapon.getUpgradeLevel()*5;
		myWeapon.AnimationName = AnimationName;
		myWeapon.range = range;
		myWeapon.setBulletPool (myBulletPool);
		myWeapon.extraDamage = bonus;
		myWeapon.cantAttackTypes = cantAttackTypes;
		myWeapon.numOfAttacks = numOfShots;
		foreach (ChangeAmmo ca in GetComponents<ChangeAmmo>()) {
			if (ca != this) {
				ca.autocast = false;
			}
		}

		updateUICommandCard ();

		if (Duration > 0)
		{
			Invoke("switchBack", Duration);
		}
	}


	void switchBack()
	{
		myWeapon.projectile = PreviousAmmo;
        myWeapon.myHitContainer = PreviousOnHitContianer;
        myWeapon.setBulletPool(Lean.LeanPool.getSpawnPool (PreviousAmmo));
	}

}
