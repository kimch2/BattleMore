using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Formations{


	public static List<Vector3> getFormation(int count, float sepDist)
	{
		List<Vector3> toReturn = new List<Vector3> ();

		if (count == 1) {
			return Odd.GetRange (0, 1);
		} else if (count == 2) {

			for (int n = 0; n < 2; n++) {
				toReturn.Add (Even [n]);//* sepDist);
			}

		}
		// Odd number of guys
		else {
			//Only two rows
			if (count < 9) {
				
				int remainder = count / 2;
				for (int n = 0; n < remainder; n++) {
					if (remainder % 2 == 1) {
						toReturn.Add ((Odd [n] - Vector3.forward * 7.5f));
					} else {
						toReturn.Add ((Even [n] - Vector3.forward * 7.5f));
					}
				}

				int RemainB = (count - remainder);
				for (int n = 0; n < RemainB; n++) {
					if (RemainB % 2 == 1) {
						toReturn.Add ((Odd [n] + Vector3.forward * 7.5f));
					} else {
						toReturn.Add ((Even [n] + Vector3.forward * 7.5f));
					}
				}
				//Three rows
			} else {

				int remainder = count / 3;
				for (int n = 0; n < remainder; n++) {
					if (remainder % 2 == 1) {
						toReturn.Add ((Odd [n] - Vector3.forward *15));
					} else {
						toReturn.Add ((Even [n] - Vector3.forward * 15));
					}
				}

				int RemainB = (count - remainder) / 2;
				for (int n = 0; n < RemainB; n++) {
					if (RemainB % 2 == 1) {
						toReturn.Add ((Odd [n] ));// * sepDist);
					} else {
						toReturn.Add ((Even [n]));// * sepDist) );
					}
				}

				int RemainC= (count  - RemainB - remainder);
				for (int n = 0; n < RemainC; n++) {
					if (RemainC % 2 == 1) {
						toReturn.Add((Odd[n] + Vector3.forward * 15));
					} else {
						toReturn.Add ((Even [n] + Vector3.forward * 15));
					}
				}
			}
		}

		Vector3 point = new Vector3(1,1,1);

		for (int i =0; i < toReturn.Count; i ++)
		{
			point = toReturn[i];
			point.x *= sepDist;
			toReturn[i] = point;
		}

		return toReturn;
	}

	private static List<Vector3> Even = new List<Vector3> () {	
		new Vector3(-7.5f, 0, 0),
		new Vector3 (7.5f, 0, 0), 
		new Vector3(-22.5f,0,0),
		new Vector3 (22.5f, 0, 0), 
		new Vector3 (-37.5f, 0, 0), 
		new Vector3 (37.5f, 0, 0), 
		new Vector3 (-52.5f, 0, 0), 
		new Vector3 (52.5f, 0, 0), 
		new Vector3 (-67.5f, 0, 0), 
		new Vector3 (67.5f, 0, 0), 
		new Vector3 (-82.5f, 0, 0), 
		new Vector3 (82.5f, 0, 0), 
		new Vector3 (-96.5f, 0, 0), 
		new Vector3 (96.5f, 0, 0), 
		new Vector3 (-111.5f, 0, 0), 
		new Vector3 (111.5f, 0, 0), 
		new Vector3 (-127.5f, 0, 0), 
		new Vector3 (127.5f, 0, 0),
		new Vector3 (-142.5f, 0, 0), 
		new Vector3 (142.5f, 0, 0),
		new Vector3 (-157.5f, 0, 0), 
		new Vector3 (157.5f, 0, 0),
		new Vector3 (-172.5f, 0, 0), 
		new Vector3 (172.5f, 0, 0)

		};

	private static List<Vector3> Odd = new List<Vector3> () {	
		new Vector3(0, 0, 0),
		new Vector3 (15, 0, 0), 
		new Vector3(-15,0,0),
		new Vector3 (30, 0, 0), 
		new Vector3 (-30, 0, 0), 
		new Vector3 (45, 0, 0), 
		new Vector3 (-45, 0, 0), 
		new Vector3 (60, 0, 0), 
		new Vector3 (-60, 0, 0), 
		new Vector3 (75, 0, 0), 
		new Vector3 (-75, 0, 0), 
		new Vector3 (90, 0, 0), 
		new Vector3 (-90, 0, 0), 
		new Vector3 (105, 0, 0), 
		new Vector3 (-105, 0, 0), 
		new Vector3 (120, 0, 0), 
		new Vector3 (-120, 0, 0), 
		new Vector3 (135, 0, 0),
		new Vector3 (-135, 0, 0), 
		new Vector3 (150, 0, 0),
		new Vector3 (-150, 0, 0), 
		new Vector3 (165, 0, 0),
		new Vector3 (-165, 0, 0), 
		new Vector3 (180, 0, 0)

	};


	public static void assignMoveCOmmand(List<UnitManager> SelectedObjects, Vector3 targetPoint, Vector3 secondPoint, bool attack, float sepDistance)
	{
		List<RTSObject> newList = new List<RTSObject>();
		foreach (UnitManager man in SelectedObjects)
		{
			newList.Add(man);
		}

		assignMoveCOmmand(newList, targetPoint, secondPoint, attack, sepDistance);	
	}


	public static void assignMoveCOmmand(List<RTSObject> SelectedObjects, Vector3 targetPoint, Vector3 secondPoint, bool attack, float sepDistance)
	{
		Dictionary<int, List<RTSObject>> trueMovers = new Dictionary<int, List<RTSObject>>();
		List<RTSObject> others = new List<RTSObject>();

		int MoverCount = 0;
		Vector3 middlePoint = Vector3.zero;
		foreach (RTSObject obj in SelectedObjects)
		{
			if (!obj.getUnitManager().cMover)
			{
				others.Add(obj);
			}
			else
			{
				MoverCount++;
				if (trueMovers.ContainsKey(obj.getUnitManager().formationOrder))
				{
					trueMovers[obj.getUnitManager().formationOrder].Add(obj);
				}
				else
				{
					trueMovers.Add(obj.getUnitManager().formationOrder, new List<RTSObject>() { obj });
				}
				middlePoint += obj.transform.position;
			}
		}


		//give move command to all nonmovers, setting rally points and such
		SelectedManager.main.StartCoroutine(staticMove(others, targetPoint));

		middlePoint /= MoverCount;
		
		float angle;

		if (sepDistance == 1)
		{
			angle = (float)(Mathf.Atan2(middlePoint.x - targetPoint.x, middlePoint.z - targetPoint.z) / Mathf.PI) * 180;
		}
		else
		{
			angle = (float)(Mathf.Atan2(secondPoint.x - targetPoint.x, secondPoint.z - targetPoint.z) / Mathf.PI) * 180 + 90;
			targetPoint = Vector3.Lerp(targetPoint, secondPoint, .5f);
		}


		float sepDistanceD = Vector3.Distance(targetPoint, secondPoint) / 15;
		sepDistance = Mathf.Max(sepDistance, 1.2f);
		List<Vector3> points = Formations.getFormation(MoverCount, Mathf.Min(5f, sepDistance));
		for (int t = 0; t < points.Count; t++)
		{
			points[t] = Quaternion.Euler(0, angle, 0) * points[t] + targetPoint;
		}


		int minPointIndex = 0;
		int maxPointIndex = 0;
		foreach (KeyValuePair<int, List<RTSObject>> kvp in trueMovers.OrderBy(i => i.Key))
		{

			maxPointIndex = minPointIndex + kvp.Value.Count;
			List<IOrderable> usedGuys = new List<IOrderable>();
			while (usedGuys.Count < kvp.Value.Count)
			{
				float maxDistance = 0;
				IOrderable closestUnit = null;
				foreach (IOrderable obj in kvp.Value)
				{
					float tempDist = Vector3.Distance(obj.getObject().transform.position, targetPoint);
					if (!usedGuys.Contains(obj) && tempDist > maxDistance)
					{
						maxDistance = tempDist;
						closestUnit = obj;
					}
				}
				usedGuys.Add(closestUnit);

				float runDistance = 1000000;
				Vector3 closestSpot = points[0];

				for (int i = minPointIndex; i < maxPointIndex; i++)
				{

					float tempDist = Vector3.Distance(closestUnit.getObject().transform.position, points[i]);
					if (tempDist < runDistance)
					{
						closestSpot = points[i];
						runDistance = tempDist;


					}
				}

				SelectedManager.main.StartCoroutine(giveCommand(attack, closestSpot, closestUnit));
				maxPointIndex--;
				points.Remove(closestSpot);


			}

			minPointIndex = maxPointIndex;

		}
	}


    public static void SetObjectPositions(List<GameObject> SelectedObjects, Vector3 targetPoint, Vector3 secondPoint, GameObject template)
    { // For now this is only being called from the UIManager for formation light indicators
        Vector3 superUp = Vector3.up * 40;

        List<RTSObject> trueMovers = new List<RTSObject>();

        Vector3 middlePoint = Vector3.zero;
        foreach (RTSObject obj in SelectedManager.main.ActiveObjectList())
        {
            if (obj.getUnitManager().cMover)
            {
                trueMovers.Add(obj);
                middlePoint += obj.transform.position;
            }
        }

        middlePoint /= trueMovers.Count;

        float angle = (float)(Mathf.Atan2(secondPoint.x - targetPoint.x, secondPoint.z - targetPoint.z) / Mathf.PI) * 180;

        if (angle < 0)
        {
            angle += 360;
        }
        angle += 90;
        Vector3 newPoint = Vector3.Lerp(targetPoint, secondPoint, .5f);

        float sepDistance = Vector3.Distance(newPoint, secondPoint) / 15;
        sepDistance = Math.Max(sepDistance, 1.2f);
        List<Vector3> points = Formations.getFormation(trueMovers.Count, Mathf.Min(5f, sepDistance));
        for (int t = 0; t < points.Count; t++)
        {
            points[t] = Quaternion.Euler(0, angle, 0) * points[t] + newPoint;
        }



        int x = SelectedObjects.Count;
        for (int n = x; n < trueMovers.Count; n++)
        {
            SelectedObjects.Add(GameObject.Instantiate<GameObject>(template));
        }

        for (int i = 0; i < SelectedObjects.Count; i++)
        {
            if (i < trueMovers.Count)
            {
                SelectedObjects[i].SetActive(true);
                SelectedObjects[i].transform.position = points[i] + superUp;
                SelectedObjects[i].transform.localEulerAngles = new Vector3(0, angle, 0);
            }
            else
            {
                SelectedObjects[i].SetActive(false);
            }
        }
    }

	static IEnumerator staticMove(List<RTSObject> others, Vector3 targetPoint)
	{
		yield return null;
		foreach (IOrderable io in others)
		{
			io.GiveOrder(Orders.CreateMoveOrder(targetPoint, Input.GetKey(KeyCode.LeftShift)));
		}

	}

	static IEnumerator giveCommand(bool attack, Vector3 closestSpot, IOrderable unittoMove)
	{
		yield return new WaitForSeconds(0.001f);
		if (attack)
		{
			Order o = Orders.CreateAttackMove(closestSpot, Input.GetKey(KeyCode.LeftShift));

			unittoMove.GiveOrder(o);
		}
		else
		{
			Order o = Orders.CreateMoveOrder(closestSpot, Input.GetKey(KeyCode.LeftShift));
			unittoMove.GiveOrder(o);

		}

	}

}
