using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOffsetter : MonoBehaviour
{
    //Make this gameobject set off to one side or another based on the flipX of the griven spriteRenderer

    public SpriteRenderer myRenderer;

    bool PrevFlipX;

    // Update is called once per frame
    void Update()
    {
        if (myRenderer.flipX != PrevFlipX)
        {
            transform.localPosition = new Vector3(transform.localPosition.x *-1, transform.localPosition.y, transform.localPosition.z);
            PrevFlipX = myRenderer.flipX;
        }
    }
}
