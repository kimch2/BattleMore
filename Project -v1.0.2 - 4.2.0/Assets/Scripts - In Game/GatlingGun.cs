using UnityEngine;
using System.Collections;

public class GatlingGun :  Ability,Notify, Modifier {


	public IWeapon myWeapon;

	public float speedIncrease = .1f;

	public float MinimumPeriod = .2f;

	private float nextActionTime;

	private float lastFired;
	private float totalSpeed;
	private bool Revved = false;

	public Animator myAnim;
	UnitManager parentManager;



	// Use this for initialization
	void Start () {
		if (myWeapon == null) {

			myWeapon = this.gameObject.GetComponent<IWeapon> ();
		}

		//healthD = GetComponent<Selected> ();
	
		myWeapon.triggers.Add (this);
		nextActionTime = Time.time + 3;
		parentManager = transform.parent.GetComponentInParent<UnitManager>();
	}



	// Update is called once per frame
	void Update () {	

		if (!Revved) {
			return;
		}

		if (myWeapon) {
			
			if (Time.time > nextActionTime) {
				nextActionTime += .4f;

				if (Time.time - lastFired > 2.5f) {
					if (myAnim) {
						myAnim.SetInteger ("State", 2);
					}
					if (revCount > 0) {

						parentManager.myStats.statChanger.changeSpecAttSpeed(-.357f ,0, this, myWeapon, true);
						revCount--;
					
					} else if (revCount == 0) {
						parentManager.myStats.statChanger.removeSpecAttSpeed(this, myWeapon);
					}
				}
			}
		}
	}

	public float trigger(GameObject source, GameObject proj,UnitManager target,float damage)
	{IncreaseSpeed (0, source);
		return damage;
	}



	int revCount = 0;

	public void IncreaseSpeed(float damage, GameObject source)
	{	
		nextActionTime = Time.time + 2.5f;
		Revved = true;

		if (revCount < 7) {
			revCount++;

			parentManager.myStats.statChanger.changeSpecAttSpeed(.357f , 0, this, myWeapon, true);
			lastFired = Time.time;
			if (myAnim) {
				myAnim.SetInteger ("State", 1);
			}
		}
	}




	public float modify(float damage, GameObject source, DamageTypes.DamageType theType)
	{ 
		
		if (myWeapon.myManager.myWeapon.Contains(myWeapon)) {
			foreach (TurretMount turr in transform.parent.GetComponentsInParent<TurretMount> ()) {

				if (turr.turret != null) {
					myWeapon.myManager.myWeapon.Contains(turr.turret.GetComponent<IWeapon> ());
					return 0 ;
				}
		
			}
				
			myWeapon.myManager.changeState (new DefaultState ());
		}
		return 0 ;

	}



	public override void setAutoCast(bool offOn){
	}


	override
	public continueOrder canActivate (bool showError)
	{

		continueOrder order = new continueOrder ();
		return order;
	}

	override
	public void Activate()
	{
		//return true;//next unit should also do this.
	}

}
