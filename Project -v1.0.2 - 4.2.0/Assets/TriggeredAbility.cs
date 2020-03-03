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
    [Tooltip("Used for some - RepeatTimer - how often, OnSpellCast - playerNumber to listen to")]
    public float VariableNumber = 1;




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
        else if (triggerType == TriggerType.OnAttack)
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
                GameManager.main.playerList[1].addDeathWatcher(this);
            }
            else
            {
                GameManager.main.playerList[0].addDeathWatcher(this);
            }
        }
        else if (triggerType == TriggerType.OnAllyDeath)
        {
            if (myManager.PlayerOwner == 1)
            {
                GameManager.main.playerList[0].addDeathWatcher(this);
            }
            else
            {
                GameManager.main.playerList[1].addDeathWatcher(this);
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
        Trigger();
        return damage;
    }


    public virtual float trigger(GameObject source, GameObject projectile, UnitManager target, float damage)
    {
        Trigger();
        return damage;
    }

    public virtual bool lethalDamageTrigger(UnitManager unit, GameObject deathSource)
    {
        Trigger();
        return true;
    }
}