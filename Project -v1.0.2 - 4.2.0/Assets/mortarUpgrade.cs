using UnityEngine;
using System.Collections;

public class mortarUpgrade : Upgrade {

	public int podIncrease = 2;
	public float reloadDecrease = .5f;

	override
	public void applyUpgrade(GameObject obj)
	{

		UnitManager manager = obj.GetComponent<UnitManager>();
		mortarPod pod = obj.GetComponent<mortarPod> ();

		if (pod && manager)
		{
			Debug.Log ("Before Mortar " + pod.totalShots + "   "+ pod.shotCount);
			
			pod.totalShots += podIncrease;
			pod.shotCount += podIncrease;

			Debug.Log ("Mortar " + pod.totalShots + "   "+ pod.shotCount);
			pod.updateUI ();
			pod.reloadRate -= reloadDecrease;

		}
	}

	public override void unApplyUpgrade (GameObject obj){

	}
}
