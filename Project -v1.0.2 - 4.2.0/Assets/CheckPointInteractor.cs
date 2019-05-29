using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointInteractor : StandardInteract
{
	public List<WayPoint> MyPath;
	int currentIndex = 0;
	public bool showArrows;
	float startTime;

	List<Vector3> arrowPoints = new List<Vector3>();
	void Start()
	{
		startTime = Time.timeSinceLevelLoad;
		myManager.GiveOrder(Orders.CreateMoveOrder(MyPath[currentIndex].transform.position));

		arrowPoints.Add(Vector3.zero);
		foreach (WayPoint p in MyPath)
		{
			arrowPoints.Add(p.transform.position);
		}

		if (showArrows)
		{
			InvokeRepeating("updateArrow", .1f, 4);
		}
	}

	void updateArrow()
	{
		MiniMapUIController.main.drawPath(arrowPoints, 3.3f, .5f);
	}

	private void OnDisable()
	{
		CancelInvoke("updateArrow");
	}

	public override void computeInteractions(Order order)
	{
		switch (order.OrderType)
		{
			
			case Const.ORDER_MOVE_TO:

				myManager.changeState(new MoveState(order.OrderLocation, myManager), false, order.queued);
				break;
		}
	}



	public override UnitState computeState(UnitState s)
	{

		if (s is MoveState)
		{
			if (MyPath.Find(item => item.transform.position == ((MoveState)s).location) == null)
			{
				return new MoveState(MyPath[currentIndex].transform.position, myManager);
			}
		}
		else if (s is DefaultState)// && Time.timeSinceLevelLoad - startTime > 1)
		{
			Invoke("giveOrder", .01f);
		}
		else
		{
			return new MoveState(MyPath[currentIndex].transform.position, myManager);
		}
		return s;
	}

	void giveOrder()
	{
		if (currentIndex +1 >= MyPath.Count)
		{
			return;
		}
		if (MyPath[currentIndex + 1].IsPathable)
		{
			arrowPoints.RemoveAt(0);
			currentIndex++;
		
			myManager.GiveOrder(Orders.CreateMoveOrder(MyPath[currentIndex].transform.position));
		}
		else
		{
			Invoke("giveOrder", 1f);
		}
	}

}
