using UnityEngine;
using System.Collections;

public class BillBoard : MonoBehaviour
{
	 public Camera LookAtCam;
	
	public float RandomRotate = 0;
	private float RndRotate;
	public Transform this_t_;

	void Start() {
		this_t_ = this.transform;
		//RndRotate = Random.value*RandomRotate;

		if (MainCamera.main) { 

		LookAtCam = MainCamera.main.GetComponent<Camera>(); }
		if (!LookAtCam)
		{
			LookAtCam = Camera.main;
		}
	}
	Vector3 LookLocation;
	void Update() {
		//if ( LookAtCam == null ) return;
		//Transform cam_t = LookAtCam.transform;
		
		//Vector3 vec = cam_t.position - this_t_.position;
		//vec.x = vec.z = 0.0f;
		//this_t_.LookAt(cam_t.position - vec); 
		//this_t_.Rotate(Vector3.forward,RndRotate);

		LookLocation =transform.position *2 - LookAtCam.transform.position;
		LookLocation.x = gameObject.transform.position.x;
		gameObject.transform.LookAt (LookLocation);
	}
}