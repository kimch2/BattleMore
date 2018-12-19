using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmogCloud : VisionTrigger
{
	public int MyOwner =1 ;
	float DamageTime;
	float angleChangeAmount;
	float timeChange;

	private void Start()
	{
		DamageTime = Time.time;
	}



	private void Update()
	{

		RaycastHit objecthit;
	
		if (Physics.Raycast(this.gameObject.transform.position, Vector3.down, out objecthit, 100, 1 << 8))
		{
			transform.position = objecthit.point + Vector3.up;
		}
		transform.Translate(transform.forward * 7 * Time.deltaTime);
		transform.Rotate(Vector3.up, angleChangeAmount * Time.deltaTime);
		if (Time.time > timeChange)
		{
			timeChange = Time.time + 4;
			angleChangeAmount = UnityEngine.Random.Range(-15, 15);
		}

		if (Time.time > DamageTime)
		{
			//Debug.Log("Damaing " + Time.time);
			DamageTime += .5f;
			foreach (UnitManager manage in InVision)
			{
				if (manage && MyOwner != manage.PlayerOwner)
				{
					manage.myStats.TakeDamage(2 +( transform.localScale.x * 2), this.gameObject, DamageTypes.DamageType.Regular);
				}
			}

		}

		transform.localScale -= new Vector3(.075f, .075f, .075f) * Time.deltaTime;
		if (transform.localScale.x < 0)
		{
			Destroy(this.gameObject);
		}
	}



	public override void UnitEnterTrigger(UnitManager manager)
	{
		
	}

	public override void UnitExitTrigger(UnitManager manager)
	{
		if (manager == null)
		{
			StartCoroutine(Grow());
		}
	}


	IEnumerator Grow()
	{
		for (float f = 0; f < 2f; f += Time.deltaTime)
		{
			transform.localScale += new Vector3(.3f, .3f, .3f) * Time.deltaTime;
			yield return null;
		}

	}
}
