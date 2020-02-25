﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaStatus
{
    //Class that handles all interactions regarding stuns, silences, fears, roots etc.

    UnitManager myManager;

    public enum statusType { Stun, Root, Silence, Pacify, Taunt, Fear, CastMove, Bless, Curse, Channel }
    Dictionary<statusType, List<StatusEffect>> cachedStatus = new Dictionary<statusType, List< StatusEffect>>();

    // Do we need these?
    public bool CanRecieveRightClick; // Stun, Taunt, Fear   
    public bool canMove = true; // Stun, Root
    public bool canCast = true; //Stun, Silence, CastMove
    public bool canAttack = true; // Stun, Pacify, Fear, CastMove
    public bool CanBuff = true; // Curse
    public bool canDebuff = true; // Bless

    public MetaStatus(UnitManager manager)
    {
        myManager = manager;
    }

    List<StatusEffect> GetStatusList(statusType myType)
    {
        if (cachedStatus.ContainsKey(myType))
        {
            return cachedStatus[myType];
        }

        List<StatusEffect> alter = new List<StatusEffect>();// (myManager);
        cachedStatus.Add(myType, alter);
        return alter;
    }

    void StoreStatus(statusType theType, UnitManager sourceunit, UnityEngine.Object sourceComponent, bool friendly, float duration = 0)
    {
        StatusEffect effect = new StatusEffect(sourceunit, sourceComponent, friendly, duration);
        GetStatusList(theType).Add(effect);
    }

    // Returns true if there are no more effects of this type currently on this unit
    bool UnCacheStatus(statusType theType, UnityEngine.Object sourceComponent)
    {
        List<StatusEffect> list = GetStatusList(theType);
        list.RemoveAll(item => item.sourceComp ==  sourceComponent);
        if (list.Count == 0) // Delete the list if its size is zero? probably not, to cut down on memory management, But we do save on computations elswhere
        {
            cachedStatus.Remove(theType);
            return true;
        }
        return false;     
    }

    //===============================================================================================
    //===============================================================================================

    void DisableMovement()
    {
        myManager.cMover.enabled = false;
        canMove = false;
    }

    void DisableAttacks()
    {
        canAttack = false;
    }

    void DisableRightClicks()
    {
        CanRecieveRightClick = false;
    }

    void DisableCasting()
    {
        canCast = false;
    }

    void DisableBuffing()
    {
       CanBuff = false;
    }

    void DisableDeBuffing()
    {
        canDebuff = false;
    }

    
    void EnableMovement()
    {
        if (!cachedStatus.ContainsKey(statusType.Stun) && !cachedStatus.ContainsKey(statusType.Root))
        {
            myManager.cMover.enabled = true;
            canMove = true;
        }
    }

    void EnableAttacks()
    {
        if (!cachedStatus.ContainsKey(statusType.Stun) && !cachedStatus.ContainsKey(statusType.CastMove) 
         && !cachedStatus.ContainsKey(statusType.Channel) && !cachedStatus.ContainsKey(statusType.Pacify))
        {
            canAttack = true;
        }
    }

    void EnableRightClicks()
    {
        if (!cachedStatus.ContainsKey(statusType.Stun) && !cachedStatus.ContainsKey(statusType.Channel)
            && !cachedStatus.ContainsKey(statusType.Fear) && !cachedStatus.ContainsKey(statusType.Taunt))
        {
            CanRecieveRightClick = true;
        }
    }
    void EnableCasting()
    {
        if (!cachedStatus.ContainsKey(statusType.Stun) && !cachedStatus.ContainsKey(statusType.Silence)
           && !cachedStatus.ContainsKey(statusType.Taunt) && !cachedStatus.ContainsKey(statusType.Fear))
        {
            canCast = true;
        }
    }
    void EnableBuffing()
    {
        if (!cachedStatus.ContainsKey(statusType.Curse))
        { canCast = true; }
    }

    void EnableDeBuffing()
    {
        if (!cachedStatus.ContainsKey(statusType.Bless))
        { canCast = true; }
    }

    //==============================================================================================
    //==============================================================================================

    public void Stun(UnitManager sourceunit, UnityEngine.Object sourceComponent, bool friendly, float duration = 0)
    {
        StoreStatus(statusType.Stun, sourceunit, sourceComponent, friendly, duration);
        DisableMovement();
        DisableAttacks();
        DisableRightClicks();
        DisableCasting();
    }
    public void UnStun(UnityEngine.Object sourceComponent)
    { if (UnCacheStatus(statusType.Stun, sourceComponent))
        {
            EnableAttacks();
            EnableMovement();
            EnableRightClicks();
            EnableCasting();
        }
    }

    public void Channel(UnitManager sourceunit, UnityEngine.Object sourceComponent, float duration = 0)
    {
        StoreStatus(statusType.Channel, sourceunit, sourceComponent, true, duration);
        DisableMovement();
        DisableAttacks();
        DisableCasting(); // Maybe not this one for early canceling of channelAbilties?
    }
    public void UnChannel(UnityEngine.Object sourceComponent)
    { if (UnCacheStatus(statusType.Channel, sourceComponent))
        {
            EnableAttacks();
            EnableMovement();
            EnableCasting();
        }
    }

    public void Root(UnitManager sourceunit, UnityEngine.Object sourceComponent, bool friendly, float duration = 0)
    {
        StoreStatus(statusType.Root, sourceunit, sourceComponent, friendly, duration);
        DisableMovement();
    }
    public void UnRoot(UnityEngine.Object sourceComponent)
    { if (UnCacheStatus(statusType.Root, sourceComponent))
        {
            EnableMovement();
        }
    }

    public void Fear(UnitManager sourceunit, UnityEngine.Object sourceComponent, bool friendly, float duration = 0)
    {
        StoreStatus(statusType.Fear, sourceunit, sourceComponent, friendly, duration);
        DisableRightClicks();
    }
    public void UnFear(UnityEngine.Object sourceComponent)
    { if (UnCacheStatus(statusType.Fear, sourceComponent))
        {
            EnableRightClicks();
        }
    }

    public void Taunt(UnitManager sourceunit, UnityEngine.Object sourceComponent, bool friendly, float duration = 0)
    {
        StoreStatus(statusType.Taunt, sourceunit, sourceComponent, friendly, duration);
        DisableRightClicks();
        DisableCasting();
    }
    public void UnTaunt(UnityEngine.Object sourceComponent)
    {if (UnCacheStatus(statusType.Taunt, sourceComponent))
        {
            EnableRightClicks();
            EnableCasting();
        }
    }

    public void Silence(UnitManager sourceunit, UnityEngine.Object sourceComponent, bool friendly, float duration = 0)
    {
        StoreStatus(statusType.Silence, sourceunit, sourceComponent, friendly, duration);
        DisableCasting();
    }
    public void UnSilence(UnityEngine.Object sourceComponent)
    { if (UnCacheStatus(statusType.Silence, sourceComponent))
        {
            EnableCasting();
        }
    }

    public void Pacify(UnitManager sourceunit, UnityEngine.Object sourceComponent, bool friendly, float duration = 0)
    {
        StoreStatus(statusType.Pacify, sourceunit, sourceComponent, friendly, duration);
        DisableAttacks();
    }
    public void UnPacify(UnityEngine.Object sourceComponent)
    { if (UnCacheStatus(statusType.Pacify, sourceComponent))
        {
            EnableAttacks();
        }
    }

    //Like a channel but can still move 
    public void MoveCast(UnitManager sourceunit, UnityEngine.Object sourceComponent, bool friendly, float duration = 0)
    {
        StoreStatus(statusType.CastMove, sourceunit, sourceComponent, friendly, duration);
        DisableAttacks();
        DisableCasting();
    }
    public void UnMoveCast(UnityEngine.Object sourceComponent)
    { if (UnCacheStatus(statusType.CastMove, sourceComponent))
        {
            EnableAttacks();
            EnableCasting();
        }
    }

    // Cannot recieve Debuffs
    public void Bless(UnitManager sourceunit, UnityEngine.Object sourceComponent, bool friendly, float duration = 0)
    {
        StoreStatus(statusType.Bless, sourceunit, sourceComponent, friendly, duration);
        DisableDeBuffing();
    }
    public void UnBless(UnityEngine.Object sourceComponent)
    { if (UnCacheStatus(statusType.Bless, sourceComponent))
        {
            EnableDeBuffing();
        }
    }

    //Cannot Recieve buffs
    public void Curse(UnitManager sourceunit, UnityEngine.Object sourceComponent, bool friendly, float duration = 0)
    {
        StoreStatus(statusType.Bless, sourceunit, sourceComponent, friendly, duration);
        DisableBuffing();
    }
    public void UnCurse(UnityEngine.Object sourceComponent)
    { if (UnCacheStatus(statusType.Curse, sourceComponent))
        { EnableBuffing(); }
    }

    // Removes all Debuffs
    public void Cleanse(UnitManager sourceunit, UnityEngine.Object sourceComponent, bool friendly)
    {foreach (KeyValuePair<statusType, List<StatusEffect>> pair in cachedStatus)
        {
            foreach (StatusEffect eff in pair.Value)
            {
                if (!eff.friendly)
                {
                    RemoveEffect(pair.Key, eff.sourceComp);
                }
            }
        }
    }

    //Removes all Buffs
    public void DeCleanse(UnitManager sourceunit, UnityEngine.Object sourceComponent, bool friendly)
    {
        foreach (KeyValuePair<statusType, List<StatusEffect>> pair in cachedStatus)
        {
            foreach (StatusEffect eff in pair.Value)
            {
                if (eff.friendly)
                {
                    RemoveEffect(pair.Key, eff.sourceComp);
                }
            }
        }
    }

    //This is a little less efficient than simply calling the specific kind of function to remove the effect
    public void RemoveEffect(statusType theType, UnityEngine.Object sourceComp)
    {
        switch (theType)
        {
            case statusType.Bless:
                UnBless(sourceComp);
                break;

            case statusType.CastMove:
                UnMoveCast(sourceComp);
                break;

            case statusType.Channel:
                UnChannel(sourceComp);
                break;

            case statusType.Curse:
                UnCurse(sourceComp);
                break;

            case statusType.Fear:
                UnFear(sourceComp);
                break;

            case statusType.Pacify:
                UnPacify(sourceComp);
                break;

            case statusType.Root:
                UnRoot(sourceComp);
                break;

            case statusType.Silence:
                UnSilence(sourceComp);
                break;

            case statusType.Stun:
                UnStun(sourceComp);
                break;

            case statusType.Taunt:
                UnTaunt(sourceComp);
                break;
        }
        
    }

    class StatusEffect {

        public UnitManager SourceManager;
        public UnityEngine.Object sourceComp;
        public float EndTime;
        public bool friendly;

        public StatusEffect(UnitManager myManager, UnityEngine.Object sourceC, bool friend , float Duration)
        {         
            SourceManager = myManager;
            sourceComp = sourceC;
            friendly = friend;
            if (friendly)
            {
                EndTime = Time.time + Duration;
            }
            else
            { // We apply the tenacity modifier only if its an unfriendly effect
                EndTime = Time.time + Duration * myManager.myStats.getTenacityMultiplier();
            }
        }
    }
}