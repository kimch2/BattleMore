using UnityEngine;
using System.Collections;

public class incendiaryUpgrade:SpecificUpgrade{

	public override void applyUpgrade (GameObject obj){

        if (confirmUnit(obj)) {

            UnitManager manager = obj.GetComponent<UnitManager>();
            foreach (IWeapon weap in manager.myWeapon)
             {
                SlowDebuff debuff =  weap.myHitContainer.gameObject.AddComponent<SlowDebuff>();
                debuff.duration = 3;
                debuff.speedDecrease = 0;
                debuff.speedPercent = -.3f;
                weap.myHitContainer.AddDamageTrigger(debuff);
            }

		}
	}


	public override void unApplyUpgrade (GameObject obj){

	}

	public override float ChangeString (string name, float number)
	{

		return number;
	}
}
