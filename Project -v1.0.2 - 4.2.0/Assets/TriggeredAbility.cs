using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredAbility : MonoBehaviour, Modifier
{
    public enum TriggerType {OnSpawn, OnDeath, RepeatTimer, OnDamaged, OnSpellCast}
    public List<TriggerData> TriggerEvents;

    public UnityEngine.Events.UnityEvent OnTrigger;

    private void Start()
    {
        if (TriggerEvents.FindAll(item => item.myTriggerType == TriggerType.OnSpawn).Count > 0)
        {
            OnTrigger.Invoke();
        }
        List<TriggerData> repeaters = TriggerEvents.FindAll(item => item.myTriggerType == TriggerType.RepeatTimer);
        if (repeaters.Count > 0)
        {
            InvokeRepeating("RepeatedInvoke", repeaters[0].variableNumber, repeaters[0].variableNumber);
        }

        if (TriggerEvents.FindAll(item => item.myTriggerType == TriggerType.OnDamaged).Count > 0)
        {
            UnitManager manager = GetComponent<UnitManager>();
            if (!manager)
            {
                manager = GetComponentInParent<UnitManager>();
                if (manager)
                { manager.myStats.addModifier(this); }
                else
                {
                    Debug.LogError("Could not find a Unitmanager on the TriggerAbility script for OnDamaged");
                }
            }       
        }


        // STILL NEED GLOBAL LISTENER FOR SPELLS CAST!
    }

    void RepeatedInvoke()
    {
        OnTrigger.Invoke();
    }



    public void Dying()
    {
        if (TriggerEvents.FindAll(item => item.myTriggerType == TriggerType.OnDeath).Count > 0)
        {
            OnTrigger.Invoke();
        }
    }

    public float modify(float damage, GameObject source, DamageTypes.DamageType theType)
    {
        OnTrigger.Invoke();
        return damage;
    }

}

public class TriggerData
{
    public TriggeredAbility.TriggerType myTriggerType;
    [Tooltip("How often on Repeat Timer or how many spells cast")]
    public float variableNumber =4;

}
