﻿using UnityEngine;
using System.Collections;

public class TurretMount : MonoBehaviour, Modifier {

	public GameObject turret;

	public TurretPlacer hasDisplayer;

	public bool rapidArms;
	public float lastUnPlaceTime = 0;
	// Use this for initialization
	public void Start () {
	
		GetComponentInParent<UnitStats> ().addDeathTrigger (this);

		FButtonManager.main.updateTankNumber ();

		rapidArms = GameManager.main.activePlayer.upgradeBall.GetComponent<RapidArmsUpgrade> ();
		if (rapidArms &&  GetComponentInParent<UnitManager> ().PlayerOwner !=3) {
		
			foreach (buildTurret bt in GameObject.FindObjectsOfType<buildTurret>()) {
				if (bt.manager.PlayerOwner == GetComponentInParent<UnitManager> ().PlayerOwner) {
					bt.addMount (this);
				}
			}
		

			foreach (TurretScreenDisplayer tm in  GameObject.FindObjectsOfType<TurretScreenDisplayer>()) {
				if (tm.GetComponent<UnitManager>().PlayerOwner == GetComponentInParent<UnitManager> ().PlayerOwner) {
					if (!tm.mounts.Contains (this)) {
					
						tm.mounts.Add (this);
						addShop (tm);
				
					}
				}
			}
		}
	}


	public float modify(float damage, GameObject source, OnHitContainer hitSource, DamageTypes.DamageType theType)
	{
		if (turret)
		{
			turret.SendMessage("Dying", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			FButtonManager.main.updateTankNumber();
		}
		return damage;
	}
	
	// Update is called once per frame
	void Update () {
		if (hasDisplayer) {
			if (turret &&hasDisplayer.gameObject.activeSelf) {
				hasDisplayer.gameObject.SetActive (false);
			} else if (!turret && !hasDisplayer.gameObject.activeSelf) {
				hasDisplayer.gameObject.SetActive (true);
			}
		}
	}

	public void setSelect()
	{
		if (turret) {
		//	Debug.Log ("Showing turret " + turret);
			if (turret.GetComponent<Selected> ().turretDisplay) {
				turret.GetComponent<Selected> ().turretDisplay.hover (true);
			}
		}
	}

	public void setDeSelect()
	{
		if (turret) {
			//Debug.Log ("Deslect " + turret);
			turret.GetComponent<Selected> ().turretDisplay.hover (false);
		}
	}

	public void addShop(TurretScreenDisplayer fact)
	{

		if ( hasDisplayer.addFact (fact)) {
		
		}
	

	}


	public void removeShop(TurretScreenDisplayer fact)
	{
		if (hasDisplayer && hasDisplayer.removeFact (fact)) {
		
		}

		
	}

	public void placeTurret(GameObject obj)
		{
		
		if (turret) {
			Destroy (obj); // If we ever have a problem where turrets are showing up in the units fielded tab but they don't exist, this is most likely the problem.
			return;
		}
		turret = obj;
		hasDisplayer.gameObject.SetActive (false);
		Vector3 spot = this.transform.position;
		spot.y += .5f;
		obj.transform.position = spot;
		obj.transform.parent = this.gameObject.transform;
		obj.transform.rotation = this.gameObject.transform.rotation;

		UnitManager manager = this.gameObject.GetComponentInParent<UnitManager> ();
		if (obj.GetComponent<IWeapon>())
		{
			manager.setWeapon(obj.GetComponent<IWeapon>());
		}
		else if (obj.GetComponent<RepairTurret>())
		{
			manager.myAddons.Add(obj.GetComponent<RepairTurret>());
		}

		if (manager.gameObject.GetComponent<Selected>().IsSelected)
		{
			RaceManager.upDateUI();
		}


		transform.parent.SendMessage ("TurretPlaced", SendMessageOptions.DontRequireReceiver);
		manager.PlayerOwner = GetComponentInParent<UnitManager> ().PlayerOwner;
		FButtonManager.main.updateTankNumber ();

	}


	public GameObject unPlaceTurret()
	{if (!turret) {
			return null;
		}

		lastUnPlaceTime = Time.time;
		hasDisplayer.gameObject.SetActive (true);
		GameObject toReturn = turret;
	//	Debug.Log ("Returning " + toReturn);
		turret = null;
		UnitManager manager = this.gameObject.GetComponentInParent<UnitManager> ();

		manager.removeWeapon(toReturn.GetComponent<IWeapon>());
		manager.removeAddon(toReturn.GetComponent<RepairTurret>());


		if (manager.gameObject.GetComponent<Selected>().IsSelected)
		{
			RaceManager.upDateUI();
		}


		transform.parent.SendMessage ("TurretRemoved", SendMessageOptions.DontRequireReceiver);
		FButtonManager.main.updateTankNumber ();
		//Debug.Log ("Deatched " + turret);
		return toReturn;
	}
}
