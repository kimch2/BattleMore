using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLock : MonoBehaviour
{

	public List<Tweener> waters = new List<Tweener>();
	public List<SpriteRenderer> UpArrows = new List<SpriteRenderer>();
	public List<SpriteRenderer> DownArrows = new List<SpriteRenderer>();
    public AudioClip WaterEffect;
    public AudioClip concreteEffect;
    public Tweener Lock;
	public WayPoint myWayPoint;
	private void Start()
	{
		if (Lock)
		{
			CheckSides();
		}

		foreach (Tweener tween in waters)
		{
			if (tween.currentPose == "up")
			{
				GoUp(tween);
			}
			else
			{
				GoDown(tween);
			}
		}
	}


	public List<WaterLock> LocksToCheck;
	public void CheckAllLocks()
	{
		foreach (WaterLock lockers in LocksToCheck)
		{
			lockers.Operate();
		}
	}

	public void Operate()
	{
		if (currentOperation == null)
		{
			currentOperation = StartCoroutine(Operation());
		}
	}

	Coroutine currentOperation;

	IEnumerator Operation()
	{
		bool allClosed = true;
		foreach (WaterLock lockers in LocksToCheck)
		{
			if (lockers.Lock.currentPose == "open")
			{
				allClosed = false;
			}
			lockers.close();
		}
        if (Time.timeSinceLevelLoad > 2)
        {
            GetComponent<AudioSource>().PlayOneShot(WaterEffect);
        }
        if (!allClosed)
		{
			yield return new WaitForSeconds(2.5f);
		}

		

		foreach (Tweener tween in waters)
		{
			if (tween.currentPose == "up")
			{
				GoDown(tween);
			}
			else
			{
				GoUp(tween);
			}
		}


		yield return new WaitForSeconds(2.5f);
		foreach (WaterLock lockers in LocksToCheck)
		{
			lockers.CheckSides();
		}
		currentOperation = null;
	}

	public void close()
	{if (Time.timeSinceLevelLoad > 2)
        {
            GetComponent<AudioSource>().PlayOneShot(concreteEffect);
        }
		Lock.GoToPose("closed");
		if (myWayPoint)
			myWayPoint.IsPathable = false;
		GraphUpdate();
	}

	public void CheckSides()
	{
		if (isSameHeight())
		{
            if (Time.timeSinceLevelLoad > 2)
            {
                GetComponent<AudioSource>().PlayOneShot(concreteEffect);
            }
			Lock.GoToPose("open");
			if (myWayPoint)
				myWayPoint.IsPathable = true;
		}
		else
		{
            if (Time.timeSinceLevelLoad > 2)
            {
                GetComponent<AudioSource>().PlayOneShot(concreteEffect);
            }
            Lock.GoToPose("closed");
			if(myWayPoint)
			myWayPoint.IsPathable = false;
		}
		GraphUpdate();	
	}

	void GoUp(Tweener t)
	{
		t.GoToPose("up");
        if (Time.timeSinceLevelLoad > 2)
        {
            GetComponent<AudioSource>().PlayOneShot(WaterEffect);
        }
        foreach (SpriteRenderer render in UpArrows)
		{
            if(render != null)
			render.color = Color.green;
		}
		foreach (SpriteRenderer render in DownArrows)
		{
            if (render != null)
                render.color = Color.gray;
		}
	}

	void GoDown(Tweener t)
	{
		t.GoToPose("down");
        if (Time.timeSinceLevelLoad > 2)
        {
            GetComponent<AudioSource>().PlayOneShot(WaterEffect);
        }
        foreach (SpriteRenderer render in DownArrows)
		{
			render.color = Color.green;
		}
		foreach (SpriteRenderer render in UpArrows)
		{
			render.color = Color.gray;
		}
	}



	bool isSameHeight()
	{
		float currentHeight = -1;
		foreach (Tweener tween in waters)
		{
			if (currentHeight == -1)
			{
				currentHeight = tween.transform.position.y;
			}
			else if ( Mathf.Abs( tween.transform.position.y - currentHeight) < .1f)
			{

			}
			else
			{
				return false;
			}
		}
		return true;
	}


	void GraphUpdate()
	{
		Bounds b = new Bounds();
		b.center = transform.position;
		b.extents = new Vector3(50,50,50);
		Pathfinding.GraphUpdateObject area = new Pathfinding.GraphUpdateObject(b);
		StartCoroutine(Rescan(area));
	}


	IEnumerator Rescan(Pathfinding.GraphUpdateObject b)
	{
		AstarPath.active.UpdateGraphs(b);
		yield return new WaitForSeconds(2.5f);
		AstarPath.active.UpdateGraphs(b);
	}

}
