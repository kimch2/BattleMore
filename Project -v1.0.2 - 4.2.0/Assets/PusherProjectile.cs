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
        Vector3 forward = transform.forward * speed * Time.deltaTime;
   
        gameObject.transform.Translate(forward, Space.World);
        currentDistance += speed * Time.deltaTime;

        forward.y = -10 * Time.deltaTime; // this is due to a weird bug where the RVOcontroller pushes the guy up into the air
        foreach (UnitManager man in ToPush)
        {
            if (man)
            {
                man.cMover.move();
                man.transform.position += forward;//.Translate(forward, Space.World);
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
            if (!ToPush.Contains(manage))
            {
                ToPush.Add(manage);
                manage.metaStatus.Root(MyHitContainer.myManager, this, false);
                MyHitContainer.trigger(this.gameObject, manage, damage);
                manage.myStats.TakeDamage(damage, Source, damageType, MyHitContainer);
                if (explosionO)
                {
                    GameObject explode = (GameObject)Instantiate(explosionO, col.transform.position, Quaternion.identity);
                    MyHitContainer.SetOnHitContainer(explode, this.damage, null);
                }
            }
        }
    }


    public void OnDespawn()
    {
        base.OnDespawn();
        foreach (UnitManager man in ToPush)
        {
            if (man)
            {
                man.metaStatus.UnRoot(this);
            }
        }
    }
}
