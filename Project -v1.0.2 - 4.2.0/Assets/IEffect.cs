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

    public Component CopyIEffect(UnitManager toCopyTo)
    {
        System.Type type = this.GetType();
        
        
        IEffect copy = (IEffect)toCopyTo.gameObject.GetComponent(type);
        if (copy == null)
        {
            copy = (IEffect)toCopyTo.gameObject.AddComponent(type);
        }
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(this));
        }
        ((IEffect)copy).SetManagers(SourceManager, toCopyTo);
        ((IEffect)copy).onTarget = true;
        return copy;
    }

}
