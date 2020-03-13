using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverTimeApplier : VisionTrigger
{

    public OnHitContainer myHitContainer;

   

    public override void UnitEnterTrigger(UnitManager manager)
    {
        myHitContainer.trigger(null,manager,0);
    }

    public override void UnitExitTrigger(UnitManager manager)
    {
        myHitContainer.RemoveEffect(manager);
    }
}
