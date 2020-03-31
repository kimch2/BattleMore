using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanSprayBubble :  VisionTrigger
{
    public float HealAmount = 50;
    public GameObject BubbleEffect;
    UnitManager ToSeek;
    //public GameObject BubbleExplosionFX;

    private void Start()
    {
        BubbleBurst.AddBubble(this);
    }

    void Heal(UnitManager manager)
    {
            manager.myStats.heal(HealAmount);
            Instantiate<GameObject>(BubbleEffect, this.transform.position, Quaternion.identity);
            BubbleBurst.RemoveBubble(this);
            Destroy(this.gameObject);        
    }

    float currentSpeed;
    float NextCheckTime;
    private void Update()
    {
        if (ToSeek )
        {
            if (ToSeek.myStats.health < ToSeek.myStats.Maxhealth - 25)
            {
                if (Vector3.Distance(ToSeek.transform.position, transform.position) < 3)
                {
                    Heal(ToSeek);
                }
                else
                {
                    currentSpeed += Time.deltaTime * 7;
                    transform.Translate((ToSeek.transform.position - transform.position).normalized * currentSpeed * Time.deltaTime);
                }
            }
            else
            {
                ToSeek = null;
            }
        }
        else
        {
            currentSpeed = 0;
        }

        if (Time.time > NextCheckTime)
        {
            NextCheckTime = Time.time + .5f;
            float Closest = 100;          

            foreach (UnitManager manag in InVision)
            {
                if (manag && manag.myStats.health < manag.myStats.Maxhealth - 25)
                {
                    float dist = Vector3.Distance(manag.transform.position, transform.position);
                    if (dist < Closest)
                    {
                        Closest = dist;
                        ToSeek = manag;
                    }
                }
            }
        }
    }

    public void Explode(OnHitContainer theContainer)
    {
        Instantiate<GameObject>(BubbleEffect, this.transform.position, Quaternion.identity);
        //Instantiate<GameObject>(BubbleExplosionFX, transform.position, Quaternion.identity);
        foreach (UnitManager manag in GameManager.GetUnitsInRange(transform.position, PlayerOwner, 10))
        {
            manag.myStats.heal(HealAmount);
        }

        foreach (UnitManager manag in GameManager.GetUnitsInRange(transform.position, PlayerOwner == 1 ? 2:1, 10))
        {
            manag.myStats.TakeDamage(HealAmount, this.gameObject , DamageTypes.DamageType.Regular, theContainer );
        }
        Instantiate<GameObject>(BubbleEffect, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public override void UnitEnterTrigger(UnitManager manager)
    {
    }

    public override void UnitExitTrigger(UnitManager manager)
    {
    }
}
