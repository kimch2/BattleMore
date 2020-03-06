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
        UnitStats potentSpawn = ToSpawn.GetComponent<UnitStats>();
        if (!potentSpawn)
        {
            potentSpawn = ToSpawn.GetComponentInChildren<UnitStats>();
        }
      

        for (int i = 0; i < spawnCount; i++)
        {
            if (potentSpawn.supply > 0 && myManager.myRacer.hasSupplyAvailable(potentSpawn.supply))
            {
                float offset = spawnCount * 10 - i * 5;
            GameObject newGuy = GameObject.Instantiate<GameObject>(ToSpawn, getSpawnLocation() + Vector3.forward * offset, Quaternion.identity);// unitMan.CreateInstance(getSpawnLocation() + Vector3.forward * i * 5, myManager.PlayerOwner);

            foreach (UnitManager man in newGuy.GetComponentsInChildren<UnitManager>())
            {
                man.myStats.cost = myCost.energy;
                myManager.Initialize(myManager.PlayerOwner, true, man.getUnitStats().isUnitType(UnitTypes.UnitTypeTag.Structure));
                if (man.cMover)
                {
                    man.GiveOrder(Orders.CreateAttackMove(transform.position + Vector3.right * 75, true));
                }
            }
        }
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
        if (myManager.PlayerOwner == 1)
        {
            return CarbotCamera.singleton.getLeftScreenEdge(myManager.transform.position, 10);
        }
        else
        {
            return CarbotCamera.singleton.getRightScreenEdge(myManager.transform.position, 10);
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(getSpawnLocation(), 1);
    }

}
