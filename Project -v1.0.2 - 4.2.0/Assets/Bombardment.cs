﻿using UnityEngine;
using System.Collections;

public class Bombardment : TargetAbility{


	public GameObject Explosion;
	public int shotCount = 35;
	public float myDamage = 40;

	public float FriendlyFire = 1;


	Lean.LeanPool myBulletPool;
    public override void Start()
    {
		base.Start();
		if (Explosion) {
			myBulletPool = Lean.LeanPool.getSpawnPool (Explosion);
		}
	}

	override
	public continueOrder canActivate(bool showError){

		continueOrder order = new continueOrder ();

		if (!myCost.canActivate (this)) {
			order.canCast = false;
		} 
		return order;
	}

	override
	public void Activate()
	{
	}  // returns whether or not the next unit in the same group should also cast it


	override
	public  void setAutoCast(bool offOn){}

	public override bool isValidTarget (GameObject target, Vector3 location){

		return true;

	}



	override
	public  bool Cast(GameObject target, Vector3 location)
	{
		myCost.payCost ();

		for (int i = 0; i < shotCount; i++) {
		
			StartCoroutine( Fire ((i * .087f), location, i));
		}
		GameObject sight = new GameObject("SightObject");
		sight.transform.position = location;
		FogOfWarUnit fogger = sight.AddComponent<FogOfWarUnit>();
		fogger.radius = 55;
		fogger.hasMoved = true;

		StartCoroutine( shakeCamera ());
		return false;
	}

	IEnumerator shakeCamera()
	{
		yield return new WaitForSeconds (1.5f);
		MainCamera.main.ShakeCamera (4, 10,.08f);
	}


	IEnumerator Fire (float time, Vector3 location, int index)
	{

		yield return new WaitForSeconds(time);
		GameObject proj = null;


		Vector3 hitzone = location;
		float radius = Mathf.Sqrt( ((float)index/(float)shotCount ))* 43;
		float angle = index * 15;

		if (index % 2 == 1) {
			angle += 180;
		}
		hitzone.x += Mathf.Sin (Mathf.Deg2Rad * angle) * radius;
		hitzone.z += Mathf.Cos (Mathf.Deg2Rad * angle) * radius;

		
		RaycastHit objecthit;

		if (Physics.Raycast(hitzone + Vector3.up, Vector3.down, out objecthit, 100, 1 << 8))
		{
			hitzone = objecthit.point;
		}
		Vector3 spawnLoc = hitzone;
		spawnLoc.y += 192;


		proj = myBulletPool.FastSpawn  (spawnLoc, Quaternion.identity);//Instantiate (Explosion, spawnLoc, Quaternion.identity);

		Projectile script = proj.GetComponent<Projectile> ();
		//proj.SendMessage ("setSource", this.gameObject, SendMessageOptions.DontRequireReceiver);
		script.setSource (this.gameObject);
		if (script) {
			script.damage = myDamage;
			script.sourceInt = 1;
			script.Source = this.gameObject;
			script.setLocation (hitzone);
            // FIX THIS BY ADDING A ONHITCONTAINER!!
            //script.FriendlyFire = FriendlyFire;

		}
		
	}



	override
	public void Cast(){
	}


}