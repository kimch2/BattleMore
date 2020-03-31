﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarbotCamera : MainCamera
{

    public GameObject HeroToFollow;

    float progression;
    public Vector3 LeftSide;
    public Vector3 RightSide;
    public float BattleFieldHeight = 100;
    float MaxXDistance;
    public float HeroOffset = 0;
    public UnityEngine.UI.Slider ProgressSlider;
    Vector3 initialPosition;

    //bool CurrentlySmashing;
    public static CarbotCamera singleton;
    public bool LockedCamera;

    float CurrentSpeed;
    public float acceleration = 1;
    public float topSpeed = 1;

    protected override void Awake()
    {
        base.Awake();
        singleton = this;

    }


    public override void Start()
    {
        initialPosition = transform.position;
        HeightAboveGround = transform.position.y;

        if (LockedCamera)
        {

            transform.position = new Vector3(LeftSide.x + MaxXDistance + 60 , transform.position.y, transform.position.z);

        }
    }
   


    public void setHero(GameObject toSet)
    {
        HeroToFollow = toSet;
      //  MaxXDistance = HeroToFollow.transform.position.x - HeroOffset;
    }

     public override void Update()
     {
        if (!LockedCamera)
        {
            base.Update();
        }
        // if (!CurrentlySmashing)
         {
             if (HeroToFollow)
             {
                Vector2 ScreenPoint = myCamera.WorldToScreenPoint(HeroToFollow.transform.position);

                float prev = MaxXDistance;
                if (ScreenPoint.x > Screen.width /2.5f)
                {
                    CurrentSpeed += acceleration * Time.deltaTime;
                    CurrentSpeed = Mathf.Min(CurrentSpeed, topSpeed);
                    MaxXDistance += CurrentSpeed * Time.deltaTime;

                }
                else if (ScreenPoint.x < Screen.width/5)
                {
                    CurrentSpeed -= acceleration * Time.deltaTime;
                    CurrentSpeed = Mathf.Max(CurrentSpeed, -1 * topSpeed);
                    MaxXDistance += CurrentSpeed * Time.deltaTime;
                }
                else if (CurrentSpeed > 0)
                {
                    CurrentSpeed -= acceleration * Time.deltaTime;
                    CurrentSpeed = Mathf.Max(CurrentSpeed, 0);
                    MaxXDistance += CurrentSpeed * Time.deltaTime;

                }
                else
                {
                    CurrentSpeed += acceleration * Time.deltaTime;
                    CurrentSpeed = Mathf.Min(CurrentSpeed, 0);
                    MaxXDistance += CurrentSpeed * Time.deltaTime;
                }

                if (MaxXDistance != prev)
                {
                    if (LockedCamera)
                    {
                        transform.position = new Vector3(Mathf.Clamp(LeftSide.x + MaxXDistance - HeroOffset, LeftSide.x, RightSide.x), transform.position.y, transform.position.z);
                    }
                    ProgressSlider.value = (HeroToFollow.transform.position.x - LeftSide.x) / (RightSide.x - LeftSide.x);
                }

             }
             else
             {
                 //ErrorPrompt.instance.notEnoughEnergy();
             }
         }
     }

    public float getProgress()
    {
        return (MaxXDistance) / (RightSide.x - LeftSide.x);
    }

    
    public override void Zoom(object sender, ScrollWheelEventArgs e)
    {
      
    }

    public override void Pan(object sender, ScreenEdgeEventArgs e)
    {}
    /*
    public void SmashCamera(GameObject target)
    {
        if(!CurrentlySmashing)
        StartCoroutine(SwoopCamera(target));
    }

    IEnumerator SwoopCamera(GameObject target)
    {
        CurrentlySmashing = true;
        Vector3 TargetSpot = target.transform.position;

        for (float i = 0; i < .2f; i += Time.deltaTime)
        {
            transform.Translate((TargetSpot - transform.position) * Time.deltaTime * 4, Space.World);
            yield return null;
        }
        ShakeCamera(.47f,100,.03f);
        yield return new WaitForSeconds(.5f);

        Vector3 startPoint = transform.position;
        for (float i = 0; i < .2f; i += Time.deltaTime)
        {
            //transform.position = Vector3.Lerp(startPoint, new Vector3(MaxXDistance - HeroOffset, initialPosition.y, initialPosition.z),i /.2f);
            transform.position = Vector3.Lerp(startPoint, new Vector3(MaxXDistance - HeroOffset, initialPosition.y, initialPosition.z), i / .2f);
            yield return null;
        }


        CurrentlySmashing = false;
    }
    */


    public Vector3 ClampYLocation(Vector3 location)
    {
        location.y = Mathf.Clamp(location.y, LeftSide.y - BattleFieldHeight/2, LeftSide.y + BattleFieldHeight / 2);
        return location;
            
    }
    

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(LeftSide, 4);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(RightSide, 4);
        Gizmos.DrawLine(LeftSide + Vector3.forward * BattleFieldHeight / 2, LeftSide - Vector3.forward * BattleFieldHeight / 2);
        Gizmos.DrawLine(RightSide + Vector3.forward * BattleFieldHeight / 2, RightSide - Vector3.forward * BattleFieldHeight / 2);
        Gizmos.DrawLine(LeftSide + Vector3.forward * BattleFieldHeight / 2, RightSide + Vector3.forward * BattleFieldHeight / 2);
        Gizmos.DrawLine(LeftSide - Vector3.forward * BattleFieldHeight / 2, RightSide - Vector3.forward * BattleFieldHeight / 2);
    }
}
