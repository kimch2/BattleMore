using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassSalvage : TargetAbility
{
    [Tooltip("should be between 0-1, 1 being 100% of the original Cast Cost")]
    public float ResourceReturnRate = 1;
    public List<string> UnitsToIgnore;


    public override void Start() // We have this here so other source can call Start and any of this guy's inheriters will have it called instead
    {
        myType = type.target;
    }

    public override void Activate()
    {
  
    }

    public override continueOrder canActivate(bool error)
    {
        return new continueOrder(true, false);
    }

    public override void setAutoCast(bool offOn)
    {

    }

    public override void Cast()
    {
        myCost.payCost();

        foreach (UnitManager pairs in GetUnitsInRange( location, myManager.PlayerOwner, areaSize))
        {
            if (!UnitsToIgnore.Contains(pairs.UnitName))
            {
                float resources = pairs.myStats.cost / pairs.myStats.supply;
                pairs.myStats.kill(null); // This won't work if they are invulnerable, Need a sacrifice outlet?
                myManager.myStats.changeEnergy(resources);     
            }
        }

    }

    public override bool Cast(GameObject target, Vector3 location)
    {

        return true;
    }

    public override bool isValidTarget(GameObject target, Vector3 location)
    {
        return true;
    }
}
