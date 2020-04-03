using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using DigitalRuby.SoundManagerNamespace;
public class IWeapon : MonoBehaviour {

	public string Title;
	public Sprite myIcon;
	public UnitManager myManager;

	public GameObject OnHitEffect;
	protected MultiShotParticle fireEffect; // Maybe can be removed?

	public AudioClip attackSoundEffect;
	protected AudioSource audioSrc;
	public Animator myAnimator;
	public string AnimationName = "Attack";

	public float attackPeriod;
    [HideInInspector]
	public float baseAttackPeriod;
	public int numOfAttacks = 1;
	[Tooltip("Amount of time between each bullet when numOfAttacks is more than 1")]
	public float RepeatDelay = .06f;
	int upgradeLevel = 0;

	//private float myRadius;

	public float baseDamage;
	private float InitialBaseDamage;



	[Tooltip("Having arange that is longer than the vision range is not supported yet")]
	public float range = 5;

	public GameObject turret;
	turret turretClass;

	public List<AnimationPoint> firePoints;

	[System.Serializable]
	public class AnimationPoint
	{
		public Vector3 position;
		public MultiShotParticle myParticle;
	}

	protected int originIndex = 0;
	public float damagePoint;
    [Tooltip("Time between when the unit starts the attack animation and damage is dealt or projectile is fired")]
	public float AttackDelay;

    //private bool onDamagePoint;
    //private float PointEnd;
    public OnHitContainer myHitContainer;


	protected bool offCooldown = true;

	public List<Notify> triggers = new List<Notify>();

	public List<Validator> validators = new List<Validator>();

	protected Lean.LeanPool myBulletPool;
	public void setBulletPool(Lean.LeanPool pool)
	{
		myBulletPool = pool;
	}

	public List<UnitTypes.UnitTypeTag> cantAttackTypes = new List<UnitTypes.UnitTypeTag>();

	[System.Serializable]
	public struct bonusDamage {


		public UnitTypes.UnitTypeTag type;
		public float bonus;
	}

	public List<bonusDamage> extraDamage;

	public float getInitialDamage()
	{
		return InitialBaseDamage;
	}

	public GameObject projectile;
	//public Effect spawnEffect;

	public void changeBasePeriod(float percent, float flat)
	{
		baseAttackPeriod += flat;
		baseAttackPeriod *= (1 - percent);
	}

	void Awake()
	{
		if (projectile) {
			myBulletPool = Lean.LeanPool.getSpawnPool (projectile);
		}
		baseAttackPeriod = attackPeriod;
		InitialBaseDamage = baseDamage;

		if (firePoints.Count == 0) {
			firePoints.Add(new AnimationPoint());
		}
        if (!myManager)
        {
            myManager = GetComponent<UnitManager>();
        }

        if (!myHitContainer)
        {
            myHitContainer = OnHitContainer.CreateDefaultContainer(this.gameObject, myManager, Title);
        }
    }

	// Use this for initialization
	public virtual void Start () {
		audioSrc = GetComponent<AudioSource> ();
		if (audioSrc) {
			audioSrc.priority += Random.Range (-60, 0);
		}


		if (turret) {
			turretClass = turret.GetComponent<turret> ();
		}
    }


	IEnumerator ComeOffCooldown( float length)
	{
		yield return new WaitForSeconds (length);
		offCooldown = true;
	}



	public bool isOffCooldown()
	{
        return offCooldown;
	}
	

	// Does not check for range
	public bool simpleCanAttack(UnitManager target)
	{
        if (!offCooldown) {
			return false;}
		if (!target) {
			return false;}



		foreach (Validator val in validators) {
			if(val.validate(this.gameObject,target.gameObject) == false)
			{return false;}
		}


		foreach (UnitTypes.UnitTypeTag tag in cantAttackTypes) {
			if (target.myStats.isUnitType (tag))
			{	//	Debug.Log (Title + "cant attack");
				return false;	}
		}


		return true;
	}

	public bool validateWeap(UnitManager target)
	{
		foreach (Validator val in validators) {
			if(val.validate(this.gameObject,target.gameObject) == false)
			{
				return false;}
		}
		return true;
	}

	public virtual bool canAttack(UnitManager target)
	{
		if (!offCooldown) {
			return false;}
		if (!target) {
			return false;}
	
		foreach (Validator val in validators) {
			if(val.validate(this.gameObject,target.gameObject) == false)
			{
				return false;}
		}

		// Account for height advantage
		float distance = Mathf.Sqrt((Mathf.Pow (transform.position.x - target.transform.position.x, 2) + Mathf.Pow (transform.position.z - target.transform.position.z, 2))) - target.CharController.radius ;

		float verticalDistance = this.gameObject.transform.position.y - target.transform.position.y;
		if (distance > (range + (verticalDistance/3))) {
			
			return false;}

		foreach (UnitTypes.UnitTypeTag tag in cantAttackTypes) {
			if (target.myStats.isUnitType (tag))
			{return false;	}
		}

		return true;
	
	}

	public virtual bool inRange(UnitManager target)
	{

		if (this && target) {

			foreach (UnitTypes.UnitTypeTag tag in cantAttackTypes) {
				if (target.myStats.isUnitType (tag))
				{	
					return false;	}
			}

			float distance = Mathf.Sqrt((Mathf.Pow (transform.position.x - target.transform.position.x, 2) + Mathf.Pow (transform.position.z - target.transform.position.z, 2))) - target.CharController.radius;
			float verticalDistance = this.gameObject.transform.position.y - target.transform.position.y;

			if (distance > (range + (verticalDistance/3 ))) {

		
				return false;
			}
		} else {

			return false;}
		return true;

	}

	/// <summary>
	/// returns false if the unit is too close
	/// </summary>
	/// <returns><c>true</c>, if minimum range was checked, <c>false</c> otherwise.</returns>
	/// <param name="target">Target.</param>
	public virtual bool checkMinimumRange(UnitManager target)
	{
		return true;
	}



	public void ResetAttackCooldown()
	{
		offCooldown = true;
		StopAllCoroutines ();
        myManager.metaStatus.UnRoot(this); // WIll this break with things with turrets??? (instead of having a coroutine that unroots it)
	}

	public void attack(UnitManager target, UnitManager toStun)
	{
		offCooldown = false;
        
		if (toStun && damagePoint > 0) {
			toStun.metaStatus.Root(toStun, this, true, damagePoint);
		}
		for (int i = 0; i < numOfAttacks; i++) {
			StartCoroutine( Fire ((i * RepeatDelay + AttackDelay), target));
		
		}
		StartCoroutine (ComeOffCooldown (attackPeriod));
	}


	IEnumerator Fire (float time, UnitManager target)
	{
        if (myAnimator && AnimationName != "") {
			myAnimator.Play (AnimationName);
		}
		else if (myManager) { // Adding an Else to the IF here, so we don't play the same animation twice
			myManager.animAttack ();
		}

		LookAtTarget (target.gameObject);
		yield return new WaitForSeconds(time);

		if (target) {
			LookAtTarget (target.gameObject);

			float damage = baseDamage;
			foreach (bonusDamage tag in extraDamage) {
				if (target.myStats.isUnitType (tag.type)) {
					damage += tag.bonus;
				}
			}

			DealDamage (damage, target);

			if (firePoints[originIndex].myParticle) {

				firePoints[originIndex].myParticle.playEffect ();
			}
			originIndex++;

			if (originIndex ==firePoints.Count) {
				originIndex = 0;}



			if (target == null) {
				myManager.cleanEnemy ();
			}

			if (attackSoundEffect && audioSrc) {
				
				audioSrc.pitch = ((float)Random.Range (7, 12) / 10);
					SoundManager.PlayOneShotSound(audioSrc, attackSoundEffect);
			}

			if (OnHitEffect) {

				if (!fireEffect) {
					GameObject temp = (GameObject)Instantiate (OnHitEffect, target.transform.position, Quaternion.identity);
					fireEffect =temp.GetComponent<MultiShotParticle> ();
					if (fireEffect) {
						fireEffect.playEffect ();
					}
				} else {
					fireEffect.transform.position = target.transform.position + Vector3.up;
					fireEffect.playEffect ();
				}
			}
		}
	}


	protected virtual void DealDamage(float damage, UnitManager target)
	{
		GameObject proj = null;
		if (projectile != null) {

			proj = createBullet ();

			damage = fireTriggers (this.gameObject, proj, target, damage);

			Projectile script = proj.GetComponent<Projectile> ();

			//Debug.Log ("Creating " + script);
			if (script) {
				script.Initialize (target, damage,myHitContainer);
			} else {
                // Should only be used for things like summoning minions out of a weapon
				proj.SendMessage ("setSource", this.gameObject, SendMessageOptions.DontRequireReceiver);
				proj.SendMessage ("setTarget", target, SendMessageOptions.DontRequireReceiver);
				proj.SendMessage ("setDamage", damage, SendMessageOptions.DontRequireReceiver);
			}
		} else {

			damage = fireTriggers (this.gameObject, proj, target, damage);
            myHitContainer.trigger(null, target, damage); 
			if (damage > 0) {
                damage = target.myStats.TakeDamage(damage, this.gameObject, DamageTypes.DamageType.Regular, myHitContainer);
            }

		}
	}

	void LookAtTarget(GameObject target)
	{

		if(target){
		    if (turretClass) {
				    turretClass.Target (target.gameObject);
		    }
		    else{

                myManager.LookAtTarget(target.transform.position);
			}
		}
	}

	protected virtual GameObject createBullet()
	{
		if (turret) {
			return myBulletPool.FastSpawn(turret.transform.rotation * firePoints[originIndex].position + turret.transform.position, Quaternion.identity);
		} else {
			return myBulletPool.FastSpawn(transform.rotation * firePoints [originIndex].position + this.gameObject.transform.position, Quaternion.identity);
		}
	}
		
	public void addNotifyTrigger(Notify not)
	{
		triggers.Add (not);
	}

	public void removeNotifyTrigger(Notify not)
	{
		triggers.Remove (not);
	}

	public float fireTriggers(GameObject source, GameObject proj, UnitManager target, float damage)
	{	
		triggers.RemoveAll (item => item == null);
        for (int i = triggers.Count - 1; i >= 0; i--)
        {
            if (triggers[i] != null)
            {
                damage =triggers[i].trigger(source, proj, target, damage);
            }
        }

		return damage;
	}

	public int getUpgradeLevel ()
	{
		return  upgradeLevel;
	}

	public void incrementUpgrade()
	{
		upgradeLevel++;
	}

	public void resetBulletPool()
	{
		myBulletPool = Lean.LeanPool.getSpawnPool (projectile);
	}

	public bool isValidTarget(UnitManager target)
	{

		foreach (UnitTypes.UnitTypeTag ty in cantAttackTypes) {
			if (target.myStats.isUnitType (ty))
				return false;
		}

		foreach (Validator val in validators) {
			if(val.validate(this.gameObject,target.gameObject) == false)
			{
				return false;
			}
		}
		return true;


	}

	public float getBasePeriod()
	{
		return baseAttackPeriod;
	}


    protected UnitManager currentIter;
    protected float currDistance;
    protected float bestPriority;

    public virtual UnitManager findBestEnemy(out float distance, UnitManager best) // Similar to above method but takes into account attack priority (enemy soldiers should be attacked before buildings)
    {
        float currentIterPriority;
        if (best != null)
        {
            distance = Vector3.Distance(best.transform.position, transform.position);
            bestPriority = best.myStats.getCombatPriority(myManager.myStats.DefensePriority);
        }
        else
        {

            distance = float.MaxValue;
            bestPriority = -1;
        }

        for (int i = 0; i < myManager.enemies.Count; i++)
        {
            currentIter = myManager.enemies[i];

            if (currentIter == null || currentIter.myStats.isUnitType(UnitTypes.UnitTypeTag.Invisible))
            {
                continue;
            }


            if (!isValidTarget(currentIter))
            {
                continue;
            }
            currentIterPriority = currentIter.myStats.getCombatPriority(myManager.myStats.DefensePriority);
            if (currentIterPriority > bestPriority)
            {
                best = currentIter;
                bestPriority = currentIterPriority;
                distance = Vector3.Distance(currentIter.transform.position, this.gameObject.transform.position);
            }
            else if (currentIterPriority == bestPriority)
            {
                currDistance = Vector3.Distance(currentIter.transform.position, this.gameObject.transform.position);

                if (currDistance < distance)
                {
                    best = currentIter;
                    distance = currDistance;
                }
            }
        }

        return best;
    }




    public void OnDrawGizmos()
	{
	foreach (AnimationPoint vec in firePoints) {
			if (turret) {
				Gizmos.DrawSphere ((turret.transform.rotation) * vec.position +turret.transform.position, .5f);
			} else {
				Gizmos.DrawSphere ((transform.rotation) *vec.position + this.gameObject.transform.position, .5f);
			}
		}
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left*range);

	}
}
