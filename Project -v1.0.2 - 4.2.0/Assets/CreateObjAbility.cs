using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObjAbility : Ability
{

	//UnitManager myManager;
	public GameObject ThingToMake;

	public List<IWeapon.AnimationPoint> numberToMake;
	// Use this for initialization

	new void Awake()
	{
		base.Awake();
		audioSrc = GetComponent<AudioSource>();
		myType = type.activated;
		if (numberToMake.Count == 0)
		{
			numberToMake.Add(new IWeapon.AnimationPoint());
		}
	}


	public override void setAutoCast(bool offOn)
	{
	}


	override
	public continueOrder canActivate(bool showError)
	{
		continueOrder order = new continueOrder();
		if (myCost && myCost.canActivate(this))
		{
			order.canCast = true;
		}
		return order;
	}

	override
	public void Activate()
	{

		if (!myCost || myCost.canActivate(this))
		{

			if (myCost)
			{
				myCost.payCost();
			}

			for (int i = 0; i < numberToMake.Count; i++)
			{
				GameObject newObj = Instantiate<GameObject>(ThingToMake, transform.rotation * numberToMake[i].position + transform.position, Quaternion.identity, null);
				newObj.SendMessage("setSource", myManager.gameObject, SendMessageOptions.DontRequireReceiver);

			}
		}
	}


	public void OnDrawGizmos()
	{
		foreach (IWeapon.AnimationPoint vec in numberToMake)
		{

			Gizmos.DrawSphere((transform.rotation) * vec.position + this.gameObject.transform.position, .5f);
			
		}
	}
}
