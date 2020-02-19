using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitExistAchievement :Achievement {


	public int MinNumber;
	public int maxNumber;

	public override string GetDecription()
	{return Description;
	}

	public override void CheckBeginning (){
	}

	public override void CheckEnd ()
	{
		if(!IsAccomplished() && isCorrectLevel()){
			
		int counter = 0;


		foreach (VeteranStats vets in  GameObject.FindObjectOfType<GameManager> ().playerList[0].getVeteranStats()) {
				if (vets.Died && vets.isWarrior) {
				counter++;

			}
		}
			if(counter >= MinNumber &&  counter <= maxNumber)
		{
				Accomplished();
		}
	
		}

	}

	public override void Reset()
	{
	}
}