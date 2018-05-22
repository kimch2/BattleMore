using UnityEngine;
using System.Collections;

public class LandMineActivate : VisionTrigger {

	public IWeapon explosion;

	public int activationsRemaining = 5;// implement this later for multi-use mines

	public float baseDamage;
	public float chargeUpTime;
	public float damagePerSec;

	public float currentDamage;

	public GameObject ChargeEffect;
	public GameObject FullChargeEffect;

	public GameObject explosionEffect;
	[Tooltip("If null, it will use clip already on explosion object")]
	public AudioClip explosionSound;
	UnitStats myVet;

	// Use this for initialization
	void Start () {
		currentDamage = baseDamage;
		StartCoroutine (chargeUp ());
	}

	IEnumerator chargeUp()
	{
		float elapsedTime = .5f;
		yield return new WaitForSeconds (.5f);
		while (elapsedTime < chargeUpTime) {

			currentDamage += damagePerSec / 2;

			yield return new WaitForSeconds (.5f);
			elapsedTime += .5f;
		}

		FullChargeEffect.SetActive (true);
		Destroy (ChargeEffect);
	}
	public override void UnitExitTrigger(UnitManager manager)
	{
	}

	bool triggered = false;

	public override void UnitEnterTrigger(UnitManager manager)
	{	
		if (!triggered) {
			if (manager.getUnitStats ()) {


				triggered = true;

				target = manager.transform;
				StartCoroutine (Attack (manager));
			}
		}
	}

	Transform target;
	Vector3 lastPosition;

	IEnumerator Attack(UnitManager manager)
	{
		float radius = target.GetComponent<CharacterController> ().radius + .5f;
		yield return null;
		float Speed = 0;

		float goingUp = 0;

		float distance = 0;
		while (true){

			if (target) {
				lastPosition = target.position;
			}
			distance = Vector3.Distance (transform.position, lastPosition);
			if (distance <= radius ) {
				break;
			}

			if (goingUp < .2f) {
				goingUp += Time.deltaTime;
				transform.Translate (Vector3.up * Time.deltaTime * 30 );
			}

			Speed += Time.deltaTime * 15;
			transform.Translate ((lastPosition - transform.position).normalized * Mathf.Min(distance, Speed) );

			yield return null;
		}

		if (target) {
			float amount;
			if (myVet) {
				amount = manager.getUnitStats ().TakeDamage (currentDamage, myVet.gameObject, DamageTypes.DamageType.True);
				myVet.veteranDamage (amount);
			} else {
				amount = manager.getUnitStats ().TakeDamage (currentDamage, null, DamageTypes.DamageType.True);
			}
			if (PlayerNumber == 1) {
				PlayerPrefs.SetInt ("TotalPlasmaMineDamage", PlayerPrefs.GetInt ("TotalPlasmaMineDamage") + (int)amount);
			}
		}
		GameObject obj = Instantiate (explosionEffect, this.gameObject.transform.position, Quaternion.identity);
		if (explosionSound) {
			obj.GetComponentInChildren<AudioPlayer> ().myClip = explosionSound;
		}
		Destroy (this.gameObject);	
	
	}


	public void setSource(GameObject obj)
	{
		myVet = obj.GetComponent<UnitStats> ();

	}

}
