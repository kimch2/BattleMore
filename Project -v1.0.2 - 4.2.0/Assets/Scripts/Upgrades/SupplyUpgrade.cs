using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyUpgrade  : Upgrade {

	bool applied;
	public int StartingSupplyBonus;
	public int MaxSupplyBuff;

	override
	public void applyUpgrade (GameObject obj){

		if (applied) {
			return;
		}

		applied = true;

		GameManager.main.activePlayer.supplyCap += MaxSupplyBuff;
		GameManager.main.activePlayer.supplyMax += StartingSupplyBonus;
		GameManager.main.activePlayer.UnitDied (0, null);

	}

	public override void unApplyUpgrade (GameObject obj){

	}


}
