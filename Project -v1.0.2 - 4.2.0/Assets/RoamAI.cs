using UnityEngine;
using System.Collections;

public class RoamAI : MonoBehaviour {

	Vector3 origin;
	public float roamRange = 22;

	UnitManager myman;

	// Use this for initialization
	void Start () {
		myman = GetComponent<UnitManager> ();
		origin = this.transform.position;
		WorldRecharger.main.addRoam (this);

		//InvokeRepeating ("setnewLocation",Random.Range(2,6), 8);
	}
	
	Vector3 hitzone;
	float radius;
	float angle;

	public void setnewLocation()
	{
		hitzone = origin;
		radius = Random.Range (0, roamRange);
		angle = Random.Range (0, 2 * Mathf.PI);

		hitzone.x += Mathf.Sin (angle) * radius;
		hitzone.z += Mathf.Cos (angle) * radius;

		myman.GiveOrder (Orders.CreateAttackMove (hitzone, false));
	}

	public void Dying()
	{
		WorldRecharger.main.removeRoam (this);
	}
}
