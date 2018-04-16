using UnityEngine;
using System.Collections;

public class HadrianLifeAch : Achievement{

	public string UnitName;
	public float maxDamage;

	public override string GetDecription()
	{return Description;
	}

	public override void CheckBeginning (){
	}


	public override void CheckEnd (){
		if (!IsAccomplished () && isCorrectLevel()) {
			
			float totalDamage = 0;
			foreach (VeteranStats vets in  GameObject.FindObjectOfType<GameManager> ().playerList[0].getUnitStats()) {
				if (vets.UnitName == UnitName) {
					totalDamage += vets.damageTaken;

					if (vets.damageTaken <= maxDamage) {
						Accomplished ();
					}

				}
			}
			if (totalDamage <= maxDamage) {
				Accomplished ();
			}
		}
	}


}
