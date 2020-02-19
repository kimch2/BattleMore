using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMSpawnUnit : Ability
{
    public GameObject ToSpawn;
    public int spawnCount;
    public override void Start() // We have this here so other source can call Start and any of this guy's inheriters will have it called instead
    {
        myType = type.activated;
    }

    public override void Activate()
    {
        myCost.payCost();
        UnitManager unitMan = ToSpawn.GetComponent<UnitManager>();
        for (int i = 0; i < spawnCount; i++)
        {
           GameObject newGuy = unitMan.CreateInstance(getSpawnLocation() + Vector3.forward * i * 5, myManager.PlayerOwner);
           newGuy.GetComponent<UnitManager>().GiveOrder(Orders.CreateAttackMove(transform.position + Vector3.right * 500, true));
        }
    }

    public override continueOrder canActivate(bool error)
    {
        return new continueOrder(myCost.canActivate(this), false);
    }

    public override void setAutoCast(bool offOn)
    {
     
    }

    Vector3 getSpawnLocation()
    {
        return transform.position + Vector3.left * 15;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(getSpawnLocation(), 1);
    }

}
