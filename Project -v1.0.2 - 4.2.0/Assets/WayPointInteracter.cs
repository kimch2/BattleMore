using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointInteracter : StandardInteract {

	public WayPoint previous;
	[Tooltip("You only have to fill in one or the other")]
	public  WayPoint next;
	public WayPoint oneAfter;
	public float randomRadius;
	public bool showArrows;

	float startTime;
	void Start() {
		startTime = Time.timeSinceLevelLoad;
		if (!oneAfter) {
			oneAfter = next.nextPoint (previous);
		}
		if (!next) {
			next = previous.myFriends [Random.Range (0, previous.myFriends.Count)];
		} else {
		
		}
		myManager.GiveOrder (Orders.CreateMoveOrder(next.transform.position));

		if (showArrows) {
			InvokeRepeating ("updateArrow",.1f,4);
		}
	}


	void updateArrow()
	{
		MiniMapUIController.main.drawPath (new List<Vector3>(){transform.position,next.transform.position, oneAfter.transform.transform.position}, 3.3f,.5f);
	}
	public override UnitState computeState(UnitState s)
	{
		if (s is DefaultState && next  &&  Time.timeSinceLevelLoad - startTime  > 1) {
			
			Invoke ("giveOrder", .01f);
		}
		return s;
	}


	void giveOrder()
	{
		previous = next;
		next = oneAfter;
	
		oneAfter =next.nextPoint (previous);
	
		Vector3 nextPoint = next.transform.position;
		nextPoint.x += UnityEngine.Random.Range (-randomRadius,randomRadius);
		nextPoint.z += UnityEngine.Random.Range (-randomRadius,randomRadius);
		myManager.GiveOrder (Orders.CreateMoveOrder (nextPoint));		
	}

    private void OnTriggerEnter(Collider other)
    {
       
		WayPointInteracter inter = other.gameObject.GetComponent<WayPointInteracter> ();
		if (inter) {
            Debug.Log("COlliding with " + other.gameObject);
            // A hack to make it so only one death cube redirects itself when they collide

            if (inter.next == next && inter.transform.position.x < transform.position.x || inter.next != next || inter.next == previous) {
				
				WayPoint temp = previous;
				previous = next;
				next = temp;

				oneAfter = next.nextPoint (previous);

				Vector3 nextPoint = next.transform.position;
				nextPoint.x += UnityEngine.Random.Range (-randomRadius, randomRadius);
				nextPoint.z += UnityEngine.Random.Range (-randomRadius, randomRadius);
				myManager.GiveOrder (Orders.CreateMoveOrder (nextPoint));
			}
		}
	}

}
