using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionUpgrade : Upgrade {


	// meant to give the zephyr splash damage
	public GameObject newProjectile;

	override
	public void applyUpgrade (GameObject obj){

		UnitManager manage = obj.GetComponent<UnitManager> ();
		if(manage && manage.UnitName == "Zephyr")
		{
			foreach(IWeapon weap in manage.myWeapon)
			{
				if(weap.Title == "Stinger Missiles")
				{weap.myIcon = iconPic;
					weap.projectile = newProjectile;
					weap.resetBulletPool ();
				}
			}
		}

	}

	public override void unApplyUpgrade (GameObject obj){

	}

}
