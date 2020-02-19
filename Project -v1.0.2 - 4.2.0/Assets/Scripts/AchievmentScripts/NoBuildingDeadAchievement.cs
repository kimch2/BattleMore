using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoBuildingDeadAchievement:Achievement{

	public int MinimumDeath;
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
			int counter = 0;

			foreach (VeteranStats vets in GameManager.main.playerList[0].getVeteranStats()) {
				if (vets.Died) {
					if (vets.unitType == "Construction Yard" || vets.unitType == "Aether Core" || vets.unitType == "Aviatrix" ||
						vets.unitType == "Engineering Bay" || vets.unitType == "Flux Array" || vets.unitType == "Academy" || vets.unitType == "Armory") {
						counter++;
					}

				}
			}

			if (counter <= MinimumDeath) {
				Accomplished ();
			}
		}
	}
}