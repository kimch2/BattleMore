using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepEffect : IEffect
{

    public float sleepTime = 5;
    float TimePutToSleep;


    public override bool canCast()
    {
        return true;
    }

    public override bool validTarget(GameObject target)
    {
        return true;
    }

    public override void applyTo(GameObject source, UnitManager target)
    {
        SleepEffect eff = target.gameObject.AddComponent<SleepEffect>();
        if (!eff)
        {
            eff = target.gameObject.AddComponent<SleepEffect>();
        }
        eff.PutToSleep(sleepTime);
    }

    public void PutToSleep(float duration)
    {
        sleepTime = duration;
        TimePutToSleep = Time.time;
        onTarget = true;
        GetComponent<UnitManager>().metaStatus.Sleep(null, this, false, sleepTime);
    }

    void WakeUp()
    {
        GetComponent<UnitManager>().metaStatus.UnSleep(this);
    }
}
