using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlasmaDischarge : Ability {


	private bool on;
	public float duration;
	public float damagePerSecond;
	 GameObject mychargeUP;
	public GameObject chargeUpEffect;
	private float timer;


	public GameObject explodeEffect;
	private GameObject popUp;

	new void Start () {

		audioSrc = GetComponent<AudioSource>();
		myType = type.activated;
	}

	// Update is called once per frame
	void Update () {
		if (on && Time.time > timer) {

			Deactivate ();

		} else if (on) {
			if (popUp) {
				popUp.GetComponent<TextMesh> ().text = ""+(int)((duration - (timer - Time.time)) * damagePerSecond);
			}
		}

	}

	public override void setAutoCast(bool offOn){
	}


	override
	public continueOrder canActivate (bool showError)
	{
		continueOrder order = new continueOrder ();

		if (myCost.canActivate (this) || on) {
			order.canCast = true;
		}
		return order;
	}

	override
	public void Activate()
	{

			if (!on) {
				if (myCost.canActivate (this)) {

					myCost.payCost ();
					on = true;
					timer = Time.time + duration;
				popUp = PopUpMaker.CreateGlobalPopUp ("0", Color.red, this.gameObject.transform.position + Vector3.up * 7,duration);
				popUp.transform.SetParent (this.gameObject.transform);
				mychargeUP = (GameObject)Instantiate (chargeUpEffect, transform.position, Quaternion.identity);
				mychargeUP.transform.SetParent (this.gameObject.transform);

				}
			} else {

				if (timer - Time.time < (duration - 1)) {
					Deactivate ();
				}
			}


		//return true;//next unit should also do this.
	}

	public void Deactivate()
	{on = false;

		Destroy (mychargeUP);
		if (explodeEffect) {
			Instantiate (explodeEffect, this.transform.position, Quaternion.identity);
		}


        float BaseDamage = (duration - (timer - Time.time)) * damagePerSecond;
		foreach(UnitManager obj in myManager.enemies)
		{
			if (obj) {

				obj.myStats.TakeDamage (BaseDamage, this.gameObject, DamageTypes.DamageType.Regular, myHitContainer);
				PopUpMaker.CreateGlobalPopUp ("-" + (int)((duration - (timer - Time.time)) * damagePerSecond), Color.red, obj.transform.position);
				if (explodeEffect) {
					Instantiate (explodeEffect, obj.transform.position, Quaternion.identity);
				}
			}
		
		}

		Destroy (popUp);

	}
}
