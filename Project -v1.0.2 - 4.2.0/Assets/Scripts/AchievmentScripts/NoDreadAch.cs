﻿using UnityEngine;
using System.Collections;

public class NoDreadAch: Achievement{

	public override string GetDecription()
	{return Description;
	}

	public override void CheckBeginning (){
	}

	public override void CheckEnd (){
		if (!IsAccomplished () && isCorrectLevel()) {

				foreach (VeteranStats vets in  GameObject.FindObjectOfType<GameManager> ().playerList[1].getUnitStats()) {
					if (vets.unitType == "DreadNaught") {
						return;
					}


				}
				Accomplished ();

		}
	}


}