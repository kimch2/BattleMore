using UnityEngine;
using System.Collections;

public class Bombardment : TargetAbility{


	public GameObject Explosion;
	public int shotCount = 35;
	public float myDamage = 40;

	public float FriendlyFire = 1;
    public bool shakeTheCamera = true;


	Lean.LeanPool myBulletPool;
    public override void Start()
    {
		base.Start();
        myTargetType = targetType.ground;
        myType = type.target;
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
	public  bool Cast(GameObject target, Vector3 locat)
	{
		myCost.payCost ();

		for (int i = 0; i < shotCount; i++) {
		
			StartCoroutine( Fire ((i * .087f), locat, i));
		}
		GameObject sight = new GameObject("SightObject");
		sight.transform.position = locat;
		FogOfWarUnit fogger = sight.AddComponent<FogOfWarUnit>();
		fogger.radius = 55;
		fogger.hasMoved = true;
        if (shakeTheCamera)
        {
            StartCoroutine(shakeCamera());
        }
		return false;
	}

	IEnumerator shakeCamera()
	{
		yield return new WaitForSeconds (1.5f);
		MainCamera.main.ShakeCamera (4, 10,.08f);
	}


	IEnumerator Fire (float time, Vector3 locat, int index)
	{

		yield return new WaitForSeconds(time);
		GameObject proj = null;


		Vector3 hitzone = locat;
		float radius = Mathf.Sqrt( ((float)index/(float)shotCount ))* areaSize;
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
        Vector3 spawnLoc;
        if (CastFromScreenEdge)
        {
            spawnLoc = DaminionsInitializer.main.getScreenEdge(locat, 15, myManager.PlayerOwner, true);
            spawnLoc.y += 75;
        }
        else
        {
            spawnLoc = hitzone;
            spawnLoc.y += 192;
        } 

		proj = myBulletPool.FastSpawn  (spawnLoc, Quaternion.identity);//Instantiate (Explosion, spawnLoc, Quaternion.identity);

		Projectile script = proj.GetComponent<Projectile> ();
        SetOnHitContainer(proj, myDamage, null);
        script.setLocation(hitzone);
	
	}



	override
	public void Cast(){
        Cast(target, location);
    }


}