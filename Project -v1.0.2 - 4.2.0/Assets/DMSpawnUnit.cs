using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMSpawnUnit : Ability
{
    public GameObject ToSpawn;
    public int spawnCount;
    public bool SpawnAtScreenEdge = true;
    public override void Start() // We have this here so other source can call Start and any of this guy's inheriters will have it called instead
    {
        myType = type.activated;
    }

    public override void Activate()
    {
        if (myCost)
        {
            myCost.payCost();
        }
        UnitStats potentSpawn = ToSpawn.GetComponent<UnitStats>();
        if (!potentSpawn)
        {
            potentSpawn = ToSpawn.GetComponentInChildren<UnitStats>();
        }
      

        for (int i = 0; i < spawnCount; i++)
        {
            if (potentSpawn.supply > 0 && myManager.myRacer.hasSupplyAvailable(potentSpawn.supply))
            {
            float offset = (i+1) * 6 - spawnCount * 3 ;
            GameObject newGuy = GameObject.Instantiate<GameObject>(ToSpawn, getSpawnLocation() + Vector3.forward * offset, Quaternion.identity);// unitMan.CreateInstance(getSpawnLocation() + Vector3.forward * i * 5, myManager.PlayerOwner);
                
            foreach (UnitManager man in newGuy.GetComponentsInChildren<UnitManager>())
            {
                if (myCost)
                {
                   man.myStats.cost = myCost.energy;
                }
                man.myStats.supply = 1;
                myManager.Initialize(myManager.PlayerOwner, true, man.getUnitStats().isUnitType(UnitTypes.UnitTypeTag.Structure));
                if (man.cMover)
                {
                    man.GiveOrder(Orders.CreateAttackMove(transform.position + Vector3.right * 75, true));
                    man.GiveOrder(Orders.CreateAttackMove(transform.position + Vector3.right * 150, true));
                    man.GiveOrder(Orders.CreateAttackMove(transform.position + Vector3.right * 250, true));
                    man.GiveOrder(Orders.CreateAttackMove(transform.position + Vector3.right * 350, true));
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
        if (SpawnAtScreenEdge)
        {
            return DaminionsInitializer.main.getSpawnLocation( myManager.PlayerOwner, spawnCount < 4);  
        }
        else
        {
            return myManager.transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(getSpawnLocation(), 1);
    }

}
