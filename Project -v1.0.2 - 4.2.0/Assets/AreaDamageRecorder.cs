using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamageRecorder : AreaDamage{

	public string DamageName;

	public override void UpdateDamage () {

		if (enemies.Count > 0) {

			enemies.RemoveAll (item => item == null);
			foreach (UnitStats s in enemies) {

				if (NonStack) {
					if (!DamageNonStacker.instance.DealDamage (gameObject.name, s, damage)) {
						return;
					}
				}
				float returned = s.TakeDamage(damage + (s.isUnitType(BonusDamage.type) ? BonusDamage.bonus : 0), this.gameObject.gameObject.gameObject, myType,myHitContainer);

				if (showPop) {
					iter++;
					if (iter == 6) {
						PopUpMaker.CreateGlobalPopUp (-(damage * 2) + "", Color.red, s.gameObject.transform.position);
						iter = 0;
					}
				}
				if (cutEffect) {
					Instantiate (cutEffect, s.gameObject.transform.position, Quaternion.identity);
				}
				PlayerPrefs.SetFloat (DamageName, PlayerPrefs.GetFloat(DamageName, 0) + returned);

				//obj.transform.SetParent (this.gameObject.transform);
			}
		}

	}
}
