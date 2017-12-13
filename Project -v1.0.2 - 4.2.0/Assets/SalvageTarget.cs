using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalvageTarget :SingleTarget, IEffect {

	void Start()
	{
		base.Start ();
		myEffect = this;
	}

	public void apply (GameObject source, GameObject target){

		float amount = target.GetComponent<UnitManager> ().myStats.cost;
		PopUpMaker.CreateGlobalPopUp ("+ " + (int)(amount *.75f), Color.white,this.transform.position);
		PopUpMaker.CreateGlobalPopUp ("+ " + (int)(amount *.75f), Color.white,target.transform.position);

		target.GetComponent<UnitManager> ().myStats.kill (source);

	}


}
