using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRevealer : Upgrade {

	bool applied;

	public Sprite upgradeIcon;
	override
	public void applyUpgrade (GameObject obj){

		if (applied) {
			return;
		}
		applied = true;
		foreach (UpgradeCenterTag tag in GameObject.FindObjectsOfType<UpgradeCenterTag>()) {
			FogOfWar.current.Unfog (tag.gameObject.transform.position,6);
			MiniMapUIController.main.showWarning (tag.gameObject.transform.position, upgradeIcon);
		}
	



	}

	public override void unApplyUpgrade (GameObject obj){

	}


}
