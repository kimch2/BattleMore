﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretScreenDisplayer : MonoBehaviour {

	private UnitManager manage;

	public List<TurretMount> mounts = new List<TurretMount>();

	public bool rapidArms;


	public buildTurret A;
	public buildTurret B;
	public buildTurret C;
	public buildTurret D;


	// Use this for initialization
	public void Start () {
		manage = GetComponent<UnitManager> ();
	
		mounts.Clear ();
		rapidArms = GameManager.main.activePlayer.upgradeBall.GetComponent<RapidArmsUpgrade> ();
		if (rapidArms && manage.PlayerOwner != 3) {
			foreach (TurretMount tm in GameObject.FindObjectsOfType<TurretMount>()) {
				if (tm.GetComponentInParent<UnitManager> ().PlayerOwner == manage.PlayerOwner) {
					if (!mounts.Contains (tm)) {
						
						mounts.Add (tm);
						tm.addShop (this);
					}
				}

			}
		} 
		else {
			foreach (UnitManager manage in manage.allies) {
				TurretMount tm = manage.GetComponentInChildren<TurretMount> ();
				if(tm && !mounts.Contains(tm)){
					mounts.Add (tm);
					tm.addShop (this);
				}
			}
		}


		CancelInvoke("UpdateButts");
		InvokeRepeating ("UpdateButts", .2f, .16f);
	}



	// Update is called once per frame
	void UpdateButts () {

	
			mounts.RemoveAll (item => item == null);
			foreach (TurretMount obj in mounts) {
				
				if (obj.gameObject.GetComponentInParent<TurretPickUp> ()) {

					if (!obj.gameObject.GetComponentInParent<TurretPickUp> ().autocast) {

						continue;
					}
				}
			

					updateButtons (obj.hasDisplayer);

			}

	}

	public void updateButtons(TurretPlacer t)
	{//Debug.Log ("Updating buttons");
		
		if (D) {
			t.initialize (A.chargeCount > 0, B.chargeCount > 0, C.chargeCount > 0, D.chargeCount > 0);
		} else if (C) {
			t.initialize (A.chargeCount > 0, B.chargeCount > 0, C.chargeCount > 0, false);
		}
		else {
			t.initialize (A.chargeCount > 0, B.chargeCount > 0, false,false);
		}

		//t.hasDisplayer.initialize (A.chargeCount > 0, B.chargeCount > 0,C.chargeCount > 0,D.chargeCount > 0);
	}








	void OnTriggerEnter(Collider other)
	{

		if (rapidArms) {
			return;}
		//need to set up calls to listener components
		//this will need to be refactored for team games

		if (!other.isTrigger) {

			UnitManager manager = other.gameObject.GetComponent<UnitManager> ();
			if (manager) {
				if (manage.PlayerOwner == manager.PlayerOwner) {
		
					foreach (TurretMount mount in other.gameObject.GetComponentsInChildren<TurretMount> ()) {
						if (mount) {
							
							mounts.Add (mount);
							mount.addShop (this);
						}

					}
				}
			}
		}
	}





	void OnTriggerExit(Collider other)
	{if (rapidArms) {
			return;}

		//need to set up calls to listener components
		//this will need to be refactored for team games
		if (other.isTrigger) {
			return;}

		UnitManager manager = other.gameObject.GetComponent<UnitManager>();

		if (manage == null || manager == null) {
			return;
		}
			
		if (manage.PlayerOwner == manager.PlayerOwner) {

			foreach (TurretMount mount in other.gameObject.GetComponentsInChildren<TurretMount> ()) {
				if (mounts.Contains(mount)) {
					mounts.Remove (mount);

					mount.removeShop (this);
				}

				}

			}
		}


	public bool buildGatling(TurretPlacer p )
	{
		if (A.chargeCount > 0) {

			p.myMount.placeTurret (A.createUnit (p.myMount.transform));
			if (A.PlaceEffect) {
				Instantiate (A.PlaceEffect,p.myMount.transform.position, Quaternion.identity,p.myMount.transform);
			}
			return true;
		}
		return false;
	}

	public bool buildRailGun(TurretPlacer p )
	{if (B.chargeCount > 0) {
			p.myMount.placeTurret (B.createUnit (p.myMount.transform));
			if (B.PlaceEffect) {
				Instantiate (B.PlaceEffect, p.myMount.transform.position, Quaternion.identity, p.myMount.transform);
			}
			return true;
		}
		return false;
	}



	public bool buildRepair(TurretPlacer p )
			{if (C.chargeCount > 0) {
				p.myMount.placeTurret (C.createUnit (p.myMount.transform));
			if (C.PlaceEffect) {
				Instantiate (C.PlaceEffect, p.myMount.transform.position, Quaternion.identity, p.myMount.transform);
			}
			return true;
		}
		return false;
	}

	public bool buildMortar(TurretPlacer p )
	{if (D.chargeCount > 0) {
			p.myMount.placeTurret (D.createUnit (p.myMount.transform));
			if (D.PlaceEffect) {
				Instantiate (D.PlaceEffect, p.myMount.transform.position, Quaternion.identity, p.myMount.transform);
			}
			return true;
		}
		return false;
	}







}
