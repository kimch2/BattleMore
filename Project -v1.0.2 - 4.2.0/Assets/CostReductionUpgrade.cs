using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostReductionUpgrade  : Upgrade {

	[Tooltip(".25 will reduce the cost by 25%")]
	public float costReduction;
	[Tooltip(".25 will reduce the research time by 25%")]
	public float timeReduction;

	override
	public void applyUpgrade (GameObject obj){

		UnitManager man = obj.GetComponent<UnitManager> ();


		foreach (Ability ab in man.abilityList) {
			if (ab is ResearchUpgrade) {

				ab.myCost.resourceCosts.reduceCostPercentage(costReduction);


					((ResearchUpgrade)ab).buildTime -= ((ResearchUpgrade)ab).buildTime * timeReduction;
					foreach (Upgrade up in ((ResearchUpgrade)ab).upgrades) {
					up.buildTime -= costReduction*up.buildTime;
						if (up.myCost) {
						up.myCost.resourceCosts.reduceCostPercentage(costReduction);

					
						} else {
							Debug.Log (up.Name + " doesn't have a cost");
						}

					}

			}
		}




	}

	public override void unApplyUpgrade (GameObject obj){

	}


}
