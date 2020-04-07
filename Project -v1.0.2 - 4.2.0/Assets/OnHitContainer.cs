using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitContainer : MonoBehaviour
{
   // public List<IWeapon> MyWeapons;

    public List<Notify> DamageTriggers = new List<Notify>(); // This is for things that specifically need to know how much damage is being dealt, but don't change it.
    public List<IEffect> toApply = new List<IEffect>();
    public UnityEngine.Events.UnityEvent OnKillGeneric;
    public InvokeGameObject onKillWithTarget;

    public UnitManager myManager;
    [HideInInspector]
    public GameObject source; // Will become null once source unit dies, if it wasn't already (world effect)
    //[HideInInspector]
    public int playerNumber;
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
        if (myManager)
        {
            source = myManager.gameObject;
            playerNumber = myManager.PlayerOwner;
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
            if (toAdd is DamagerIeffect)
            {
                ((DamagerIeffect)toAdd).myHitContainer = this;
            }
        }
    }


    // add functions that modify base variables on projectiles and explosions, such as travel speed and explosion radius.

    public static OnHitContainer CreateDefaultContainer(GameObject toApply, UnitManager  sourceManager, string ContainerName)
    {
        Debug.LogError("Creating default container for " + toApply);
        GameObject obj = new GameObject(ContainerName + ":hitContainer");
        obj.transform.parent = toApply.transform;
        obj.transform.localPosition = Vector3.zero;
        OnHitContainer HitContainer = obj.AddComponent<OnHitContainer>();
        HitContainer.source = toApply.gameObject;
        HitContainer.myManager = sourceManager;
        if (sourceManager)
        {
            HitContainer.playerNumber = sourceManager.PlayerOwner;
        }

        return HitContainer;
    }

    public void Initialize(UnitManager toSet)
    {
        myManager = toSet;
        source = toSet.gameObject;
        playerNumber = toSet.PlayerOwner;
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
            for (int i = toApply.Count - 1; i >= 0; i--)
            {
              //  Debug.Log("Applying Ieffect " + target);
                toApply[i].applyTo(source, target);
            }

            for (int i = DamageTriggers.Count - 1; i >= 0; i--)
            {
                DamageTriggers[i].trigger(source, proj, target, Damage);
            }
        }
    }

   

    public void RemoveEffect(UnitManager target)
    {
        foreach (IEffect fect in toApply)
        {
            System.Type type = fect.GetType();
            IEffect copy = null;

            foreach (Component comp in target.gameObject.GetComponents(type))
            {
                copy = (IEffect)comp.gameObject.GetComponent(type);
                if (copy.EffectName == fect.EffectName)
                {
                    copy.EndEffect();
                }
            }
        }       
    }

    public void RemoveNotifier(Component target)
    {
        DamageTriggers.Remove((Notify)target);
    }

    public void SelfDestruct()
    {
        if(this.gameObject)
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Record the damage done in the unit's Veteranstats. (keeps track of things like kills and whatnot)
    /// </summary>
    /// <param name="amount"></param>
    public void RecordDamageDone(float amount)
    {
        if (myManager)
        {
            myManager.myStats.veternStat.UpdamageDone(amount);
            // There is also a function in the Unitstats, that check if its a turret attached to antoher unit
            // But it's not well optimized so this will need to be fixed if we got back to battlemore or have things that need to reference 
            // another things stats as its own, such as summons. Which will probably quite soon actually. Might have been faster to fix it 
            // than to write this.
        }
    }

    public void UnitKilled(UnitManager manag)
    {
        OnKillGeneric.Invoke();
        onKillWithTarget.Invoke(manag.gameObject);

        if (myManager)
        {
            myManager.myStats.upKills();
        }
    }


    /// <summary>
    /// Returns true if the Thing was successfully set
    /// </summary>
    /// <param name="spawnedObject"></param>
    /// <returns></returns>
    public bool SetOnHitContainer(GameObject spawnedObject, float Damage, UnitManager target)
    {
        // Create a path that will check for over-Time Effectors?
        Projectile proj = spawnedObject.GetComponent<Projectile>();
        if (proj)
        {
            proj.Initialize(target, Damage, this);
            return true;
        }

        explosion sploder = spawnedObject.GetComponent<explosion>();
        if (sploder)
        {
            sploder.Initialize(Damage, this);
            return true;
        }

        OverTimeApplier Timer = spawnedObject.GetComponent<OverTimeApplier>();
        if (Timer)
        {
            Timer.myHitContainer = this;
            return true;
        }

            VisionTrigger vision = spawnedObject.GetComponent<VisionTrigger>();
            if (vision)
            {
                vision.PlayerOwner = myManager.PlayerOwner;
                if (vision.AppliesToAllies || vision.AppliesToEnemies)
                {
                    vision.PlayersToLookFor.Clear();
                    if (vision.AppliesToAllies)
                    {
                        vision.PlayersToLookFor.Add(myManager.PlayerOwner);
                    }
                    if (vision.AppliesToEnemies)
                    {
                        vision.PlayersToLookFor.Add(myManager.PlayerOwner == 1 ? 2 : 1);
                    }
                }
               
                return true;
            }

        UnitManager man = spawnedObject.GetComponent<UnitManager>();
        if (man)
        {
            DaminionsInitializer.main.AlterUnit(man);
            myManager.Initialize(myManager.PlayerOwner, true, man.getUnitStats().isUnitType(UnitTypes.UnitTypeTag.Structure));
            if (man.cMover)
            {
                man.GiveOrder(Orders.CreateAttackMove(transform.position + Vector3.right * 75, true));
                man.GiveOrder(Orders.CreateAttackMove(transform.position + Vector3.right * 150, true));
                man.GiveOrder(Orders.CreateAttackMove(transform.position + Vector3.right * 250, true));
                man.GiveOrder(Orders.CreateAttackMove(transform.position + Vector3.right * 350, true));
            }
        }


        return false;
    }



}

// Scripts/prefabs that will need to be fixed  with this new class
// HalfLifeStarter, FearEffect, EmBomb, ExplodeOnDeath
// InflectionStarter
// SPlitterShot, OnTriggerApply, SingleTarget
// StunStrike