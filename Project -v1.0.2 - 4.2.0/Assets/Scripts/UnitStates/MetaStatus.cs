using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaStatus
{
    //Class that handles all interactions regarding stuns, silences, fears, roots etc.

    UnitManager myManager;

    public enum statusType { Stun, Root, Silence, Pacify, Taunt, Fear, CastMove, Bless, Curse, Channel, Sleep }
    Dictionary<statusType, List<StatusEffect>> cachedStatus = new Dictionary<statusType, List<StatusEffect>>();
    Dictionary<statusType, EffectTag> CachedFX = new Dictionary<statusType, EffectTag>();
    // Do we need these?
    public bool CanRecieveRightClick = true; // Stun, Taunt, Fear   
    public bool canMove = true; // Stun, Root
    public bool canCast = true; //Stun, Silence, CastMove
    public bool canAttack = true; // Stun, Pacify, Fear, CastMove
    public bool CanBuff = true; // Curse
    public bool canDebuff = true; // Bless

    int statusMask;

    public MetaStatus(UnitManager manager)
    {
        myManager = manager;
    }

    List<StatusEffect> TimerQueue = new List<StatusEffect>();



    public void Update()
    {
        // May need to optimize this at some point, maybe a while loop instead, or someway to turn itself off if there is nothing to update.
        for (int i = 0; i < TimerQueue.Count; i++)
        {
            if (TimerQueue[i].EndTime < Time.time)
            {
                CancelEffect(TimerQueue[0]);
                //TimerQueue.RemoveAt(0);
                i--;
            }
            else
            {
                break;
            }
        }
    }

    List<StatusEffect> GetStatusList(statusType myType)
    {
        List<StatusEffect> toReturn;

        if (cachedStatus.TryGetValue(myType, out toReturn))
        {
            return toReturn;
        }

        toReturn = new List<StatusEffect>();// (myManager);
        cachedStatus.Add(myType, toReturn);       

        return toReturn;
    }

    /// <summary>
    /// Returns true if it the first effect from this source
    /// </summary>
    /// <param name="theType"></param>
    /// <param name="sourceunit"></param>
    /// <param name="sourceComponent"></param>
    /// <param name="friendly"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    bool StoreStatus(statusType theType, UnitManager sourceunit, UnityEngine.Object sourceComponent, bool friendly, float duration = 0)
    {
        List<StatusEffect> currentEffects = GetStatusList(theType);

        bool AlreadyExists = false;
        StatusEffect effect = null;
        for (int i = 0; i < currentEffects.Count; i++)
        {
            if (currentEffects[i].sourceComp == sourceComponent)
            {
                AlreadyExists = true;
                effect = currentEffects[i];
                break;
            }
        }
      
        if (AlreadyExists)
        {
            effect.ResetDuration(friendly ? duration : myManager.myStats.getTenacityMultiplier() * duration);
        }
        else
        {
            effect = new StatusEffect(sourceComponent, theType, friendly, friendly ? duration : myManager.myStats.getTenacityMultiplier() * duration);
        }

        if (duration > 0)
        {
            if (AlreadyExists)
            {
                TimerQueue.Remove(effect);
            }
            else
            {
                currentEffects.Add(effect);
            }

            if (TimerQueue.Count > 0)
            {
                for (int i = 0; i < TimerQueue.Count; i++)
                {
                    if (effect.EndTime < TimerQueue[i].EndTime)
                    {
                        TimerQueue.Insert(i, effect);  // IN THEORY, We only need to check the first thing in the list if they are ordered by when they end.
                        break;
                    }
                }
            }
            else
            {
                TimerQueue.Add(effect);
            }
        }

        return !AlreadyExists;
    }

    // Returns true if there are no more effects of this type currently on this unit
    bool UnCacheStatus(statusType theType, UnityEngine.Object sourceComponent)
    {
        List<StatusEffect> list;

        if (cachedStatus.TryGetValue(theType, out list))
        {
            for (int i = TimerQueue.Count - 1; i > -1; i--)
            {
                if (TimerQueue[i].sourceComp == sourceComponent)
                {
                    TimerQueue.RemoveAt(i);
                    break;
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].sourceComp == sourceComponent)
                {
                    list.RemoveAt(i);
                    break;
                }
            }
            //list.Remove(item => item.sourceComp == sourceComponent);

            return list.Count == 0;/*
            if (list.Count == 0) // Delete the list if its size is zero? probably not, to cut down on memory management, But we do save on computations elswhere
            {
                //cachedStatus.Remove(theType);
                return true;
            }
            else
            {
                return false;
            }*/
        }
        return true;
    }

    //===============================================================================================
    //===============================================================================================

    void DisableMovement()
    {
        if (myManager.cMover)
        {
            myManager.cMover.enabled = false;
            myManager.cMover.stop();
        }
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

    public bool HasSubscribers(statusType toCheck)
    {
        List<StatusEffect> toCompare;
        if (!cachedStatus.TryGetValue(statusType.Stun, out toCompare))
        {
            return false;
        }
        return toCompare.Count > 0;
    }

    void EnableMovement()
    {
        if (!HasSubscribers(statusType.Stun) && !HasSubscribers(statusType.Root)
            && !HasSubscribers(statusType.Sleep))
        {
            if (myManager.cMover)
            {
                myManager.cMover.enabled = true;
            }
            canMove = true;
        }
    }

    void EnableAttacks()
    {
        if (!HasSubscribers(statusType.Stun)    && !HasSubscribers(statusType.CastMove)
         && !HasSubscribers(statusType.Channel) && !HasSubscribers(statusType.Pacify)
         && !HasSubscribers(statusType.Sleep))
        {
            canAttack = true;
        }
    }

    void EnableRightClicks()
    {
        if (!HasSubscribers(statusType.Stun)    && !HasSubscribers(statusType.Channel)
            && !HasSubscribers(statusType.Fear) && !HasSubscribers(statusType.Taunt)
            && !HasSubscribers(statusType.Sleep))
        {
            CanRecieveRightClick = true;
        }
    }
    void EnableCasting()
    {
        if (!HasSubscribers(statusType.Stun)    && !HasSubscribers(statusType.Silence)
           && !HasSubscribers(statusType.Taunt) && !HasSubscribers(statusType.Fear)
           && !HasSubscribers(statusType.Sleep))
        { 
            canCast = true;
        }
    }

    void EnableBuffing()
    {
        if (!HasSubscribers(statusType.Curse) )
        { canCast = true; }
    }

    void EnableDeBuffing()
    {
        if (!HasSubscribers(statusType.Bless) )
        { canCast = true; }
    }

    //==============================================================================================
    //==============================================================================================


    //Returns null if its already active
    GameObject TurnOnFX(statusType theType)
    {
        EffectTag toReturn;

        if (CachedFX.TryGetValue(theType, out toReturn))
        {
            if (toReturn.FXObject.activeSelf)
            {
                return null;
            }
            else
            {
                toReturn.FXObject.SetActive(true);
                myManager.myStats.getEffectTagContainer().AddVisualFX(toReturn, false);
            }
        }
        else if (theType == statusType.Sleep)
        {
            GameObject FXObject = GenericEffectsManager.SleepEffect();
            toReturn = new EffectTag(FXObject, EffectTagContainer.TagLocation.HPBar);
            CachedFX.Add(statusType.Sleep, toReturn);
            myManager.myStats.getEffectTagContainer().AddVisualFX(toReturn, false);
        }
        else if (theType == statusType.Stun)
        {
            GameObject FXObject = GenericEffectsManager.StunEffect();
            toReturn = new EffectTag(FXObject, EffectTagContainer.TagLocation.HPBar);
            CachedFX.Add(statusType.Stun, toReturn);
            myManager.myStats.getEffectTagContainer().AddVisualFX(toReturn, false);
        }
        //To be continued

        return toReturn.FXObject;
    }

    void TurnOffFX(statusType theType)
    {
        EffectTag toReturn;

        if (CachedFX.TryGetValue(theType, out toReturn))
        {
            myManager.myStats.getEffectTagContainer().RemoveEffect(toReturn, false);
            toReturn.FXObject.SetActive(false);
        }
    }

    //==============================================================================================
    //==============================================================================================

    public void Stun(UnitManager sourceunit, UnityEngine.Object sourceComponent, bool friendly, float duration = 0)
    {

        if (StoreStatus(statusType.Stun, sourceunit, sourceComponent, friendly, duration))
        {
            DisableMovement();
            DisableAttacks();
            DisableRightClicks();
            DisableCasting();
            if (cachedStatus[statusType.Stun].Count == 1)
            {
                myManager.changeState(new StunState(myManager), true, false);
                if (duration > .3f)
                {
                    TurnOnFX(statusType.Stun);
                }
            }
        }
        

    }

    public void UnStun(UnityEngine.Object sourceComponent)
    {
        if (UnCacheStatus(statusType.Stun, sourceComponent))
        {
            EnableAttacks();
            EnableMovement();
            EnableRightClicks();
            EnableCasting();
            TurnOffFX(statusType.Stun);
            if (!(myManager.getState() is SleepState)) // shouldn't these be getting queued?
            {
                myManager.changeState(new DefaultState());
            }            
        }
    }

    public void Sleep(UnitManager sourceunit, UnityEngine.Object sourceComponent, bool friendly, float duration = 0)
    {
        if (StoreStatus(statusType.Sleep, sourceunit, sourceComponent, friendly, duration))
        {
            DisableMovement();
            DisableAttacks();
            DisableRightClicks();
            DisableCasting();
            if (cachedStatus[statusType.Sleep].Count == 1)
            {
                myManager.changeState(new SleepState(myManager), true, false);
                TurnOnFX(statusType.Sleep);
            }
        }
    }

    public void UnSleep()
    {
        TimerQueue.RemoveAll(item => item.statType == statusType.Sleep);
        cachedStatus.Remove(statusType.Sleep);

        EnableAttacks();
        EnableMovement();
        EnableRightClicks();
        EnableCasting();
        WakeUp();
    }

    public void UnSleep(UnityEngine.Object sourceComponent)
    {
        if (UnCacheStatus(statusType.Sleep, sourceComponent))
        {
            EnableAttacks();
            EnableMovement();
            EnableRightClicks();
            EnableCasting();

            WakeUp();
        }
    }

    void WakeUp()
    {
        TurnOffFX(statusType.Sleep);
        if (!(myManager.getState() is StunState)) // !cachedStatus.ContainsKey(statusType.Stun))
        {
            myManager.changeState(new DefaultState());
        }
        else
        {
            myManager.changeState(new StunState(myManager));
        }
    }


    public void Channel(UnitManager sourceunit, UnityEngine.Object sourceComponent, float duration = 0)
    {
        if (StoreStatus(statusType.Channel, sourceunit, sourceComponent, true, duration))
        {
            DisableMovement();
            DisableAttacks();
            DisableCasting(); // Maybe not this one for early canceling of channelAbilties?
        }
    }

    public void UnChannel(UnityEngine.Object sourceComponent)
    {
        if (UnCacheStatus(statusType.Channel, sourceComponent))
        {
            EnableAttacks();
            EnableMovement();
            EnableCasting();
        }
    }

    public void Root(UnitManager sourceunit, UnityEngine.Object sourceComponent, bool friendly, float duration = 0)
    {
        if (StoreStatus(statusType.Root, sourceunit, sourceComponent, friendly, duration))
        {
            DisableMovement();
        }
    }

    public void UnRoot(UnityEngine.Object sourceComponent)
    {
        if (UnCacheStatus(statusType.Root, sourceComponent))
        {
            EnableMovement();
        }
    }

    public void Fear(UnitManager sourceunit, UnityEngine.Object sourceComponent, bool friendly, Vector3 FearLocation, float duration = 0)
    {
        // if(cached)
        if (StoreStatus(statusType.Fear, sourceunit, sourceComponent, friendly, duration))
        {
            DisableRightClicks();
            //target.GiveOrder(Orders.CreateMoveOrder(RunToSide ? transform.position + Vector3.left * 20 : TargetLocation));

        }
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
    {
        if (UnCacheStatus(statusType.Silence, sourceComponent))
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
    {
        // ToDo Remove all effects from StatChanger in UnitStats
        foreach (KeyValuePair<statusType, List<StatusEffect>> pair in cachedStatus)
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
        // ToDo Remove all effects from StatChanger in UnitStats
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
    void CancelEffect(StatusEffect toRemove)
    {
        foreach (KeyValuePair<statusType, List<StatusEffect>> pair in cachedStatus)
        {
            if (pair.Value.Contains(toRemove))
            {
                RemoveEffect(pair.Key, toRemove.sourceComp);
                break;
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

            case statusType.Sleep:
                UnSleep(sourceComp);
                break;
        }
      

    }



    class StatusEffect {

      //  public UnitManager SourceManager;
        public UnityEngine.Object sourceComp;
        public float EndTime;
        public bool friendly;
        public statusType statType;
        //UnitManager sourceMan,
        public StatusEffect( UnityEngine.Object sourceC, statusType theType, bool friend , float Duration)
        {         
            //SourceManager = sourceMan;
            sourceComp = sourceC;
            friendly = friend;
            EndTime = Time.time + Duration;
            statType = theType;
        }

        public void ResetDuration(float duration)
        {
            EndTime = Time.time + duration;
        }
    }


    // =================================================================================================
    // =================OnHit Special FX======================================================

    Dictionary<string, SimpleAnimator> CachedEffects = new Dictionary<string, SimpleAnimator>();

    public void ShowCriticalHit()
    {
        SimpleAnimator anim;
        if (!CachedEffects.TryGetValue("Crit", out anim))
        {
            anim = GenericEffectsManager.CriticalHitEffect().GetComponent<SimpleAnimator>();
            CachedEffects.Add("Crit", anim);
            anim.gameObject.transform.parent = myManager.transform;
            anim.gameObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            anim.OnEnable();
        }
    }

    public void ShowHealing()
    {
        SimpleAnimator anim;
        if (!CachedEffects.TryGetValue("Heal", out anim))
        {
            anim = GenericEffectsManager.HealingEffect().GetComponent<SimpleAnimator>();
            CachedEffects.Add("Heal", anim);
            anim.gameObject.transform.parent = myManager.transform;
            anim.gameObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            anim.OnEnable();
        }
    }

    public void ShowStunEffect()
    {
        SimpleAnimator anim;
        if (!CachedEffects.TryGetValue("Stun", out anim))
        {
            anim = GenericEffectsManager.StunEffect().GetComponent<SimpleAnimator>();
            CachedEffects.Add("Stun", anim);
            anim.gameObject.transform.parent = myManager.transform;
            anim.gameObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            anim.OnEnable();
        }
    }

    public void ShowSlowEffect()
    {
        SimpleAnimator anim;
        if (!CachedEffects.TryGetValue("Slow", out anim))
        {
            anim = GenericEffectsManager.SlowMovementEffect().GetComponent<SimpleAnimator>();
            CachedEffects.Add("Slow",anim);
            anim.gameObject.transform.parent = myManager.transform;
            anim.gameObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            anim.OnEnable();
        }
    }

  
}