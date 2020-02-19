using UnityEngine;
using System.Collections;

public class SpeedAch : Achievement{

	public float maxTime;
	public override void CheckBeginning (){
	}

	public override string GetDecription()
	{return Description;
	}

	public override void CheckEnd (){
		if (!IsAccomplished () && isCorrectLevel()) {

			if(Clock.main.getTotalSecond() <= maxTime )

				Accomplished ();
		}
	}


}