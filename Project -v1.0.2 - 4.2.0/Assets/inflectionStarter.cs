using UnityEngine;
using System.Collections;

public class inflectionStarter: MonoBehaviour, Notify{

	public AbilityCastType myType;
	public enum AbilityCastType
	{
		inflectionBarrier, InversionBarrier
	}
	public GameObject barrier;
	void Start () {
		GetComponent<Projectile> ().triggers.Add (this);
	}





	public float trigger(GameObject source,GameObject proj, UnitManager target, float damage)
	{
		if (myType == AbilityCastType.inflectionBarrier) {
			inflectionBarrier existingShield = target.GetComponentInChildren<inflectionBarrier> ();
			if (existingShield) {
				existingShield.castAgain ();

			} else {
				GameObject obj = (GameObject)Instantiate (barrier, target.transform.position, target.transform.rotation);
				obj.transform.SetParent (target.transform);
				obj.GetComponent<inflectionBarrier> ().setSource (GetComponent<Projectile> ().Source);
				obj.GetComponent<inflectionBarrier> ().initialize (target);
			}
		} else {
		
			InversionBarrier existingShield = target.GetComponentInChildren<InversionBarrier> ();
			if (existingShield) {
				existingShield.Reset ();

			} else {
				
				GameObject obj = (GameObject)Instantiate (barrier, target.transform.position, target.transform.rotation,target.transform);

			}
		}





		return damage;
	}


}
