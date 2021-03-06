﻿using UnityEngine;
using System.Collections;

public class evasiveMenuvers : Ability,Modifier{

		UnitStats myStats;

		public float chanceMultiplier = 1;
		IMover mover;

	public Animator MyAnim;
	float lastDodgeTime;


	new void Awake()
	{
		base.Awake();
		audioSrc = GetComponent<AudioSource> ();
			myType = type.passive;
		}


		// Use this for initialization
		new void Start () {
		myStats = GetComponent<UnitManager> ().myStats;
		myStats.addModifier (this, 0);
		mover = GetComponent<UnitManager> ().cMover;


		}

		
	public float modify(float amount, GameObject src, OnHitContainer hitSource, DamageTypes.DamageType theType)
		{
		if (theType != DamageTypes.DamageType.Energy) {
			int rand = Random.Range (0, 100);
			//Debug.Log ("Current move speed is " + mover.speed);
			if (rand <= mover.myspeed * chanceMultiplier) {


				amount = 0;
				PopUpMaker.CreateGlobalPopUp ("Dodged", Color.yellow, this.transform.position);
				if (MyAnim && Time.time > lastDodgeTime) {
					MyAnim.Play ("HarpySpin");
				}
				lastDodgeTime = Time.time;
			}
		}
			return amount;
		}

		public override void setAutoCast(bool offOn){
		}


		override
		public continueOrder canActivate (bool showError)
		{

			continueOrder order = new continueOrder ();
			return order;
		}

		override
		public void Activate()
		{
			//return true;//next unit should also do this.
		}

	}