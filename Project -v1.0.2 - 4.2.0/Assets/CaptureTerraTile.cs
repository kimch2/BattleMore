using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureTerraTile : TargetAbility
{

    public int PlayerOwner =1 ;
    private new void Start()
    {
        myType = type.target;
        InitializeCharges();
    }

    public override void Activate()
    {
        Debug.Log("Casting C");
    }

    public override continueOrder canActivate(bool error)
    {
        return new continueOrder(true,false);
    }

    public override void Cast()
    {
        Debug.Log("Casting");
        RaycastHit hit;

        if (Physics.Raycast(new Ray(location + Vector3.up , Vector3.down), out hit, Mathf.Infinity, 1 << 8))
        {
            TerraCraftTile tile = hit.collider.gameObject.GetComponent<TerraCraftTile>();

            if (tile && tile.PlayerOwner != PlayerOwner && !tile.isSpawnZone)
            {
                hit.collider.gameObject.GetComponent<TerraCraftTile>().Capture(myManager == null ? PlayerOwner : myManager.PlayerOwner);
                changeCharge(-1);
            }
        }
    }

    public override bool Cast(GameObject targ, Vector3 loc)
    {
        location = loc;
        target = targ;
        Cast();
        return true;
    }

    public override bool isValidTarget(GameObject targ, Vector3 location)
    {
        return true;
    }

    public override void setAutoCast(bool offOn)
    {
    
    }
}
