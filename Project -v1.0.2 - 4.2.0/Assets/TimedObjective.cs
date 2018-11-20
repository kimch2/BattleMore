using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedObjective : Objective {
	
	public float remainingTime;
	public UnityEngine.Events.UnityEvent OnTimeOut;
	string initialDescript;
	public bool loseOnComplete = true;
	public bool completeOnTimeOut = true;
	// Use this for initialization
	new void Start () {

		if (ActiveOnStart) {
			BeginObjective ();
		}

		initialDescript = description;

	}

	public override void BeginObjective()
	{base.BeginObjective ();
		StartCoroutine (countDown());
	}


	IEnumerator countDown()
	{
		//Debug.Log ("Starting countddown");
		while (remainingTime > 0 && !completed) {
			yield return new WaitForSeconds (1);
			remainingTime -= 1;
			description = initialDescript + "  " + Clock.convertToString(remainingTime);
			if (remainingTime < 60)
			{
				VictoryTrigger.instance.UpdateObjective(this , Color.red);
			}
			else
			{
				VictoryTrigger.instance.UpdateObjective(this);
			}
		
		}
		if (completed) {
			complete ();

		}

		else if (loseOnComplete) {
			if (!completed) {
				VictoryTrigger.instance.Lose ();
			}
		} else if (completeOnTimeOut) {
			complete ();
		} else {
			OnTimeOut.Invoke ();
		}




	}
	// Update is called once per frame

}
