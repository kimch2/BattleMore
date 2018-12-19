using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FButtonManager : MonoBehaviour {

	public Text Ffive;
	public Text Fsix;
	public Text fSeven;
	public Text fEight;

	public static FButtonManager main;

	public Text idleWorkers;
	public Text TotalArmy;
	public Text unbound;
	public Text AllBuildings;

	public Button IdleButton;
	public Button TurretButton;

	//SelectedManager selectManager;


	// Use this for initialization
	void Awake () {
		//selectManager = GameObject.Find ("Manager").GetComponent<SelectedManager>();
		//setButtons ();
		main = this;

		Invoke("changeWorkers",1);
		Invoke("updateTankNumber", 1);
	}

	


	public void updateNumbers(Dictionary<string, List<UnitManager>> myUnits)
	{
		//Debug.Log("Check this and maybe remove");
		int tArmy = 0;
		int totalBuilding = 0;

		foreach (KeyValuePair<string, List<UnitManager>> pair in myUnits) {
			if (pair.Value.Count > 0) {
			
				if (pair.Value [0].myStats.isUnitType (UnitTypes.UnitTypeTag.Structure)) {
					totalBuilding++;
				}
			 else if (!pair.Value[0].myStats.isUnitType (UnitTypes.UnitTypeTag.Worker) && !pair.Value[0].myStats.isUnitType (UnitTypes.UnitTypeTag.Turret)) {
				tArmy++;
				}
			}
		}

		TotalArmy.text = "" + tArmy;
		AllBuildings.text = "" + totalBuilding;
	}


	// THis will need to be changed for future race workers.
	public void changeWorkers ()
	{
		int workerCount = 0;

		if(GameManager.main.activePlayer.getFastUnitList().ContainsKey("SteelCrafter"))
		foreach (UnitManager manage in GameManager.main.activePlayer.getFastUnitList()["SteelCrafter"]){// newWorkerInteract worker in GameObject.FindObjectsOfType<newWorkerInteract>()) {

				if (manage && manage.getState () is DefaultState) {
				workerCount++;
			}
		}
		if (workerCount == 0) {
		
			IdleButton.interactable = false;
			idleWorkers.transform.parent.gameObject.SetActive(false);
		} else {
			IdleButton.interactable = true;

			idleWorkers.transform.parent.gameObject.SetActive(true);
		}
		idleWorkers.text = "" + workerCount;
	}

	public void updateTankNumber()
	{


		unbound.text = "" + SelectedManager.main.getUnarmedTankCount ();
		if (unbound.text == "0")
		{

			TurretButton.interactable = false;
			unbound.transform.parent.gameObject.SetActive(false);
		}
		else
		{
			unbound.transform.parent.gameObject.SetActive(true);
			TurretButton.interactable = true;
		}
		//Debug.Log ("Updated " + SelectedManager.main.getUnarmedTankCount ());
	}

}
