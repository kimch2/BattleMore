using UnityEngine;
using System.Collections;

public class selfDestructTimer : MonoBehaviour {
	public float timer;
	public bool showTimer;
	private float deathTime;

	float originalTime;

	private Selected hd;
	// Use this for initialization
	void Start () {

		
		if (showTimer) {
			StartCoroutine (checkForDeathWithUI ());


		} else {
			StartCoroutine (checkForDeath());
		}
		deathTime = Time.time + timer;



	}

	IEnumerator checkForDeath()
	{
		yield return new WaitForSeconds (timer);
		//while (Time.time < deathTime) {
			//yield return new WaitForSeconds (.07f);
		//}

		if (GetComponent<UnitStats> ()) {
			GetComponent<UnitStats> ().kill (null);
		} else {

			Destroy (this.gameObject);}

	}

	public void modifyRemainingByPercent(float perc)
	{
		Debug.Log("before " + deathTime);
		deathTime += (deathTime - Time.time) * perc;
		Debug.Log("after " + deathTime);
	}

	IEnumerator checkForDeathWithUI()
	{
		hd = GetComponent<Selected> ();
		yield return null;
		while (Time.time < deathTime) {
			if (hd) {
				hd.updateCoolDown ((deathTime - Time.time) / timer);
			}
			yield return new WaitForSeconds (.05f);

		}

		if (hd) {
			hd.updateCoolDown (1);
		}
		if (GetComponent<UnitStats> ()) {
			GetComponent<UnitStats> ().kill (null);
		} else {

			Destroy (this.gameObject);}
	}


}