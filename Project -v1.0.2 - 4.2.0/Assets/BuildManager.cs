using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildManager : MonoBehaviour {



	public List<UnitProduction> buildOrder = new List<UnitProduction>();

	private Selected mySelect;
	private BuilderUI build;

	private bool isWorker;
	public bool waitingOnSupply;
	[Tooltip("This will appear if the building could be building something but isn't")]
	public GameObject buildDecal;
	UnitManager manager;

	public int maxBuildQue = 5;
	public AugmentAttachPoint augmenter;
	public bool showSupplyError = true;
	// Use this for initialization
	public void Start () {
		
		build =  GameObject.FindObjectOfType<BuilderUI> ();
		mySelect = GetComponent<Selected> ();
		if (GetComponent<newWorkerInteract> ()) {
			isWorker = true;}
		manager = GetComponent<UnitManager> ();
		if (manager.PlayerOwner == 1)
		{
			StartCoroutine(checkBuildDecal());
		}
		else
		{
			if (buildDecal)
			{
				buildDecal.SetActive(false);
			}
		}
	}

	

	public void cancel()
	{
		//Debug.Log ("Cancel one");
		
			if (buildOrder.Count > 0) {
			if (!isWorker) {
				buildOrder [0].DeQueueUnit ();
		
				buildOrder [0].cancelBuilding ();
			}
				buildOrder.RemoveAt (0);

			if (buildOrder.Count > 0) {
				float Sup = 0;

				if (buildOrder [0].unitToBuild) {
					Sup = buildOrder [0].unitToBuild.GetComponent<UnitStats> ().supply;
				}
				
				if (Sup == 0 || manager.myRacer.hasSupplyAvailable (Sup)) {

					buildOrder [0].startBuilding ();
					build.hasSupply ();
				} else {
					build.NoSupply ();
					StartCoroutine (waitOnSupply (Sup));
				}


			} else {
				if (augmenter) {
					augmenter.stopBuilding ();
				}
				waitingOnSupply = false;
				if (mySelect.IsSelected) {
					//Debug.Log ("Resetting it");
					build.hasSupply ();
				}
			}
				if (mySelect.IsSelected) {
					build.bUpdate (this.gameObject);
				}

		}

		mySelect.updateIconNum ();
		StartCoroutine( checkBuildDecal ());
	}

	IEnumerator checkBuildDecal()
	{
		yield return null;
		if (buildDecal) {

			if (buildOrder.Count == 0) {
				bool hasAbil = false;
				foreach (Ability ab in manager.abilityList) {
					if (ab != null) {
						hasAbil = true;
						break;
					}
				}

				buildDecal.SetActive (hasAbil);
				
			} else {
				buildDecal.SetActive (false);
			}
		}
	}

	public void checkForSupply()
	{if (!buildOrder [0].unitToBuild) {
			buildOrder [0].startBuilding ();
			return;}
		float Sup = buildOrder [0].unitToBuild.GetComponent<UnitStats> ().supply;
		//Debug.Log("No SUpply " + raceMan.hasSupplyAvailable(Sup));
		if (Sup == 0 || manager.myRacer.hasSupplyAvailable (Sup)) {
			buildOrder [0].startBuilding ();
			if (mySelect.IsSelected) {
				waitingOnSupply = false;
				build.hasSupply ();
			
			}
		} else {
			if (mySelect.IsSelected){
				waitingOnSupply = true;
				build.NoSupply ();}
			StartCoroutine (waitOnSupply (Sup));
		}
	}

	public void cancel(int n)
	{//	Debug.Log ("Cancel other");
		if (n == 0) {
			if (buildOrder.Count > 0) {

				buildOrder [0].DeQueueUnit ();
				buildOrder [0].cancelBuilding ();

				buildOrder.RemoveAt (0);

				if (buildOrder.Count > 0) {
					checkForSupply ();

				}
				if (mySelect.IsSelected) {
					build.bUpdate (this.gameObject);
				}

			}
		} else {
			
			if (buildOrder.Count > n) {
				buildOrder [n].DeQueueUnit ();
				buildOrder.RemoveAt (n);


				if (mySelect.IsSelected) {
					build.bUpdate (this.gameObject);
				}
			}


		}
		mySelect.updateIconNum ();
		StartCoroutine( checkBuildDecal ());
	}

	public bool buildUnit(UnitProduction prod)
	{if (buildOrder.Count >= maxBuildQue) {
			return false;
		
		}
		if (augmenter) {
		
			augmenter.BuildingStuff ();
		}
		buildOrder.Add (prod);
		if (mySelect.IsSelected) {
			build.bUpdate (this.gameObject);
		}
	
		if(buildOrder.Count == 1)
		{
			checkForSupply ();

		}
		mySelect.updateIconNum ();
		StartCoroutine( checkBuildDecal ());
		return true;

	}


	public bool unitFinished(UnitProduction prod)
	{if (buildOrder.Count > 0) {
			buildOrder.RemoveAt (0);
		}
		if(buildOrder.Count > 0)
		{
			checkForSupply ();

		}
		if (mySelect.IsSelected) {
			build.bUpdate (this.gameObject);
		}
		mySelect.updateIconNum ();
		StartCoroutine( checkBuildDecal ());
		return true;
	}


	IEnumerator waitOnSupply(float supply)
	{
		if (manager.myRacer.supplyMax == manager.myRacer.supplyCap)
		{
			if (manager.PlayerOwner == 1 && showSupplyError)
			{
				ErrorPrompt.instance.atMaxSupply();
			}
		}
		else
		{
			if (manager.PlayerOwner == 1 && showSupplyError)
			{
				ErrorPrompt.instance.notEnoughSupply();
			}
		}
		while (buildOrder.Count > 0) {
			yield return new WaitForSeconds (.3f);
			
			if (buildOrder.Count > 0) {
				if (supply == 0 || manager.myRacer.hasSupplyAvailable (supply)) {
					buildOrder [0].startBuilding ();
					build.hasSupply ();
					waitingOnSupply = false;
					break;
				} 
			} else {
				break;
			}
		}
		
	}

}
