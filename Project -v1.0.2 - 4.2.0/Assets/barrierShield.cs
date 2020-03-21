using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class barrierShield : MonoBehaviour {


	public float Health;
	public float duration;
	public float TotalHealth;

	public GameObject Effect;
	public float DecayRate;

	float TotalAbsorbed;

	private float radius;
	public Slider cooldownSlider;
	// Use this for initialization
	void Start () {
		TotalHealth = Health;
		DecayRate = (Health / duration) / 10;
		radius = GetComponent<SphereCollider> ().radius;
		StartCoroutine (RunTime (duration));

	}
		

	IEnumerator RunTime(float dur)
	{
		GetComponent<Collider> ().enabled = false;
		yield return new WaitForSeconds (1);
		GetComponent<Collider> ().enabled = true;

		while (Health > 0) {
			yield return new WaitForSeconds (.1f);
			Health -= DecayRate; 
			cooldownSlider.value = Health / TotalHealth;

		}
		cooldownSlider.gameObject.SetActive (false);
		GetComponent<Animator> ().SetInteger ("State", 1);
		yield return new WaitForSeconds (1.1f);
		GetComponent<Collider> ().enabled = false;
		yield return new WaitForSeconds (1.9f);

		PlayerPrefs.SetInt ("TotalBarrierBlocked", PlayerPrefs.GetInt("TotalBarrierBlocked") +  (int)TotalAbsorbed);
		Destroy (this.gameObject);
	}



	void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.layer == 15) {
			if (other.isTrigger) {
				return;
			}
		
			Projectile proj = other.GetComponent<Projectile> ();

			if (proj.MyHitContainer.playerNumber != 1) {
				if (proj.Source) {
					float dist = Vector3.Distance ( this.gameObject.transform.position,proj.Source.transform.position);
					if (dist > radius ) {
						AbsorbShot (proj);
					}
				}
				else
				{					
					AbsorbShot (proj);
				}
			}
		}
	}


	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer != 15) {
			return;
		}
		if (other.isTrigger) {
			return;
		}

		Projectile proj = other.GetComponent<Projectile> ();

		if (proj.MyHitContainer.playerNumber != 1) {
				
			if (proj.Source) {
				float dist = Vector3.Distance ( this.gameObject.transform.position,proj.Source.transform.position);
				if (dist < radius ) {
					AbsorbShot (proj);
				}
			}
			else
			{					
				AbsorbShot (proj);
			}			
		}
	}


	void AbsorbShot(Projectile proj)
	{
		Health -= proj.damage;
		TotalAbsorbed += proj.damage;
		Vector3 location = gameObject.transform.position + (proj.gameObject.transform.position - gameObject.transform.position).normalized * radius;
		Instantiate (Effect,   location, proj.gameObject.transform.rotation);
		proj.selfDestruct ();


		if (Health <= 0) {
			StartCoroutine (RunTime (0));
		}
	}


	public void takeDamage(float amount)
	{
		Health -= amount;
		TotalAbsorbed += amount;

		if (Health <= 0) {
			StartCoroutine (RunTime (0));
		}
	}


}
