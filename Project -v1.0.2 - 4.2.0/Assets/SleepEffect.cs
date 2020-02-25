using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepEffect : MonoBehaviour, Notify, Modifier
{

    public float sleepTime = 5;
    public UnitTypes.UnitTypeTag mustTarget;
    public bool OnTarget;
    float TimePutToSleep;
    // Use this for initialization
    void Start()
    {
        if (GetComponent<Projectile>())
        {
            GetComponent<Projectile>().triggers.Add(this);
        }

        explosion  myexplode = this.gameObject.GetComponent<explosion>();
        if (myexplode)
        {
            myexplode.triggers.Add(this);

        }
    }





    public float trigger(GameObject source, GameObject proj, UnitManager target, float damage)
    {
        Debug.Log("Triugginer sleep " + target);
        if (target && source != target)
        {
            Debug.Log("A");

            if (mustTarget != UnitTypes.UnitTypeTag.Dead)
            {
                Debug.Log("B");
                if (!target.myStats.isUnitType(mustTarget))
                {
                    Debug.Log("C");
                    return damage;
                }
            }

            Debug.Log("D");
            target.gameObject.AddComponent<SleepEffect>().PutToSleep();

        }
        return damage;
    }


    public void PutToSleep()
    {
        TimePutToSleep = Time.time;
        Debug.Log("Putting to sleep");
        OnTarget = true;
        GetComponent<UnitManager>().metaStatus.Stun(null, this, false, sleepTime);
        GetComponent<UnitStats>().addModifier(this);
        //Invoke("WakeUp", sleepTime); // Multiply this by tenacity later

    }


    float Modifier.modify(float damage, GameObject source, DamageTypes.DamageType theType)
    {
        if (Time.time > TimePutToSleep + .5f) {  // Adding this because the autoattack following a spell will wake them up.
            Debug.Log("Took Damage");
            WakeUp();
            Destroy(this);
        }
        return damage;
    }

    void WakeUp()
    {
        Debug.Log("Wake up");
        GetComponent<UnitManager>().metaStatus.UnStun(this);
        GetComponent<UnitStats>().removeModifier(this);
    }

}
