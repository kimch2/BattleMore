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
	void Start()
	{
		StartCoroutine (NightToDay ());
		StartCoroutine (DayShadow());
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
			dimmerLight.shadowStrength = (i / duration) + .2f;
			yield return null;
		}	
		StartCoroutine (DayToNight ());
	}

	IEnumerator DayToNight()
	{	float duration = normalDayLength / 2;
		yield return new WaitForSeconds (duration);
		for (float i = 0; i < duration; i += Time.deltaTime) {
			
			dimmerLight.color = Color.Lerp ( dayColor, nightColor, i / duration);
			dimmerLight.shadowStrength = (1- i / duration) + .2f;
			yield return null;
		}	
		StartCoroutine (NightToDay());
	}

}
