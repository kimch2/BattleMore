using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPulse : MonoBehaviour {

	public float amplitude = 1.6f;
	public float rate = .5f;

	float startScale;
	float MaxScale;
	Vector3 growRate;

	[Tooltip("How long before the amplitude is cut in half, 0 if it never should")]
	public float rescaleDelay = 12;

	void Start()
	{
		startScale = transform.localScale.x;
		MaxScale = startScale * amplitude;
		growRate = (MaxScale - startScale) * rate *Vector3.one;
		StartCoroutine (pulse());

		if (rescaleDelay > 0) {
			Invoke ("HalfScale", rescaleDelay);
		}
	}

	void HalfScale()
	{
		MaxScale =  startScale   + (MaxScale - startScale)*.4f;
	}


	IEnumerator pulse()
	{
		while (this.enabled) {
			while (transform.localScale.x < MaxScale) {
				transform.localScale = transform.localScale + growRate * Time.deltaTime;
				yield return null;
			}
			while (transform.localScale.x > startScale) {
				transform.localScale = transform.localScale - growRate * Time.deltaTime;
				yield return null;
			}
		}
	}

}
