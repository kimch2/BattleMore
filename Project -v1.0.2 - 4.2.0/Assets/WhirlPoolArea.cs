using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlPoolArea : OverTimeApplier
{

    public float spinSpeed  =25;

    
    private void Update()
    {
        foreach (UnitManager man in InVision)
        {
            if (man)
            {
                float distanceToMiddle = Vector3.Distance(transform.position, man.transform.position);

                Vector3 ToRotate = (transform.position - man.transform.position).normalized;
                Vector3 LookAtDirection = Quaternion.Euler(0, 82, 0) * ToRotate * spinSpeed;

                  man.ExternalMove(LookAtDirection, false);

            }
        }
    }

    public void TurnOff()
    {
        foreach (UnitManager man in InVision)
        {
            UnitExitTrigger(man);
        }
        Destroy(this.gameObject);
    }

}
