using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour {

	protected  Transform cam;
	//public List<Image> buffList = new List<Image>();

	public SpriteRenderer BuildingUnit;
	public SpriteRenderer background;
	Vector3 LookLocation;


	protected virtual void Awake()
	{
		if (BuildingUnit) {
			BuildingUnit.gameObject.SetActive (false);
		}
		if (background) {
			background.gameObject.SetActive (false);
		}

	}

	// Use this for initialization
	void Start () {
		cam = MainCamera.main.transform;// GameObject.FindObjectOfType<MainCamera> ().gameObject;
		LateUpdate();
		transform.localScale = new Vector3(-1,1,1);
	}


	protected Quaternion lastRotation;
	protected Vector3 lastCameraPosition;
	protected  Vector3 myLastPosition;
	// Update is called once per frame
	void LateUpdate () {

		if (cam.position != lastCameraPosition)
		{
			//LookLocation = cam.position;
			//LookLocation.x = gameObject.transform.position.x;
			//gameObject.transform.LookAt (LookLocation);
			gameObject.transform.rotation = cam.rotation; // commented out orthographic camera code
			lastCameraPosition = cam.position;

		}
		else if (transform.position != myLastPosition)
		{
			myLastPosition = transform.position;
			gameObject.transform.rotation = cam.rotation;

		}
		else if (lastRotation != transform.rotation)
		{
			lastRotation = transform.rotation;
			gameObject.transform.rotation = cam.rotation;
		}


	}

	public void  loadIMage(Sprite m)
	{
		BuildingUnit.sprite = m;
		BuildingUnit.gameObject.SetActive(true);
		background.gameObject.SetActive (true);
	}

	public void stopBuilding()
	{BuildingUnit.sprite = null;
		BuildingUnit.gameObject.SetActive(false);
		background.gameObject.SetActive (false);
	}

	public void activate(bool offOn)
	{
		this.gameObject.SetActive (offOn);
	}

	public void DeleteMe()
	{
		transform.SetParent (null);
		transform.position = new Vector3 (0,-100,0);
		enabled = false;
	
		StartCoroutine (delayedDelete());
	}

	IEnumerator delayedDelete()
	{
		yield return null;
		Destroy (this.gameObject);
	}

}
