using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBurst : TargetAbility
{
    // ability which bursts OCean SPreay bubbles, in theory healing nearby allies and harming enemies.

    static Dictionary<int, List<OceanSprayBubble>> BubbleOwnerMap = new Dictionary<int, List<OceanSprayBubble>>();
    
    public override void Start() // We have this here so other source can call Start and any of this guy's inheriters will have it called instead
    {
        if (!BubbleOwnerMap.ContainsKey(myManager.PlayerOwner))
        {
            BubbleOwnerMap.Add(myManager.PlayerOwner, new List<OceanSprayBubble>());
        }
        myType = type.target;
    }

    public static void AddBubble(OceanSprayBubble bubble)
    {
        if (BubbleOwnerMap.ContainsKey(bubble.PlayerOwner))
        {
            BubbleOwnerMap[bubble.PlayerOwner].Add(bubble);
        }
    }

    public static void RemoveBubble(OceanSprayBubble bubble)
    {
        if (BubbleOwnerMap.ContainsKey(bubble.PlayerOwner))
        {
            BubbleOwnerMap[bubble.PlayerOwner].Remove(bubble);
        }
    }

    public override void Activate()
    {}

    public override continueOrder canActivate(bool error)
    {
        return DefaultCanActivate(false, true);
    }

    public override void Cast()
    {
        myCost.payCost();
        changeCharge(-1);

        if (soundEffect)
        {
            audioSrc.PlayOneShot(soundEffect);
        }
        Burst(location);
    }

    public override bool Cast(GameObject target, Vector3 location)
    {
        changeCharge(-1);
        myCost.payCost();
        Burst(location);

        return false;
    }

    void Burst( Vector3 locat)
    {
        foreach (OceanSprayBubble bubble in BubbleOwnerMap[myManager.PlayerOwner])
        {
            if (bubble)
            {
                if (Vector3.Distance(bubble.transform.position, locat) < areaSize)
                {
                    bubble.Explode(myHitContainer);
                }
            }
        }
        BubbleOwnerMap[myManager.PlayerOwner].RemoveAll(item => item == null);
    }

    public override bool isValidTarget(GameObject target, Vector3 location)
    {
        return true;
    }

    public override void setAutoCast(bool offOn)
    {
        BaseSetAutoCast(offOn);
    }
}
