using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarbotCamera : MainCamera
{

    public GameObject HeroToFollow;

    float progression;
    public Vector3 startPoint;
    public Vector3 endPoint;
    float MaxXDistance;
    public float HeroOffset = 0;
    public UnityEngine.UI.Slider ProgressSlider;
    Vector3 initialPosition;

    //bool CurrentlySmashing;
    public static CarbotCamera singleton;
    public bool LockedCamera;

   public float CurrentSpeed;
    public float acceleration = 1;
    public float topSpeed = 1;

    public override void Start()
    {
        singleton = this;
        initialPosition = transform.position;
        HeightAboveGround = transform.position.y;

        if (LockedCamera)
        {

            transform.position = new Vector3(startPoint.x + MaxXDistance - HeroOffset, transform.position.y, transform.position.z);

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
                float prev = MaxXDistance;
                if (HeroToFollow.transform.position.x - startPoint.x > MaxXDistance + topSpeed)
                {
                    CurrentSpeed += acceleration * Time.deltaTime;
                    CurrentSpeed = Mathf.Min(CurrentSpeed, topSpeed);
                    MaxXDistance += CurrentSpeed * Time.deltaTime;

                }
                else if (CurrentSpeed > 0) 
                {
                    CurrentSpeed -= acceleration * Time.deltaTime;
                    CurrentSpeed = Mathf.Max(CurrentSpeed, 0);
                    MaxXDistance += CurrentSpeed * Time.deltaTime;

                }

                 //MaxXDistance = Mathf.Max(HeroToFollow.transform.position.x - startPoint.x, MaxXDistance);
                 if (MaxXDistance != prev)
                 {
                    if (LockedCamera)
                    {
                        transform.position = new Vector3(startPoint.x + MaxXDistance - HeroOffset, transform.position.y, transform.position.z);
                    }
                    ProgressSlider.value = (MaxXDistance) / (endPoint.x - startPoint.x);
                 }
             }
             else
             {
                 ErrorPrompt.instance.notEnoughEnergy();
             }
         }
     }

    public float getProgress()
    {
        return (MaxXDistance) / (endPoint.x - startPoint.x);
    }

    /*
    public override void Zoom(object sender, ScrollWheelEventArgs e)
    {
      
    }

    public override void Pan(object sender, ScreenEdgeEventArgs e)
    { }*/
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

    public Vector3 getRightScreenEdge(Vector3 unitLocation, float YVariance)
    {
        Vector2 screenPoint = new Vector3(0, Screen.height / 2); //myCamera.WorldToScreenPoint(unitLocation);
        //screenPoint.x =Screen.width;
        Ray ray = myCamera.ScreenPointToRay(screenPoint);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point + Vector3.forward * Random.Range(-1 * YVariance, YVariance); ;
        }
        Debug.Log(" Could not find a terrain on the right side of the screen");
        return unitLocation;
    }

    public Vector3 getLeftScreenEdge(Vector3 unitLocation, float YVariance)
    {
        Vector2 screenPoint = new Vector3(-0, Screen.height/2); //myCamera.WorldToScreenPoint(unitLocation);

        Ray ray = myCamera.ScreenPointToRay(screenPoint);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point + Vector3.forward * Random.Range(-1*YVariance , YVariance) ;
        }
        Debug.Log(" Could not find a terrain on the Left side of the screen");
        return unitLocation;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(startPoint, 4);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(endPoint, 4);

    }
}
