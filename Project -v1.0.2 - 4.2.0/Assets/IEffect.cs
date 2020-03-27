using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public abstract class IEffect : MonoBehaviour{


    //The IEffect class is represents both the source and the thing applied to the target, the "onTarget" variable
    // being the determiner.
    // For one time effects like 'Heal target', you don't need to copy the effect to the target, just run the code to 
    // heal the target

    public string EffectName = "NameMe!";
	public bool onTarget;
    protected UnitManager SourceManager;
    protected UnitManager OnTargetManager;

    public List<EffectTag> VisualEffects;
    List<GameObject> AppliedFX = new List<GameObject>();
    int channelSource;

    public abstract bool validTarget(GameObject target);
    public abstract void applyTo(GameObject source, UnitManager target);
    public abstract void BeginEffect(); // this gets called on the copied components

    protected void Start()
    {
        if (onTarget)
        {
            BeginEffect();
            ApplyFX();
        }
    }

    /// Call this last if you are overriding to add additional things
    public virtual void EndEffect()
    {
        RemoveVisualFX();
        Destroy(this);
    }

    public void SetManagers(UnitManager source, UnitManager target)
    {
        SourceManager = source;
        OnTargetManager = target;
    }

    public Component CopyIEffect(UnitManager toCopyTo, bool CopyToAsTarget)
    {
        System.Type type = this.GetType();
        IEffect copy = null;

        if (!CopyToAsTarget) // If we are applying this thing as a source effect, IE giving a unit an ability, (not giving it to them as an effect)
        {
            foreach (OnHitContainer contain in toCopyTo.GetComponentsInChildren<OnHitContainer>())
            {
                copy = (IEffect)contain.gameObject.GetComponent(type);
                if (copy == null || !copy.onTarget)
                {
                    copy = (IEffect)contain.gameObject.AddComponent(type);
                }
                copy.SetManagers(contain.myManager,null);
                copy.onTarget = CopyToAsTarget;
              
                System.Reflection.FieldInfo[] fields = type.GetFields();
                foreach (System.Reflection.FieldInfo field in fields)
                {
                    field.SetValue(copy, field.GetValue(this));
                }
                contain.AddEffectApplier(copy);
            }
        }
        else
        {
            bool AlreadyExists = true;
            foreach (Component fect in toCopyTo.gameObject.GetComponents(type))
            {
                copy = (IEffect)toCopyTo.gameObject.GetComponent(type);
                if (copy.EffectName == this.EffectName)
                {
                    break;
                }
            }
           
            if (copy == null)
            {
                AlreadyExists = false;
                copy = (IEffect)toCopyTo.gameObject.AddComponent(type);             
            }
            copy.enabled = true;
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(this));
            }
            copy.SetManagers(SourceManager,toCopyTo);
            copy.onTarget = CopyToAsTarget;
            if (!AlreadyExists)
            {
                copy.ApplyFX();
            }
        }
        return copy;
    }

    public void ApplyFX()
    {
        foreach (EffectTag tag in VisualEffects)
        {
             AppliedFX.Add(OnTargetManager.myStats.getEffectTagContainer().AddVisualFX(tag, true));
        }
    }

    public void RemoveVisualFX ()
    {
        foreach (GameObject obj in AppliedFX)
        {
            Destroy(obj);
        }
        OnTargetManager.myStats.getEffectTagContainer().ResetFX();
    }
}

[System.Serializable]
public class EffectTag
{
    public GameObject FXObject;
    public EffectTagContainer.TagLocation tagLocation;

    public EffectTag(GameObject obj, EffectTagContainer.TagLocation loc)
    {
        FXObject = obj;
        tagLocation = loc;
    }
}
