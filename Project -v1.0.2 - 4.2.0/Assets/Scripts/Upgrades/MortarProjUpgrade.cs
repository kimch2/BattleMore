using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarProjUpgrade : Upgrade {


	public List<unitAmount> unitsToUpgrade ;
	[System.Serializable]
	public struct unitAmount
	{
		public string UnitName;
		public float ArcAmount;
		public float ProjectileSpeed;
	}


	override
	public void applyUpgrade (GameObject obj){


		UnitManager manager = obj.GetComponent<UnitManager>();
		foreach (unitAmount ua in unitsToUpgrade) {
			if (manager.UnitName.Contains(ua.UnitName)) {

				MortarWeapon weap = obj.GetComponent<MortarWeapon> ();
				if (weap) {
					weap.attackArc = ua.ArcAmount;
					weap.projectileSpeed = ua.ProjectileSpeed;
					}

			}
		}


	}
		

	public override void unApplyUpgrade (GameObject obj){

	}


}
