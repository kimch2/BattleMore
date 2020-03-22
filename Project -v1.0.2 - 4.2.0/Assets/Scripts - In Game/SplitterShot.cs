using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplitterShot : Projectile {

	public float chargesRemaning; // each target hit will decrease by 1. at zero stop bouncing.

	public List<UnitManager> nearbyTargets= new List<UnitManager>();

	public GameObject ShotINstance;
	public SplitterHitList hitlist;

	public int NumOfBranches=1;

	void Awake()
	{
		Invoke ("resetTarget", .1f);
	}

	void resetTarget()
	{
		randomOffset = Vector3.zero;
	}
	
	public void setDamage(float so)
	{		
		damage = so;
	}

    public override void setup()
    {
        base.setup();
        hitlist = MyHitContainer.myManager.GetComponent<SplitterHitList>();
        hitlist.hitTargets.Add(this.target);
    }

    override
	public void Terminate(UnitManager targ)
	{
		if (targ != null) {
			targ.GetComponent<UnitStats>().TakeDamage(damage,Source, DamageTypes.DamageType.Regular, MyHitContainer);
		}
       
		if (chargesRemaning > 0) {
			if (nearbyTargets.Count > 0) {
				chargesRemaning --;

				this.target = findBestEnemy ();

                if (this.target)
                {

                    lastLocation = this.target.transform.position;
                    distance = Vector3.Distance(this.transform.position, this.target.transform.position);
                    currentDistance = 0;
                    hitlist.hitTargets.Add(this.target);


                    for (int i = 1; i < NumOfBranches; i++)
                    {
                        UnitManager nextTarget = findBestEnemy();
                        if (nextTarget)
                        {


                            GameObject clone = (GameObject)Instantiate(ShotINstance, this.gameObject.transform.position, new Quaternion());
                            clone.GetComponent<SplitterShot>().Initialize(nextTarget, damage, MyHitContainer);
                            clone.GetComponent<SplitterShot>().chargesRemaning = chargesRemaning;
                            clone.GetComponent<SplitterShot>().hitlist = this.hitlist;
                            hitlist.hitTargets.Add(nextTarget);


                            foreach (UnitManager obj in clone.GetComponent<SplitterShot>().nearbyTargets)
                            {
                                this.nearbyTargets.Add(obj);
                            }
                        }
                    }
                    return;
                }

			}
		} 
		Destroy (this.gameObject);
		
	}




	void OnTriggerEnter(Collider other)
	{
		if (MyHitContainer) {

			if (!other.isTrigger) {
			
				//if (other.gameObject.layer.Equals("Unit"))
				UnitManager manage = other.GetComponent<UnitManager> ();
				if (manage == null) {
					manage = other.GetComponentInParent<UnitManager> ();
				}
			
				if (manage != null) {
					if (manage.PlayerOwner != MyHitContainer.playerNumber)
                    {					
						nearbyTargets.Add (manage);										
					}
				}
			}
		}
	}


	void OnTriggerExit(Collider other)
	{
		UnitManager manage = other.GetComponent<UnitManager> ();
        if (manage == null)
        {
            manage = other.GetComponentInParent<UnitManager>();
        }

        if (manage) {
			nearbyTargets.Remove (manage);
		}		
	}

	public UnitManager findBestEnemy()
	{
		UnitManager best = null;
		float priority = 1000;

		nearbyTargets.RemoveAll(item => item == null);
		for (int i = nearbyTargets.Count -1; i >= 0; i --) {

		//	Debug.Log(obj.name);
			if(nearbyTargets[i] == null)
			{
                nearbyTargets.Remove(nearbyTargets[i]);
			}
			else if(hitlist.isValidEnemy(nearbyTargets[i]) && 
                Vector3.Distance(nearbyTargets[i].transform.position, this.gameObject.transform.position) < priority)
			    {
				    best = nearbyTargets[i];
				    priority = Vector3.Distance(nearbyTargets[i].transform.position, this.gameObject.transform.position);
			    }
		}
	
		return best;
	}
}
