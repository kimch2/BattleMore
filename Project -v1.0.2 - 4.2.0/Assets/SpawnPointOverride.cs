using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointOverride : MonoBehaviour
{
    int PlayerOwner;
    public OnHitContainer myHitContainer;

    public void setSource(GameObject source)
    {
        UnitManager man = source.GetComponent<UnitManager>();
        PlayerOwner = man.PlayerOwner;
        myHitContainer.Initialize(man);

    }

    private void Start()
    {
        DaminionsInitializer.main.SetSpawnLocation(this,PlayerOwner);
    }


    public void ModifyUnit(UnitManager unit)
    {
        myHitContainer.trigger(null, unit, 0);
    }
}
