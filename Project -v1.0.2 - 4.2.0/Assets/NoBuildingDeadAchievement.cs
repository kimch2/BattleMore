using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoBuildingDeadAchievement:Achievement{

public override string GetDecription()
{return Description;
}

public override void CheckBeginning (){
}


public override void CheckEnd (){
		if (IsAccomplished ()) {
			return;
		}
		if (isCorrectLevel ()) {


			foreach (VeteranStats vets in  GameObject.FindObjectOfType<GameManager> ().playerList[0].getVeteranStats()) {
				if (vets.Died) {
					if (vets.UnitName == "Construction Yard" || vets.UnitName == "Aether Core" || vets.UnitName == "Ballistics Lab" ||
					    vets.UnitName == "Engineering Bay" || vets.UnitName == "Flux Array" || vets.UnitName == "Academy") {
						return;
					}

				}
			}
			Accomplished ();
		}
	}
}