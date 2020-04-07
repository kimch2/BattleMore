using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{

    public Camera LookAtCam;

    public float RandomRotate = 0;
    private float RndRotate;
    public Transform this_t_;
    public SpriteRenderer mySprite;
    void Start()
    {
        this_t_ = this.transform;

        if (MainCamera.main)
        {
            LookAtCam = MainCamera.main.GetComponent<Camera>();
        }
        if (!LookAtCam)
        {
            LookAtCam = Camera.main;
        }
        if (!mySprite)
        {
            mySprite = GetComponent<SpriteRenderer>();

            if (!mySprite)
            {
                mySprite = GetComponentInChildren<SpriteRenderer>();
            }
        }

    }
    Vector3 LookLocation;

    void LateUpdate()
    {
        if (!mySprite)
        {
            return;
        }
        // Plane p = new Plane(LookAtCam.transform.forward, LookAtCam.transform.position);
        // LookLocation = p.ClosestPointOnPlane(transform.position);// transform.position * 2 - (LookAtCam.transform.position - LookAtCam.transform.up * LookAtCam.orthographicSize);
        // LookLocation.x = gameObject.transform.position.x;

        Quaternion Rot = Quaternion.LookRotation(LookAtCam.transform.forward, LookAtCam.transform.up);
 
        gameObject.transform.rotation = Rot;// .LookAt(LookLocation);
        if(transform.parent)
        mySprite.flipX = transform.parent.rotation.eulerAngles.y > 180;//|| transform.parent.rotation.eulerAngles.y < 270;
    }
}
