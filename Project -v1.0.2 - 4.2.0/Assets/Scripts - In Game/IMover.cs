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
	
}
