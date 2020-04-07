using UnityEngine;
using System.Collections;

public class OathFury : Ability, Modifier, Notify {
	// Unit modifier that make them move Life Steal more the less health they have
	private IWeapon myWeapon;

	private float initialLifeSteal;
	private UnitStats myStats;

	private LifeSteal myStealer;

	// Use this for initialization
	new void Start () {
		myWeapon = GetComponent<IWeapon> ();

		myStats = GetComponent<UnitStats> ();
		myStats.addModifier (this);
		myWeapon.triggers.Add (this);
		myStealer = GetComponent<LifeSteal> ();
		initialLifeSteal = myStealer.percentage;
	}

	public override continueOrder canActivate(bool showError){
		return new continueOrder ();
	}
	public  override void Activate(){
	}  // returns whether or not the next unit in the same group should also cast it
	public  override void setAutoCast(bool offOn){}





	public float trigger(GameObject source, GameObject projectile, UnitManager target, float damage)
	{
		myStealer.percentage = initialLifeSteal + (1 - (myStats.health / myStats.Maxhealth));
		return damage;

	}

	public float modify(float damage, GameObject source, OnHitContainer hitSource, DamageTypes.DamageType theType)
	{

		myStealer.percentage = initialLifeSteal + (1 - (myStats.health / myStats.Maxhealth));


		return damage;
	}

}