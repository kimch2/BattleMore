using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarProjectile : Projectile {

	public float inaccuracy;
	public float arcAngle;
	[Tooltip("Put any objects in here if you want the nose of the projectile to track up and down with the arc")]
	public GameObject myModel;
	public GameObject TargetIndicator;
	public Texture indicatorPic;
	public float indicatorSize;
	private GameObject myIndiactor;
    protected float yAmount;

    


    protected override void Update () {
		/*
		if (Vector3.Distance (transform.position, lastLocation) < 2.5f) {
			Terminate (target);
			return;
		}
*/
		if (currentDistance > distance-1) {
			Terminate (target);
			return;
		}

		gameObject.transform.Translate (Vector3.forward* speed * Time.deltaTime);

		currentDistance += speed * Time.deltaTime ;

		yAmount = (((distance / 2) - currentDistance) / distance) * arcAngle * Time.deltaTime;
		control.Move (Vector3.up * yAmount);

		if (myModel) {
			myModel.transform.LookAt (this.transform.position + transform.forward * -1 + Vector3.up * -yAmount);
		}
	}



	public override void setup()
	{
		if (!control) {
			control = GetComponent<CharacterController> ();
		}

		if (TargetIndicator != null && vetSource.myUnit.PlayerOwner != 1 ) {
			TargetIndicator.GetComponentInChildren<Light> ().color = Color.red;
		}

		Vector3 hitzone = target.transform.position;

		if (inaccuracy > 0) {
			hitzone = target.transform.position;
			float radius = Random.Range (0, inaccuracy);
			float angle = Random.Range (0, 360);

			hitzone.x += Mathf.Sin (Mathf.Deg2Rad * angle) * radius;
			hitzone.z += Mathf.Cos (Mathf.Deg2Rad * angle) * radius;


		}
		transform.LookAt (hitzone);
		lastLocation = hitzone;
		distance = Vector3.Distance (hitzone, transform.position);
		if (TargetIndicator != null) {

			myIndiactor = (GameObject)Instantiate (TargetIndicator, lastLocation, Quaternion.identity);
			myIndiactor.GetComponentInChildren<Light>().cookie = indicatorPic;
			myIndiactor.GetComponentInChildren<Light> ().spotAngle = indicatorSize;
		}
		Update ();
	}

    protected override void OnControllerColliderHit(ControllerColliderHit other)
    {
        if (!target)
        {
            return;
        }

        if (other.gameObject == target || other.gameObject.transform.IsChildOf(target.transform) || (Source && Source.transform.IsChildOf(other.transform)))
        {
            Debug.Log("Terminating on " + target + "   Source is " + Source);
            Terminate(other.gameObject.GetComponent<UnitManager>());
        }

		if (currentDistance / distance < .5) {
			return;
		}

        if (Source && (other.gameObject != Source && !other.gameObject.transform.IsChildOf(Source.transform) && (!Source.transform.IsChildOf(other.transform))))
        {
            Debug.Log("Terminating on " + target + "   Source is " + Source);
            Terminate(null);
        }
    }



    protected override void onTerminate ()
	{
		if (myIndiactor) {
			Destroy (myIndiactor);
		}
	}			
}
