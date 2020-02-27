using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassSalvage : Ability
{
    [Tooltip("should be between 0-1, 1 being 100% of the original Cast Cost")]
    public float ResourceReturnRate = 1;
    public List<string> UnitsToIgnore;


    public override void Start() // We have this here so other source can call Start and any of this guy's inheriters will have it called instead
    {
        myType = type.activated;
    }

    public override void Activate()
    {
        myCost.payCost();

        foreach (KeyValuePair<string, List<UnitManager>> pairs in myManager.myRacer.getUnitList())
        {
            if (!UnitsToIgnore.Contains(pairs.Key))
            {
                foreach (UnitManager man in pairs.Value)
                {
                    if (man != myManager)
                    {
                        float resources = man.myStats.cost / man.myStats.supply;
                        man.myStats.kill(null); // This won't work if they are invulnerable, Need a sacrifice outlet?
                        myManager.myStats.changeEnergy(resources);
                    }
                }
            }
        }   
    }

    public override continueOrder canActivate(bool error)
    {
        return new continueOrder(true, false);
    }

    public override void setAutoCast(bool offOn)
    {

    }

}
