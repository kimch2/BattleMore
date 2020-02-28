using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class TargetAbility : Ability
{

    public float range;

    public GameObject target;
    [HideInInspector]
    public Vector3 location;
    public Sprite targetArea;
    public float areaSize;
    public enum targetType { ground, unit, skillShot }
    public targetType myTargetType;
    GameObject myIndicator;

    public bool inRange(Vector3 location)
    {

        float pyth = Mathf.Pow(myManager.transform.position.x - location.x, 2) + Mathf.Pow(myManager.transform.position.z - location.z, 2);
        if (Mathf.Pow(pyth, .5f) < range)
        { return true; }


        //Debug.Log ("Distance " + Vector3.Distance(this.gameObject.transform.position, location));
        return false;

    }

    public abstract void Cast();

    public abstract bool Cast(GameObject target, Vector3 location);

    public abstract bool isValidTarget(GameObject target, Vector3 location);

    public bool onPathableGround(Vector3 location)
    {//float dist = Vector3.Distance(location, AstarPath.active.graphs [0].GetNearest (location).node.Walkable);
     //Debug.Log ("distance is " + dist);
        return AstarPath.active.graphs[0].GetNearest(location).node.Walkable;// (dist < 5);
    }

    public void setTarget(Vector3 position, GameObject Target)
    {
        target = Target;
        location = position;
    }

    public void ShowSkillShotIndicator(Vector3 TargetSpot)
    {
        if (myTargetType == targetType.skillShot)
        {
            if (!myIndicator)
            {
                myIndicator = new GameObject("TargetIndicator");

                myIndicator.transform.parent = myManager.transform;
                myIndicator.transform.localPosition = Vector3.zero;

                GameObject ChildSprite = new GameObject("ChildSprite");
                ChildSprite.transform.parent = myIndicator.transform;
                ChildSprite.transform.localPosition = Vector3.forward * range / 2;
                ChildSprite.transform.localScale = new Vector3(range, range, 0);
                ChildSprite.transform.localEulerAngles = new Vector3(90, 0, 0);
                ChildSprite.AddComponent<SpriteRenderer>().sprite = targetArea;

            }
            myIndicator.SetActive(true);
            myIndicator.transform.LookAt(TargetSpot, Vector3.up);
        }
    }

    public void DisableSkillShotIndicator()
    {
        if (myIndicator)
        {
            myIndicator.SetActive(false);
        }
    }
    /// <summary>
    ///  The Active Player is 1, enemy is 2, nuetral is 3, 4+ are other enemies
    /// </summary>
    /// <param name="Origin"></param>
    /// <param name="playerNumber"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    protected List<UnitManager> GetUnitsInRange(Vector3 Origin, int playerNumber, float radius)
    {

        radius /= 2;
        List<UnitManager> toTarget = new List<UnitManager>();

        foreach (KeyValuePair<string, List<UnitManager>> unitList in GameManager.main.playerList[playerNumber - 1].getUnitList())
        {
            foreach (UnitManager unit in unitList.Value)
            {
                if (unit == null)
                {
                    continue;
                }
                if (new Vector2(unit.transform.position.x - Origin.x, unit.transform.position.z - Origin.z).sqrMagnitude <= Mathf.Pow(radius + unit.CharController.radius, 2))
                {
                    toTarget.Add(unit);
                }
            }
        }
        return toTarget;
    }

    /// <summary>
    /// Checks for chargeCount and Costs. Next cast should be true if multiple selected units should all cast the spell, like stim pack
    /// </summary>
    /// <param name="NextCast"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    protected continueOrder DefaultCanActivate(bool NextCast,  bool error)
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
            order.nextUnitCast = NextCast;
        }
        if (order.canCast)
        {
            order.nextUnitCast = NextCast;
        }
        return order;
    }

}