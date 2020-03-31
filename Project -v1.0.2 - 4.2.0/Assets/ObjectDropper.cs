using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDropper : TriggeredAbility
{
    public GameObject thingToDrop;
    [Tooltip("This gets passed to the the thing if it needs a number like damage")]
    public float PossibleNumberAmount;
    public int NumberToSpawn = 1;

    public void DropItemOnUnit(GameObject toPlace)
    {
        RaycastHit objecthit;

        if (Physics.Raycast(toPlace.transform.position, Vector3.down, out objecthit, 100, 1 << 8))
        {
            for (int i = 0; i < NumberToSpawn; i++)
            {
                GameObject obj = Instantiate<GameObject>(thingToDrop, toPlace.transform.position + Vector3.right * 3 * i, Quaternion.identity);
                myHitContainer.SetOnHitContainer(obj, PossibleNumberAmount, null);

                Projectile script = obj.GetComponent<Projectile>();
                if (script)
                {
                    script.setLocation(objecthit.point);
                }
            }
        }
    }

    public void DropItem()
    {
        DropItemOnUnit(myManager.gameObject);
    }
}
