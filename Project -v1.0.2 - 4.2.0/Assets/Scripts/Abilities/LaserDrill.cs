using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDrill : MonoBehaviour {

	public static LaserDrill instance;

	public List<UnitManager> spottedTargets = new List<UnitManager>();
	public UnitManager currentTarget;

	public LineRenderer line;
	public Vector3 LaserStartPoint;
	public  GameObject particle;


	// Use this for initialization
	void Start () {
		instance = this;
		

	}
	public GameObject EndEffect;
	protected MultiShotParticle fireEffect;

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



	Coroutine firing;

	public void addtarget(UnitManager obj)
	{

		if (!currentTarget) {
			currentTarget = obj;
			firing =	StartCoroutine (attackTarget());
		}

		spottedTargets.Add (obj);

	}

	public void removeTarget(UnitManager toRemove)
	{	spottedTargets.RemoveAll (item => item == null);
		spottedTargets.Remove (toRemove);
		if (currentTarget == toRemove) {
			switchTarget ();
		}
	}

	public void removeTargets(List<UnitManager> toRemove)
	{	spottedTargets.RemoveAll (item => item == null);
		if (currentTarget != null && toRemove.Contains (currentTarget)) {
			foreach (UnitManager manage in toRemove) {
				spottedTargets.Remove (manage);
			}

			switchTarget ();
		} else {
			foreach (UnitManager manage in toRemove) {
				spottedTargets.Remove (manage);
			}
		}
	}


	void switchTarget()
	{
		currentTarget = null;
		if (particle) {
			particle.SetActive (false);
		}
		fireEffect.gameObject.SetActive (false);
		if (firing != null) {
			StopCoroutine (firing);	
		}	

		spottedTargets.RemoveAll (item => item == null);
		if (spottedTargets.Count > 0) {
			currentTarget = spottedTargets [0];
			firing = StartCoroutine (attackTarget());
		}

	}

	IEnumerator attackTarget()
	{
		StartCoroutine (trackTarget ());
		particle.SetActive (true);
		while (currentTarget) {
			yield return new WaitForSeconds (.3f);
			if (currentTarget) {
				
				RaycastHit objecthit;
				Vector3 towards = currentTarget.transform.position - transform.position; 
				Ray poniter = new Ray (transform.position,towards);

	
				if (Physics.Raycast (this.gameObject.transform.position, towards, out objecthit, 20000, 1 << 20, QueryTriggerInteraction.Collide)) {
					if (Vector3.Distance(this.transform.position, currentTarget.transform.position)  > objecthit.distance) {
						barrierShield shield = objecthit.transform.gameObject.GetComponentInParent<barrierShield> ();

						if (shield) {
							shield.takeDamage (50);
						}
					} else {
						currentTarget.myStats.TakeDamage (25, this.transform.root.gameObject, DamageTypes.DamageType.Regular);
					}
				
				} else {
					
					currentTarget.myStats.TakeDamage (25, this.transform.root.gameObject, DamageTypes.DamageType.Regular);
				}
			}
		}
		firing = null;
		switchTarget ();
	}

	IEnumerator trackTarget()
	{
		Vector3 spotter = currentTarget.transform.position;

		while(currentTarget){
	
			RaycastHit objecthit;
			Vector3 towards = currentTarget.transform.position - transform.position; 
			Ray poniter = new Ray (transform.position,towards);


			if (Physics.Raycast (this.gameObject.transform.position, towards, out objecthit, 20000, 1 << 20, QueryTriggerInteraction.Collide)) {

				if (Vector3.Distance(transform.position, currentTarget.transform.position) > objecthit.distance) {
					line.SetPositions (new Vector3[] {
						(transform.rotation) * LaserStartPoint + this.gameObject.transform.position,
						objecthit.point});
					CreateEndEffect (objecthit.point + Vector3.up*2);
				
				} else {
					line.SetPositions (new Vector3[] {
						(transform.rotation) * LaserStartPoint + this.gameObject.transform.position,
						currentTarget.transform.position + Vector3.up * 2
					});
					CreateEndEffect (currentTarget.transform.position + Vector3.up*2);
				}

			} else {

				line.SetPositions (new Vector3[] {
					(transform.rotation) * LaserStartPoint + this.gameObject.transform.position,
					currentTarget.transform.position + Vector3.up * 2
				});
				CreateEndEffect (currentTarget.transform.position + Vector3.up*2);
			}
				
				spotter = currentTarget.transform.position;
				spotter.y = this.transform.position.y;
				particle.transform.LookAt (currentTarget.transform.position + Vector3.up * 2);
				this.gameObject.transform.LookAt (spotter);
			
			yield return null;
		}

		line.SetPositions (new Vector3[]{(transform.rotation) * LaserStartPoint + this.gameObject.transform.position, (transform.rotation) * LaserStartPoint + this.gameObject.transform.position});	
	}

	public void OnDrawGizmos()
	{

		Gizmos.DrawSphere ((transform.rotation) * LaserStartPoint + this.gameObject.transform.position, .5f);

	}

}
