using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepEffect : IEffect
{

    public float sleepTime = 5;
    float TimePutToSleep;

    public override void BeginEffect()
    {

    }


    public override bool validTarget(GameObject target)
    {
        return true;
    }

    public override void applyTo(GameObject source, UnitManager target)
    {
        target.metaStatus.Sleep(SourceManager, this, false, sleepTime);
    }

}
