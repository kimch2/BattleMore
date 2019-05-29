using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryBastion  : TargetAbility {


	public GameObject TargetReticule;
	public float ZoneRadius;
	public GameObject BasicShell;
	Coroutine currentCharger;

	public int maxChargeCount = 500;


    // Use this for initialization
    public override void Start()
    {
		myType = type.target;
		WaitForAmmo ();
	}


	override
	public continueOrder canActivate(bool showError){

		continueOrder order = new continueOrder ();
		order.canCast = true;
		return order;
	}

	override
	public void Activate()
	{
	}  // returns whether or not the next unit in the same group should also cast it


	override
	public  void setAutoCast(bool offOn){

		autocast = !autocast;

	}

	public override bool isValidTarget (GameObject target, Vector3 location){
		return true;
	}




	override
	public  bool Cast(GameObject target, Vector3 location)
	{
		
		this.location = location;
		TargetReticule.transform.position = location;
		autocast = true;

		updateAutocastCommandCard ();
		return false;

	}
	override
	public void Cast(){
		
		autocast = true;
		TargetReticule.transform.position = location;
		updateAutocastCommandCard ();

	}

	void Fire()
	{
		
		Vector3 hitzone = location;
		float radius = Random.Range (0, ZoneRadius);
		float angle = Random.Range (0, 360);

		hitzone.x += Mathf.Sin (Mathf.Deg2Rad * angle) * radius;
		hitzone.z += Mathf.Cos (Mathf.Deg2Rad * angle) * radius;

		RaycastHit objecthit;

		if (Physics.Raycast (hitzone + Vector3.up * 100, Vector3.down, out objecthit, 1000, 1 << 8)) {

			GameObject obj= (GameObject)Instantiate (BasicShell, objecthit.point,Quaternion.identity);
			obj.SendMessage ("setSource", this.gameObject,SendMessageOptions.DontRequireReceiver);

			changeCharge (-1);
		}
	


		if (chargeCount > 0) {
			Invoke ("Fire", .1f);
		} else {
			WaitForAmmo ();
		}
	}

	void WaitForAmmo()
	{
		if (chargeCount > 0 && autocast) {
			Fire ();
		}
		else{
			Invoke ("WaitForAmmo", .1f);
		}
			
	}


	public void changeCharge(int n)
	{
		chargeCount += n;

	
		updateUICommandCard ();
	}

}