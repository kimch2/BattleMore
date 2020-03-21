using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerMonoBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public OnHitContainer myHitContainer;

    protected virtual void Awake()
    {
        if (!myHitContainer)
        {
            UnitManager manag = GetComponent<UnitManager>();
            if (!manag)
            {
                manag = GetComponentInParent<UnitManager>();
            }
            myHitContainer = OnHitContainer.CreateDefaultContainer(this.gameObject,manag, this.gameObject.name + "HitContainer");
        }
    }
    
}
