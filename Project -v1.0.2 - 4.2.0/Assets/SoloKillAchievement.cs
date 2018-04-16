using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloKillAchievement : Achievement{

	public string UnitName;

	public override string GetDecription()
	{return Description;
	}

	public override void CheckBeginning (){
	}


	public override void CheckEnd (){
		if (!IsAccomplished () && isCorrectLevel()) {
	

			foreach (VeteranStats vets in  GameObject.FindObjectOfType<GameManager> ().playerList[1].getUnitStats()) {
				if (vets.Died && vets.UnitName != UnitName) {
					return;
				}
			}
			Accomplished ();
				
		}
	}


}
