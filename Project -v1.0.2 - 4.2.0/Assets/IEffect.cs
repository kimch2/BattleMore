using UnityEngine;
using System.Collections;

public abstract class IEffect : MonoBehaviour{

	public bool onTarget;
    protected UnitManager SourceManager;
    protected UnitManager OnTargetManager;

    public void SetManagers(UnitManager source, UnitManager target)
    {
        SourceManager = source;
        OnTargetManager = target;
    }


	public abstract bool validTarget(GameObject target);

	public abstract void applyTo (GameObject source, UnitManager target);
    public abstract void RemoveEffect(UnitManager target);

    public void RegisterBuff(UnitManager manager, bool isFriendly)
    {
        
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
                copy.SetManagers(contain.myManager, null);
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
            copy = (IEffect)toCopyTo.gameObject.GetComponent(type);
            if (copy == null)
            {
                copy = (IEffect)toCopyTo.gameObject.AddComponent(type);             
            }
            copy.enabled = true;
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(this));
            }
            copy.SetManagers(SourceManager, toCopyTo);
            copy.onTarget = CopyToAsTarget;
        }
        return copy;
    }
}
