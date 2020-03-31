using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour {




	public float dropPeriod;
	public GameObject toDrop;
    // Use this for initialization
    void Start()
    {
        if (dropPeriod > 0)
        {
            InvokeRepeating("DropObject", dropPeriod, dropPeriod);
        }
    }


    public void DropOnUnit(GameObject toPlace)
    {
        RaycastHit objecthit;
        Vector3 down = toPlace.transform.TransformDirection(Vector3.down);

        if (Physics.Raycast(toPlace.transform.position, down, out objecthit, 1000, 1 << 8))
        {

            Instantiate(toDrop, new Vector3(toPlace.transform.position.x, objecthit.point.y, toPlace.transform.position.z), Quaternion.identity);
        }
    }

	public void DropObject()
	{
        DropOnUnit(this.gameObject);
	}

}
