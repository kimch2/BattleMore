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
    
    public override void Start()
    {
        singleton = this;
        initialPosition = transform.position;
        HeightAboveGround = transform.position.y;
    }
   


    public void setHero(GameObject toSet)
    {
        HeroToFollow = toSet;
        MaxXDistance = HeroToFollow.transform.position.x - HeroOffset;
    }

    /* public override void Update()
     {
         if (!CurrentlySmashing)
         {
             if (HeroToFollow)
             {
                 MaxXDistance = Mathf.Max(HeroToFollow.transform.position.x - HeroOffset, MaxXDistance);
                 if (MaxXDistance == HeroToFollow.transform.position.x - HeroOffset)
                 {
                     transform.position = new Vector3(MaxXDistance - HeroOffset, transform.position.y, transform.position.z);
                     ProgressSlider.value = (transform.position.x - startPoint.x) / (endPoint.x - startPoint.x);
                 }
             }
             else
             {
                 ErrorPrompt.instance.notEnoughEnergy();
             }
         }
     }*/
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

    public Vector3 getRightScreenEdge(Vector3 unitLocation)
    {
        Vector3 screenPoint = myCamera.WorldToScreenPoint(unitLocation);
        screenPoint.x =Screen.width;
        Ray ray = myCamera.ScreenPointToRay(screenPoint, Camera.MonoOrStereoscopicEye.Mono);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        Debug.Log(" Could not find a terrain on the right side of the screen");
        return unitLocation;
    }

    public Vector3 getLeftScreenEdge(Vector3 unitLocation)
    {
        Vector3 screenPoint = myCamera.WorldToScreenPoint(unitLocation);
        screenPoint.x = 0;
        Ray ray = myCamera.ScreenPointToRay(screenPoint, Camera.MonoOrStereoscopicEye.Mono);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
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
