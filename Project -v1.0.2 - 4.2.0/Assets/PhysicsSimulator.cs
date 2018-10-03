using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSimulator : MonoBehaviour {

	public static PhysicsSimulator main;


	void Start()
	{
		main = this;
	}
	/// <summary>
	/// Knocks the back.
	/// </summary>
	/// <param name="source">Source.</param>
	/// <param name="target">Target.</param>
	/// <param name="force">The max distance the guy should be knocked back</param>
	/// <param name="HPReduce">If set to <c>true</c> HP reduce.</param>
	public void KnockBack (Vector3 sourceLocation, UnitManager target, Vector2 force, System.Action onFinish,  bool TenacityReduce = false)
	{
		
		Vector3 startLocation = target.transform.position;
		if (target.myStats.isUnitType (UnitTypes.UnitTypeTag.Structure)) {
			return;
		}

		if (TenacityReduce) {
			if (target.myStats.isUnitType (UnitTypes.UnitTypeTag.Medium)) 
			{
				force *= .75f;
			}
			else if(target.myStats.isUnitType (UnitTypes.UnitTypeTag.Large))
			{
				force *= .5f;
			}
			else if(target.myStats.isUnitType (UnitTypes.UnitTypeTag.Massive))
			{
				force *= .25f;
			}
		}

		Vector3 theoreticalTarget =  target.transform.position +  force.x * (target.transform.position - sourceLocation).normalized;

		Vector3 ActualTarget =  (Vector3)AstarPath.active.graphs [0].GetNearest (theoreticalTarget).node.position;
		float travelTime = Vector3.Distance (ActualTarget, target.transform.position)/150;
		StartCoroutine (fly(target, ActualTarget,travelTime, force.y,false, onFinish));
	}



	IEnumerator fly(UnitManager target, Vector3 targetLocation, float travelTime, float UpForce, bool linear,  System.Action onFinish)
	{
		
		Vector3 startPoint = target.transform.position;

		float height = target.transform.position.y;
		if (target.cMover is airmover) {
			height = ((airmover)target.cMover).flyerHeight;
		} else {
			RaycastHit objecthit;

			if (Physics.Raycast (target.transform.position, Vector3.down, out objecthit, 1000, 1 << 8)) {
				height = objecthit.distance;
			}
		}
		targetLocation.y += height;	
	
		target.StunForTime (target,travelTime, !linear);

		float UpSpeed = UpForce;

		float UpDistance = 0;

		for (float i = 0; i < travelTime; i += Time.deltaTime) {
			if (!target) {
				break ;
			}

			UpDistance += UpSpeed * Time.deltaTime * UpForce/2;
			UpSpeed -= (Time.deltaTime * (UpForce*2 / travelTime));
			target.transform.position = Vector3.Lerp (startPoint, targetLocation, linear ? i/travelTime :  Mathf.Sqrt(i/travelTime)) + Vector3.up * (Mathf.Max(0, UpDistance));
		
			yield return null;
		}
		if (target) {
			if (target.fogger) {
				target.fogger.hasMoved = true;
			}
			target.transform.position = targetLocation;
		}
		if (onFinish != null) {
			onFinish.Invoke ();
		}
	
	}

	public void Dash (UnitManager target, Vector3 location, Vector2 speed,  System.Action onFinish)
	{

		Vector3 startLocation = target.transform.position;

		Vector3 ActualTarget =  (Vector3)AstarPath.active.graphs [0].GetNearest (location).node.position;
		StartCoroutine (fly(target, ActualTarget, Vector3.Distance(target.transform.position, location) / speed.x, speed.y, true, onFinish));
	}


}
