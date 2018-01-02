using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapLineCreator : MonoBehaviour {


	public List<MinimapPath> myPaths;

	public void showPath(int index)
	{
		MiniMapUIController.main.drawPath (myPaths [index].points, myPaths [index].duration);
	}


	[System.Serializable]
	public class MinimapPath
	{public string title;
		public List<Vector3> points;
		public float duration;

	}


	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		foreach (MinimapPath pat in myPaths) {
			for (int i = 0; i < pat.points.Count; i++) {
				if (i != 0) {
					Gizmos.DrawLine (pat.points [i - 1], pat.points [i]);
				}
				Gizmos.DrawSphere (pat.points[i],5);
			}
		}

	}
}


