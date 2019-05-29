using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildUnitObjective : Objective  {

	public List<GameObject> unitsToBuild = new List<GameObject> ();
	public bool anyCombo;

	private int total = 0;
	public bool startCountingWhenTriggered = true;
	new void Start()
	{
		base.Start ();
		GameManager.main.playerList [0].addBuildUnitObjective (this);
	}



	public void buildUnit(UnitManager obj)
	{
		if (startCountingWhenTriggered && !started) {
			return;
		}

		for (int i = 0; i < unitsToBuild.Count; i++) {

		
			if (unitsToBuild [i].GetComponent<UnitManager> ().UnitName == obj.UnitName) {


				if (anyCombo) {
					total++;
					if (total == unitsToBuild.Count) {
						complete ();
					}
				} else {
					unitsToBuild.RemoveAt (i);
				
					if (unitsToBuild.Count == 0) {
					
						complete ();

					}
				}
				return;
			
			}
		}
	}
}
