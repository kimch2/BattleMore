using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchAbility : SingleTargetCombo {

    // Dashes at target, then punches them , knocing them back

	public Vector2 PunchDistance;
	public float ComboBonusDamage;
	public float DashSpeed = 150;
    //public bool TargetGround;

    override
	public  bool Cast(GameObject tar, Vector3 location)
	{
		target = tar;
		Cast ();

		return false;

	}
	override
	public void Cast(){

		if (target) {


			if (myCost) {
				myCost.payCost ();
			}

			UnitManager targetGuy = target.GetComponent<UnitManager> ();
			Vector3 startPosition = transform.position;
			Vector3 dashLocation = targetGuy.transform.position - (targetGuy.transform.position - transform.position).normalized * 5;

            PhysicsSimulator.main.Dash (manage,this, dashLocation, new Vector2( DashSpeed,0), 
				() =>{
					if(this.gameObject && targetGuy){
						PhysicsSimulator.main.KnockBack (startPosition, targetGuy, this, PunchDistance, () => { 
					if(targetGuy != null){
								if (ComboTag.CastTag (target, TagType, Combination)) {
									targetGuy.myStats.TakeDamage (ComboBonusDamage, this.gameObject,DamageTypes.DamageType.Regular, manage );
									if(myCost)
									{
										myCost.cooldownTimer = 0;
									}
								}
							}
				        } ,false);

			
					targetGuy.myStats.TakeDamage (ComboBonusDamage, this.gameObject,DamageTypes.DamageType.Regular, manage );

				
				    }
				}
			);
		}
	}

}
