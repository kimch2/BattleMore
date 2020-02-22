using UnityEngine;
using System.Collections;

public class BloodMist : TargetAbility {


	protected Selected mySelect;
	public GameObject BloodMistObj;
	public bool OnlyOnPathable;

	// Use this for initialization
	new void Start () {
        base.Start();
		myType = type.target;
		mySelect = GetComponent<Selected> ();
        InitializeCharges();
	}



	override
	public continueOrder canActivate(bool showError){

        return BaseCanActivate(showError);
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
        myCost.payCost();
        changeCharge(-1);
		GameObject proj = null;

		Vector3 pos = location;
		pos.y += 5;
		proj = (GameObject)Instantiate (BloodMistObj, pos, Quaternion.identity);


		proj.SendMessage ("setSource", this.gameObject,SendMessageOptions.DontRequireReceiver);
	}



}
