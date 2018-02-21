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
	void Start()
	{
		StartCoroutine (NightToDay ());
		StartCoroutine (DayShadow());
		timeLeft = normalNightLength;
		InvokeRepeating ("UpdateTime", 1, 1);
	}

	int dayCount = 1;
	float timeLeft;


	void UpdateTime()
	{
		timeLeft--;
		if (inDay) {
			myText.text = "Day: " + dayCount + "\n" + Clock.convertToString (timeLeft);
			
		} else {
			myText.text = "Night: " + dayCount + "\n" + Clock.convertToString (timeLeft);
		}
	}


	IEnumerator DayShadow()
	{
		while(true){
			float duration = normalDayLength + normalNightLength / 2;
			yield return new WaitForSeconds (normalNightLength/ 2);
			for (float i = 0; i < duration; i += Time.deltaTime) {
				dimmerLight.GetComponent<Transform> ().eulerAngles = Vector3.Lerp (DayStartRotation, DayEndRotation, i /duration);
			
			yield return null;
			}	
		}
	}



	IEnumerator NightToDay()
	{
		float duration = normalNightLength / 2;
		yield return new WaitForSeconds (duration);
		for (float i = 0; i < duration; i += Time.deltaTime) {
			
			dimmerLight.color = Color.Lerp (nightColor, dayColor, i / duration);
			dimmerLight.shadowStrength =  Mathf.Clamp((i / duration) + .2f,.3f,1);
			yield return null;
		}	

		StartCoroutine (DayToNight ());
		inDay = true;
		dayCount++;
		timeLeft = normalNightLength;
		myText.GetComponentInParent<UnityEngine.UI.Image> ().color = Color.red;
	}

	IEnumerator DayToNight()
	{	float duration = normalDayLength / 2;
		yield return new WaitForSeconds (duration);
		for (float i = 0; i < duration; i += Time.deltaTime) {
			
			dimmerLight.color = Color.Lerp ( dayColor, nightColor, i / duration);
			dimmerLight.shadowStrength =  Mathf.Clamp((1- i / duration) + .2f,.3f,1);
			yield return null;
		}	
		StartCoroutine (NightToDay());
		inDay = false;
		timeLeft = normalDayLength;
		myText.GetComponentInParent<UnityEngine.UI.Image> ().color = Color.white;

	}

}
