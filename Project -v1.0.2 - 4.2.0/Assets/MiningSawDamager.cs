
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public class MiningSawDamager : MonoBehaviour {


		private List<UnitStats> enemies = new List<UnitStats> ();


	public DamageTypes.DamageType myType = DamageTypes.DamageType.Regular;
	public int Owner;
	public GameObject cutEffect;
	public GameObject impactEffect;
	public float damage = 5;
	public float turretRatio = .2f;
	private AudioSource myAudio;
	public AudioClip chopSound;
	public UnitManager myManager;
	public VeteranStats myVets;
	private int iter = 0;


		// Use this for initialization
		void Start () {
		myAudio = GetComponent<AudioSource> ();

		InvokeRepeating ("UpdateDamage", .1f, .2f);
		}

		// Update is called once per frame
		void UpdateDamage () {

				if (enemies.Count > 0) {
	
					enemies.RemoveAll (item => item == null);

			float amount = 0;
					foreach (UnitStats s in enemies) {

					if (s.isUnitType (UnitTypes.UnitTypeTag.Turret)) {
					amount += 	s.TakeDamage (damage * (turretRatio), this.gameObject.gameObject.gameObject, myType,myManager);
				
					} else {

					amount += s.TakeDamage (damage, this.gameObject.gameObject.gameObject, myType,myManager);

						iter++;
						if (iter == 6) {
								PopUpMaker.CreateGlobalPopUp (-(damage*2) + "", Color.red, s.gameObject.transform.position);
							iter = 0;
						}
					}
					if (cutEffect) {
						Instantiate (cutEffect, getImpactLocation (), Quaternion.identity);
					}
					//obj.transform.SetParent (this.gameObject.transform);
					}

			if (myVets!= null) {
				myVets.UpdamageDone (amount);
			}

			}


		}


		public void turnOn()
		{
			GetComponent<BoxCollider> ().enabled = true;
			this.enabled = true;
		}


		public void setOwner(int n)
		{Owner = n;
		}

	public Vector3 getImpactLocation()
	{
		Vector3 vec = this.transform.position;
		vec.y = 1;
		return vec;

	}

		void OnTriggerEnter(Collider other)
		{
		if (other.isTrigger) {
			return;}

		if (other.name == "Ground" && impactEffect) {
			Instantiate (impactEffect, getImpactLocation(), Quaternion.identity);
		}
		if (chopSound) {
			myAudio.PlayOneShot (chopSound);
		}

			UnitManager manage = other.gameObject.GetComponent<UnitManager> ();
			if (manage == null) {
				return;
			}

			if (manage.PlayerOwner != Owner) {
			float amount = manage.myStats.TakeDamage (damage, this.gameObject.gameObject.gameObject, myType, myManager);
			if (myManager) {
				myManager.myStats.veteranDamage (amount);
			}
				enemies.Add (manage.myStats);

				return;
			}


		}


		void OnTriggerExit(Collider other)
		{
		if (other.isTrigger || other.gameObject.layer == 15) {
			return;}


		UnitManager manage = other.gameObject.GetComponent<UnitManager> ();


			if (manage == null) {
				return;
			}

			if (manage.PlayerOwner == Owner) {
				return;
			}

			if (enemies.Contains (manage.myStats)) {
				enemies.Remove (manage.myStats);
			}
		}


	}
