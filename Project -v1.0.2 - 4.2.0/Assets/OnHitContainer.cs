using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitContainer : MonoBehaviour
{
   // public List<IWeapon> MyWeapons;

    public List<Notify> DamageTriggers = new List<Notify>(); // This is for things that specifically need to know how much damage is being dealt, but don't change it.
    public List<IEffect> toApply = new List<IEffect>();
    public UnitManager myManager;
    GameObject source; // Will become null once source unit dies, if it wasn't already (world effect)

    public float FriendlyFireRatio;

    [Tooltip("Will add all Ieffects and Notify Triggers already attached to this gameobject on Start()")]
    public bool AutoAddTriggers = true;

    private void Start()
    {
        if (!myManager)
        {
            myManager = GetComponentInParent<UnitManager>();
            if (!myManager && transform.parent) {
                myManager = transform.parent.GetComponentInParent<UnitManager>();

            }
        }
        if (!myManager)
        {
            source = myManager.gameObject;
        }
        if (AutoAddTriggers)
        {
            foreach (Notify not in GetComponents<Notify>())
            {
                AddDamageTrigger(not);
            }
            foreach (IEffect not in GetComponents<IEffect>())
            {
               AddEffectApplier(not);
            }
        }
    }

    public void AddDamageTrigger(Notify toAdd)
    {
        if (!DamageTriggers.Contains(toAdd))
        {
            DamageTriggers.Add(toAdd);
        }
    }

    public void AddEffectApplier(IEffect toAdd)
    {
        if (!toApply.Contains(toAdd))
        {
            toAdd.SetManagers(myManager, null);
            toApply.Add(toAdd);
        }
    }



    // add functions that modify base variables on projectiles and explosions, such as travel speed and explosion radius.

    public static OnHitContainer CreateDefaultContainer(UnitManager toApply, string name)
    {
        GameObject obj = new GameObject(name + ":hitContainer");
        obj.transform.parent = toApply.transform;
        obj.transform.localPosition = Vector3.zero;
        OnHitContainer HitContainer = obj.AddComponent<OnHitContainer>();
        HitContainer.Initialize(toApply);
        return HitContainer;
    }

    public void Initialize(UnitManager toSet)
    {
        myManager = toSet;
        source = toSet.gameObject;
    }

    public void Dying()
    {
        Detach();
    }

    public void Detach()
    {
        transform.parent = null;
        Invoke("SelfDestruct", 15);
    }

    public void trigger(GameObject proj, UnitManager target, float Damage)
    {
        if (target)
        {
            //if (!myManager || myManager.gameObject != target) // Applies to the source if it hits it? I think yes
            
            foreach (IEffect fect in toApply)
            {
                fect.applyTo(source, target);                
            }

            foreach (Notify mod in DamageTriggers)
            {
                mod.trigger(source, proj, target, Damage);
            }           
        }
    }

    // This going to work?? (on things that we apply a copy of the effect to)
    public void RemoveEffect(UnitManager target)
    {
        foreach (IEffect fect in toApply)
        {
            fect.RemoveEffect(target);
        }
    }

    public void SelfDestruct()
    {
        if(this.gameObject)
        Destroy(this.gameObject);
    }
}

// Scripts/prefabs that will need to be fixed  with this new class
// HalfLifeStarter, FearEffect, EmBomb, ExplodeOnDeath
// InflectionStarter
// SPlitterShot, OnTriggerApply, SingleTarget
// StunStrike