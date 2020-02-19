using UnityEngine;

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
        Debug.Log("Disableing");
        myIndicator.SetActive(false);
    }
}