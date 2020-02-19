using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectUpgradeApplier : MonoBehaviour {

	public List<GameObject> existingButtons = new List<GameObject> ();
	public Image upgradeIcon;
	public Text purchasedTitle;
	public Text upgradeDescription;
	public Canvas DirectApplierCanvas;

	public static DirectUpgradeApplier instance;
	void Awake()
	{instance = this;
	}

	SpecificUpgrade currentUpgrade;
	//To Do get references to upgrade description/title/pic and set those inthe applier menu, activate the applier menu.
	//create close button in applier menu

	public void CreatePage(SpecificUpgrade myupgrade)
	{
	//	Debug.Log ("Openign page");
		DirectApplierCanvas.enabled = true;
		currentUpgrade = myupgrade;
		upgradeIcon.sprite = myupgrade.iconPic;
		purchasedTitle.text = myupgrade.Name;
		upgradeDescription.text = myupgrade.Descripton;
		int currentLevel = LevelData.getHighestLevel ();
		foreach (GameObject obj in existingButtons) {
			obj.SetActive (false);
			foreach (RaceInfo.unitType type in myupgrade.ApplicableUnits) {
				if (type.ToString () == obj.name) {
					CampTechCamManager.TechOption tempOption = CampTechCamManager.instance.TechChoices.Find (item => item.name.Contains (obj.name.Substring (0, 4)));
					if (tempOption.levelAcquired <= currentLevel) {


						obj.SetActive (true);
					}
				}
			}
		
		}
	}


	public void applyUpgrade(CampaignUpgrade upgrader)
	{
	//	Debug.Log ("Applyinh " + upgrader.name);
		upgrader.setUpgrade (currentUpgrade);
	}

	public void applyAll()
	{
		foreach (GameObject obj in existingButtons) {
			if (obj.activeSelf) {
				//Debug.Log ("Triggering on " + obj);
				obj.GetComponent<Button> ().onClick.Invoke ();
			}
		}
	}
}