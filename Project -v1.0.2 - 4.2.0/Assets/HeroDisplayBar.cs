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

    public float HPperLine = 50;
    public LineRenderer HPIncrements;
    public GameObject ShieldBar;
    Vector3 ShieldVector = new Vector3(1, 1, 1);
    UnitStats myStats;
    float currentShieldAmount = 0;

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
            myStats = GetComponentInParent<UnitStats>();
            SetHealthIncrements(myStats.Maxhealth);
		}
	}


    void SetHealthIncrements(float HealthAmount)
    {
        float LineCount = HealthAmount / HPperLine;
        HPIncrements.SetPositions(new Vector3[2] { new Vector3(-1 * LineCount / 2, 0, 0), new Vector3(LineCount / 2, 0, 0) });
        HPIncrements.transform.localScale = new Vector3(BarWidth / LineCount, 1, 1);
    }

	public override bool updateRatio(float ratio, UnitIconInfo unitIcon, UnitIconInfo slider)
	{
		Start();

		bool toReturn = base.updateRatio(ratio, unitIcon, slider);
		updateSplashRatio(ratio);


        if (currentShieldAmount > 0)
        {
            toReturn = true;
            SetShieldAmount(currentShieldAmount);
        }
		return toReturn;
	}

	Coroutine shakingUp;
    Coroutine loweringBar;
    float ReductionPoint;

	public void updateSplashRatio(float ratio)
	{
		//endTime = Time.time + changeDuration;
		TargetVector.x = ratio;

		if ( gameObject.activeInHierarchy)
		{
            if (loweringBar == null)
            {
                loweringBar = StartCoroutine(changeHealth());
            }
			if (shakingUp == null && SplashBar.transform.localScale.x - TargetVector.x > shakeRatio)
			{
				shakingUp = StartCoroutine(shakeUp(SplashBar.transform.localScale.x - TargetVector.x));
			}
		}
	}

	IEnumerator changeHealth()
	{
        yield return new WaitForSeconds(.5f);
        if (SplashVector.x < TargetVector.x)
        {
            SplashVector.x = TargetVector.x;
        }
        
        while (SplashVector.x > TargetVector.x)
        {
            SplashVector.x -= changeDuration * Time.deltaTime ;
            SplashBar.transform.localScale = SplashVector;
            yield return null;
        }

        loweringBar = null;
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

    public override void SetShieldAmount(float amount) // Need a way to keep track of health that is subtracted while you still have a shield
    {
        currentShieldAmount = amount;
        if (amount > 0)
        {
            gameObject.SetActive(true);
            ShieldBar.SetActive(true);
        }
        else
        {
            ShieldBar.SetActive(false);
        }


        float totalMaxHP = Mathf.Max ( myStats.Maxhealth, amount + myStats.health);
        float totalCurrentHP = amount + myStats.health;
        if (totalCurrentHP < myStats.Maxhealth)
        {
            ShieldVector.x = totalCurrentHP / myStats.Maxhealth;
            ShieldBar.transform.localScale = ShieldVector;
        }
        else
        {
          
            ShieldVector.x = 1;
            ShieldBar.transform.localScale = ShieldVector;          
        }

        base.updateRatio(myStats.health / totalMaxHP, null, null);
        SetHealthIncrements(totalMaxHP);
    }
}
