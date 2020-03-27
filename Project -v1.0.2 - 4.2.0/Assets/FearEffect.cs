using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearEffect :MonoBehaviour, Notify
{

    public float duration;
    [Tooltip("if false, the effected unit will run from the source of the fear")]
    public bool RunToSide;

    public float trigger(GameObject source, GameObject projectile, UnitManager target, float damage)
    {
        Vector3 TargetLocation;
        if (projectile.GetComponent<explosion>())
        {
            TargetLocation = (projectile.transform.position - target.transform.position).normalized * target.cMover.getMaxSpeed()
                * target.myStats.getTenacityMultiplier() + target.transform.position;
        }
        else
        {
            TargetLocation = (source.transform.position - target.transform.position).normalized * target.cMover.getMaxSpeed() * target.myStats.getTenacityMultiplier() + target.transform.position;
        }
        
        target.metaStatus.Fear(GetComponent<OnHitContainer>().myManager, this, false, TargetLocation,  duration);
        return damage;
    }
    /*
    public override void applyTo(GameObject source, UnitManager target)
    {

        target.metaStatus.Fear(SourceManager, this,false, duration);
        
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

            target.GiveOrder(Orders.CreateMoveOrder(RunToSide ? transform.position + Vector3.left * 20 : TargetLocation));
            // Give an order to then continue attack moving?
            */
        

}