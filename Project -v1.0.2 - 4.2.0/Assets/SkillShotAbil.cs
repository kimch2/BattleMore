using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShotAbil : TargetAbility
{
    public SkillShotData SkillShotProjectile;
    bool Casting; // NEED TO HAVE THIS CHECK A CHANNEL STATE IN THE UNITMANAGER!!
    public float Damage;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        myType = type.target;
        InitializeCharges();
        if (!myHitContainer)
        {
            OnHitContainer.CreateDefaultContainer(myManager, Name);
        }
    }



    override
    public continueOrder canActivate(bool showError)
    {
        continueOrder order = new continueOrder();
        if (chargeCount == 0 || Casting) 
        {
            order.canCast = false;
        }

        if (!myCost.canActivate(this))
        {
            order.canCast = false;
            // FIX THIS LINE IN THE FUTURE IF IT BREAKS! its currently in here to allow guys with multiple charges to use them even though the cooldown timer is shown.
            if (myCost.energy == 0 && myCost.resourceCosts.MyResources.Count == 0 && chargeCount > 0)
            {
                order.canCast = true;
            }
        }
        else
        {
            order.nextUnitCast = false;
        }
        if (order.canCast)
        {
            order.nextUnitCast = false;
        }
        return order;
    }

    override
    public void Activate()
    {
    }  // returns whether or not the next unit in the same group should also cast it


    override
    public void setAutoCast(bool offOn)
    { }

    public override bool isValidTarget(GameObject target, Vector3 location)
    {
        return true;
    }


    override
    public bool Cast(GameObject target, Vector3 location)
    {
        Vector3 direction = location - transform.parent.position;
        direction.y = 0;

        StartCoroutine(StringCast(chargeCount, direction.normalized));
        // changeCharge(-1 * chargeCount);

        myCost.payCost();

        return false;
    }
    override
    public void Cast()
    {
        Vector3 direction = location - transform.parent.position;
        direction.y = 0;

        StartCoroutine(StringCast(chargeCount, direction.normalized));
        // changeCharge(-1 * chargeCount);

        myCost.payCost();
    }


    IEnumerator StringCast(int quillNumber, Vector3 direction)
    {
        Casting = true;
        yield return null;
        // Cached for efficiency. Used for projectile randomizations
        float totalAngle = SkillShotProjectile.SpreadAngle * 2;
        float anglePerShot = totalAngle / SkillShotProjectile.NumOfShots - 1;


        // myManager.setStun(true, this, false);
        for (int i = 0; i < SkillShotProjectile.NumOfShots; i++)
        {
            myManager.LookAtTarget(myManager.transform.position + direction);
            myManager.cMover.LockRotation(true);
            myManager.myAnim.Play("Cast");
            //      changeCharge(-1);
            GameObject toFire = null;
            if (SkillShotProjectile.randomPrefabSelection)
            {
                toFire = SkillShotProjectile.SkillShotPrefabs[Random.Range(0, SkillShotProjectile.SkillShotPrefabs.Count)];
            }
            else
            {
                toFire = SkillShotProjectile.SkillShotPrefabs[i % SkillShotProjectile.SkillShotPrefabs.Count];
            }

            GameObject proj = (GameObject)Instantiate(toFire, transform.parent.position, Quaternion.identity);
            proj.GetComponent<SkillShotProjectile>().OnKill.AddListener(() => {
               // changeCharge(1);
            });
            SkillShotProjectile skillShotComp = proj.GetComponent<SkillShotProjectile>();

            skillShotComp.TotalRange = range;
            SetOnHitContainer(proj, Damage, null);
            
            float CurrentAngle = i * anglePerShot;                            
            CurrentAngle -= SkillShotProjectile.SpreadAngle;             
            if (SkillShotProjectile.NumOfShots == 1)
            {
                CurrentAngle = 0;
            }

            float RandomizedArcLimit = CurrentAngle + Random.Range(-1 * SkillShotProjectile.SpreadAngle, SkillShotProjectile.SpreadAngle) * SkillShotProjectile.SpreadRandomness;
            RandomizedArcLimit = Mathf.Clamp(RandomizedArcLimit, -1 * SkillShotProjectile.SpreadAngle, SkillShotProjectile.SpreadAngle);

            Vector3 newDirection = Quaternion.Euler(0, RandomizedArcLimit, 0) * direction;

            skillShotComp.setTarget(proj.transform.position + newDirection);
            if (SkillShotProjectile.timeBetweenShots > 0)
            {
                yield return new WaitForSeconds(SkillShotProjectile.timeBetweenShots);
            }
        }
        myManager.cMover.LockRotation(false);
        Casting = false;
    }

}

[System.Serializable]
public class SkillShotData{
    public int NumOfShots= 1;

    [Tooltip("If not 0, this will fire shots in an arc")]
    public float SpreadAngle = 1;

    [Tooltip("between 0-1, 0 is perfectly spread between the arc bounds, 1 is totally random")]
    public float SpreadRandomness= 0;

    public float timeBetweenShots = 0;

    public List<GameObject> SkillShotPrefabs = new List<GameObject>();
    [Tooltip("If false, this will fire the first prefab first, and so on, and must line up with the NumOfShots, or it will wrap around")]
    public bool randomPrefabSelection = false;
}