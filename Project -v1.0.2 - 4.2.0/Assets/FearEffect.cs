using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearEffect : MonoBehaviour, Notify
{

    public float duration;
    [Tooltip("if false, the effected unit will run from the source of the fear")]
    public bool RunToSide;

    // Use this for initialization
    void Start()
    {
        if (GetComponent<Projectile>())
        {
            GetComponent<Projectile>().triggers.Add(this);
        }

        explosion myexplode = this.gameObject.GetComponent<explosion>();
        if (myexplode)
        {
            myexplode.triggers.Add(this);
        }
    }
    
    public float trigger(GameObject source, GameObject proj, UnitManager target, float damage)
    {       
        if (target && source != target)
        {
            Vector3 TargetLocation;
            if (GetComponent<explosion>()) 
            {
                TargetLocation = (transform.position - target.transform.position).normalized * target.cMover.getMaxSpeed() 
                    * target.myStats.getTenacityMultiplier() + target.transform.position;
            }
            else
            {
                TargetLocation = (source.transform.position - target.transform.position).normalized * target.cMover.getMaxSpeed() * target.myStats.getTenacityMultiplier() + target.transform.position;
            }

            // Probably should run the fear code through the unitmanager like we do silences, and stuns
            // especially since fears should cancel channel spells sometimes

            target.GiveOrder(Orders.CreateMoveOrder(RunToSide? transform.position + Vector3.left * 20 : TargetLocation));
            // Give an order to then continue attack moving?
        }
        return damage;
    }
}
