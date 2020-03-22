using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredAbility : ActivatedAbility, Modifier, Notify, LethalDamageinterface
{
    public enum TriggerType { OnSpawn, OnAttack, OnDamaged, OnLethalDamage, OnDeath,
                            RepeatTimer, OnSpellCast, OnAllyDeath, OnEnemyDeath, OnHealed }

    [Tooltip("If not checked, this will be a passive ability, Always on")]
    public bool Activatable;
    public TriggerType triggerType;
    [Tooltip("Used for some - RepeatTimer - how often, OnSpellCast - playerNumber to listen to, " +
        "OnAttack - Number of Attacks (0 =indefinite), OnAllyDeath/EnemyDeath - max range")]
    public float VariableNumber = 1;
    float hiddenVariableStored;
    public GameObject OnTriggerEffect;

    public override void Start()
    {
        base.Start();
                                             
        if (triggerType == TriggerType.OnSpawn)
        {
            OnActivate.Invoke();
            myType = type.passive;
            return;
        }

        if (Activatable)
        {
            myType = type.activated;
        }
        else
        {
            myType = type.passive;
            StartListening();
        }
    }

    public override void Activate()
    { 
         autocast = !autocast;
         if (autocast)
         {
             StartListening();
             if (DurationWhenActivated > 0)
                {
                    Invoke("TurnOff", DurationWhenActivated);
                }
         }
         else
         {
            StopListening();
            CancelInvoke("TurnOff");
         }       
    }


    void StartListening()
    {
        hiddenVariableStored = VariableNumber;
        if (triggerType == TriggerType.RepeatTimer)
        {
            InvokeRepeating("Fire", VariableNumber, VariableNumber);
        }
        else if (triggerType == TriggerType.OnDamaged)
        {
            if (myManager)
            {
                myManager.myStats.addModifier(this);
            }
            else
            {
                Debug.LogError("Could not find a Unitmanager on the TriggerAbility script for OnDamaged");
            }
        }
        else if (triggerType == TriggerType.OnSpellCast)
        {
            WorldRecharger.main.AddSpellCastNotifier(new int[1] { (int)VariableNumber }, this);
        }
        else if (triggerType == TriggerType.OnAttack && !GetComponent<OnHitContainer>()) // This is so if its on an onHitContainer, it doesn't add twice 
        {
            foreach (IWeapon myWeap in myManager.myWeapon) // WILL HAVE PROBLEMS IF WEAPONS CAN SWAP OUT
            {
                myWeap.addNotifyTrigger(this);
            }
        }
        else if (triggerType == TriggerType.OnDeath)
        {
            myManager.myStats.addDeathTrigger(this);
        }
        else if (triggerType == TriggerType.OnLethalDamage)
        {
            myManager.myStats.addLethalTrigger(this);
        }
        else if (triggerType == TriggerType.OnHealed)
        {
            myManager.myStats.addHealModifier(this);
        }
        else if (triggerType == TriggerType.OnEnemyDeath)
        {
            if (myManager.PlayerOwner == 1)
            {
                // MAYBE CHANGE THE FUNCTION CALLBACK INTERFACE TYPE? (Its hardly ever used)
                GameManager.main.playerList[1].addActualDeathWatcher(this);
            }
            else
            {
                GameManager.main.playerList[0].addActualDeathWatcher(this);
            }
        }
        else if (triggerType == TriggerType.OnAllyDeath)
        {
            if (myManager.PlayerOwner == 1)
            {
                GameManager.main.playerList[0].addActualDeathWatcher(this);
            }
            else
            {
                GameManager.main.playerList[1].addActualDeathWatcher(this);
            }
        }
    }

    void StopListening()
    {
        if (triggerType == TriggerType.RepeatTimer)
        {
            CancelInvoke("RepeatedInvoke");
        }
        else if (triggerType == TriggerType.OnDamaged)
        {
            if (myManager)
            {
                myManager.myStats.removeModifier(this);
            }
            else
            {
                Debug.LogError("Could not find a Unitmanager on the TriggerAbility script for OnDamaged");
            }
        }
        else if (triggerType == TriggerType.OnSpellCast)
        {
            WorldRecharger.main.RemoveSpellCaster(this);
        }
        else if (triggerType == TriggerType.OnAttack)
        {
            foreach (IWeapon myWeap in myManager.myWeapon)
            {
                myWeap.removeNotifyTrigger(this);
            }
        }
        else if (triggerType == TriggerType.OnDeath)
        {
            myManager.myStats.removeDeathTrigger(this);
        }
        else if (triggerType == TriggerType.OnLethalDamage)
        {
            myManager.myStats.removeLethalTrigger(this);
        }
        else if (triggerType == TriggerType.OnHealed)
        {
            myManager.myStats.removeHealModifier(this);
        }
        else if (triggerType == TriggerType.OnEnemyDeath)
        {
            if (myManager.PlayerOwner == 1)
            {
                GameManager.main.playerList[1].removeActualDeathWatcher(this);
            }
            else
            {
                GameManager.main.playerList[0].removeActualDeathWatcher(this);
            }
        }
        else if (triggerType == TriggerType.OnAllyDeath)
        {
            if (myManager.PlayerOwner == 1)
            {
                GameManager.main.playerList[0].removeActualDeathWatcher(this);
            }
            else
            {
                GameManager.main.playerList[1].removeActualDeathWatcher(this);
            }
        }
    }

    void RepeatedInvoke()
    {
       Trigger();
    }

    /// Used for OnAttack, OnDeath
    public virtual float modify(float damage, GameObject source, DamageTypes.DamageType theType)
    {
        Debug.Log("Attacking");
       
        Trigger();

        if (OnTriggerEffect)
        {
            Instantiate<GameObject>(OnTriggerEffect, myManager.transform.position +myManager.transform.forward*2, Quaternion.identity);
        }

        return damage;
    }


    public virtual float trigger(GameObject source, GameObject projectile, UnitManager target, float damage)
    {
        if (VariableNumber > 0)
        {
            hiddenVariableStored--;
            if (hiddenVariableStored == 0)
            {
                StopListening();
                CancelInvoke("TurnOff");
            }
        }
        if (OnTriggerEffect)
        {
            Instantiate<GameObject>(OnTriggerEffect, myManager.transform.position + myManager.transform.forward * 5, Quaternion.identity);
        }

        Trigger();
        return damage;
    }

    public virtual bool lethalDamageTrigger(UnitManager unit, GameObject deathSource)
    {      
        if (myManager &&  Vector3.Distance( unit.transform.position, myManager.transform.position) < VariableNumber)
        {         
            Trigger();
        }
        return true;
    }
}