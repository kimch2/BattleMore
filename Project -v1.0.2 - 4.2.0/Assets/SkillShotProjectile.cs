using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShotProjectile  : Projectile {


    public float TotalRange;
    public new void Start()
    {
        AudSrc = GetComponent<AudioSource>();
        control = GetComponent<CharacterController>();

        if (!myBulletPool)
        {
            myBulletPool = Lean.LeanPool.getSpawnPool(this.gameObject);
        }
    }


    public new void OnSpawn()
    {
        currentDistance = -3;
    }

    protected RaycastHit objecthit;
    protected override void Update()
    {
        //Debug.Log("Updating penetratiin g" + currentDistance +   "     "+TotalRange);
        if (currentDistance > TotalRange)
        {
            Terminate(null);
            Lean.LeanPool.Despawn(this.gameObject, 0);
            return;
            //Destroy (this.gameObject);
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

        if (AbilityHeatMap.main)
        {
            AbilityHeatMap.main.UpdateLine(transform.position - transform.forward * 7, transform.position + transform.forward * (TotalRange - currentDistance),  this);
        }
    }


    public override void setup()
    {
      
    }

    public virtual void OnDespawn()
    {
        currentDistance = -3;
        if (AbilityHeatMap.main)
        {
            AbilityHeatMap.main.RemoveArea(this);
        }

    }

    public void setTarget(Vector3 Location)
    {
        lastLocation = Location;
        lastLocation.y = transform.position.y;
        if (Physics.Raycast(this.gameObject.transform.position + Vector3.up * 10, Vector3.down, out objecthit, 100, (1 << 8)))
        {
            Vector3 newPos = transform.position;
            newPos.y = objecthit.point.y + 3;

            gameObject.transform.position = newPos;
        }

        distance = Vector3.Distance(this.gameObject.transform.position, lastLocation);
        gameObject.transform.LookAt(lastLocation);


        if (AbilityHeatMap.main)
        {
            Vector3 direction = (lastLocation - transform.position).normalized;
            AbilityHeatMap.main.AddLine(transform.position, transform.position + direction * TotalRange, GetComponent<BoxCollider>().size.x, this, 1, 50);
        }
    }


    public virtual void OnTriggerEnter(Collider col)
    {

        if (col.isTrigger)
        {
            return;
        }

        UnitManager manage = col.GetComponent<UnitManager>();
        if (manage && manage.PlayerOwner != MyHitContainer.playerNumber)// && manage.PlayerOwner != 3)
        {
            //Debug.Log("Hit " + manage.gameObject);
            Terminate(manage); 
        }
    }
   
}

