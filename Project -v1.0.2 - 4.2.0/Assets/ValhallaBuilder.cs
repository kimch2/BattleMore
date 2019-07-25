using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValhallaBuilder : UnitProduction
{


    Coroutine currentCharger;

    Vector3 targetLocation;
    float supplyNeeded;

    public float Range;

    RaceManager racer;
    public bool FaceRight = true;

    // Use this for initialization
    new void Start()
    {
        myType = type.building;
        supplyNeeded = unitToBuild.GetComponent<UnitStats>().supply;
        racer = GameManager.getInstance().playerList[myManager.PlayerOwner - 1];
    }



    override
    public continueOrder canActivate(bool showError)
    {

        continueOrder order = new continueOrder();
        /*
		if (UIManager.main.m_ObjectBeingPlaced) {
			if (Vector3.Distance (UIManager.main.m_ObjectBeingPlaced.transform.position, this.transform.position) < Range) {
				order.canCast = false;
			}
		}
*/

        if (supplyNeeded != 0 && racer.currentSupply + supplyNeeded > racer.supplyMax)
        {
            order.canCast = false;
            order.reasonList.Add(continueOrder.reason.supply);
        }
        if (!myCost.canActivate(this, order, false))
        {
            order.canCast = false;

            // FIX THIS LINE IN THE FUTURE IF IT BREAKS! its currently in here to allow guys with multiple charges to use them even though the cooldown timer is shown.
            if (myCost.energy == 0 && myCost.resourceCosts.MyResources.Count == 0 && chargeCount > 0)
            {
                order.canCast = true;
            }
            else
            {
                order.reasonList.Add(continueOrder.reason.cooldown);
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
        myCost.payCost();
        GameObject inConstruction = null;

        if (this.gameObject.name.Contains(unitToBuild.name))
        {
            //Debug.Log ("Loading unit :" + unitToBuild.name);
            // this is so it can construct itself as a prefab and not a copy of itself
            inConstruction = (GameObject)Instantiate(Resources.Load<GameObject>(unitToBuild.GetComponent<UnitManager>().UnitName), targetLocation, Quaternion.Euler(0,FaceRight ? 90 : -90,0));
        }
        else
        {
            //Debug.Log ("Second unit " + unitToBuild);
            inConstruction = (GameObject)Instantiate(unitToBuild, targetLocation, Quaternion.Euler(0, FaceRight ? 90 : -90, 0));
        }

        UnitManager UnitMan = inConstruction.GetComponent<UnitManager>();
        UnitMan.Start();
        UnitMan.setStun(true, this, false);


    }  // returns whether or not the next unit in the same group should also cast it



    override
    public void setAutoCast(bool offOn)
    { }

    public bool isValidTarget(GameObject target, Vector3 location)
    {
        TerraCraftTile tile = target.GetComponent<TerraCraftTile>();

        if (tile && tile.isSpawnZone && tile.PlayerOwner == myManager.PlayerOwner)
        {
            return true;
        }

        return false;
    }

    public void setBuildSpot(Vector3 buildSpot, GameObject ghostPlacer)
    {
        targetLocation = buildSpot;
        Destroy(ghostPlacer);
    }

    public bool Cast(GameObject target, Vector3 location)
    {
        Cast();
        return false;
    }

    public void Cast()
    {
        myCost.payCost();
        GameObject inConstruction = null;

        Vector3 pos = targetLocation;
        inConstruction = (GameObject)Instantiate(unitToBuild, pos, Quaternion.identity);
        UnitManager unitMan = inConstruction.GetComponent<UnitManager>();
        unitMan.PlayerOwner = myManager.PlayerOwner;
        unitMan.setInteractor();
        unitMan.interactor.initializeInteractor();
        unitMan.Start();
        //unitMan.enabled = false;
        myManager.myRacer.UnitCreated(unitToBuild.GetComponent<UnitStats>().supply);
    }


    public override void startBuilding() { }

    public override void DeQueueUnit()
    {

    }


    public override void cancelBuilding()
    {

    }

    public override float getProgress()
    {
        return 0;
    }

    public override void InitializeGhostPlacer(GameObject ghost)
    { 
        BuildingPlacer placer = ghost.GetComponent<BuildingPlacer>();
        placer.building.transform.rotation = Quaternion.Euler(0, FaceRight ? 90 : -90, 0);
    }
}
