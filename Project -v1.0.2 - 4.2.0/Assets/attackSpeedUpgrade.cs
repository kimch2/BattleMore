using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class attackSpeedUpgrade  : Upgrade {


	[System.Serializable]
	public struct unitAmount
	{
		public string UnitName;
		public List<float> flatAmount;
		public List<float> percentageAmount;
	}

	[Tooltip("If you use this, all units will use the first entry")]
	public bool AllUnits;



	public List<unitAmount> unitsToUpgrade = new List<unitAmount> ();


	override
	public void applyUpgrade (GameObject obj){

		UnitManager manager = obj.GetComponent<UnitManager>();
		//if (obj.GetComponentInChildren<TurretMount> ()) {
		//return;}




		foreach (unitAmount ua in unitsToUpgrade) {
			if (AllUnits || manager.UnitName.Contains(ua.UnitName)) {
				//Debug.Log ("Applying to "+ ua.UnitName + "  " + obj.name);


				foreach (ChangeAmmo ca in manager.GetComponents<ChangeAmmo>()) {
					ca.attackPeriod -= ua.flatAmount[0];
				}



				for (int i = 0; i < manager.myWeapon.Count; i++) {

					if (manager.myWeapon [i]) {


						manager.myWeapon [i].changeAttackSpeed (ua.percentageAmount [AllUnits ? 0 : i], ua.flatAmount [AllUnits ? 0 : i], true, null);
						manager.gameObject.SendMessage ("upgrade", Name, SendMessageOptions.DontRequireReceiver);
					}
				
				if (manager.GetComponent<Selected> ().IsSelected) {
					//RaceManager.upDateUI ();
					}
				}

			}
		}




	}

	public override void unApplyUpgrade (GameObject obj){

	}

}
