using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerIeffect : IEffect
{
    public OnHitContainer myHitContainer;

    protected virtual void Awake()
    {
        /*
        if (!myHitContainer)
        {
            myHitContainer = GetComponent<OnHitContainer>();
            if (!myHitContainer)
            {
                UnitManager manag = GetComponent<UnitManager>();
                if (!manag)
                {
                    manag = GetComponentInParent<UnitManager>();
                }
                if (manag) // Testing this???
                {
                    myHitContainer = OnHitContainer.CreateDefaultContainer(this.gameObject, manag, this.gameObject.name + "HitContainer");
                }
            }
        }*/
    }



    public override void applyTo(GameObject source, UnitManager target)
    {    
    }

    public override void RemoveEffect(UnitManager target)
    {
    }

    public override bool validTarget(GameObject target)
    {
        return true;
    }
}
