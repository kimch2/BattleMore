using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShotProjectile  : Projectile {


    public float TotalRange;
    public UnityEngine.Events.UnityEvent OnHit;
    public UnityEngine.Events.UnityEvent OnKill;
    public new void Start()
    {
        AudSrc = GetComponent<AudioSource>();
        control = GetComponent<CharacterController>();

        if (!myBulletPool)
        {
            myBulletPool = Lean.LeanPool.getSpawnPool(this.gameObject);
        }
    }

    new void OnSpawn()
    {
        currentDistance = -3;
    }

    RaycastHit objecthit;
    protected override void Update()
    {
        //Debug.Log("Updating penetratiin g" + currentDistance +   "     "+TotalRange);
        if (currentDistance > TotalRange)
        {
            Lean.LeanPool.Despawn(this.gameObject, 0);
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

        gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);

        currentDistance += speed * Time.deltaTime;
    }


    public override void setup()
    {
      
    }

    public void OnDespawn()
    {
        currentDistance = -3;
    }

    public void setTarget(Vector3 Location)
    {

        lastLocation = Location + Vector3.up * 2;
        


        if (Physics.Raycast(this.gameObject.transform.position + Vector3.up * 10, Vector3.down, out objecthit, 100, (1 << 8)))
        {
            Vector3 newPos = transform.position;
            newPos.y = objecthit.point.y + 3;

            gameObject.transform.position = newPos;
        }

        distance = Vector3.Distance(this.gameObject.transform.position, lastLocation);
        gameObject.transform.LookAt(lastLocation);
    }


    public void OnTriggerEnter(Collider col)
    {

        if (col.isTrigger)
        {
            return;
        }

        UnitManager manage = col.GetComponent<UnitManager>();
        if (manage && manage.PlayerOwner != sourceInt && manage.PlayerOwner != 3)
        {
            //Debug.Log("Hit " + manage.gameObject);
            manage.getUnitStats().TakeDamage(damage, Source, damageType);
            //	Debug.Log ("Dealing " + damage * (1 - NumGuysHit * PercDamLost*.01f) + " to  "+ manage.gameObject );
            if (SpecialEffect)
            {
                Instantiate(SpecialEffect, transform.position, Quaternion.identity);
            }
            OnHit.Invoke();
            OnHit.RemoveAllListeners();

            
            if (!manage || manage.myStats.health <= 0)
            {
                Debug.Log("We killed him ");
                OnKill.Invoke();
            }

            Lean.LeanPool.Despawn(this.gameObject, 0);

        }

    }




}

