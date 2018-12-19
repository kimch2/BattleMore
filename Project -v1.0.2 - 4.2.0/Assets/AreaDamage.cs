using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamage : MonoBehaviour {


	public List<UnitStats> enemies = new List<UnitStats> ();


	public DamageTypes.DamageType myType = DamageTypes.DamageType.Regular;
	public IWeapon.bonusDamage BonusDamage;
	public int Owner;
	public GameObject cutEffect;

	public float damage = 5;
	protected AudioSource myAudio;
	public AudioClip chopSound;
	public bool showPop = true;

	protected int iter = 0;
	public bool NonStack = false;


	// Use this for initialization
	void Start () {
		myAudio = GetComponent<AudioSource> ();

		InvokeRepeating ("UpdateDamage", .1f, .2f);
	}

	// Update is called once per frame
public	virtual void UpdateDamage () {

		if (enemies.Count > 0) {

			enemies.RemoveAll (item => item == null);
			foreach (UnitStats s in enemies) {

				if (NonStack) {
					if (!DamageNonStacker.instance.DealDamage (gameObject.name, s, damage)) {
						return;
					}
				}
		
				s.TakeDamage (damage + (s.isUnitType(BonusDamage.type)? BonusDamage.bonus : 0), this.gameObject.gameObject.gameObject, myType);
		
				if (showPop) {
					iter++;
					if (iter == 6) {
						PopUpMaker.CreateGlobalPopUp (-(damage * 2) + "", Color.red, s.gameObject.transform.position);
						iter = 0;
					}
				}
				if (cutEffect) {
					Instantiate (cutEffect, s.gameObject.transform.position, Quaternion.identity);
				}
				//obj.transform.SetParent (this.gameObject.transform);
			}
		}


	}

	public void setVeteran(VeteranStats vet)
	{

		Debug.Log ("Setting me " + vet.playerOwner);
		if (vet!= null) {
			Owner = vet.playerOwner;
		}
	}

	public void setSource(GameObject obj)
	{
		UnitManager manage = obj.GetComponent<UnitManager> ();
		if (manage) {
		
			Owner = manage.PlayerOwner;
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
		//vec.y = 1;
		return vec;

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.isTrigger) {
			return;}

		if (other.gameObject.layer == 15) { // Its a projectile, the most common kind of trigger
			return;
		}
		UnitManager manage = other.gameObject.GetComponent<UnitManager> ();
		if (manage == null) {
			return;
		}

		if (manage.PlayerOwner != Owner) {
			enemies.Add (manage.myStats);
			if (chopSound) {
				myAudio.PlayOneShot (chopSound);
			}
			return;
		}


	}


	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 15) { // Its a projectile, the most common kind of trigger
			return;
		}

		UnitManager manage = other.gameObject.GetComponent<UnitManager> ();


		if (manage == null) {
			return;
		}

		if (manage.PlayerOwner == Owner) {
			return;
		}

		enemies.Remove (manage.myStats);
		
	}


}
