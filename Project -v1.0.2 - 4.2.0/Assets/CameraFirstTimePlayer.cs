using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFirstTimePlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (LevelData.getHighestLevel () == 1) {
		
		} else {
			GetComponent<Animator> ().enabled = false;
			GetComponent<EventManager> ().EnterState ("MainArea");
			GetComponent<TimeDelayTrigger> ().enabled = false;
		}
	}

}
