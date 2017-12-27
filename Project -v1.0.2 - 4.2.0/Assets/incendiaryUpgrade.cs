using UnityEngine;
using System.Collections;

public class incendiaryUpgrade:SpecificUpgrade{

	public override void applyUpgrade (GameObject obj){

		if (confirmUnit (obj)) {


			SlowDebuff debuff = obj.AddComponent<SlowDebuff> ();
			debuff.duration = 3;
			debuff.speedDecrease = 0;
			debuff.percent = -.3f;
			debuff.AddToWeapon ();

		}
	}


	public override void unApplyUpgrade (GameObject obj){

	}

	public override float ChangeString (string name, float number)
	{

		return number;
	}
}
