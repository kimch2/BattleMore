using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PusherProjectile : SkillShotProjectile
{
    List<UnitManager> ToPush = new List<UnitManager>();

    protected override void Update()
    {
        if (currentDistance > TotalRange)
        {
            Terminate(null);
            Lean.LeanPool.Despawn(this.gameObject, 0);
        }


        if (Physics.Raycast(this.gameObject.transform.position + Vector3.up * 5, Vector3.down, out objecthit, 30, (1 << 8)))
        {
            float dist = Vector3.Distance(objecthit.point, transform.position);
            if (dist < 5f)
            {
                Vector3 newPos = transform.position;
                newPos.y = objecthit.point.y + 5;
                gameObject.transform.position = newPos;
            }
            else if (dist > 9)
            {
                Vector3 newPos = transform.position;
                newPos.y = objecthit.point.y + 9;
                gameObject.transform.position = newPos;
            }
        }

        gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        currentDistance += speed * Time.deltaTime;

        foreach (UnitManager man in ToPush)
        {
            if (man)
            {
                man.transform.Translate( transform.forward * speed * Time.deltaTime, Space.World);
            }
        }
    }



    public override void OnTriggerEnter(Collider col)
    {

        if (col.isTrigger)
        {
            return;
        }

        UnitManager manage = col.GetComponent<UnitManager>();
        if (manage && manage.PlayerOwner != MyHitContainer.playerNumber)// && manage.PlayerOwner != 3)
        {
            ToPush.Add(manage);

            MyHitContainer.trigger(this.gameObject, manage, damage);
            manage.myStats.TakeDamage(damage, Source, damageType, MyHitContainer);
        }
    }
}
