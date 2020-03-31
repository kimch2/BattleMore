using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogBank : OverTimeApplier
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 15)
        {
            if (other.isTrigger)
            {
                return;
            }

            Projectile proj = other.GetComponent<Projectile>();

            if (proj.MyHitContainer.playerNumber != myHitContainer.playerNumber)
            {
                proj.damage = 0;               
            }
        }
    }

}
