using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneQuillAbil : TargetAbility
{
    // Fires X number of projectiles equal to charge count in a line, get charge counters back with cooldown (~2 seconds)
    // Killing an enemy with this refunds a charge count, for the next time it is cast.

    protected Selected mySelect;
    public GameObject ToSpawn;
    public int QuillNumber = 9;
    int recastCount;
    public float QuillDelay = .33f;

    // Use this for initialization
    new void Start()
    {
        recastCount = QuillNumber;
        base.Start();
        myType = type.target;
    
        mySelect = GetComponent<Selected>();
        InitializeCharges();
    }



    override
    public continueOrder canActivate(bool showError)
    {

        continueOrder order = new continueOrder();
        if (chargeCount == 0  || Casting)
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
    bool Casting;

    IEnumerator StringCast(int quillNumber, Vector3 direction)
    {
        Casting = true;
        yield return null;
      
       // myManager.setStun(true, this, false);
        for (int i = 0; i < quillNumber; i++)
        {
            myManager.LookAtTarget(myManager.transform.position + direction);
            myManager.cMover.LockRotation(true);
            myManager.myAnim.Play("Cast");
            changeCharge(-1);
            GameObject proj = (GameObject)Instantiate(ToSpawn, transform.parent.position, Quaternion.identity);
            proj.GetComponent<SkillShotProjectile>().OnKill.AddListener(() => {// myCost.resetCoolDown();
                changeCharge(1);
            });
            proj.GetComponent<SkillShotProjectile>().TotalRange = range;
            proj.SendMessage("setSource", this.gameObject, SendMessageOptions.DontRequireReceiver);
            proj.GetComponent<SkillShotProjectile>().setTarget(proj.transform.position + direction);
            yield return new WaitForSeconds(QuillDelay);
        }
        myManager.cMover.LockRotation(false);
        // myManager.setStun(false, this, false);
        Casting = false;
    }

}
