using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyRestoreAbility : SingleTargetCombo {


	public float energyRestoreTotal;
	public float RestoreTime;

	override
	public  bool Cast(GameObject tar, Vector3 location)
	{
		target = tar;
		Cast ();

		return false;

	}
	override
	public void Cast(){

		if (target) {


			if (myCost) {
				myCost.payCost ();
			}

			if (ComboTag.CastTag (target, TagType, Combination)) {
				foreach (Ability ab in target.GetComponent<UnitManager>().abilityList) {

					if (ab && ab.myCost) {
						ab.myCost.cooldownTimer -= 5;
					
					}
				}
			}
			StartCoroutine (recharge (target.GetComponent<UnitStats> (), 1));

		}
	
	}


	IEnumerator recharge(UnitStats toRecharge, float multiplier) // Change this later so that this energy restore will stack multiplicativly with others.
	{
		for (float i = 0; i < RestoreTime * 2; i++) {
			if (!toRecharge) {
				break;
			}
			toRecharge.changeEnergy ((energyRestoreTotal * multiplier) / (RestoreTime*2));
			yield return new WaitForSeconds (.5f);
		}

	}
}
