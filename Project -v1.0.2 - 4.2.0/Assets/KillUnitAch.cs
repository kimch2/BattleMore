using UnityEngine;
using System.Collections;

public class KillUnitAch :Achievement {

	public string UnitName;
	public int MinNumber;
	public bool OnlyInOneLevel;

	public override string GetDecription()
	{if (OnlyInOneLevel) {
			return Description;
		}
	return Description + "          " + PlayerPrefs.GetInt (UnitName + "Deaths", 0)  + "/" + MinNumber;
	
	}

	public override void CheckBeginning (){
	}

	public override void CheckEnd ()
	{
		if (!IsAccomplished ()) {
			if (isCorrectLevel ()) {

				int counter = 0;

				foreach (VeteranStats vets in  GameObject.FindObjectOfType<GameManager> ().playerList[1].getVeteranStats()) {

					if (vets.unitType.Contains (UnitName) && vets.Died) {
						counter++;

					}
				}
				if (OnlyInOneLevel) {
					if (counter >= MinNumber) {
						Accomplished ();
					}
				} else {
					int total = PlayerPrefs.GetInt (UnitName + "Deaths", 0) + counter;
					PlayerPrefs.SetInt (UnitName + "Deaths", total);
					if (total >= MinNumber) {
						Accomplished ();
					}
				}
			}
		}

	}

	public override void Reset()
	{PlayerPrefs.SetInt (UnitName + "Deaths", 0);
		PlayerPrefs.SetInt (Title, 0);
	}
}