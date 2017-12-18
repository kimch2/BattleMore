using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class CampTechCamManager : MonoBehaviour {

	AudioSource mySource;
	public AudioClip buttonPress;

	TechOption currentTech;

	public GameObject MainCam;
	Vector3 CameraStart;
	Vector3 CamStartLookAt;

	public GameObject OverAllPArticle;
	[Tooltip("This is a list of all buttons to be turned on throughout levels, and the obejct the camera will go to")]
	public List<TechOption> TechChoices = new List<TechOption>();

	Coroutine CameraFlight;


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
		mySource = GetComponent<AudioSource> ();
		Time.timeScale = 1;
		
		CameraStart = MainCam.transform.position;
		CamStartLookAt = MainCam.transform.position+ MainCam.transform.forward * 10;

		int n = LevelData.getHighestLevel ();

		foreach (TechOption to in TechChoices) {
			if (to.levelAcquired > n) {
				//to.HUD.SetActive (false);
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



	IEnumerator returnToOrigin(TechOption OldOb)
	{mySource.PlayOneShot (buttonPress);
		Vector3 BeginCam = MainCam.transform.position;
		Vector3 BeginLookAt;

		if (OldOb != null) {
			BeginLookAt = OldOb.CamFocus.transform.position;
		} else {
			BeginLookAt = MainCam.transform.position + MainCam.transform.forward* 5;
		}

		float currentTime = 0;
		while (currentTime <= 1) {

			currentTime += Time.deltaTime;

			MainCam.transform.position = Vector3.Lerp (BeginCam, CameraStart, currentTime);
			MainCam.transform.LookAt (Vector3.Lerp (BeginLookAt, CamStartLookAt, currentTime));
			yield return 0;
		}	
	}

	public void returnToStart()
	{
		
		//currentHud = null;
		currentTech = null;
		if (CameraFlight != null) {
			StopCoroutine (CameraFlight);

		}
		CameraFlight = StartCoroutine (returnToOrigin (currentTech));

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

		if (CameraFlight != null) {
			StopCoroutine (CameraFlight);
		}
		//CameraFlight = StartCoroutine (FocusOnObject(currentTech));
	//	Debug.Log("Going to the thing " + currentTech.CamFocus);
		currentTech.CamFocus.GetComponent<Tweener> ().GoToPose ("Poser");
		mySource.PlayOneShot (buttonPress);

	}


}
