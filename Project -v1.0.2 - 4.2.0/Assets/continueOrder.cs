﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class continueOrder {

    public bool canCast = true;
    public bool nextUnitCast = true;

    public List<reason> reasonList = new List<reason>();
    public List<ResourceType> InsufficientResources = new List<ResourceType>();

    public enum reason { cooldown, requirement, energy, health, charge, supply }

    public continueOrder()
    {
    }
        public continueOrder(bool CanCast,bool NextCast)
    {
        canCast = CanCast;
        nextUnitCast = NextCast;
    }

}
