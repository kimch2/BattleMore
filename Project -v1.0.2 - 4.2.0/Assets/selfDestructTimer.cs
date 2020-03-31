using UnityEngine;
using System.Collections;

public class selfDestructTimer : MonoBehaviour
{

    public float timer;
	public bool showTimer;
	private float deathTime;

	private Selected hd;
	// Use this for initialization
	void Start () {
        deathTime = Time.time + timer;

        if (showTimer) {
			StartCoroutine (checkForDeathWithUI ());


		} else {
			StartCoroutine (checkForDeath());
		}
    }

	IEnumerator checkForDeath()
	{

	while (Time.time < deathTime) {
			yield return null;
		}

		if (GetComponent<UnitStats> ()) {
			GetComponent<UnitStats> ().kill (null);
		} else {

			Destroy (this.gameObject);}

	}

    public void AddTime(float seconds)
    {
        deathTime += seconds;
    }

	public void modifyRemainingByPercent(float perc)
	{
		deathTime += (deathTime - Time.time) * perc;
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