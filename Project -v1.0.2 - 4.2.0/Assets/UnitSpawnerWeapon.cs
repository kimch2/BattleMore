using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnerWeapon : IWeapon
{

    public int MaxUnitCount;
    public bool spawnAtScreenEdge;
    List<GameObject> CurrentUnits = new List<GameObject>();


    public override bool canAttack(UnitManager target)
    {
        if (!offCooldown)
        {
            Debug.Log("A");
            return false;
        }
        if (!target)
        {
            Debug.Log("B");
            return false;
        }

        // Account for height advantage
        float distance = Mathf.Sqrt((Mathf.Pow(transform.position.x - target.transform.position.x, 2) + Mathf.Pow(transform.position.z - target.transform.position.z, 2))) - target.CharController.radius;

        float verticalDistance = this.gameObject.transform.position.y - target.transform.position.y;
        if (distance > (range + (verticalDistance / 3)))
        {
            Debug.Log("C");
            return false;
        }

        foreach (UnitTypes.UnitTypeTag tag in cantAttackTypes)
        {
            if (target.myStats.isUnitType(tag))
            {
                Debug.Log("D");
                return false;
            }
        }

        CurrentUnits.RemoveAll(item => item == null);
        if (CurrentUnits.Count >= MaxUnitCount)
        {
            Debug.Log("E");
            return false;
        }
        Debug.Log("F");
        return true;
    }

    protected override GameObject createBullet()
    {
        GameObject newGuy = GameObject.Instantiate<GameObject>(projectile, getSpawnLocation(), Quaternion.identity);// unitMan.CreateInstance(getSpawnLocation() + Vector3.forward * i * 5, myManager.PlayerOwner);
        CurrentUnits.Add(newGuy);
        return newGuy;
    }

    Vector3 getSpawnLocation()
    {
        if (spawnAtScreenEdge)
        {
            return DaminionsInitializer.main.getSpawnLocation(myManager.PlayerOwner, numOfAttacks < 4);
        }
        else
        {
            return myManager.transform.position;
        }
    }
}
