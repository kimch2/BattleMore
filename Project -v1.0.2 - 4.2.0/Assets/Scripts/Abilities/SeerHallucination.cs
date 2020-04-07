using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeerHallucination : Ability , Modifier
{
	//UnitManager myManager;
	public GameObject myShade;
	GameObject currentShade;
	float previousHealth;
	float previousEnergy;

	public float shadeDuration = 10;

	new void Awake()
	{
		base.Awake();
		audioSrc = GetComponent<AudioSource>();

		myType = type.activated;
	}


	public override void setAutoCast(bool offOn)
	{
		Activate();
	}


	override
	public continueOrder canActivate(bool showError)
	{
		if (currentShade)
		{
			return new continueOrder();
		}

		continueOrder order = new continueOrder
		{
			canCast = myCost.canActivate(this)
		};
		return order;
	}


	override
	public void Activate()
	{

		if (!currentShade)
		{
			myCost.payCost();
			//Set thing that makes it so you cant reactivate within 2 seconds.
			autocast = true;
			currentShade = Instantiate<GameObject>(myShade, myShade.transform.position, myShade.transform.rotation, null);
			myManager.myStats.addModifier(this);
			updateUICommandCard();
			previousHealth = myManager.myStats.health;
			previousEnergy = myManager.myStats.currentEnergy;
			myCost.startCooldown();
			Invoke("endSpell", shadeDuration);
		}
		else
		{
			endSpell();
		}
	}

	public float modify(float damage, GameObject source, OnHitContainer hitSource, DamageTypes.DamageType theType)
	{
		if (damage > myManager.myStats.health)
		{
			endSpell();
			return 0;
		}
		return damage;
	}

	void endSpell()
	{
		if (currentShade)
		{
			transform.position = currentShade.transform.position;
			myManager.myStats.removeModifier(this);
			myCost.resetCoolDown();
			myManager.myStats.setHealthValue (Mathf.Max(previousHealth, myManager.myStats.health));
			myManager.myStats.setEnergy(Mathf.Max(previousEnergy, myManager.myStats.currentEnergy));
			Destroy(currentShade);
		}
	}


	void destroyShade()
	{
		if (currentShade)
		{
			Destroy(currentShade);
		}
	}
}
