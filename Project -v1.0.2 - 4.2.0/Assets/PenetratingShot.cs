﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenetratingShot : Projectile {

	public List<GameObject> hitGuys = new List<GameObject>();

	public int NumGuysHit;
    public int MaxNumCanHit = 5;

	public float PercDamLost;
	public float TotalRange;

	public new void Start () {
		AudSrc= GetComponent<AudioSource> ();
		control = GetComponent<CharacterController> ();

		if (!myBulletPool) {
			myBulletPool = Lean.LeanPool.getSpawnPool (this.gameObject);
		}
	}

	new void OnSpawn()
	{
		currentDistance = -3;
		NumGuysHit = 0;
		hitGuys.Clear ();
	}

	RaycastHit objecthit;
	protected override void Update()
	{
		//Debug.Log("Updating penetratiin g" + currentDistance +   "     "+TotalRange);
		if(currentDistance > TotalRange)
		{
			Lean.LeanPool.Despawn (this.gameObject, 0);
			//Destroy (this.gameObject);
		}
			

		if (Physics.Raycast (this.gameObject.transform.position + Vector3.up * 5, Vector3.down, out objecthit, 30, ( 1 << 8))) {
			float dist = Vector3.Distance (objecthit.point, transform.position);
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

		gameObject.transform.Translate (Vector3.forward* speed * Time.deltaTime );

		currentDistance += speed * Time.deltaTime ;
	}


	public override void setup()
	{
		if (target) {

			//CharacterController cont = 
				target.GetComponent<CharacterController> ();

			randomOffset = Vector3.up;

			lastLocation = target.transform.position + randomOffset;
			distance = Vector3.Distance (this.gameObject.transform.position, lastLocation);
            setTarget(target);
		}
	}

	public void OnDespawn()
	{
		currentDistance = -3;
		NumGuysHit = 0;
		hitGuys.Clear ();	
	}
		
	public void setTarget(UnitManager so)
	{
		target = so;

		if (target)
        {		
			lastLocation = target.transform.position + Vector3.up*2;
		}


		if (Physics.Raycast (this.gameObject.transform.position + Vector3.up * 10, Vector3.down, out objecthit, 100, ( 1 << 8)))
        {
			Vector3 newPos = transform.position;
			newPos.y = objecthit.point.y + 3;
			gameObject.transform.position = newPos;
		}

		distance = Vector3.Distance (this.gameObject.transform.position, lastLocation);
		gameObject.transform.LookAt (lastLocation);
	}


	public  void OnTriggerEnter(Collider col)
	{

		if (col.isTrigger) {

			return;}

		if (!hitGuys.Contains (col.gameObject) && currentDistance > 0) {
			

			hitGuys.Add (col.gameObject);
			UnitManager manage = col.GetComponent<UnitManager> ();
			if (manage && manage.PlayerOwner != MyHitContainer.playerNumber && manage.PlayerOwner != 3) {
				
			
				manage.getUnitStats ().TakeDamage (damage * (1 - NumGuysHit * PercDamLost * .01f), Source, GetComponent<Projectile> ().damageType, MyHitContainer);
				//	Debug.Log ("Dealing " + damage * (1 - NumGuysHit * PercDamLost*.01f) + " to  "+ manage.gameObject );
				NumGuysHit++;
				if (SpecialEffect) {
					Instantiate (SpecialEffect, transform.position, Quaternion.identity);
				}

				if (NumGuysHit * PercDamLost * .01f > 1 || NumGuysHit >= MaxNumCanHit) {
					Lean.LeanPool.Despawn (this.gameObject, 0);
					//Destroy (this.gameObject);
				}
			
			
			}
		}
	}
}
