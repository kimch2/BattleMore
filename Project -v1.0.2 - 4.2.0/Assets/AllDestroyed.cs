using UnityEngine;
using System.Collections;

public class AllDestroyed : Achievement{


	public override string GetDecription()
	{return Description;
	}

	public override void CheckBeginning (){
	}


	public override void CheckEnd (){
		if (!IsAccomplished ()) {
			
			if (isCorrectLevel()) {

				if (GameManager.getInstance().playerList [1].getArmyCount() == 0) {
				
					Accomplished ();
				}
			
			}
		}
	}


}
