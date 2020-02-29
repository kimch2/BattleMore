using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalvageTarget : IEffect {

	// Designed to be used with Ore and Waste
	public override void applyTo (GameObject source, GameObject target){

		UnitManager manage = target.GetComponent<UnitManager>();
		if (manage.myStats.Cost.MyResources.Count == 0)
		{
			return;
		}

		float amountA = manage.myStats.Cost.MyResources[0].currentAmount *.5f;
		float amountB = amountA;
		if (manage.myStats.Cost.MyResources.Count > 1)
		{
			amountB += manage.myStats.Cost.MyResources[1].currentAmount ;

		}


		PopUpMaker.CreateGlobalPopUp ("+ " + (int)(amountA ), Color.white,this.transform.position);
		PopUpMaker.CreateGlobalPopUp ("+ " + (int)(amountA ), Color.white,target.transform.position);

		PopUpMaker.CreateGlobalPopUp("+ " + (int)(amountB), Color.green, this.transform.position + Vector3.down *3);
		PopUpMaker.CreateGlobalPopUp("+ " + (int)(amountB ), Color.green, target.transform.position + Vector3.down * 3);

		target.GetComponent<UnitManager> ().myStats.kill (source);
		GameManager.main.playerList[0].collectOneResource(new ResourceTank(ResourceType.Ore, (int)amountA), false );
		GameManager.main.playerList[0].collectOneResource(new ResourceTank(ResourceType.Waste, (int)amountB), false);
	}
	 
	public override bool canCast() { return true; }

	public override bool validTarget(GameObject target) {
		return true;
	}



}
