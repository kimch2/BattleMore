using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepEffect : IEffect
{
    public float sleepTime = 5;
    float TimePutToSleep;
    [Tooltip("This triggers if the unit sleeps the whole time")]
    public InvokeGameObject OnFinishNap;

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
        StartCoroutine(CheckIfAlseep(target));
    }

    IEnumerator CheckIfAlseep(UnitManager target)
    {
        yield return new WaitForSeconds(sleepTime - .1f);
        if (target && target.getState() is SleepState)
        {
            OnFinishNap.Invoke(target.gameObject);
        }
    }
}
