﻿using UnityEngine;
using System.Collections;

public class ThrasherBlade : Projectile {

	// Use this for initialization
	public float rollTime = 5;
	float trueROlltime;
	bool goingOut = true;
	Vector3 Origin;
	Vector3 EndTarget;
	RaycastHit objecthit;

	float initialSpeed;
	Vector3 dir;


	public override void setup()
	{
		base.Start ();

		Origin = this.transform.position;
		EndTarget = target.transform.position;
		dir = (EndTarget - transform.position);
		dir.y = 0;

		lastLocation = target.transform.position -this.gameObject.transform.position ;
        gameObject.transform.LookAt(lastLocation);
        trueROlltime = rollTime;
		initialSpeed = speed /1.25f;

        GetComponent<MiningSawDamager>().myHitContainer = MyHitContainer;	
    }

	protected override void Update () {

		Vector3 tempDir = dir;
		//Make sure your the right height above the terrain

		if (Physics.Raycast (this.gameObject.transform.position + Vector3.up * 4, Vector3.down, out objecthit, 100, ( 1 <<8))) {
			transform.position = objecthit.point + Vector3.up * 3;
			//float h = Vector3.Distance (this.gameObject.transform.position, objecthit.point);
			//if (h < 2.8f || h > 4.4f) {

				//tempDir.y -=   (this.gameObject.transform.position.y -(objecthit.point.y + 3f) ) *speed *8;
			//}
		}
		if (rollTime < 1.25f && goingOut) {
			speed -= Time.deltaTime * initialSpeed;
		} else if (rollTime > .45f && !goingOut) {
			speed += Time.deltaTime * initialSpeed;
		}

		tempDir.Normalize ();
		tempDir *= speed * Time.deltaTime;

		this.gameObject.transform.Translate (tempDir,Space.World);



		rollTime -= Time.deltaTime;
		if (rollTime <0) {
			if (goingOut) {
				dir = (Origin - this.transform.position);
				dir.y = 0;
				foreach (rotater rot in GetComponentsInChildren<rotater>()) {
					rot.speed *=-1; 
				}
				speed = 10;
				rollTime = trueROlltime;
				goingOut = false;
			}
			else{
				Destroy (this.gameObject);}


			}

		}
		



	new void OnControllerColliderHit(ControllerColliderHit other)
	{
		
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == Source && !goingOut) {
			Destroy (this.gameObject);
		}
	}

	 void OnTriggerExit(Collider other)
	{}
}
