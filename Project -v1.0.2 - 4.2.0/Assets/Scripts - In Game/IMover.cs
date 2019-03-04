using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.RVO;
public abstract class IMover: MonoBehaviour {



	public float myspeed = 0;
	public float acceleration;
	public float MaxSpeed = 10;
	public float initialSpeed;

	public abstract bool move ();
	public abstract void stop ();

	public FogOfWarUnit myFogger;
	public 	abstract void resetMoveLocation (Vector3 location);
	public 	abstract void resetMoveLocation (Transform theTarget);

	private struct SpeedMod{
		public float perc;
		public float flat;
		public Object source;
	}

	private List<SpeedMod> ASMod = new List<SpeedMod>();

	void Awake()
	{
		initialSpeed = MaxSpeed;
		myFogger = GetComponent<FogOfWarUnit> ();

	}

	public float getMaxSpeed()
	{
		return MaxSpeed;
	}


	void Start()
	{
		initialSpeed = MaxSpeed;

	}
	/*
	public void removeSpeedBuff(Object obj)
	{
		for (int i = 0; i < ASMod.Count; i++) {
			if (ASMod [i].source ==obj) {
				ASMod.RemoveAt (i);
			}
		}
		adjustSpeed ();

	}

	/// <summary>
	///  to reduce speed by a percent, pass in .2f to make it attack 20% slower
	/// </summary>
	public void changeSpeed(float perc, float flat, bool perm, Object obj, bool stackable = false )
	{	if (perm) {
			//Debug.Log ("Initial speed " + initialSpeed + "   " + flat + "   " + perc);
			initialSpeed += flat;
			initialSpeed *= perc;

			//Debug.Log ("Final speed is " + initialSpeed);
		}
		else
		{
			// This will need to be changed if a source can apply different amounts of speed changes
			if (obj != null)
			{
				for (int i = 0; i < ASMod.Count; i++)
				{
					SpeedMod a = ASMod[i];
					if (a.source == obj)
					{
						if (!stackable)
						{
							return;
						}
						else
						{
							a.flat += flat;
							a.perc +=  perc;

							ASMod[i] = a;
							adjustSpeed();
							return;
						}
						
					}
				}
			}

			SpeedMod temp = new SpeedMod ();
			temp.flat = flat;
			temp.perc = perc;
			temp.source = obj;
			ASMod.Add (temp);
		}

		adjustSpeed ();

	}
	


	private void adjustSpeed()
	{
		float tempspeed = initialSpeed;

		foreach (SpeedMod a in ASMod) {
			tempspeed += a.flat;
		
		}

		float percent = 1;
		foreach (SpeedMod a in ASMod) {

			percent += a.perc;
		}
		percent = Mathf.Max(0, percent);
		tempspeed *= percent;

		if (tempspeed < .001f) {
			tempspeed = .001f;}
		else if(tempspeed >1000) {
			tempspeed = 1000;}


		MaxSpeed = tempspeed;
		if (myspeed > MaxSpeed) {
			myspeed = MaxSpeed;}
		
		RVOController rvo = GetComponent<RVOController> ();
		if (rvo) {
			rvo.maxSpeed = MaxSpeed;
		}
	
	}
	*/
}
