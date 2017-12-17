using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class laserGun : MonoBehaviour {


	public bool raycastToUI;
	Camera myCam;
	GameObject lastHit;

	// Use this for initialization
	void Start () {
		myCam = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!isPointerOverUIObject() || raycastToUI){// !isPointerOverUIObject() || raycastToUI) {

			Ray rayb = myCam.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitb;

			if (Physics.Raycast (rayb, out hitb, Mathf.Infinity, ~(1 << 16))) {
				//Debug.Log ("hitting " + hitb.collider.gameObject);
				if (hitb.collider.gameObject != lastHit) {
					
					if (lastHit && lastHit.GetComponent<MouseClicker> ()) {
						lastHit.GetComponent<MouseClicker> ().executeOnExit();
					}
					lastHit = hitb.collider.gameObject;
					if (lastHit.GetComponent<MouseClicker> ()) {
						lastHit.GetComponent<MouseClicker> ().executeOnHover ();
					}

				
				}


				if (Input.GetMouseButtonDown (0)) {
					MouseClicker clicker = lastHit.GetComponent<MouseClicker> ();
					if (clicker && clicker.enabled) {
						clicker.executeOnClick ();
					}

					//Debug.Log ("hit a  " + hitb.collider.gameObject);
					if (hitb.collider.gameObject.GetComponent<blowUp> ()) {
						hitb.collider.gameObject.GetComponent<blowUp> ().blowUpTrigger ();
					}

					if (hitb.collider.gameObject.GetComponent<MenuAnimationPlayer> ()) {

						hitb.collider.gameObject.GetComponent<MenuAnimationPlayer> ().ClickOn ();
					}
				}
			}
		
		}

	}


	public bool isPointerOverUIObject()
	{
		PointerEventData eventDatacurrenPosition = new PointerEventData (EventSystem.current);
		eventDatacurrenPosition.position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);

		List<RaycastResult> results = new List<RaycastResult> ();
		EventSystem.current.RaycastAll (eventDatacurrenPosition, results);

		if (results.Count == 0) {
			return false;
		} 
		//Debug.Log ("returning " + (results[0].distance == 0) + "   " + results.Count + "  " + results[0].gameObject + "   " +results[1].gameObject);

		return true;//results[0].distance == 0;


	}
}
