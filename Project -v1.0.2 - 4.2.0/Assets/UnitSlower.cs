﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSlower : MonoBehaviour {

    // Is this script used specifically on the Payload maps of battlemore?
	public bool slowUnits;

	int playerOwner;
	//float mySpeed;

	public float slowAmount = -.45f;

	public List<string> excludedUnits;

	List<UnitManager> inSight = new List<UnitManager>();

	// Use this for initialization
	void Start () {
		playerOwner = GetComponentInParent<UnitManager> ().PlayerOwner;
		//mySpeed = GetComponentInParent<UnitManager> ().cMover.MaxSpeed;
		
	}
	
	void OnTriggerEnter(Collider col)
	{
		//Debug.Log ("Entering unit " + col.gameObject);
		UnitManager manage = col.GetComponent<UnitManager> ();
		if (manage) {
			if (manage.PlayerOwner == playerOwner && manage.cMover && !excludedUnits.Contains (manage.UnitName)) {
				StartCoroutine (waitForSec (manage));
			
			} else if (manage.PlayerOwner == 1) {
				Dying ();
			
			}
		}
	}

	void OnTriggerExit(Collider col)
	{
		UnitManager manage = col.GetComponent<UnitManager> ();
		if (manage) {
			if (manage.PlayerOwner == playerOwner && manage.cMover && !excludedUnits.Contains(manage.UnitName) && inSight.Contains(manage)) {
				manage.myStats.statChanger.removeMoveSpeed(this);
			}
		}
	}

	public void Dying()
	{
		foreach (UnitManager manage in inSight) {
			if (manage && manage.cMover) {
				manage.myStats.statChanger.removeMoveSpeed(this);
			}
		
		}

		inSight.Clear ();
	}

	IEnumerator waitForSec( UnitManager manage)
	{
		yield return null;
		inSight.Add (manage);

		if (slowUnits) {
			manage.myStats.statChanger.changeMoveSpeed(slowAmount,0, this, true);
		} else {
			manage.myStats.statChanger.changeMoveSpeed(slowAmount, 0, this, true);
		}


	}




}
