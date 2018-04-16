using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCubeAchievement : Achievement{

	public override string GetDecription()
	{return Description;
	}

	public override void CheckBeginning (){
	}

	public override void CheckEnd (){

		float lastDeathTime =0;
		if (!IsAccomplished () && isCorrectLevel()) {

			foreach (VeteranStats vets in GameManager.main.playerList[1].getUnitStats()) {
					
					if (vets.unitType == "Death Cube" && vets.Died) {
						if (vets.kills == 0) {
							Accomplished ();
						}	

					}

			}
		}
	}


}