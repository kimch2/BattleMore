using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrecodDance : Ability, Modifier
{

	UnitStats myStats;

	public float chanceMultiplier = 1;
	public float dodgeFrequancy = 1.5f;
	public Animator MyAnim;
	float nextDodgeTime;


	new void Awake()
	{
		base.Awake();
		audioSrc = GetComponent<AudioSource>();
		myType = type.passive;
	}


	// Use this for initialization
	new void Start()
	{
		base.Start();
		myStats = GetComponent<UnitManager>().myStats;
		myStats.addModifier(this, 0);

	}


	public float modify(float amount, GameObject src, OnHitContainer hitSource, DamageTypes.DamageType theType)
	{

		if (nextDodgeTime > Time.time)
		{
			return amount ;
		}
	
		if (theType != DamageTypes.DamageType.Energy)
		{
			int rand = Random.Range(0, 100);

			if (rand <= chanceMultiplier)
			{
				nextDodgeTime = Time.time + dodgeFrequancy;
			
				PopUpMaker.CreateGlobalPopUp("Dodged", Color.yellow, this.transform.position);
				if (MyAnim)
				{
					//MyAnim.Play("HarpySpin");
				}
				
				if(myManager.getState() is AttackMoveState)
				{
					if (src)
					{
						UnitManager man =  ((AttackMoveState)myManager.getState()).getEnemy();
						if (man && Vector3.Distance(man.transform.position, transform.position) > 16)
						{
							transform.position = (man.transform.position - transform.position).normalized * 15 + man.transform.position;
						}
					}
				}
				return 0;
			}
		}
		return amount;
	}

	public override void setAutoCast(bool offOn)
	{
	}


	override
	public continueOrder canActivate(bool showError)
	{

		continueOrder order = new continueOrder();
		return order;
	}

	override
	public void Activate()
	{
		//return true;//next unit should also do this.
	}

}