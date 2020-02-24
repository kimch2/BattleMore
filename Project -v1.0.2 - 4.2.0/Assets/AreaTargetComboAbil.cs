using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTargetComboAbil : TargetAbility
{

    public GameObject ObjectToSpawn;
    public ComboTag.AbilType TagType;

    public List<ComboTag.AbilType> Combination;

    new void Start()
    {
        base.Start();
        myType = type.target;      
        InitializeCharges();
    }

    public override void Activate()
    {
   
    }

    public override continueOrder canActivate(bool error)
    {
        continueOrder order = new continueOrder();
        if (chargeCount == 0 && chargeCount != -1)
        {
            order.canCast = false;
        }

        if (!myCost.canActivate(this))
        {
            order.canCast = false;
            // FIX THIS LINE IN THE FUTURE IF IT BREAKS! its currently in here to allow guys with multiple charges to use them even though the cooldown timer is shown.
            if (myCost.energy == 0 && myCost.resourceCosts.MyResources.Count == 0 && chargeCount > 0)
            {
                order.canCast = true;
            }
        }
        else
        {
            order.nextUnitCast = false;
        }
        if (order.canCast)
        {
            order.nextUnitCast = false;
        }
        return order;
    }

    public override void Cast()
    {
        myCost.payCost();
        changeCharge(-1);
        GameObject proj = null;

        Vector3 pos = location;
        pos.y += 5;
        proj = (GameObject)Instantiate(ObjectToSpawn, pos, Quaternion.identity);


        proj.SendMessage("setSource", myManager.gameObject, SendMessageOptions.DontRequireReceiver);
    }

    public override bool Cast(GameObject target, Vector3 location)
    {
        changeCharge(-1);
        myCost.payCost();

        Vector3 pos = location;
        pos.y += 5;
        GameObject proj = (GameObject)Instantiate(ObjectToSpawn, pos, Quaternion.identity);

        proj.SendMessage("setSource", this.gameObject);

        return false;
    }

    public override bool isValidTarget(GameObject target, Vector3 location)
    {
        return true;
    }

    public override void setAutoCast(bool offOn)
    {
        BaseSetAutoCast(offOn);
    }
}
