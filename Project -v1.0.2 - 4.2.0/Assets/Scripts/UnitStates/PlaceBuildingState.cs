﻿using UnityEngine;
using System.Collections;

public class PlaceBuildingState :UnitState {
	
	private Vector3 location;

	public Ability myAbility;


	private bool Follow;
	GameObject myGhost;
	BuildingPlacer myPlacer;

	bool waiting;

	public PlaceBuildingState(GameObject ghost,Vector3 loc, Ability abil)
	{
		myGhost = ghost;
		//Debug.Log ("Has place " + myGhost);
		myPlacer = myGhost.GetComponentInChildren<BuildingPlacer> ();
		location = loc;
		//Debug.Log ("Setting location to " + location + "  ghost " + ghost);
		myAbility = abil;

		myAbility.myCost.showCostPopUp(false);

		myAbility.myCost.payCost ();

	}

	public void cancel()
	{
		
		UIManager.main.DestroyGhost (myGhost);

		if (!waiting) {
			myAbility.myCost.refundCost ();
			myAbility.myCost.showCostPopUp(true);
		}
	}

	public override void initialize()
	{if (!myGhost) {
			myManager.changeState (new DefaultState ());
		} else {
			myManager.cMover.resetMoveLocation (location);
		}
		
		//Debug.Log ("Going to " + location + "   Ghost  " + myGhost);

	}

	// Update is called once per frame
	override
	public void Update () {


		if (!waiting) {
			if (myManager.cMover.move ()) {
				waiting = true;
				Vector3 endSpot = location;
				if (myManager.cMover is airmover) {
					endSpot.y += ((airmover)myManager.cMover).flyerHeight;
				}

				myManager.gameObject.transform.position = endSpot;


				if (myPlacer.canBuild ()) {
					if (myAbility is BuildStructure) {
						((BuildStructure)myAbility).setBuildSpot (location);
					}
					//((TargetAbility)myAbility).target = myGhost;
					myAbility.Activate ();
					UIManager.main.DestroyGhost (myGhost);
	
				} else {

					myManager.StartCoroutine (waitToCheck (myPlacer, location, myAbility));

			
				}
			}

		}

	}

	IEnumerator waitToCheck( BuildingPlacer placer, Vector3 loc, Ability ab)
	{
		while (true) {
			yield return new WaitForSeconds (1.5f);
		
			if (placer.canBuild ()) {
				if (myAbility is BuildStructure) {
					((BuildStructure)myAbility).setBuildSpot (loc);
				}
				ab.Activate ();

				UIManager.main.DestroyGhost (myGhost);
				break;
			}
		}


	}
	/*
	override
	public void attackResponse(UnitManager src, float amount)
	{
	}
*/
	override
	public void endState()
	{
		UIManager.main.DestroyGhost (myGhost);
		if (!waiting) {
			if (myAbility.myCost) {
				myAbility.myCost.refundCost ();
				myAbility.myCost.showCostPopUp(true);
			}
		}
	}


}
