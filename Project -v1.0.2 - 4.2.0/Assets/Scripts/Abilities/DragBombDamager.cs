﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBombDamager : VisionTrigger
{
	public float velocityDamageMultiplier = 1.5f;
	public float explodeDamage;
	public GameObject explosionO;
	public UnitManager Source;
	public VeteranStats vetSource;
	public float chainLength = 60;

	public LineRenderer lineRender;
	Vector3[] LinePoints = new Vector3[2];
	Rigidbody RBody;

	public Material defaultMaterial;
	public Material highlightMaterial;
	public MeshRenderer myRenderer;

	public float DragDuration = 4;
	public AudioSource myAudio;
	public AudioSource secondaryAudio;

	public AudioClip TimerSound;


	//Deal damage equal to speed of bombs

	private void Awake()
	{
		detachTime = Time.time + DragDuration;
		ExplodeTime = Time.time + DragDuration + 3 + UnityEngine.Random.value;
		StartCoroutine(Drag());
	}

	private void Update()
	{
		myAudio.volume = RBody.velocity.magnitude / 40;
	}

	public void setSource(GameObject source)
	{
		RBody = GetComponent<Rigidbody>();
		Source = source.GetComponent<UnitManager>();

		vetSource = Source.myStats.veternStat;
		PlayerNumber = (Source.PlayerOwner == 1 ? 2 : 1);


	}

	float detachTime;
	float ExplodeTime;

	IEnumerator Drag()
	{
		yield return null;
		for (float i = 0; i < 1f; i += Time.deltaTime)
		{

			if (Source)
			{
				Vector3 force = (Source.transform.position - transform.position) * Time.deltaTime * 120;
				force.y = -1;
				RBody.AddForce(force);
			}
			else
			{
				break;
			}
			SetChain();
			yield return null;
		}

		for (float i = 0; i < DragDuration - 2f; i += Time.deltaTime)
		{
			if (Source)
			{
				pullChain();
			}
			else
			{
				break;
			}
			yield return null;
		}
		secondaryAudio.PlayOneShot(TimerSound);
		myRenderer.material = highlightMaterial;
		for (float i = 0; i < 2f; i += Time.deltaTime)
		{
			if (Source)
			{
				pullChain();
			}
			else
			{
				break;
			}
			yield return null;
		}
		lineRender.enabled = false;

		myRenderer.material = defaultMaterial;
		secondaryAudio.PlayOneShot(TimerSound);
		yield return new WaitForSeconds(1f + UnityEngine.Random.value);
		myRenderer.material = highlightMaterial;
		secondaryAudio.PlayOneShot(TimerSound);
		yield return new WaitForSeconds(.4f);
		myRenderer.material = defaultMaterial;
		secondaryAudio.PlayOneShot(TimerSound);
		yield return new WaitForSeconds(.3f );
		myRenderer.material = highlightMaterial;
		secondaryAudio.PlayOneShot(TimerSound);
		yield return new WaitForSeconds(.3f);
		myRenderer.material = defaultMaterial;
		secondaryAudio.PlayOneShot(TimerSound);
		yield return new WaitForSeconds(.1f );
		myRenderer.material = highlightMaterial;
		secondaryAudio.PlayOneShot(TimerSound);
		yield return new WaitForSeconds(.1f);
		myRenderer.material = defaultMaterial;
		secondaryAudio.PlayOneShot(TimerSound);
		yield return new WaitForSeconds(.1f );


		Explode();
		
	}

	public void SecondClick()
	{
		Source = null;
	}

	void pullChain()
	{
		if (Vector3.Distance(transform.position, Source.transform.position) > chainLength)
		{
			transform.position = Source.transform.position - (Source.transform.position - transform.position).normalized * chainLength;
		}
		if (Vector3.Distance(transform.position, Source.transform.position) > chainLength / 2)
		{
			Vector3 force = (Source.transform.position - transform.position) * 400 * Time.deltaTime;
			force.y *= .3f;
			RBody.AddForce(force);
		}
		SetChain();
	}

	void SetChain()
	{
		LinePoints[0] = transform.position;

		if (Source)
		{
			LinePoints[1] = Source.transform.position;
		}

		lineRender.SetPositions(LinePoints);
	}
	

	void Explode()
	{
		GameObject explode = (GameObject)Instantiate(explosionO, transform.position, Quaternion.identity);

		explosion Escript = explode.GetComponent<explosion>();
		if (Escript)
		{
			if (Source)
			{
				Escript.setSource(Source.gameObject);
			}
			Escript.damageAmount = explodeDamage;
			Escript.setVeteran(vetSource);
		}
		else
		{
			explode.SendMessage("setVeteran", vetSource, SendMessageOptions.DontRequireReceiver);
		}
		Destroy(this.gameObject);
	}


	public override void UnitEnterTrigger(UnitManager manager)
	{
		if (manager.myStats.isUnitType(UnitTypes.UnitTypeTag.Structure) || !Source)
		{
			Explode();
		}
		else
		{
			manager.myStats.TakeDamage(RBody.velocity.magnitude * velocityDamageMultiplier, Source != null ? Source.gameObject : null, DamageTypes.DamageType.Regular, Source);
		}
	}

	public override void UnitExitTrigger(UnitManager manager)
	{
	
	}

}
