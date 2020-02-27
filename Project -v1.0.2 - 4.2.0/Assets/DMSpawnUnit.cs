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

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject newGuy = GameObject.Instantiate<GameObject>(ToSpawn);// unitMan.CreateInstance(getSpawnLocation() + Vector3.forward * i * 5, myManager.PlayerOwner);
            newGuy.transform.position = getSpawnLocation() + Vector3.forward * i * 5;
            // Debug.Log(transform.position + "   " + (transform.position + Vector3.right * 75));
            foreach (UnitManager man in newGuy.GetComponentsInChildren<UnitManager>())
            {

                myManager.Initialize(myManager.PlayerOwner, true, man.getUnitStats().isUnitType(UnitTypes.UnitTypeTag.Structure));
                if (man.cMover)
                {                  
                    man.GiveOrder(Orders.CreateAttackMove(transform.position + Vector3.right * 75, true));
                }
            } 
          // newGuy.GetComponent<UnitManager>().GiveOrder(Orders.CreateAttackMove(transform.position + Vector3.right * 75, true));
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
