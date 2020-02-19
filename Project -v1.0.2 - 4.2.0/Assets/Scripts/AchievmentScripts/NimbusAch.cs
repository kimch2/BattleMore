using UnityEngine;
using System.Collections;

public class NimbusAch  : Achievement{


	public string UnitName = "Nimbus";
	public int minimumKillAmount = 20;
	public bool AllOnOne = false;
	public override string GetDecription()
	{return Description;
	}

	public override void CheckBeginning (){
	}

	public override void CheckEnd (){
		if (!IsAccomplished () && isCorrectLevel()) {

			float counter = 0;
			foreach (VeteranStats vets in GameManager.main.activePlayer.getUnitStats()) {
				if (vets.UnitName == UnitName) {

					if (AllOnOne) {
						
						if (vets.kills >= minimumKillAmount) {
							Accomplished ();
						}
					} else {
						counter += vets.kills;
					}
				}
			}
			if (counter >= minimumKillAmount) {
				Accomplished ();
			}
		}
	}


}
