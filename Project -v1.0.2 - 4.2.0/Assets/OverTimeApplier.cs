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
        if (AbilityHeatMap.main && AppliesToEnemies)
        {
            AbilityHeatMap.main.AddCircleWarning(transform.position, GetComponent<SphereCollider>().radius * transform.localScale.x, this, 0, 100);
        }
    }

    private void Start()
    {
        if (myHitContainer)
        {
            PlayersToLookFor.Clear();
            UnitManager sourceMan = myHitContainer.myManager;
            PlayerOwner = sourceMan.PlayerOwner;
            if (AppliesToAllies && AppliesToEnemies)
            {
                PlayersToLookFor.Add(1);
                PlayersToLookFor.Add(2);
                ZoneName += "12";
            }
            else if (AppliesToAllies)
            {
                PlayersToLookFor.Add(1);
                ZoneName += "1";
            }
            else if (AppliesToEnemies)
            {
                PlayersToLookFor.Add(2);
                ZoneName += "2";
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
       // Debug.Log("seeing "+ manager);
            
        myHitContainer.trigger(null,manager,0);
    }

    public override void UnitExitTrigger(UnitManager manager)
    {
        if (manager)
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



    public void TurnOff()
    {
        CancelInvoke("TriggerAll");
        foreach (UnitManager man in InVision)
        {
            if (!StacksEffect)
            {
                Unstackables[ZoneName].Remove(man);
                if (!Unstackables[ZoneName].Contains(man))
                {
                    UnitExitTrigger(man);
                }
            }
            else
            {
                UnitExitTrigger(man);
            }
        }
        if (AbilityHeatMap.main && AppliesToEnemies)
        {
            AbilityHeatMap.main.RemoveArea(this);
        }

        Destroy(this.gameObject);
    }
}
