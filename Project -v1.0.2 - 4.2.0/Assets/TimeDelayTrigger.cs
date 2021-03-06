﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TimeDelayTrigger : MonoBehaviour {


	public float timeDelay;
	public int player = 1;

	public  int index;
	public  float input;
	public  GameObject target; 
	public  bool doIt;
	public float delay;

	public UnityEngine.Events.UnityEvent OnTrigger;

	public List<SceneEventTrigger> myTriggers;
	// Use this for initialization
	void Start () {
		Invoke ("DelayedUpdate", timeDelay);
	}


	void DelayedUpdate () {
		if (gameObject.activeInHierarchy) {
			StartCoroutine (Fire ());
		}
	}



	IEnumerator Fire ()
	{
		yield return new WaitForSeconds (delay + .0001f);

		OnTrigger.Invoke();
		foreach (SceneEventTrigger trig in myTriggers) {
			if (trig) {
				trig.trigger (index, input, target, doIt);
			}
			yield return null;
		}
		Destroy (this);

	}


}
