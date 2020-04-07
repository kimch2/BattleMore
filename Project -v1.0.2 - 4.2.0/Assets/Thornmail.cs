using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thornmail : DamagerMonoBehavior{
    
    //Whenever this unit is attacked, it damaged all units within a radius

	public float damagePerHit = 5;
	public bool canHitAir;
	public float Range = 25;

    public void AttackEnemy(GameObject target)
    {
        if (Vector3.Distance(target.transform.position, myHitContainer.myManager.transform.position) <= Range)
        {
            target.GetComponent<UnitStats>().TakeDamage(damagePerHit, this.gameObject, DamageTypes.DamageType.Regular, myHitContainer);
        }
    }

    public void AttackEnemies()
    {
		float damageDone = 0;
		foreach (UnitManager man in myHitContainer.myManager.enemies) {
			if (man) {
				if (!man.myStats.isUnitType (UnitTypes.UnitTypeTag.Structure)) {
					if (canHitAir || !man.myStats.isUnitType (UnitTypes.UnitTypeTag.Air)) {
						if (man.myStats.isUnitType (UnitTypes.UnitTypeTag.Turret)) {
							continue;}
				
						if (Vector3.Distance (transform.position, myHitContainer.myManager.transform.position) <= Range) {
							damageDone += man.myStats.TakeDamage (damagePerHit, this.gameObject, DamageTypes.DamageType.Regular, myHitContainer);		
						}
					}
				}
			}
		}

		foreach (UnitManager man in myHitContainer.myManager.allies) {
			if (man) {
				if (!man.myStats.isUnitType (UnitTypes.UnitTypeTag.Structure)) {
					if (canHitAir || !man.myStats.isUnitType (UnitTypes.UnitTypeTag.Air)) {
						if (man.myStats.isUnitType (UnitTypes.UnitTypeTag.Turret)) {
							continue;}

						if (Vector3.Distance (transform.position, myHitContainer.myManager.transform.position) <= Range) {
							man.myStats.TakeDamage (damagePerHit * myHitContainer.FriendlyFireRatio, this.gameObject, DamageTypes.DamageType.Regular, myHitContainer);		
						}
					}
				}
			}
		}
	}
}
