using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DigitalRuby.SoundManagerNamespace;
public class ChainWhip : MonoBehaviour {


	public ChainWhip childWhip;
	public GameObject Chain;
	public UnitManager myManager;

	public float maxDamage;
	public float maxRadius;
	public IWeapon myWeap;
	//int breakCount = 0;
	bool whipOn;

	public float speed;
	Coroutine myCoro = null;

	Quaternion curRotation;
	public AudioSource audioSource;
	public AudioClip hitSound;

	public List<Vector3> ChainExtensionPoints;

	void Start()
	{
		if (childWhip) {

			if (myCoro == null) {
				myCoro = StartCoroutine (WhipSpin ());
				//mySpinner = StartCoroutine (UpdateRotation ());
			}
		}

	}

	void OnTriggerEnter(Collider other)
	{
		//need to set up calls to listener components
		//this will need to be refactored for team games

		if (!other.isTrigger) {

			if (other.gameObject.layer == 15) { // Its a projectile, the most common kind of trigger
				return;
			}


			//Debug.Log (this.gameObject + " hit " + other.gameObject);
			UnitManager manage = other.gameObject.GetComponent<UnitManager>();

			if (manage) {


				if (manage.PlayerOwner != myManager.PlayerOwner) {
	
					if (childWhip && myWeap.simpleCanAttack (manage)) {
		
					} else if (!childWhip) {
						float distance = Vector3.Distance (transform.position, manage.transform.position);
						manage.myStats.TakeDamage (maxDamage * (distance / maxRadius), myManager.gameObject, DamageTypes.DamageType.Regular);
						SoundManager.PlayOneShotSound (audioSource, hitSound);
		
						/*
					if (distance < transform.localScale.x / 2) {
						breakCount++;
						if (breakCount > 3) {
							halfChain ();
						}
					}*/
					}
			
				}
			}
		}
	}

	IEnumerator WhipSpin()
	{
		whipOn = true;
		yield return null;

		while (myManager.enemies.Count > 0) {
			setScale (2);
			yield return new WaitForSeconds (.2f);
			myManager.enemies.RemoveAll (item => item == null);
		
		}

		while(childWhip.transform.localScale.x > 5){
			yield return new WaitForSeconds(.1f);

			if (myManager.enemies.Count > 0) {

				myCoro = StartCoroutine (WhipSpin());
			
				break;
			} else {
				setScale (-2);
			}
		}
		if (myManager.enemies.Count > 0) {
			myCoro = StartCoroutine (WhipSpin ());
			//StopCoroutine (mySpinner);


		} else {
			whipOn = false;
			myCoro = null;
		}
	}

	void setScale(float changeAmount)
	{
		if (Chain.transform.lossyScale.x == maxRadius * 2) {
			return;
		}
		Vector3 newScale = Chain.transform.localScale;
		newScale.x += changeAmount;
		newScale.x = Mathf.Clamp (newScale.x, 10, maxRadius*2);
		speed = 250 - newScale.x;
		myWeap.range = newScale.x/2 -5;
		Chain.transform.localScale = newScale;
	}

	void halfChain()
	{	
		//breakCount = 0;
		Vector3 newScale = transform.localScale;
		newScale.x *= .5f;
		if (newScale.x < 10) {
			newScale.x = 10;
		}
		myWeap.range = newScale.x/2 -5;
		transform.localScale = newScale;
	}


	void Update () 
	{ 
	if (whipOn) {
			
			childWhip.transform.parent.rotation = curRotation;
			childWhip.transform.parent.Rotate (Vector3.up, speed * Time.deltaTime);
			curRotation = childWhip.transform.parent.rotation;
			
		}

	}
}
