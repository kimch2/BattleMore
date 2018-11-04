using UnityEngine;
using System.Collections;

public class PopUp : MonoBehaviour {
	private GameObject cam;
	public float speed = 5;
	// Use this for initialization
	float timeToDie;

	void Start () {
		cam = MainCamera.main.gameObject;
	
	}
	public void setDuration(float f)
	{
		timeToDie = Time.time + f;
	}

	public void OnSpawn()
	{
		timeToDie = Time.time + 1.3f;

	}

	// Update is called once per frame
	Vector3 location;
	void Update () {

		this.transform.Translate (Vector3.up * Time.deltaTime * speed);
	
		location = cam.transform.position;
		location.x = this.gameObject.transform.position.x;
		gameObject.transform.LookAt (location);
	
		if(Time.time > timeToDie)
		{
			Lean.LeanPool.getSpawnPool (this.gameObject).FastDespawn(this.gameObject);

		}
	}
}
