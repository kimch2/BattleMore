using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.SoundManagerNamespace;

public  class Projectile : MonoBehaviour {
    
    [HideInInspector]
	public UnitManager target;
    [HideInInspector]
    public GameObject Source;

    //protected VeteranStats vetSource;
    [HideInInspector]
    public OnHitContainer MyHitContainer;

    public float damage;
	public float speed;

	protected float distance; // Used in a bunch of child classes
	protected float currentDistance;

    
	public DamageTypes.DamageType damageType = DamageTypes.DamageType.Regular;
	protected AudioSource AudSrc;
	protected CharacterController control;

	//If you are using an explosion , you should set the variables in the explosion prefab itself.
	public GameObject explosionO;
	public bool SepDamWithExplos;
	public GameObject SpecialEffect;

	protected Vector3 lastLocation;

	protected Vector3 randomOffset;
	protected Vector3 originPoint; // Used by abilities that need to know where the shot was fired from (FrontalShield)

	GameObject cachedEffect; 
	MultiShotParticle multiParticle;

	protected Lean.LeanPool myBulletPool;

	List<TrailRenderer> renders = new List<TrailRenderer>();

	public void Start () {	
		if (!myBulletPool) {
			myBulletPool = Lean.LeanPool.getSpawnPool (this.gameObject);
		}

		AudSrc= GetComponent<AudioSource> ();
		if (AudSrc && AudSrc.clip) {
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
		originPoint = transform.position;
		if (AudSrc && AudSrc.clip) {
			AudSrc.pitch +=((float)Random.Range (-3, 3)) / 10;
			SoundManager.PlayOneShotSound (AudSrc, AudSrc.clip);
		}
		foreach (TrailRenderer rend in renders) {
			rend.Clear ();	
		}
	}

	public void Initialize(UnitManager targ, float dam, OnHitContainer hitContain)
	{
		target = targ;
        if (dam >= 0)
        {
            damage = dam;
        }
        MyHitContainer = hitContain;
        setup();
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

		}		
	}

    public Vector3 getOrigin()
    {
        return originPoint;
    }

    protected virtual void onTerminate()
	{}
		

	public void setLocation(Vector3 loc)
	{
		lastLocation = loc + randomOffset;
		gameObject.transform.LookAt (lastLocation);
		distance = Vector3.Distance (this.gameObject.transform.position, lastLocation);
	}

	float movementAmount;
	protected virtual void Update () {

		movementAmount = speed * Time.deltaTime;
		gameObject.transform.Translate (Vector3.forward* movementAmount);

		if(target){
            if (Vector3.Distance(target.transform.position + randomOffset, transform.position) < movementAmount)
            {
                Terminate(target);
            }
            else
            {
                lastLocation = target.transform.position + randomOffset;
                gameObject.transform.LookAt(lastLocation);
            }
        }
		else
		{
			if (Vector3.Distance (lastLocation, transform.position) < movementAmount) {
				Terminate (target);
                return;
			}
		}
    }

	protected virtual void OnControllerColliderHit(ControllerColliderHit other)
	{
		if (!target ) {
			return;}
		
		if (other.gameObject == target || other.gameObject.transform.IsChildOf(target.transform)|| (Source && Source.transform.IsChildOf(other.transform))) {
            Debug.Log("Terminating on " + target + "   Source is "); 
                Terminate (other.gameObject.GetComponent<UnitManager>());
		}

		if(Source && (other.gameObject!= Source && !other.gameObject.transform.IsChildOf(Source.transform) &&  (!Source.transform.IsChildOf(other.transform))))
        {
            Debug.Log("Terminating on " + target + "   Source is " + Source);
            Terminate(null);
        }
	}

	public virtual void Terminate(UnitManager target)
	{
		if (!gameObject.activeSelf) { 		
			return;
		}
		try{
            if (explosionO)
            {
                GameObject explode = (GameObject)Instantiate(explosionO, transform.position, Quaternion.identity);

                MyHitContainer.SetOnHitContainer(explode, this.damage, null);

                if (target && (!explosionO || explosionO && SepDamWithExplos))
                {
                    if (MyHitContainer)
                    {
                        MyHitContainer.trigger(this.gameObject, target, damage);
                    }

                    target.myStats.TakeDamage(damage, Source, damageType, MyHitContainer);
                }
            }
            else if(target)
            {
                target.myStats.TakeDamage(damage, Source, damageType, MyHitContainer);
            }
                if (SpecialEffect)
                {
                    if (!cachedEffect || cachedEffect.activeSelf) // Will this second statement cause stuff to get left in the scene?
                    {
                        cachedEffect = Instantiate(SpecialEffect, transform.position, transform.rotation);
                        multiParticle = cachedEffect.GetComponent<MultiShotParticle>();
                    }
                    else
                    {
                        cachedEffect.transform.position = (transform.position + lastLocation + randomOffset) * .5f;
                        cachedEffect.transform.rotation = transform.rotation;
                    }

                    if (multiParticle)
                    {
                        multiParticle.playEffect();
                    }
                }

		}catch(System.Exception e) {
			Debug.LogError ("Projectile Broke: " + e);
		}
        onTerminate();
		myBulletPool.FastDespawn (this.gameObject, 0);

	}



	public void Despawn()
	{
        MyHitContainer = null;
	}


	public void setSource(GameObject so)
	{
        MyHitContainer = so.GetComponent<OnHitContainer>();
	}
	
	


	public void selfDestruct()
	{
        target = null;

		if (SpecialEffect) {

			if (!cachedEffect) {
                cachedEffect = Instantiate (SpecialEffect, transform.position, transform.rotation);
				multiParticle = cachedEffect.GetComponent<MultiShotParticle> ();

			} else {
                cachedEffect.transform.position = (transform.position + lastLocation + randomOffset) * .5f;
                cachedEffect.transform.rotation = transform.rotation;

			}

			if (multiParticle) {
				multiParticle.playEffect ();
			}
		}

		onTerminate ();
		myBulletPool.FastDespawn (this.gameObject, 0);
		
	}
}