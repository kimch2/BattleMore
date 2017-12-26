using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class CampTechCamManager : MonoBehaviour {

	TechOption currentTech;

	public GameObject OverAllPArticle;
	[Tooltip("This is a list of all buttons to be turned on throughout levels, and the obejct the camera will go to")]
	public List<TechOption> TechChoices = new List<TechOption>();

	public static CampTechCamManager instance;

	[Serializable]
	public class TechOption{
		public string name;
		public bool showPos;
		public GameObject openButton;
		public GameObject CamFocus;
		public List<GameObject> HUDObjects;
		public int levelAcquired;
		public MultiShotParticle effect;
	}

	void Awake()
	{instance = this;}
		
	//THis gets rid of all dropdown menus when clicking outside of them.
	void Update()
	{

		if (Input.GetMouseButtonDown (0)) {
			foreach (Dropdown dd in GameObject.FindObjectsOfType<Dropdown>()) {
				dd.Hide ();
			}
		}
	}

	public void AssignTechEffect()
	{
		if (currentTech != null) {
			Instantiate (OverAllPArticle, currentTech.effect.transform.position, currentTech.effect.transform.rotation);}
	}

	void Start()
	{
		Time.timeScale = 1;

		int n = LevelData.getHighestLevel ();

		foreach (TechOption to in TechChoices) {
			if (to.levelAcquired > n) {

				to.openButton.SetActive (false);
				foreach (GameObject obj in to.HUDObjects) {
					if (obj.GetComponent<CampaignUpgrade> ()) {
						obj.GetComponent<CampaignUpgrade> ().unlocked = false;
					}
					to.openButton.GetComponent<Button> ().enabled = false;

					foreach (Transform t in obj.transform) {
						if (t.GetComponent<CampaignUpgrade> ()) {
							t.GetComponent<CampaignUpgrade> ().unlocked = false;
						}
					}
				}
				to.openButton.GetComponent<Button> ().enabled = false;
				to.CamFocus.SetActive (false);
			}
		}
	}


	public void loadTech(string nameOfThing)
	{
		if (currentTech != null) {
			currentTech.CamFocus.GetComponent<Tweener> ().StopAllTweens ();
		}
	
		foreach (TechOption to in TechChoices) {
			if (nameOfThing == to.name) {
				if (currentTech != null) {
					foreach (GameObject obj in currentTech.HUDObjects) {
						obj.SetActive (false);
					}
				}
				currentTech = to;
				foreach (GameObject obj in currentTech.HUDObjects) {
					obj.SetActive (true);
				}
				break;
			}
		}
		if (currentTech != null) {
			currentTech.CamFocus.GetComponent<Tweener> ().GoToPose ("Poser");
			TrueUpgradeManager.instance.playSimpleSound ();
		}
	}

}