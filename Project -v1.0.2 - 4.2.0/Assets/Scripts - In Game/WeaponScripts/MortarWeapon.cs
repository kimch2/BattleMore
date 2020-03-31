using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MortarWeapon : IWeapon {

	public float minimumRange;
	public float attackArc;
	public float projectileSpeed;
    public bool prioritizeFarAway;

	public override bool checkMinimumRange(UnitManager target)
	{
		float distance = Vector3.Distance(target.transform.position, transform.position) - target.CharController.radius;
	
		if ( distance < minimumRange) {

			return false;
		}
		return true;

	}


	public override bool inRange(UnitManager target)
	{
		if (this && target) {

			foreach (UnitTypes.UnitTypeTag tag in cantAttackTypes) {
				if (target.myStats.isUnitType (tag))
				{	
					return false;	}
			}


			float distance = Mathf.Sqrt((Mathf.Pow (transform.position.x - target.transform.position.x, 2) + Mathf.Pow (transform.position.z - target.transform.position.z, 2))) - target.CharController.radius;

			float verticalDistance = this.gameObject.transform.position.y - target.transform.position.y;

			if (distance > (range + (verticalDistance/3 )) ||  (minimumRange >0 && distance < minimumRange)) {


				return false;
			}
		} else {

			return false;}
		return true;

	}

	protected override GameObject createBullet()
	{
        GameObject proj;
		if (turret) {
			proj = myBulletPool.FastSpawn(turret.transform.rotation * firePoints [originIndex].position + this.gameObject.transform.position, Quaternion.identity);
		} else {
			proj= myBulletPool.FastSpawn(transform.rotation * firePoints[originIndex].position + this.gameObject.transform.position, Quaternion.identity);
		}
		MortarProjectile projScript = proj.GetComponent<MortarProjectile> ();
		projScript.arcAngle = attackArc;
		projScript.speed = projectileSpeed;
		return proj;

	}



    public override UnitManager findBestEnemy(out float distance, UnitManager best) // Similar to above method but takes into account attack priority (enemy soldiers should be attacked before buildings)
    {
        if (prioritizeFarAway)
        {
            float currentIterPriority;
            if (best != null)
            {
                distance = Vector3.Distance(best.transform.position, transform.position);
                bestPriority = best.myStats.getCombatPriority(myManager.myStats.DefensePriority);
            }
            else
            {
                distance = 0;
                bestPriority = -1;
            }

            for (int i = 0; i < myManager.enemies.Count; i++)
            {
                currentIter = myManager.enemies[i];
                if (currentIter == null || currentIter.myStats.isUnitType(UnitTypes.UnitTypeTag.Invisible))
                {
                    continue;
                }

               
                if (!isValidTarget(currentIter))
                {
                    continue;
                }
                currentIterPriority = currentIter.myStats.getCombatPriority(myManager.myStats.DefensePriority);
                if (currentIterPriority > bestPriority)
                {
                    best = currentIter;
                    bestPriority = currentIterPriority;
                    distance = Vector3.Distance(currentIter.transform.position, this.gameObject.transform.position);
                }
                else if (currentIterPriority == bestPriority)
                {
                    currDistance = Vector3.Distance(currentIter.transform.position, this.gameObject.transform.position);

                    if (currDistance > distance && currDistance < range)
                    {
                        best = currentIter;
                        distance = currDistance;
                    }
                }
            }
        }
        else
        {
            return base.findBestEnemy(out distance, best);
        }
        return best;
    }

}
