using UnityEngine;
using System.Collections;

public class StimPack : Ability {


	private bool on;
	public float duration;
	public float speedBoost;


	private float timer;

	public MultiShotParticle BoostEffect;

	// Use this for initialization

	new void Awake()
	{
        base.Awake();
        audioSrc = GetComponent<AudioSource> ();
		myType = type.activated;
	}

	new void Start () {

        description = "Uses life to give a short burst of speed";
		select = GetComponent<Selected> ();
		myManager = GetComponent<UnitManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (on && Time.time  > timer) {
			
		
				Deactivate ();

		} 

	}

	public override void setAutoCast(bool offOn){
	}


	override
	public continueOrder canActivate (bool showError)
		{

		continueOrder order = new continueOrder ();

		if (chargeCount == 0) {
			order.canCast = false;
			return order;}
		if (!myCost.canActivate (this)) {
			order.canCast = false;
		}
		return order;
		}

	override
	public void Activate()
	{
		if (myCost.canActivate (this)) {

			if (!on) {

				myManager.myStats.statChanger.changeMoveSpeed (0,speedBoost,this,true);

			
				BoostEffect.continueEffect ();
				myCost.payCost ();
				on = true;
				timer = Time.time + duration;
				chargeCount--;
				if (select.IsSelected) {
					RaceManager.upDateUI ();
				}
			
			}
		
		}
		//return true;//next unit should also do this.
	}

	public void Deactivate()
	{on = false;
		myManager.myStats.statChanger.removeMoveSpeed (this);
		BoostEffect.stopEffect ();


	}


}
