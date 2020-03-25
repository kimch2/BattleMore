using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDropper : TriggeredAbility
{
    public GameObject thingToDrop;
    [Tooltip("This gets passed to the the thing if it needs a number like damage")]
    public float PossibleNumberAmount;

    public void DropItem()
    {
        RaycastHit objecthit;

        if (Physics.Raycast(myManager.transform.position, Vector3.down, out objecthit, 100, 1 << 8))
        {
            GameObject obj = Instantiate<GameObject>(thingToDrop, transform.position, Quaternion.identity);
            myHitContainer.SetOnHitContainer(obj, PossibleNumberAmount, null);

            Projectile script = obj.GetComponent<Projectile>();
            if (script)
            {
                script.setLocation(objecthit.point);
            }
        }
    }
}
