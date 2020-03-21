using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneQuillAbil : SkillShotAbil
{
    // Fires X number of projectiles equal to charge count in a line, get charge counters back with cooldown (~2 seconds)
    // Killing an enemy with this refunds a charge count, for the next time it is cast.

    override
    public void Activate()
    {
    }  // returns whether or not the next unit in the same group should also cast it

    protected override void AlterProjectile(SkillShotProjectile proj)
    {
        /*
        proj.OnKill.AddListener(() =>
        {
            myCost.cooldownTimer -= 2;
        });*/
    }



    /*
    override
    public bool Cast(GameObject target, Vector3 location)
    {/*
        Debug.Log("Cast A");
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
        Debug.Log("Cast B");
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
*/
}
