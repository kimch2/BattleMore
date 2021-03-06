﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class APMCounter : MonoBehaviour {


	public Text counter ;

	List<float> actions = new List<float>();


	private float nextActionTime;
	public int totalActions;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 150;
		nextActionTime = Time.time + 3;
		InvokeRepeating ("updateAverage",2,2);
	}

	// Update is called once per frame
	void Update () {

		if (Input.anyKeyDown) {
			updateAPM ();
			totalActions++;
		}

	}



	public void updateAPM()
	{//Debug.Log ("adding money" + resOne);


		if(actions.Count==10){
			actions.RemoveAt(0);
		}
		actions.Add (Time.time);

	}

	float apm;
	int Acounter;



	public void updateAverage()
	{
		apm = 0;
		Acounter = actions.Count;

		foreach (float f in actions) {
			
			if ((Time.time - f) < 3.5f) {
				apm = Time.time - actions [0];
			
				break;
			} else {
				Acounter--;
			}
		}
	//	Debug.Log ("size " + actions.Count + "  apm  " + apm);
		if (counter.gameObject.activeInHierarchy) {



			if (Acounter > 0) {

				counter.text = "Actions Per Minute\n" + (int)((Acounter / apm) * 60) + "\nGame Average\n" + (int)(totalActions / (Clock.main.getTotalSecond () / 60)) +
				"\n\nFPS: " + (int)(Time.timeScale / Time.smoothDeltaTime);
			} else {
				counter.text = "Actions Per Minute\n0" + "\nGame Average\n" + (int)(totalActions / (Clock.main.getTotalSecond () / 60)) +
				"\n\nFPS: " + (int)(Time.timeScale / Time.smoothDeltaTime);
			}
		}
	}


	public int getAPM()
	{
		return (int)(totalActions/ (Clock.main.getTotalSecond() / 60));
	}




}
