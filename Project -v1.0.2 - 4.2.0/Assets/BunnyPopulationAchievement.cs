using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyPopulationAchievement: Achievement{

	public int eggCount;

	public override string GetDecription()
	{
		return Description;
	}

	public override void CheckBeginning (){
	}


	public override void CheckEnd (){
		if (!IsAccomplished () && isCorrectLevel()) {
			if (eggCount > GameObject.FindObjectOfType<bunnyManager> ().highestAmountSoFar) {
				Accomplished ();
			}
		}
	}

}