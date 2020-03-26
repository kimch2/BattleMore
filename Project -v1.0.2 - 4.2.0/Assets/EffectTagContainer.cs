using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTagContainer : MonoBehaviour
{

    public enum  TagLocation{Feet, Body,HPBar}
    List<GameObject> TopIcons = new List<GameObject>();
    List<GameObject> BodyIcons = new List<GameObject>();
    List<GameObject> FeetIcons = new List<GameObject>();
    float unitSize;
    Transform BodyTransform;
    Transform HPBarTransform;
    float HPOffset;

    private void Awake()
    {
        unitSize = GetComponent<CharacterController>().radius;
        BodyTransform = GetComponentInChildren<SpriteBillboard>().transform;
        HPBarTransform = GetComponentInChildren<HealthDisplay>().transform;
        HPOffset = HPBarTransform.GetChild(2).localPosition.y - 2;
    }

    public GameObject AddVisualFX(EffectTag toAdd)
    {
        GameObject newObject = Instantiate<GameObject>(toAdd.FXObject);

        if (toAdd.tagLocation == TagLocation.HPBar)
        {
            newObject.transform.parent = BodyTransform;
            newObject.transform.localPosition = Vector3.up * HPOffset - Vector3.right * TopIcons.Count * 2;
            newObject.transform.localRotation = Quaternion.identity;
            TopIcons.Add(newObject);
            ResetTopIcons();
        }
        else if (toAdd.tagLocation == TagLocation.Body)
        {
            newObject.transform.parent = BodyTransform;
            newObject.transform.localPosition = Vector3.up * unitSize / 2;
            newObject.transform.localRotation = Quaternion.identity;
            BodyIcons.Add(newObject);
        }
        else
        {
            newObject.transform.parent = transform;
            newObject.transform.localPosition = Vector3.zero;
            newObject.transform.LookAt(transform.position + Vector3.up);
            FeetIcons.Add(newObject);
            ResetFootIcons(); 
        }
        newObject.transform.localScale = Vector3.one;

        return newObject;
    }

    public void ResetFX()
    {
        ResetFootIcons();
        ResetTopIcons();
    }

    void ResetTopIcons()
    {
        Vector3 BasePosition = Vector3.up * HPOffset - Vector3.right * TopIcons.Count;
        for (int i = 0; i < TopIcons.Count; i++)
        {
            TopIcons[i].transform.localPosition =  Vector3.right * i*3 + BasePosition;
        }
    }



    void ResetFootIcons()
    {
        for (int i = 0; i < FeetIcons.Count; i++)
        {
            // Change the size of them??
        }
    }
}
