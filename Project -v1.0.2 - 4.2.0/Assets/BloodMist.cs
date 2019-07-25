using UnityEngine;
using System.Collections;

public class BloodMist : TargetAbility {


	protected Selected mySelect;
	public GameObject BloodMistObj;
	public bool OnlyOnPathable;

	// Use this for initialization
	new void Start () {
		myType = type.target;
		mySelect = GetComponent<Selected> ();
        InitializeCharges();
	}



	override
	public continueOrder canActivate(bool showError){

		continueOrder order = new continueOrder ();
		if (chargeCount == 0 && chargeCount != -1) {
			order.canCast = false;
		}

		if (!myCost.canActivate (this)) {
			order.canCast = false;

			// FIX THIS LINE IN THE FUTURE IF IT BREAKS! its currently in here to allow guys with multiple charges to use them even though the cooldown timer is shown.
			if (myCost.energy == 0 && myCost.resourceCosts.MyResources.Count == 0 && chargeCount > 0) {
				order.canCast = true;
			}
		} else {
			order.nextUnitCast = false;
		}
		if (order.canCast) {
			order.nextUnitCast = false;
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
		if (OnlyOnPathable) {
			return onPathableGround (location);
		}



		return true;

	}


	override
	public  bool Cast(GameObject target, Vector3 location)
	{

        changeCharge(-1);

		myCost.payCost ();

		Vector3 pos = location;
		pos.y += 5;
		GameObject proj = (GameObject)Instantiate (BloodMistObj, pos, Quaternion.identity);
        
		proj.SendMessage ("setSource", this.gameObject);


		return false;

	}
	override
	public void Cast(){

        changeCharge(-1);
		GameObject proj = null;

		Vector3 pos = location;
		pos.y += 5;
		proj = (GameObject)Instantiate (BloodMistObj, pos, Quaternion.identity);


		proj.SendMessage ("setSource", this.gameObject,SendMessageOptions.DontRequireReceiver);
	}



}
