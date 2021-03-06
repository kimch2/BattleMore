﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoomer : MonoBehaviour {

    //Used in campaign hub so the camera can move between the different areas


	float lastZoom;
	public EventManager PureArmory;
	// Update is called once per frame

	int numTimes = 0;
	void Update () {

		if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			numTimes++;

			if (numTimes > 3) {

				numTimes = 0;
				if (lastZoom + 1 < Time.time) {
					lastZoom = Time.time;
					if (GetComponent<EventManager> ().getCurrentState () == "Armory") {

						if (PureArmory.getCurrentState () == "MainArmory") {
							GetComponent<EventManager> ().EnterState ("MainArea");
						} else {

							GetComponent<EventManager> ().EnterState ("Armory");
							PureArmory.EnterState ("MainArmory");
						}
					
					
					
					
					} else if (GetComponent<EventManager> ().getCurrentState () == "LevelIntro") {
			
						GetComponent<EventManager> ().EnterState ("Table");
					} else if (GetComponent<EventManager> ().getCurrentState () == "ScienceLog") {
						
						GetComponent<EventManager> ().EnterState ("MainArea");
					} else if (GetComponent<EventManager> ().getCurrentState () == "Computer") {
				
						GetComponent<EventManager> ().EnterState ("MainArea");
			
					} else if (GetComponent<EventManager> ().getCurrentState () == "Table") {

						GetComponent<EventManager> ().EnterState ("MainArea");
					}
				}
			}


		}
		else
		{numTimes = 0;}
	}
}
