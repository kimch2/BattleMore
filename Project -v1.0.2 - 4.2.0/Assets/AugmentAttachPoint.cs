using UnityEngine;
using System.Collections;

public class AugmentAttachPoint : Ability {

	public GameObject myAugment;
	public Vector3 attachPoint;
	public bool showPoint;

	public void BuildingStuff()
	{
		if (myAugment) {
			myAugment.GetComponent<Augmentor> ().startBuilding ();
		}
	}

	public void stopBuilding()
	{
		if (myAugment) {
			myAugment.GetComponent<Augmentor> ().stopBuilding();
		}
	}

	public void OnDrawGizmos()
	{if (showPoint) {
			Gizmos.color = Color.green;
			Gizmos.DrawSphere (transform.rotation * attachPoint + transform.position, 1.5f);

		}
	}


	public override continueOrder canActivate(bool error)
	{
		continueOrder ord = new continueOrder();
		ord.canCast = myAugment == null;
		ord.nextUnitCast = true;
		return ord;
	}

	public override void Activate()
	{

	}

	public override void setAutoCast(bool offOn)
	{

	}




}
