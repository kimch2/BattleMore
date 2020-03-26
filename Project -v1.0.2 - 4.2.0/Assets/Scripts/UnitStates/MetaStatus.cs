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
    public bool CanRecieveRightClick; // Stun, Taunt, Fear   
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
        StatusEffect effect = new StatusEffect(sourceunit, sourceComponent, theType, friendly, friendly ? duration : myManager.myStats.getTenacityMultiplier() * duration);
        if (duration > 0)
        {
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
        GetStatusList(theType).Add(effect);
    }

    // Returns true if there are no more effects of this type currently on this unit
    bool UnCacheStatus(statusType theType, UnityEngine.Object sourceComponent)
    {
        List<StatusEffect> list;
        cachedStatus.TryGetValue(theType, out list);

        if (list != null)
        {
            for (int i = TimerQueue.Count - 1; i > -1; i--)
            {
                if (TimerQueue[i].sourceComp == sourceComponent)
                {
                    TimerQueue.RemoveAt(i);
                }
            }

            list.RemoveAll(item => item.sourceComp == sourceComponent);
            Debug.Log("List size is now " + list.Count);
            if (list.Count == 0) // Delete the list if its size is zero? probably not, to cut down on memory management, But we do save on computations elswhere
            {
                cachedStatus.Remove(theType);
                return true;
            }
            else
            {
                return false;
            }
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


    void EnableMovement()
    {
        if (!cachedStatus.ContainsKey(statusType.Stun) && !cachedStatus.ContainsKey(statusType.Root)
            && !cachedStatus.ContainsKey(statusType.Sleep))
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
        if (!cachedStatus.ContainsKey(statusType.Stun)    && !cachedStatus.ContainsKey(statusType.CastMove)
         && !cachedStatus.ContainsKey(statusType.Channel) && !cachedStatus.ContainsKey(statusType.Pacify)
         && !cachedStatus.ContainsKey(statusType.Sleep))
        {
            canAttack = true;
        }
    }

    void EnableRightClicks()
    {
        if (!cachedStatus.ContainsKey(statusType.Stun)    && !cachedStatus.ContainsKey(statusType.Channel)
            && !cachedStatus.ContainsKey(statusType.Fear) && !cachedStatus.ContainsKey(statusType.Taunt)
            && !cachedStatus.ContainsKey(statusType.Sleep))
        {
            CanRecieveRightClick = true;
        }
    }
    void EnableCasting()
    {
        if (!cachedStatus.ContainsKey(statusType.Stun)    && !cachedStatus.ContainsKey(statusType.Silence)
           && !cachedStatus.ContainsKey(statusType.Taunt) && !cachedStatus.ContainsKey(statusType.Fear)
           && !cachedStatus.ContainsKey(statusType.Sleep))
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


    //Returns null if its already active
    GameObject TurnOnFX(statusType theType)
    {
        EffectTag toReturn = null;
        CachedFX.TryGetValue(theType, out toReturn);

        if (toReturn != null)
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
        EffectTag toReturn = null;
        CachedFX.TryGetValue(theType, out toReturn);
        if (toReturn != null)
        {
            Debug.Log("Turning off effec " + toReturn.FXObject);
            myManager.myStats.getEffectTagContainer().RemoveEffect(toReturn, false);
            toReturn.FXObject.SetActive(false);
        }
        else
        {
            Debug.Log("Couldn't find it");
        }
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
        myManager.changeState(new StunState(myManager), true, false);
        TurnOnFX(statusType.Stun);
    }

    public void UnStun(UnityEngine.Object sourceComponent)
    {
        Debug.Log("Calling UNstun");
        if (UnCacheStatus(statusType.Stun, sourceComponent))
        {
            Debug.Log("Unstunned");
            EnableAttacks();
            EnableMovement();
            EnableRightClicks();
            EnableCasting();
            if (!cachedStatus.ContainsKey(statusType.Stun) && !cachedStatus.ContainsKey(statusType.Sleep))
            {
                myManager.changeState(new DefaultState());
            }
            TurnOffFX(statusType.Stun);
        }
    }

    public void Sleep(UnitManager sourceunit, UnityEngine.Object sourceComponent, bool friendly, float duration = 0)
    {
        StoreStatus(statusType.Sleep, sourceunit, sourceComponent, friendly, duration);
        DisableMovement();
        DisableAttacks();
        DisableRightClicks();
        DisableCasting();
        myManager.changeState(new SleepState(myManager), true, false);
        TurnOnFX(statusType.Sleep);

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
        if (!cachedStatus.ContainsKey(statusType.Stun))
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
       // if(cached)
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

        public UnitManager SourceManager;
        public UnityEngine.Object sourceComp;
        public float EndTime;
        public bool friendly;
        public statusType statType;

        public StatusEffect(UnitManager sourceMan, UnityEngine.Object sourceC, statusType theType, bool friend , float Duration)
        {         
            SourceManager = sourceMan;
            sourceComp = sourceC;
            friendly = friend;
            EndTime = Time.time + Duration;
            statType = theType;
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