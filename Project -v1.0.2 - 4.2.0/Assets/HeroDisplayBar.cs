using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDisplayBar : DisplayBar
{

	public float changeDuration = .75f;
	public GameObject SplashBar;
	Vector3 SplashVector = new Vector3(1, 1, 1);
	Vector3 TargetVector = new Vector3(1,1,1);
	float endTime;
	public SpriteRenderer splashSprite;

	public float shakeRatio = .25f;
	bool started = false;

	new void Start()
	{
		if (!started)
		{
			base.Start();
			SplashVector.y = colorBar.transform.parent.localScale.y;
			splashSprite.size = new Vector2(BarWidth, splashSprite.size.y);
			GetComponentInChildren<SpriteRenderer>().size = new Vector2(BarWidth, splashSprite.size.y);
			splashSprite.transform.parent.localPosition = new Vector3(BarWidth / 2, 0, 0);
			splashSprite.transform.localPosition = new Vector2(0 - BarWidth / 2, 0);
			started = true;
		}
	}


	public override bool updateRatio(float ratio, UnitIconInfo unitIcon, UnitIconInfo slider)
	{
		Start();

		bool toReturn = base.updateRatio(ratio, unitIcon, slider);
		updateSplashRatio(ratio);
		return toReturn;
	}

	Coroutine shakingUp;

	public void updateSplashRatio(float ratio)
	{
		endTime = Time.time + changeDuration;
		float changeAmount = TargetVector.x - ratio;
		TargetVector.x = ratio;

		if ( gameObject.activeInHierarchy)
		{
			StartCoroutine(changeHealth(changeAmount));
			if (shakingUp == null && SplashBar.transform.localScale.x - TargetVector.x > shakeRatio)
			{
				shakingUp = StartCoroutine(shakeUp(SplashBar.transform.localScale.x - TargetVector.x));
			}
		}
	}

	IEnumerator changeHealth(float changeAmount)
	{
		for (float i = 0; i < changeDuration; i += Time.deltaTime)
		{ 
			SplashVector.x -= changeAmount * Time.deltaTime / changeDuration;
			SplashBar.transform.localScale = SplashVector;
			yield return null;
		}
	}

	IEnumerator shakeUp(float changeAmount)
	{
		float elapsed = 0.0f;
		Vector3 totalMovement = Vector3.zero;

		transform.localScale *= 1 + changeAmount * 2;

		while (elapsed < changeDuration - .2f)
		{
			float MiniShake = 0;
			Vector3 toMove = new Vector3(Random.value - .5f, Random.value - .5f, Random.value - .5f) * 15;

			while (MiniShake < .04f)
			{
				MiniShake += Time.deltaTime;
				transform.Translate(toMove * Time.deltaTime);
				totalMovement += toMove * Time.deltaTime;
				yield return null;
			}
			elapsed += MiniShake;

		}

		float ReturnTime = 0.0f;

		while (ReturnTime < .1f)
		{
			ReturnTime += Time.deltaTime;
			transform.Translate((totalMovement * -1) * Time.deltaTime / .1f);
			transform.localScale -=  Vector3.one * 20 * changeAmount  * Time.deltaTime;
			yield return null;
		}
		transform.localScale = Vector3.one;
		shakingUp = null;
	}
}
