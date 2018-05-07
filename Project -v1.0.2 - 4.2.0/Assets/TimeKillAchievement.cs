using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeKillAchievement : Achievement{


	public string UnitToKill = "Nimbus";
	public float TimeFrame;


	public override string GetDecription()
	{return Description;
	}

	public override void CheckBeginning (){
	}

	public override void CheckEnd (){
		if (!IsAccomplished () && isCorrectLevel()) {

			List<VeteranStats> targetUnits = new List<VeteranStats> ();
			foreach (VeteranStats vets in GameManager.main.playerList[1].getVeteranStats()) {
				
				if (vets.Died && vets.unitType == UnitToKill) {
					targetUnits.Add (vets);
				}
			}
				
			foreach (VeteranStats vetsA in targetUnits) {
				foreach (VeteranStats vetsB in targetUnits) {
					if (vetsA != vetsB) {

						if (Mathf.Abs (vetsA.DeathTime - vetsB.DeathTime) <  TimeFrame) {
							Accomplished ();
							return;
						} 
					}
				}				
			}
		}
	}


}
