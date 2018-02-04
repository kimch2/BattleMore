using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasFader : MonoBehaviour {

	public float fadeDuration;

	public void FadeOut()
	{
		StartCoroutine (fade());
	}
	public void FadeIn()
	{
		StartCoroutine (fadeIn ());
	}

	IEnumerator fade()
	{
		for (float i = 0; i < fadeDuration; i += Time.deltaTime) {
			yield return null;
			GetComponent<CanvasGroup> ().alpha = 1 - i;
		}

	}

	IEnumerator fadeIn()
	{
		for (float i = 0; i < fadeDuration; i += Time.deltaTime) {
			yield return null;
			GetComponent<CanvasGroup> ().alpha =  i;
		}

	}

}
