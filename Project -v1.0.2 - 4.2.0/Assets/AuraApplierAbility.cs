﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraApplierAbility : TargetAbility
{


    protected Selected mySelect;
    public GameObject AuraToApply;
    public bool appliesToEnemies = true;
    public bool AppliesToAllies;
    public float duration = .1f;

    // Use this for initialization
    new void Start()
    {
        myType = type.target;
        mySelect = GetComponent<Selected>();
        InitializeCharges();
    }



    override
    public continueOrder canActivate(bool showError)
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

    override
    public void Activate()
    {
    }  // returns whether or not the next unit in the same group should also cast it


    override
    public void setAutoCast(bool offOn)
    { }

    public override bool isValidTarget(GameObject target, Vector3 location)
    {
        return true;
    }


    override
    public bool Cast(GameObject target, Vector3 location)
    {
        Cast();
        return false;
    }

    override
    public void Cast()
    {
        myCost.payCost();
        changeCharge(-1);

        Vector3 pos = location;
        pos.y += 5;
        GameObject proj = (GameObject)Instantiate(ResourceLoader.getMain().getResource("AreaAuraApplier"), pos, Quaternion.identity);
        AreaAuraApplier aura = proj.GetComponent<AreaAuraApplier>();
        proj.GetComponent<SphereCollider>().radius = areaSize / 2;
        aura.AuraToApply = AuraToApply.GetComponent<ModularAura>();
        aura.duration = duration;
        aura.affectsAllies = AppliesToAllies;
        aura.affectsEnemies = appliesToEnemies;
        aura.setSource(myManager.gameObject);
    }
}