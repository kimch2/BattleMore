using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.SoundManagerNamespace;
public  class Projectile : MonoBehaviour {



	public UnitManager target;
 	protected UnitManager SourceMan;
	public float damage;
	public float speed;

	public bool trackTarget;

	protected float distance;
	protected float currentDistance;

	//public ProjectileMover mover;
	public GameObject Source;
	public int sourceInt =1;

	public DamageTypes.DamageType damageType = DamageTypes.DamageType.Regular;
	public AudioClip mySound;
	protected AudioSource AudSrc;
	//private bool selfDest = false;
	protected CharacterController control;

	//If you are using an explosion , you should set the variables in the explosion prefab itself.
	public GameObject explosionO;
	public bool SepDamWithExplos;
	public GameObject SpecialEffect;

	public List<Notify> triggers = new List<Notify> ();

	protected Vector3 lastLocation;

	public float FriendlyFire;
	protected Vector3 randomOffset;

	GameObject myEffect;
	MultiShotParticle multiParticle;

	Lean.LeanPool myBulletPool;

	List<TrailRenderer> renders = new List<TrailRenderer>();
	// Use this for initialization
	public void Start () {	
		if (!myBulletPool) {
			myBulletPool = Lean.LeanPool.getSpawnPool (this.gameObject);
		}

		AudSrc= GetComponent<AudioSource> ();
		if (AudSrc && mySound) {
			AudSrc.priority += Random.Range (-60, 0);
			AudSrc.volume = ((float)Random.Range (1, 5)) / 10;
			AudSrc.pitch +=((float)Random.Range (-3, 3)) / 10;
		}

		control = GetComponent<CharacterController> ();


		foreach (TrailRenderer rend in GetComponentsInChildren<TrailRenderer>(true)) {
			renders.Add (rend);
		}
		TrailRenderer rendA = GetComponent<TrailRenderer> ();
		if (rendA && !renders.Contains(rendA)) {
			renders.Add (rendA);
		}
	}

	public void OnSpawn()
	{	currentDistance = 0;
		
		if (AudSrc && mySound) {
			AudSrc.pitch +=((float)Random.Range (-3, 3)) / 10;
			SoundManager.PlayOneShotSound (AudSrc, mySound);
		}
		foreach (TrailRenderer rend in renders) {
			rend.Clear ();	
		}
	}
			
	public void Initialize(UnitManager targ, float dam, UnitManager src)
	{
		target = targ;
		damage = dam;
		Source = src.gameObject;
		SourceMan = src;
		sourceInt = src.PlayerOwner;
	}

	public virtual void setup()
	{
		if (target) {

			CharacterController cont = target.GetComponent<CharacterController> ();

			randomOffset = UnityEngine.Random.onUnitSphere * cont.radius;
			if (randomOffset.y < -cont.radius * .333f) {
				randomOffset.y = Mathf.Abs (randomOffset.y);
			}
			randomOffset *= .9f;
			randomOffset += Vector3.up;

			lastLocation = target.transform.position + randomOffset;
			distance = Vector3.Distance (this.gameObject.transform.position, lastLocation);

		
			InvokeRepeating ("lookAtTarget", .05f, .05f);
		}
		lookAtTarget ();
			
	}

	protected virtual void onHit()
	{}
		

	protected void lookAtTarget()
	{
		if (target != null) {
			lastLocation = target.transform.position + randomOffset;
		}

		gameObject.transform.LookAt (lastLocation);
	}

	public void setLocation(Vector3 loc)
	{

		lastLocation = loc + randomOffset;

		gameObject.transform.LookAt (lastLocation);
		distance = Vector3.Distance (this.gameObject.transform.position, lastLocation);
	}

	protected float yAmount;

	float movementAmount;
	// Update is called once per frame
	protected virtual void Update () {
		/*
		if (distance - currentDistance < 1.5f  || speed * Time.deltaTime * 2) {
			if (target && trackTarget) {
				if (Vector3.Distance (transform.position, target.transform.position) < 2) {
					Terminate (target);
					return;
				}

				float trueDist = Vector3.Distance (transform.position, target.transform.position + randomOffset);
				if (trueDist > 2) {
					distance = trueDist;
					currentDistance = 0;
				} else {
					Terminate (target);
				}
			} else {
				Terminate (target);
			}
		}
	
*/

		movementAmount = speed * Time.deltaTime;
		gameObject.transform.Translate (Vector3.forward* movementAmount);

		if(target && trackTarget){
			if (Vector3.Distance (target.transform.position + randomOffset, transform.position) < movementAmount) {
				Terminate (target);
			}
		}
		else
		{
			if (Vector3.Distance (lastLocation, transform.position) < movementAmount) {
				Terminate (target);
			}
		}

	//	currentDistance += speed * Time.deltaTime;
	}

	protected virtual void OnControllerColliderHit(ControllerColliderHit other)
	{
		if (!target) {
			return;}
		if (other.gameObject == target || other.gameObject.transform.IsChildOf(target.transform)) {
			Terminate (other.gameObject.GetComponent<UnitManager>());
		}

		if (currentDistance / distance < .5) {
			return;
		}

		if(!trackTarget && (other.gameObject!= Source || !other.gameObject.transform.IsChildOf(Source.transform) ))
		{
			Terminate(null);}
	}

	//DO I NEED THIS?
	public virtual void OnTriggerEnter(Collider other)
	{if (!target) {
			return;}
		
		if (other.isTrigger) {
			
			return;}
	

		if (other.gameObject == target || other.gameObject.transform.IsChildOf(target.transform)) {
			Terminate (other.gameObject.GetComponent<UnitManager> ());
			//Debug.Log (" COLLIDER HIT ENEMY");

			return;
			}


		if(!trackTarget && (other.gameObject!= Source || !other.gameObject.transform.IsChildOf(Source.transform) ))
			{Terminate(null);
			//Debug.Log (" COLLIDER HIT ENEMY");

		}
		
	}



	public virtual void Terminate(UnitManager target)
	{
		if (!gameObject.activeSelf) {
		
			return;
		}

		if (explosionO) {

			GameObject explode = (GameObject)Instantiate (explosionO, transform.position, Quaternion.identity);

			explosion Escript = explode.GetComponent<explosion> ();
			if (Escript) {
				Escript.setSource (Source);
				Escript.damageAmount = this.damage;
				Escript.friendlyFireRatio = FriendlyFire;
			}
		}

		if (explosionO && SepDamWithExplos && target || !explosionO && target) {
			
			foreach (Notify not in triggers) {
				if (not != null) {
					not.trigger (this.gameObject, this.gameObject, target, damage);
				}
			}
			if (target != null && target.myStats != null) {

				//Debug.Log ("Giveing damage");
				float total =  target.myStats.TakeDamage (damage, Source,damageType);
				if (SourceMan)
					{
					SourceMan.myStats.veteranDamage (total);

				}
			}
			if (target == null) {
				{
					SourceMan.cleanEnemy ();}
			}
		} 
	

		if (SpecialEffect) {

			if (!myEffect) {
				myEffect = Instantiate (SpecialEffect, transform.position, transform.rotation);
				multiParticle = myEffect.GetComponent<MultiShotParticle> ();

			}
			else {
				myEffect.transform.position = (transform.position + lastLocation + randomOffset) * .5f;
				myEffect.transform.rotation = transform.rotation;

			}

			if (multiParticle) {
				multiParticle.playEffect ();
			}

		} 

		onHit ();
		CancelInvoke ("lookAtTarget");
		myBulletPool.FastDespawn (this.gameObject, 0);

	}



	public void Despawn()
	{
		triggers.Clear ();
	}


	public void setSource(GameObject so)
	{
		
		Source = so;
		SourceMan = so.GetComponent<UnitManager> ();
		if (SourceMan) {
			sourceInt = SourceMan.PlayerOwner;
		} else {
			sourceInt = 1;
		}

	}
	
	


	public void selfDestruct()
	{target = null;

		if (SpecialEffect) {

			if (!myEffect) {
				myEffect = Instantiate (SpecialEffect, transform.position, transform.rotation);
				multiParticle = myEffect.GetComponent<MultiShotParticle> ();

			} else {
				myEffect.transform.position = (transform.position + lastLocation + randomOffset) * .5f;
				myEffect.transform.rotation = transform.rotation;

			}

			if (multiParticle) {
				multiParticle.playEffect ();
			}
		}

		onHit ();
		myBulletPool.FastDespawn (this.gameObject, 0);
		
	}




}