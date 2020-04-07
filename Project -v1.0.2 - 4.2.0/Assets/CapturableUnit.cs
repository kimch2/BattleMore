﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CapturableUnit : MonoBehaviour, Modifier  {

	public List<UnitManager> myManagers = new List<UnitManager>();
	public bool cutscene;
	bool alreadyCaptured;
	public float captureRange =50;
	//Add all managers in the unit to this list
	// In order to use this, set the units playerNumber(UnitManager) to 3
	// and disable the FogOfWarUnitScript
	// Set the Vision Range in the Unitmanger 5 more than what it should be,


	public float modify(float damage, GameObject source, OnHitContainer hitSource, DamageTypes.DamageType theType)
	{
		if (source) {
			UnitManager sourceManage = source.GetComponent<UnitManager> ();
			if (sourceManage) {

				capture ();
			}
		}
		return damage;
	}



	IEnumerator Start()
	{
		yield return null;
		foreach (UnitManager manager in myManagers) {
			manager.myStats.addModifier (this);
			manager.metaStatus.Stun (manager, this, true);
			manager.getVisionSphere ().radius = captureRange;
		}

		yield return null;
		foreach (UnitManager manager in myManagers) {
			setDecal( manager);
		}
	}

	public UnityEngine.Events.UnityEvent OnCapture;


	public void capture()
	{
		if (!alreadyCaptured) {
			alreadyCaptured = true;
			foreach (UnitManager manager in myManagers) {
				if (manager) {
					StartCoroutine (SwapUnits (manager));
				}
			}


			OnCapture.Invoke ();
			if (cutscene) {
				MainCamera.main.setCutScene (this.gameObject.transform.position, 120);
			}
		}

	}


	IEnumerator SwapUnits(UnitManager manage)
	{
		manage.getVisionSphere ().radius = 150;
		yield return null;

		List<UnitManager> newAllies = new List<UnitManager> ();
		List<UnitManager> newEnemies = new List<UnitManager> ();
		List<GameObject> removeFromNuetral = new List<GameObject> ();

        

		foreach (UnitManager unit in manage.enemies) {
			if (unit.PlayerOwner == 2) {
				newEnemies.Add (unit);
				if (unit.neutrals.Remove (manage.gameObject)) {
					unit.enemies.Add (manage);
				}
			}
			if (unit.PlayerOwner == 1) {
				newAllies.Add (unit);
				if (unit.neutrals.Remove (manage.gameObject)) {
					unit.allies.Add (manage);
				}
			}
		}

		foreach (UnitManager unit in manage.allies) {
			if (unit.PlayerOwner == 2) {
				newEnemies.Add (unit);
				if (unit.neutrals.Remove (manage.gameObject)) {
					unit.enemies.Add (manage);
				}
			}
			if (unit.PlayerOwner == 1) {
				newAllies.Add (unit);
				if (unit.neutrals.Remove (manage.gameObject)) {
					unit.allies.Add (manage);
				}
			}
		}

		foreach (GameObject unit in manage.neutrals) {
			UnitManager manners = unit.GetComponent<UnitManager> ();
			if (manners) {
				if (manners.PlayerOwner == 2) {
					newEnemies.Add (manners);
					removeFromNuetral.Add (unit);
			
				} else if (manners.PlayerOwner == 1) {
					newAllies.Add (manners);
					removeFromNuetral.Add (unit);
				}
			}	
		}

		manage.allies = newAllies;
		manage.enemies = newEnemies;

			foreach (GameObject ob in removeFromNuetral) {
				manage.neutrals.Remove (ob);
			}


		manage.getVisionSphere ().radius = manage.visionRange;

		manage.metaStatus.UnStun( this);
		manage.myStats.veternStat.playerOwner = 1;

		manage.PlayerOwner = 1;
		manage.myRacer = GameManager.main.playerList[0];

        manage.AddUnFoggerManual();

		FogOfWarUnit foggy = manage.GetComponent<FogOfWarUnit> ();
       
		manage.myStats.upgradesApplied = false;
		GameManager.main.activePlayer.UnitCreated (manage.myStats.supply);
		GameManager.main.activePlayer.addUnit (manage);
		GameManager.main.activePlayer.addVeteranStat (manage.myStats.veternStat);
		Selected sel =  manage.GetComponent<Selected> ();
		foreach (TurretMount tm in GetComponentsInChildren<TurretMount>()) {
			tm.Start ();
		}
		foreach (buildTurret tm in GetComponents<buildTurret>()) {
			tm.Start ();
		}
		foreach (AbstractCost tm in GetComponents<AbstractCost>())
		{
			tm.Start();
		}
		foreach (TurretScreenDisplayer tm in GetComponents<TurretScreenDisplayer>()) {
			tm.Start ();
		}
		sel.interact();
		yield return null;
		Destroy (this);
	}


	void setDecal(UnitManager mana)
	{
		if (mana.UnitName != "Armory") {
			Selected sel = mana.GetComponent<Selected> ();

			sel.decalCircle.GetComponent<MeshRenderer> ().material = Resources.Load<Material> ("CaptureMat");
			sel.decalCircle.GetComponent<MeshRenderer> ().enabled = true;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.isTrigger) {
		
			return;}
		UnitManager manage = other.gameObject.GetComponent<UnitManager>();

		if (manage) {
			if (manage.PlayerOwner == 1) {
				capture ();
			}
		}
	}
}
