﻿using UnityEngine;
using System.Collections;

public class HarpyLandingPad : MonoBehaviour {

	public Vector3 RightPadPoint;
	public Vector3 LeftPadPoint;

	public float landingTime = 4.5f;
	public GameObject leftPadInUse;
	public GameObject rightPadInUse;

	public Vector3 requestLanding( GameObject incoming)
	{
		if (!rightPadInUse || rightPadInUse == incoming) {
	
			rightPadInUse = incoming;
	

			return (transform.rotation) * RightPadPoint + this.gameObject.transform.position;

		}


		if (!leftPadInUse || leftPadInUse == incoming) {


			leftPadInUse = incoming;
			return (transform.rotation) * LeftPadPoint + this.gameObject.transform.position;

		}


		return Vector3.zero;
	}



	public float startLanding( GameObject incoming)
	{

		return landingTime;

	}


	public void finished(GameObject outgoing)
	{

		StartCoroutine (delayedClosing (outgoing));
	}

	IEnumerator delayedClosing(GameObject outgoing)
	{
		if (rightPadInUse == outgoing) {
			rightPadInUse =null;
			yield return new WaitForSeconds (.2f);


		}
		if (leftPadInUse == outgoing) {
			leftPadInUse = null;

			yield return new WaitForSeconds (.2f);

		}

	}


	public bool hasAvailable()
	{
		return (!rightPadInUse || !leftPadInUse);

	}

	public void OnDrawGizmos()
	{
		
		Gizmos.DrawSphere ((transform.rotation) * RightPadPoint + this.gameObject.transform.position, .5f);
		Gizmos.DrawSphere ((transform.rotation) * LeftPadPoint + this.gameObject.transform.position, .5f);
	}
}
