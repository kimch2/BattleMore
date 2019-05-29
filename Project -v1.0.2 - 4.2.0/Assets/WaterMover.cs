using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMover : IMover
{
	private Vector3 targetPosition;
	private CharacterController controller;

	//The calculated path
	public float turnSpeed;
	//The AI's speed per second

	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;

	private bool workingframe = false;

	private Vector3 dir;

	public float flyerHeight;

	public void Start()
	{
		myFogger = GetComponent<FogOfWarUnit>();
		controller = GetComponent<CharacterController>();
		initialSpeed = getMaxSpeed();
	}

	override
	public void stop()
	{
		GetComponent<UnitManager>().animStop();
		myspeed = .1f;
	}


	UnitManager collidingObject; // This is used to help cache units we are tryingto navigate around

	private void Update()
	{
		//Make sure your the right height above the terrain
		RaycastHit objecthit;

		Vector3 down = this.gameObject.transform.TransformDirection(Vector3.down);

		if (Physics.Raycast(this.gameObject.transform.position + Vector3.up * 100, down, out objecthit, 1000, 1 << 16))
		{
			transform.position = new Vector3(transform.position.x, objecthit.point.y + flyerHeight, transform.position.z);
		}
	}

	override
	public bool move()
	{
		if (!workingframe)
		{
			workingframe = !workingframe;
			//Debug.Log ("Returnin 1 ");
			return false;
		}

		float tempDist = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(targetPosition.x, targetPosition.z));
		if (tempDist <= nextWaypointDistance)
		{
			ComeToStop();
			myspeed = 0;
			return true;
		}

		if (myspeed < getMaxSpeed())
		{
			myspeed += .1f * acceleration;

			if (myspeed > getMaxSpeed())
			{
				myspeed = getMaxSpeed();
			}

		}
		dir = (targetPosition - transform.position).normalized;

		

		RaycastHit lookAhead = new RaycastHit();
		Vector3 vec = this.gameObject.transform.forward;

		if (Physics.Raycast(this.gameObject.transform.position, vec, out lookAhead, 7, 1 << 9))
		{

			Vector3 heading = lookAhead.collider.gameObject.transform.position - transform.position;
			float dirNum = AngleDir(transform.forward, heading, transform.up);
			dir -= this.gameObject.transform.TransformDirection(Vector3.right) * dirNum;

			if (!collidingObject || collidingObject.gameObject != lookAhead.collider.gameObject)
			{
				collidingObject = lookAhead.collider.gameObject.GetComponent<UnitManager>();

				if (!collidingObject)
				{
					collidingObject = lookAhead.collider.gameObject.GetComponentInParent<UnitManager>();
				}
			}

			if (collidingObject && collidingObject.getState() is DefaultState)
			{
				if (Vector3.Distance(collidingObject.gameObject.transform.position, targetPosition) < collidingObject.CharController.radius + .5f)
				{
					ComeToStop();
					return true;
				}
			}
		}



		dir *= myspeed * Time.deltaTime;

		controller.Move(dir);

		

		//Debug.Log ("air movin " + dir);
		if (myFogger)
		{
			myFogger.move();
		}
		Vector3 turnAmount = targetPosition - transform.position;
		turnAmount.y = 0;
		if (turnAmount != Vector3.zero)
		{
			Quaternion toTurn = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(turnAmount), Time.deltaTime * turnSpeed * 0.2f);

			if (!float.IsNaN(toTurn.x) && !float.IsNaN(toTurn.y) && !float.IsNaN(toTurn.z) && !float.IsNaN(toTurn.w))
			{
				transform.rotation = toTurn;
			}
		}
		//	Debug.Log ("Returnin 3 ");
		return false;
	}

	Coroutine slowingDown;

	void ComeToStop()
	{
		if (slowingDown != null)
		{
			StopCoroutine(slowingDown);
		}
		slowingDown = StartCoroutine(slowDown());
	}

	void BeginMoving()
	{
		if (slowingDown != null)
		{
			StopCoroutine(slowingDown);
			myspeed = slowDownSpeed;
		}
	}

	float slowDownSpeed;
	IEnumerator slowDown()
	{
		slowDownSpeed = myspeed;
		for (float i = 0; i < .7f; i += Time.deltaTime)
		{
			slowDownSpeed -= Time.deltaTime * MaxSpeed;
			controller.Move(slowDownSpeed * transform.forward * Time.deltaTime);
			yield return null;
		}
		slowingDown = null;
	}


	float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
	{
		Vector3 perp = Vector3.Cross(fwd, targetDir);
		float dir = Vector3.Dot(perp, up);

		if (dir > 0f)
		{
			return 1f;
		}
		else if (dir < 0f)
		{
			return -1f;
		}
		else
		{
			return 0f;
		}
	}



	override
	public void resetMoveLocation(Vector3 location)
	{//	location.y += 2;


		location = MainCamera.main.getMapClampedLocation(location);
		RaycastHit objecthit;

		if (Physics.Raycast(location + Vector3.up * 30, Vector3.down, out objecthit, 1000, 1 << 16))
		{
			//if (Physics.Raycast (this.gameObject.transform.position, down, out objecthit, 1000, (~8))) {

			targetPosition = objecthit.point + Vector3.up * flyerHeight;

		}
		else
		{
			targetPosition = location + Vector3.up * flyerHeight;
		}

		if (myspeed == 0)
		{
			myspeed = .1f;
		}

		BeginMoving();

		GetComponent<UnitManager>().animMove();

		workingframe = false;
	}

	override
	public void resetMoveLocation(Transform targ)

	{
	}
	
}