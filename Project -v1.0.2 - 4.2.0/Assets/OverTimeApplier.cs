using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverTimeApplier : VisionTrigger
{

    public OnHitContainer myHitContainer;
    public List<UnitTypes.UnitTypeTag> CantTarget;
   

    public override void UnitEnterTrigger(UnitManager manager)
    {
        foreach (UnitTypes.UnitTypeTag typ in CantTarget)
        {
            if (manager.myStats.isUnitType(typ))
            {
                return;
            }
        }
        myHitContainer.trigger(null,manager,0);
    }

    public override void UnitExitTrigger(UnitManager manager)
    {
        foreach (UnitTypes.UnitTypeTag typ in CantTarget)
        {
            if (manager.myStats.isUnitType(typ))
            {
                return;
            }
        }
        myHitContainer.RemoveEffect(manager);
    }
}
