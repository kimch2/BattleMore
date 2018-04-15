using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNonStacker : MonoBehaviour {
	// Makes it so you can have non-Stacking Area Damage;
	// also keeps track of Damage Totals for things like achievements so we dont have to constantly update playerprefs

	public static DamageNonStacker instance;

	// Use this for initialization
	void Awake () {
		instance = this;

	}

	List<AOEffect> CurrentDamagers = new List<AOEffect>();

	public bool DealDamage(string DamName, UnitStats manager, float DamageAmount)
	{
		AOEffect aoe = CurrentDamagers.Find (item =>item.DamageName == DamName);
		if (aoe!= null) {
			return aoe.DealDamage (manager, DamageAmount);
		} else {
			aoe = new AOEffect ();
			aoe.DamageName = DamName;
			CurrentDamagers.Add (aoe);
			return aoe.DealDamage (manager, DamageAmount);
		}
	}


}


class AOEffect{

	public string DamageName;

	public Dictionary<int, float> LastDamageTime = new Dictionary<int, float> ();

	public float TotalDamageDone;

	public bool DealDamage(UnitStats toDamage, float DamageAmount)
	{

		float time;
		int ID = toDamage.gameObject.GetInstanceID ();
		if (LastDamageTime.TryGetValue (ID, out time)) {

			if (time < Time.time - .2f) {
				LastDamageTime [ID] = Time.time;
				TotalDamageDone += DamageAmount;
				return true;
			} else {
				return false;
			}

		} else {
			LastDamageTime.Add (ID, Time.time);
			TotalDamageDone += DamageAmount;
			return true;
		}
	}
	 
}

