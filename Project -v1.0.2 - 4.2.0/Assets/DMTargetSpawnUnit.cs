using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMTargetSpawnUnit : TargetAbility
{
    public GameObject ToSpawn;
    public int spawnCount;
    public override void Start() // We have this here so other source can call Start and any of this guy's inheriters will have it called instead
    {
        myType = type.target;
    }

    public override void Activate()
    {
       
    }

    public override continueOrder canActivate(bool error)
    {
        return new continueOrder(myCost.canActivate(this), false);
    }

    public override void setAutoCast(bool offOn)
    {

    }

    public override void Cast()
    {
        myCost.payCost();
 
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject newGuy = GameObject.Instantiate<GameObject>(ToSpawn);// unitMan.CreateInstance(getSpawnLocation() + Vector3.forward * i * 5, myManager.PlayerOwner);
            newGuy.transform.position = location;
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

    public override bool Cast(GameObject target, Vector3 Location)
    {
        myCost.payCost();
        location = Location;
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject newGuy = GameObject.Instantiate<GameObject>(ToSpawn);// unitMan.CreateInstance(getSpawnLocation() + Vector3.forward * i * 5, myManager.PlayerOwner);
            newGuy.transform.position = location;
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
        return true;
    }

    public override bool isValidTarget(GameObject target, Vector3 location)
    {
        if (target && (target.layer == 8 || target.layer == 16))
        {
            return onPathableGround(location);
        }
       
        return true;
    }
}
