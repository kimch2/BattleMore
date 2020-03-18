using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSimulator : MonoBehaviour {

	public static PhysicsSimulator main;
    
	void Start()
	{
		main = this;
	}
    
	Dictionary<UnitManager, Coroutine> currentlyMoving = new Dictionary<UnitManager, Coroutine>();
	Dictionary<UnitManager, Vector3> currentTargets = new Dictionary<UnitManager, Vector3>();
	// When a force hits a unit with more force than what is remaining in that guys coruotine, it will have priority



	/// <summary>
	/// Knocks the back.
	/// </summary>
	/// <param name="source">Source.</param>
	/// <param name="target">Target.</param>
	/// <param name="force">The max distance the guy should be knocked back</param>
	/// <param name="HPReduce">If set to <c>true</c> HP reduce.</param>
	public void KnockBack (Vector3 sourceLocation, UnitManager target, UnityEngine.Object sourceComponent, Vector2 force, System.Action onFinish,  bool TenacityReduce = false)
	{
		Vector3 startLocation = target.transform.position;
		if (target.myStats.isUnitType (UnitTypes.UnitTypeTag.Structure)) {
			return;
		}

		if (TenacityReduce) {

			force *=   target.myStats.getTenacityMultiplier();
        }

       // Debug.Log(startLocation +"  " + sourceLocation + "   " + (startLocation - sourceLocation).normalized);

		Vector3 theoreticalTarget = startLocation +  force.x * (startLocation - sourceLocation).normalized;

		Vector3 ActualTarget = theoreticalTarget;
		if (!target.myStats.isUnitType(UnitTypes.UnitTypeTag.Air))
		{
			ActualTarget = (Vector3)AstarPath.active.graphs[0].GetNearest(theoreticalTarget).node.position;
		}
		
		RaycastHit objecthit;

		if (Physics.Raycast(ActualTarget + Vector3.up * 40, Vector3.down, out objecthit, 1000, 1 << 8))
		{
			ActualTarget.y = objecthit.point.y;
		}
		if (target.myStats.isUnitType(UnitTypes.UnitTypeTag.Air))
		{
			ActualTarget.y += ((airmover)target.cMover).flyerHeight;
		}
		else {
			ActualTarget.y += 3; // fix this later
		}


		float travelTime = Vector3.Distance (ActualTarget, target.transform.position);

		if (currentlyMoving.ContainsKey(target))
		{
			float distance = Vector3.Distance(target.transform.position, currentTargets[target]);
			if (distance < travelTime)
			{
				StopCoroutine(currentlyMoving[target]);
				currentlyMoving.Remove(target);
				currentTargets.Remove(target);
			}
			else
			{
				Debug.Log("Returning - Already being knocked by somethign bigger");
				return;
			}

		}
		travelTime /= 120;
        //Debug.Log("Actual " + ActualTarget);
		
		Coroutine toAdd = StartCoroutine (fly(target,sourceComponent, ActualTarget,travelTime, force.y,false, onFinish));
		currentlyMoving.Add(target, toAdd);
		currentTargets.Add(target, ActualTarget);
	}


    
	IEnumerator fly(UnitManager target, UnityEngine.Object sourceComp, Vector3 targetLocation, float travelTime, float UpForce, bool linear,  System.Action onFinish)
	{
		Vector3 startPoint = target.transform.position;
	
		target.metaStatus.Stun (target, sourceComp, true,travelTime); // How to determine if its friendly or not???

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

       if (target)
            { target.metaStatus.UnStun(sourceComp);
                if (target.fogger) {
				target.fogger.hasMoved = true;
			}
			target.transform.position = targetLocation;
		}
		if (onFinish != null) {
			onFinish.Invoke ();
		}
		currentlyMoving.Remove(target);
		currentTargets.Remove(target);
	}

	public void Dash (UnitManager target,UnityEngine.Object sourceComp, Vector3 location, Vector2 speed,  System.Action onFinish)
	{
		Vector3 startLocation = target.transform.position;

		Vector3 ActualTarget =  (Vector3)AstarPath.active.graphs [0].GetNearest (location).node.position;
		StartCoroutine (fly(target, sourceComp, ActualTarget, Vector3.Distance(target.transform.position, location) / speed.x, speed.y, true, onFinish));
	}


}
