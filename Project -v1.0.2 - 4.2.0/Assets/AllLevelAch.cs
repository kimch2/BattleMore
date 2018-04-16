using UnityEngine;
using System.Collections;

public class AllLevelAch: Achievement{


	public override string GetDecription()
	{return Description;
	}

	public override void CheckBeginning (){
	}

	public override void CheckEnd (){
		if (!IsAccomplished () && isCorrectLevel()) {

					Accomplished ();

		}
	}


}