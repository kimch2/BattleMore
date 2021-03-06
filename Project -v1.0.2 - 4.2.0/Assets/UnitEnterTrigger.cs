﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitEnterTrigger : MonoBehaviour {

	public int player = 1;

	public  int index;
	public  float input;
	public  Vector3 location;
	public  GameObject target; 
	public  bool doIt;
	public float delay;

	public List<string> specificUnits;
	public List<SceneEventTrigger> myTriggers;

	public UnityEngine.Events.UnityEvent OnEnter;
	bool alreadyTriggered;
	public bool repeatable;

	void OnTriggerEnter(Collider other)
	{
		if (!alreadyTriggered || repeatable) {
		
			if (other.isTrigger) {
				return;
			}
			if (other.GetComponent<UnitManager> ()) {
				if (other.GetComponent<UnitManager> ().PlayerOwner == player) {

					if (specificUnits.Count > 0) {
						if (specificUnits.Contains (other.GetComponent<UnitManager> ().UnitName)) {
							StartCoroutine (Fire ());
						}
					} else {
						//Debug.Log (other.gameObject);
						StartCoroutine (Fire ());
					}
				}
			}
		}
	}



	IEnumerator Fire ()
	{
		alreadyTriggered = true;
		yield return new WaitForSeconds (delay + .0001f);

		foreach (SceneEventTrigger trig in myTriggers) {
			//Debug.Log ("Triggering " + trig);
			if (trig != null) {
				trig.trigger (index, input, target, doIt);
			}
		}
		OnEnter.Invoke ();
		if (!repeatable) {
			GetComponent<Collider> ().enabled = false;
		
			yield return new WaitForSeconds (15);
			Destroy (this.gameObject);
		}
	}


}
