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
    bool Rooted;


    public abstract bool Move ();
	public abstract void stop ();

	public FogOfWarUnit myFogger;
	public 	abstract void resetMoveLocation (Vector3 location);
	public 	abstract void resetMoveLocation (Transform theTarget);
    [HideInInspector]
    public bool m_lockRotation;

	void Awake()
	{
		initialSpeed = MaxSpeed;
		myFogger = GetComponent<FogOfWarUnit> ();

	}

    public bool move()
    {
        if (!Rooted)
        {
            return Move();
        }
        else
        {
            return true;
        }
    }

    public virtual void LockRotation(bool isLocked)
    {
        m_lockRotation = isLocked;
            
    }

	public float getMaxSpeed()
	{
		return MaxSpeed;
	}

    public void Root(bool rootMe)
    {
        Rooted = rootMe;
    }

	void Start()
	{
		initialSpeed = MaxSpeed;

	}
    public virtual void SetMaxSpeed(float m)
    { MaxSpeed = m; }
}
