using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*#if RVOImp
 * using RVO;
 #endif*/
using Pathfinding;
using Pathfinding.RVO;

[HelpURL("http://arongranberg.com/astar/docs/class_r_v_o_example_agent.php")]
public class CustomRVO : IMover {

	public float repathRate = 1;

	private float nextRepath = 0;

#if RVOImp
	private int agentID;
#endif

	private bool pathSet;
	private Vector3 target;
	private bool canSearchAgain = true;

	private RVOController controller;
	private int currentWaypoint = 0;

	Path path = null;
	private float latestDistance =0;

	List<Vector3> vectorPath;
	int wp;

	#if RVOImp
	public bool astarRVO = true;
	#endif

	public float moveNextDist = 1;
	Seeker seeker;

	MeshRenderer[] rends;

	//IAgent rvoAgent;
	#if RVOImp
	public Vector3 position {
	get {
	if (astarRVO) return rvoAgent.InterpolatedPosition;
	else return RVO.Simulator.Instance.getAgentPosition3(agentID);
	//#else
	return rvoAgent.InterpolatedPosition;
	}
	}
	#endif

	public void Awake () {
        base.Awake();
		initialSpeed = getMaxSpeed();
		seeker = GetComponent<Seeker>();
        controller = GetComponent<RVOController>();
    }



	// Use this for initialization
	public void Start () {
		myFogger = GetComponent<FogOfWarUnit>();
#if RVOImp
	
#endif


	}



	override
	public void resetMoveLocation (Vector3 target) {
		
		//Debug.Log ("Resetting to " + target + "  " + Time.deltaTime);
		this.target = target;
		RecalculatePath();
		//Debug.Log ("Setting move path " + target);
		
		//manager.animMove ();
	}



	public void RecalculatePath () {
		pathSet = true;
		canSearchAgain = false;
		nextRepath = Time.time+repathRate*(Random.value+0.5f);
	
		latestDistance = 1000000;

		seeker.StartPath(transform.position, target, OnPathComplete);
	

	}

	public void OnPathComplete (Path _p) {
        //Debug.Log("Completing  " + Time.deltaTime);
		ABPath p = _p as ABPath;
		currentWaypoint =1;
		canSearchAgain = true;

		if (path != null) path.Release(this);
		path = p;
		p.Claim(this);

		if (p.error) {
			wp = 0;
			vectorPath = null;
			return;
		}


		Vector3 p1 = p.originalStartPoint;
		Vector3 p2 = transform.position;
		p1.y = p2.y;
		float d = (p2-p1).magnitude;
		wp = 0;

		vectorPath = p.vectorPath;
		Vector3 waypoint;

		for (float t = 0; t <= d; t += moveNextDist*0.6f) {
			wp--;
			Vector3 pos = p1 + (p2-p1)*t;

			do {
				wp++;
				waypoint = vectorPath[wp];
				waypoint.y = pos.y;
			} while ((pos - waypoint).sqrMagnitude < moveNextDist*moveNextDist && wp != vectorPath.Count-1);
		}
	}


	override
	public void stop ()
	{if (controller) {
			controller.Move (Vector3.zero);
            myspeed = 0;
		}
   
		GetComponent<UnitManager> ().animStop();
	}

    override
    public void  LockRotation(bool LockIt)
    {
        controller.enableRotation = !LockIt;
    }


	override 
	public bool Move()
	{

        if (!enabled)
        {
            controller.Move(Vector3.zero);
            return false; }

        //Debug.Log(this.gameObject + "  moving");
		if (Time.time >= nextRepath && canSearchAgain) {
			RecalculatePath();

		}
        
		if (path == null && !pathSet) {
			if (controller) {

				controller.Move (Vector3.zero);
			}

			return true;
		} else if (pathSet && (path == null || path.vectorPath.Count == 0)) {
          //  Debug.Log("Exiting here" + pathSet + "  " + path);
			return false;
        }

        /*

		if (currentWaypoint >= path.vectorPath.Count) {
			myspeed = 0;
            
			path = null;
			pathSet = false;
			if (controller) {
				
				controller.Move (Vector3.zero);
			}
			//Debug.Log ("Returning 1"+this.gameObject);
			return true;
		}
        */
		if (myspeed < getMaxSpeed()) {
			myspeed += acceleration * Time.deltaTime;

			if (myspeed > getMaxSpeed()) {
				myspeed =  getMaxSpeed();
			}
		}

	
		//Direction to the next waypoint
	
		Vector3 dir = (path.vectorPath[currentWaypoint] -this.transform.position).normalized * myspeed;
	
		if (myFogger) {
			myFogger.move ();
		}
		//Debug.Log ("Moving " + dir);

		if(controller){
            myManager.animMove();
            controller.Move (dir);
        }
		else{
			Debug.Log (this.gameObject +  " is missing its rvocontroller");
		}
	

		float dist = Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]);
		if (dist > latestDistance && currentWaypoint < path.vectorPath.Count -1) {

			currentWaypoint++;
			latestDistance = 100000;
            return CheckForPathEnd();
			//Debug.Log (" is movingB"+this.gameObject);
		//	return false;
		} else {
			latestDistance = dist;}

		if (dist < 2) {
			//Debug.Log ("Waypoint " + currentWaypoint + "   total " +path.vectorPath.Count   + "  distance  " + Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]));
			currentWaypoint++;
            CheckForPathEnd();

		
		}
		//Debug.Log (" is movingA"+this.gameObject);
		return false;

	}

    public override void SetMaxSpeed(float m)
    {
        MaxSpeed = m;
        controller.SetMaxSpeed(m);
    }

    bool CheckForPathEnd()
    {
        if (currentWaypoint >= path.vectorPath.Count)
        {
            myspeed = 0;

            path = null;
            pathSet = false;
            if (controller)
            {
                controller.Move(Vector3.zero);
            }

            return true;
        }
        return false;
    }


	override
	public void resetMoveLocation(Transform targ)

	{
	}

	void OnControllerColliderHit(ControllerColliderHit other)
	{
		if (other.gameObject.transform.position == target) {

			path = null;
		}
	}
}
