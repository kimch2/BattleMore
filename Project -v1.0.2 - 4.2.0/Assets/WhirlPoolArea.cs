﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlPoolArea : VisionTrigger
{

    public float spinSpeed  =25;
    public float duration = 5;


    public void setSource(GameObject source)
    {
        UnitManager sourceMan = source.GetComponent<UnitManager>();
        if (sourceMan.PlayerOwner == 1)
        {
            PlayerNumber = 2;
        }
        else
        {
            PlayerNumber = 1;
        }

        Invoke("TurnOff", duration);
    }


    private void Update()
    {
        foreach (UnitManager man in InVision)
        {
            if (man)
            {
                float distanceToMiddle = Vector3.Distance(transform.position, man.transform.position);

                Vector3 ToRotate = (transform.position - man.transform.position).normalized;
                Vector3 LookAtDirection = Quaternion.Euler(0, 82, 0) * ToRotate * spinSpeed;

                  man.ExternalMove(LookAtDirection, false);

            }
        }
    }

    public override void UnitEnterTrigger(UnitManager manager)
    {
        manager.myStats.statChanger.changeArmor(0,-2,this, true);
        manager.myStats.statChanger.changeMoveSpeed(-1, 0,this, true);
    }

    public override void UnitExitTrigger(UnitManager manager)
    {
        manager.myStats.statChanger.removeArmor(this);
        manager.myStats.statChanger.removeMoveSpeed(this);
    }

    public void TurnOff()
    {
        foreach (UnitManager man in InVision)
        {
            UnitExitTrigger(man);
        }
        Destroy(this.gameObject);
    }

}
