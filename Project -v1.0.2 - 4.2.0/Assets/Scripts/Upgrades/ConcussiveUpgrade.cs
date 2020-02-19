using UnityEngine;
using System.Collections;

public class ConcussiveUpgrade :SpecificUpgrade{




	//[ToolTip("Only fill these in if this upgrade replaces another one")]

	//public GameObject UIButton;

	public override void applyUpgrade (GameObject obj){
		if (confirmUnit (obj)) {
			//obj.GetComponent<SlowDebuff> ().enabled = true;

			IWeapon weap = obj.GetComponent<IWeapon> ();

			if (weap) {

				IWeapon.bonusDamage bd = new IWeapon.bonusDamage ();
				bd.bonus = 6;
				for ( int i =0; i < weap.extraDamage.Count; i++) {
					if (weap.extraDamage[i].type == UnitTypes.UnitTypeTag.Structure) {
						bd.bonus += weap.extraDamage [i].bonus;
						weap.extraDamage [i] = bd;
						return;
					}
				}
			
				bd.bonus = 6;
				bd.type = UnitTypes.UnitTypeTag.Structure;
				obj.GetComponent<IWeapon> ().extraDamage.Add (bd);

			}
		}
	}


	public override void unApplyUpgrade (GameObject obj){
		if (confirmUnit (obj)) {
		IWeapon.bonusDamage toRemove = new IWeapon.bonusDamage();
		foreach (IWeapon.bonusDamage bd in GetComponent<IWeapon>().extraDamage) {
			if (bd.type == UnitTypes.UnitTypeTag.Structure && bd.bonus == 6) {
				toRemove = bd;
			}
		}

		if (toRemove.bonus != 0) {
			GetComponent<IWeapon> ().extraDamage.Remove (toRemove);
		}
		//obj.GetComponent<SlowDebuff> ().enabled = false;
		}
	}

	public override float ChangeString (string name, float number)
	{
		return number;
	}

	public override string AddString (string name, string ToAddOn)
	{
		if ("Damage" == name) {
			return ToAddOn + " (+6 vs Structures)";
		}
		return ToAddOn;
	}
}
