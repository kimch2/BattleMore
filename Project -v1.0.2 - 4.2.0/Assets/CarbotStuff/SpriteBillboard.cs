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

       
    }
    Vector3 LookLocation;

    void Update()
    {
        LookLocation = transform.position * 2 - (LookAtCam.transform.position - LookAtCam.transform.up * LookAtCam.orthographicSize);
        LookLocation.x = gameObject.transform.position.x;
        gameObject.transform.LookAt(LookLocation);
        mySprite.flipX = transform.parent.rotation.eulerAngles.y > 180;//|| transform.parent.rotation.eulerAngles.y < 270;
    }
}
