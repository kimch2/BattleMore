﻿using UnityEngine;
using System.Collections;

public class deexaArmor : Upgrade
{


    public float defaultAmount = 1;

    override
    public void applyUpgrade(GameObject obj)
    {

        UnitManager manager = obj.GetComponent<UnitManager>();

		if (!manager.myStats.isUnitType(UnitTypes.UnitTypeTag.Structure) || manager.myStats.isUnitType(UnitTypes.UnitTypeTag.Add_On) )
        {
			obj.GetComponent<UnitStats>().statChanger.changeArmor(0,1, null,true);
        }
    }
	public override void unApplyUpgrade (GameObject obj){

	}


}
