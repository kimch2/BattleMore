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
	
		if (Physics.Raycast(this.gameObject.transform.position, Vector3.down, out objecthit, 100, 1 << 8 | (1 << 16)))
		{
			transform.position = objecthit.point + Vector3.up;
		}
		transform.Translate(transform.forward * 5 * Time.deltaTime);
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
					manage.myStats.TakeDamage(4 +( transform.localScale.x * 2), this.gameObject, DamageTypes.DamageType.Regular);
				}
			}

		}
		float m = Mathf.Max(.07f, transform.localScale.x/80);
		transform.localScale -= new Vector3(m,m,m) * Time.deltaTime;
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
			transform.localScale += new Vector3(.25f, .25f, .25f) * Time.deltaTime;
			yield return null;
		}

	}
}
