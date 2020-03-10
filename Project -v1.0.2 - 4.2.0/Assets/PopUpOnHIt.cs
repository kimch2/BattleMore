using UnityEngine;
using System.Collections;

public class PopUpOnHIt : MonoBehaviour, Notify {

	public string toShow;
	public Color myColor = Color.red;

	public float trigger(GameObject source,GameObject proj,UnitManager target, float damage)
	{	
		PopUpMaker.CreateGlobalPopUp (toShow, myColor, target.transform.position);
		return damage;
	}

}
