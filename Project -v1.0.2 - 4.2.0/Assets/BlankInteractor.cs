using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankInteractor : MonoBehaviour, Iinteract
{
    protected UnitManager myManager;
    // Use this for initialization
    void Awake()
    {
        myManager = GetComponent<UnitManager>();
        myManager.setInteractor(this);
    }

    public void computeInteractions(Order order)
    {
        myManager.changeState(new DefaultState());
    }

    public UnitState computeState(UnitState state)
    {
        return new DefaultState();
    }

    public void initializeInteractor()
    {
        Awake();
    }
}
