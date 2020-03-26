using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraTileChecker : MonoBehaviour
{
    public float refreshRate = .65f;
    UnitManager manager;
   public bool currentlyOnFriendly;
    public ModularAura myAura;

    private void Start()
    {
        manager = GetComponent<UnitManager>();
        StartCoroutine(CheckForTile());
    }


    IEnumerator CheckForTile()
    {

        while (true)
        {
            bool onFriendly = TerraTileController.main.onFriendlyTile(manager);
            if (onFriendly && !currentlyOnFriendly)
            {
                MoveOnTile();
            }
            else if (!onFriendly && currentlyOnFriendly)
            {
                MoveOffTile();
            }
            yield return new WaitForSeconds(refreshRate);
        }

    }

    // Add in function later for OnEnemyTile? (for negative effects) 
    void MoveOffTile()
    {
        currentlyOnFriendly = false;
        if (myAura)
        {
            myAura.RemoveBuff(manager);
        }
        TerraTileController.main.RemoveAura(manager);
    }

    void MoveOnTile()
    {
        currentlyOnFriendly = true;
        if (myAura)
        {
            myAura.ApplyBuff(manager,1);
        }
        TerraTileController.main.ApplyAura(manager);
    }

}