using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCaster : MonoBehaviour
{
    public LayerMask GroundLayer = 1 << 8 | 1 << 16;
    public GameObject ShadowObject;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.gameObject.transform.position, Vector3.down, out hit, 1000, GroundLayer))
        {
            ShadowObject.transform.position = hit.point + Vector3.up * .1f;

        }
    }
}
