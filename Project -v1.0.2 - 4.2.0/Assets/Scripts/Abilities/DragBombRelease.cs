using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBombRelease : Ability
{
	//This script is very similar to the CreateObjectAbility, except it allows a second cast

	//UnitManager myManager;
	public GameObject ThingToMake;
	public float secondClickDelayTime = 8;

	public List<IWeapon.AnimationPoint> numberToMake;
	// Use this for initialization
	List<GameObject> createdObjects = new List<GameObject>();

	bool firstClickHappened;

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

		if (!firstClickHappened)
		{
			if (!myCost || myCost.canActivate(this))
			{

				if (myCost)
				{
					myCost.payCost();

					Invoke("DelaySecondClick", secondClickDelayTime);
				}
				myCost.resetCoolDown();
				createdObjects.Clear();
				firstClickHappened = true;
				for (int i = 0; i < numberToMake.Count; i++)
				{
					GameObject newObj = Instantiate<GameObject>(ThingToMake, transform.rotation * numberToMake[i].position + transform.position, Quaternion.identity, null);
					newObj.SendMessage("setSource", myManager.gameObject, SendMessageOptions.DontRequireReceiver);
					createdObjects.Add(newObj);
				}
			}
		}
		else
		{
			firstClickHappened = false;
			if (myCost)
			{
				myCost.startCooldown();
			}
			CancelInvoke("DelaySecondClick");
			foreach (GameObject obj in createdObjects)
			{
				if (obj)
				{
					obj.SendMessage("SecondClick", SendMessageOptions.DontRequireReceiver);
				}
			}
			createdObjects.Clear();
		}
	}

	void DelaySecondClick()
	{
		firstClickHappened = false;
		myCost.startCooldown();
	}


	public void OnDrawGizmos()
	{
		foreach (IWeapon.AnimationPoint vec in numberToMake)
		{

			Gizmos.DrawSphere((transform.rotation) * vec.position + this.gameObject.transform.position, .5f);

		}
	}
}
