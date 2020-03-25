using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverTimeApplier : VisionTrigger
{
    // Is used for an area affect on that ground that applys to units when they enter and 
    // ends when they leave

    public OnHitContainer myHitContainer;
    public List<UnitTypes.UnitTypeTag> CantTarget;
    public float duration = 5;
    [Tooltip("This apply the effect to all units inside every X seconds if this is not 0")]
    public float RepeatPeriod;

    private void Awake()
    {
        if (duration > 0)
        {
            Invoke("TurnOff", duration);
        }
        if (RepeatPeriod > 0)
        {
            InvokeRepeating("TriggerAll",RepeatPeriod, RepeatPeriod);
        }
    }

    private void Start()
    {
        if (myHitContainer)
        {
            UnitManager sourceMan = myHitContainer.myManager;
            if (AppliesToAllies && AppliesToEnemies)
            {
                PlayerNumber = 1;
                AdditionaPlayerNums.Add(2);
            }
            else if (AppliesToAllies)
            {
                if (sourceMan.PlayerOwner == 1)
                {
                    PlayerNumber = 1;
                }
                else
                {
                    PlayerNumber = 2;
                }               
            }
            else if (AppliesToEnemies)
            {
                if (sourceMan.PlayerOwner == 1)
                {
                    PlayerNumber = 2;
                }
                else
                {
                    PlayerNumber = 1;
                }
            }
        }
    }

    private void TriggerAll()
    {
        foreach (UnitManager manag in InVision)
        {
            myHitContainer.trigger(null, manag, 0);
        }
    }

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



    public void TurnOff()
    {
        foreach (UnitManager man in InVision)
        {
            UnitExitTrigger(man);
        }
        Destroy(this.gameObject);
    }
}
