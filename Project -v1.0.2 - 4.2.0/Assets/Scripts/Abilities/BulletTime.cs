using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletTime : MonoBehaviour {


	public float duration;

	public GameObject Effect;
	public List<Projectile> bulletList;
	public List<Projectile> stoppedProjectiles;
	public float slowdownSpeed = 500;

	private float radius;
	public Slider cooldownSlider;
	// Use this for initialization
	void Start()
	{
		radius = GetComponent<SphereCollider>().radius;
		StartCoroutine(RunTime(duration));

	}


	IEnumerator RunTime(float dur)
	{
		GetComponent<Collider>().enabled = false;
		yield return new WaitForSeconds(1);
		GetComponent<Collider>().enabled = true;

		for (float f = 0; f < duration; f += Time.deltaTime)
		{
			foreach (Projectile p in bulletList)
			{
				if (p.speed >0)
				{
					//Debug.Log("Slowing" + (Time.deltaTime * p.speed * 2));
					p.speed -= Time.smoothDeltaTime * slowdownSpeed;
					if (p.speed < 5)
					{
						p.speed = 0;

					}
				}
			}
			yield return null;
		}

		cooldownSlider.gameObject.SetActive(false);
		GetComponent<Animator>().SetInteger("State", 1);
		yield return new WaitForSeconds(1.1f);

		foreach (Projectile proj in bulletList)
		{
			proj.speed *= 10f;
		}

		GetComponent<Collider>().enabled = false;
		yield return new WaitForSeconds(1.9f);

		Destroy(this.gameObject);
	}



	void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.layer == 15)
		{
			if (other.isTrigger)
			{
				return;
			}

			Projectile proj = other.GetComponent<Projectile>();

			if (proj.MyHitContainer.playerNumber != 1)
			{

				if (!bulletList.Contains(proj))
				{
					proj.transform.position = transform.position +  (proj.transform.position - transform.position).normalized * radius;
					bulletList.Add(proj);
					proj.speed *= .6f;
					proj.target = null;

				}
			}
		}
	}

}
