using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour {

	public float normalNightLength = 240;
	public float normalDayLength = 180;

	public Light dimmerLight;
	public Color dayColor = Color.yellow;
	public Color nightColor = Color.blue;

	public Vector3 DayStartRotation;
	public Vector3 DayEndRotation;
	bool inDay = false;
	public UnityEngine.UI.Text myText;
	public AudioSource mySrc;
	public AudioClip NightSound;
	public AudioClip MorningSound;

	public UnityEngine.Events.UnityEvent OnDay;
	public UnityEngine.Events.UnityEvent OnNight;
	void Start()
	{
		StartCoroutine (NightToDay ());
		StartCoroutine (DayShadow());
		timeLeft = normalNightLength;
		InvokeRepeating ("UpdateTime", 1, 1);
	}

	int dayCount = 1;
	float timeLeft;

	Coroutine currentDayShift = null;

	void UpdateTime()
	{
		timeLeft--;
		if (timeLeft < 1) {
			inDay = !inDay;

			if (currentDayShift != null) {
				StopCoroutine (currentDayShift);
			}

			if (inDay) {
				dayCount++;
				timeLeft = normalDayLength;
				myText.GetComponentInParent<UnityEngine.UI.Image> ().color = Color.red;
				if (MorningSound) {
					mySrc.PlayOneShot (MorningSound);}
				StartCoroutine (DayToNight ());
			} else {

				if (NightSound) {
					mySrc.PlayOneShot (NightSound);}
				timeLeft = normalNightLength;
				myText.GetComponentInParent<UnityEngine.UI.Image> ().color = Color.white;
				StartCoroutine (NightToDay());
			}
		}
		if (inDay) {
			myText.text = "Day: " + dayCount + "\n" + Clock.convertToString (timeLeft);
			
		} else {
			myText.text = "Night: " + dayCount + "\n" + Clock.convertToString (timeLeft);
		}
	}


	IEnumerator DayShadow()
	{
		float duration = normalDayLength + normalNightLength / 2;
		while(true){

			yield return new WaitForSeconds (normalNightLength/ 2);
			for (float i = 0; i < duration; i += Time.deltaTime) {
				dimmerLight.GetComponent<Transform> ().eulerAngles = Vector3.Lerp (DayStartRotation, DayEndRotation, i /duration);
			
			yield return null;
			}	
		}
	}



	IEnumerator NightToDay()
	{
		float duration = normalNightLength / 4;
		yield return new WaitForSeconds (duration*3);
		for (float i = 0; i < duration; i += Time.deltaTime) {
			
			dimmerLight.color = Color.Lerp (nightColor, dayColor, i / duration);
			dimmerLight.shadowStrength =  Mathf.Clamp((i / duration) + .2f,.3f,1);
			yield return null;
		}	
			

	}

	IEnumerator DayToNight()
	{	float duration = normalDayLength /4;
		yield return new WaitForSeconds (duration*3);
		for (float i = 0; i < duration; i += Time.deltaTime) {
			
			dimmerLight.color = Color.Lerp ( dayColor, nightColor, i / duration);
			dimmerLight.shadowStrength =  Mathf.Clamp((1- i / duration) + .2f,.3f,1);
			yield return null;
		}	



	}

}
