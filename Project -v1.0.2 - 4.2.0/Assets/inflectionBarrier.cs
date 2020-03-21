using UnityEngine;
using System.Collections;

public class inflectionBarrier : MonoBehaviour, Notify {


	public float Health;
	public float duration;

	public GameObject Effect;
	public GameObject source;

	UnitManager attachedUnit;

	public void setSource(GameObject o)
	{source = o;}

	//private float radius;
	// Use this for initialization
	void Start () {

		Instantiate (Effect, this.gameObject.transform.position, Quaternion.identity);
		//radius = GetComponent<SphereCollider> ().radius;

	}

	// Update is called once per frame
	void Update () {

		duration -= Time.deltaTime;
		if (duration <= 0) {
			foreach (IWeapon weap in  attachedUnit.myWeapon) {
				weap.removeNotifyTrigger (this);
			}
			Destroy (this.gameObject);
		}

	}

	public void initialize (UnitManager attachUnit)
	{
		attachedUnit = attachUnit;
		foreach (IWeapon weap in  attachedUnit.myWeapon) {
			weap.addNotifyTrigger (this);
		
		}

	}

	public void castAgain()
	{

		Health = Health/2 + 150;
		duration = duration/2 + 10;
	}


	public float trigger(GameObject source, GameObject projectile, UnitManager target, float damage)
	{
		//Debug.Log ("Triggering");

		Health -= damage;

		Vector3 direction = source.transform.position +  (target.gameObject.transform.position - source.transform.position).normalized *9;
		if (projectile) {

			Projectile proj = projectile.GetComponent<Projectile> ();
			if (proj) {
				proj.Despawn ();
				if (proj.explosionO) {

					attachedUnit.myStats.TakeDamage (damage,source,DamageTypes.DamageType.Regular, proj.MyHitContainer );
					explosion Escript =proj.explosionO.GetComponent<explosion> ();
					if (Escript) {
						
						Instantiate (Escript.particleEff , direction, Quaternion.identity);
					}



				}


			}
		} 

	
		Instantiate (Effect, direction, Quaternion.identity);


		if (Health <= 0) {
			foreach (IWeapon weap in  attachedUnit.myWeapon) {
				weap.removeNotifyTrigger (this);
			}
			if(this && this.gameObject)
			Destroy (this.gameObject);
		}
		return 0;
	}


}
