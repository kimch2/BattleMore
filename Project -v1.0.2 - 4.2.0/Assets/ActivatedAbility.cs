using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedAbility : Ability
{

    public UnityEngine.Events.UnityEvent OnActivate;
    public UnityEngine.Events.UnityEvent OnTurnOff;

    [Tooltip("If 0 and this is activated, then it will turn off when it is activated again")]
    public float DurationWhenActivated;

    public override void Start()
    {
        base.Start();
        myType = type.activated;

    }

    public override void Activate()
    {
        autocast = !autocast;
        if (autocast)
        {
            audioSrc.PlayOneShot(soundEffect);
            Trigger();
            if (DurationWhenActivated > 0)
            {
                Invoke("TurnOff", DurationWhenActivated);
            }
        }
        else
        {
            TurnOff();
        }
    } 
    

    protected void Trigger()
    {
        OnActivate.Invoke();
        
    }

    protected void TurnOff()
    {
        OnTurnOff.Invoke();
    }




    public override continueOrder canActivate(bool error)
    {
        return BaseCanActivate(error);
    }

    public override void setAutoCast(bool offOn)
    {
        Activate();
    }
}
