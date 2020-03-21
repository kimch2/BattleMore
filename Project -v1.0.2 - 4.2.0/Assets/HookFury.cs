using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HookFury : MonoBehaviour {

    // Changes attack speed based on this guys current health percentage


	private UnitStats myStats;

	void Start () {
		myStats = GetComponent<UnitStats> ();
	}

	public void trigger()
	{
		float toChange = -(.5f - (myStats.health / myStats.Maxhealth) / 2);
		//Debug.Log ("Changing " + toChange);

		myStats.statChanger.removeAttackSpeed(this);
		myStats.statChanger.changeAttackSpeed(toChange, 0, this, true);
	}


}
