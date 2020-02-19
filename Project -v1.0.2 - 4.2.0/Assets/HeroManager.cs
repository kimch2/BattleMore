using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : UnitManager
{



    public void RootMovement(bool RootIt)
    {
        if (cMover)
        {
            cMover.Root(RootIt);
        }
    }

    public void DisableRotation(bool LockIt)
    {
        if (cMover)
        { cMover.LockRotation(LockIt); }
    }
}


public class HeroState
{
    public bool RootMovement;
    public bool DisableRotation;
    public bool DisableOtherAbilities;
    public Ability SourceAbility;
}