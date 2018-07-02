using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealArea : MonoBehaviour {
	public int player = 1;
	public float HealRateHalfSecond = 25;


	public MultiShotParticle particleEff;
	void OnTriggerEnter(Collider other)
	{

			if (other.isTrigger) {
				return;
			}

			if (other.GetComponent<UnitManager> ()) {
				if (other.GetComponent<UnitManager> ().PlayerOwner == player) {

				StartCoroutine (HealUp(other.GetComponent<UnitManager>()));

				}
			}

	}

	IEnumerator HealUp(UnitManager manage)
	{

		while(manage && Vector3.Distance(transform.position, manage.transform.position) < 17)
		{
			if (!manage.getUnitStats ().atFullHealth ()) {
				particleEff.playEffect ();
			}
			manage.getUnitStats ().heal (HealRateHalfSecond, DamageTypes.DamageType.Regular);

			yield return new WaitForSeconds (.5f);

		}


	}
}
