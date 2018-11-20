using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamWeapon : IWeapon{


	public LineRenderer line;
	public Vector3 LaserStartPoint;
	public  GameObject particle;


	public GameObject EndEffect;
	protected override void DealDamage(float damage, UnitManager target)
	{

		if (target) {
			damage = fireTriggers (this.gameObject, null, target, damage);


			RaycastHit objecthit;
	
			Vector3 towards =  target.transform.position - transform.position; 
			float distance = Vector3.Distance (transform.position,  target.transform.position) -5; 

			if (Physics.Raycast (this.gameObject.transform.position, towards, out objecthit, 20000, 1 << 20, QueryTriggerInteraction.Collide)) {
					if (objecthit.distance < distance) {
					barrierShield shield = objecthit.transform.gameObject.GetComponentInParent<barrierShield> ();
						
						if (shield) {
							shield.takeDamage(damage);
						CreateEndEffect (objecthit.point + Vector3.up*2);

						}
					} else {
					CreateEndEffect (target.transform.position + Vector3.up*2);
					target.myStats.TakeDamage (damage, this.transform.root.gameObject, DamageTypes.DamageType.Regular);
					}

				} else {
				CreateEndEffect (target.transform.position + Vector3.up*2);
				target.myStats.TakeDamage (damage, this.transform.root.gameObject, DamageTypes.DamageType.Regular);
				}
			}

		if (Tracking != null) {
			StopCoroutine (Tracking);
		}
		Tracking = StartCoroutine (trackTarget(target));

	}

	void CreateEndEffect(Vector3 location)
	{
		if (EndEffect) {

			if (!fireEffect) {
				GameObject temp = (GameObject)Instantiate (EndEffect,location, Quaternion.identity);
				fireEffect =temp.GetComponent<MultiShotParticle> ();
				if (fireEffect) {
					fireEffect.playEffect ();
				}
			} else {
				if (!fireEffect.gameObject.activeSelf) {
				
				
					fireEffect.gameObject.SetActive (true);
					fireEffect.playEffect ();
				}
				fireEffect.transform.position = location;

			}
		}
	}

	Coroutine Tracking;

	IEnumerator trackTarget(UnitManager target)
	{
		particle.SetActive (true);
		fireEffect.playEffect();
		for(float i = 0; i < attackPeriod ; i += Time.deltaTime)
			{yield return null;
			if (target) {

				particle.transform.LookAt (target.transform.position);
				RaycastHit objecthit;
				Vector3 towards =  target.transform.position - transform.position; 
				float distance = Vector3.Distance (transform.position,  target.transform.position) -5; 

				if (Physics.Raycast (this.gameObject.transform.position, towards, out objecthit, 20000, 1 << 20, QueryTriggerInteraction.Collide)) {
					if (objecthit.distance < distance) {
						fireEffect.transform.position = objecthit.point+ Vector3.up *2;
						line.SetPositions (new Vector3[] {line.transform.position,objecthit.point + Vector3.up *2+ towards.normalized*2});
					}
					 else {
						fireEffect.transform.position = target.transform.position+ Vector3.up *2;
						line.SetPositions (new Vector3[] {line.transform.position,target.transform.position+ Vector3.up *2+ towards.normalized *2});
					}

				} else {
					fireEffect.transform.position = target.transform.position+ Vector3.up *2;
					line.SetPositions (new Vector3[] {line.transform.position,target.transform.position+ Vector3.up *2 + towards.normalized*2});
				}
			}
		}
		yield return null;
		fireEffect.gameObject.SetActive (false);
		particle.SetActive (false);
		line.SetPositions (new Vector3[]{(transform.rotation) * LaserStartPoint + this.gameObject.transform.position, (transform.rotation) * LaserStartPoint + this.gameObject.transform.position});	
	}

	public new void OnDrawGizmos()
	{

		Gizmos.DrawSphere ((transform.rotation) * LaserStartPoint + this.gameObject.transform.position, .5f);

	}

	public void Dying()
	{
		if (fireEffect) {
			Destroy (fireEffect.gameObject);
		}
	}

}
