using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gigapede : VisionTrigger, Modifier {

	public List<GameObject> segments = new List<GameObject> ();

	public GameObject toSpawn;
	int segmentLeft = 5;
	float DamageAmount;
	private UnitStats mystats;

	// Use this for initialization
	void Start () {

		mystats = GetComponent<UnitStats> ();
		mystats.addModifier (this);
		DamageAmount = mystats.Maxhealth / segmentLeft;
	}



	public float modify(float damage, GameObject source, OnHitContainer hitSource, DamageTypes.DamageType theType)
	{

		if (segmentLeft > 1 && (mystats.health - (damage -mystats.armor)) < (segmentLeft - 1) * DamageAmount) {
			segmentLeft--;
		
			mystats.armor--;
			Instantiate (toSpawn, segments[segmentLeft].transform.position, Quaternion.identity);
			segments [segmentLeft].SetActive (false);

			foreach (UnitManager unit in InVision) {
				updateDamageBuff (unit);
			}
		}


		return damage;
	}


	public override void  UnitEnterTrigger(UnitManager manager)
	{
		if(segmentLeft >1 && manager.gameObject != gameObject){
			manager.myStats.statChanger.changeWeaponDamage(0, segmentLeft - 1, this, true);
		}
		
	}

	public override void  UnitExitTrigger(UnitManager manager)
	{
		manager.myStats.statChanger.removeWeaponDamage(this);
	}


	public void updateDamageBuff(UnitManager manager )
	{
		manager.myStats.statChanger.removeWeaponDamage(this);


		if(this &&manager && gameObject && segmentLeft >1 && manager.gameObject != gameObject){
			manager.myStats.statChanger.changeWeaponDamage(0, segmentLeft - 1, this, true);
		}
	}

}
