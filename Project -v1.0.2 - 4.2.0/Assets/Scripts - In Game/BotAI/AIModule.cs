using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIModule {

	protected List<UnitManager> AllUnits;// under my module's pervayance
	protected Dictionary<string, List<UnitManager>> UnitRoster;



	public virtual void setUnits(List<UnitManager> toSet)
	{
		UnitRoster = new Dictionary<string, List<UnitManager>>();

		foreach (UnitManager obj in toSet)
		{
			addUnit(obj);
		}
	}

	public void addUnits(List<UnitManager> toAdd)
	{
		foreach (UnitManager obj in toAdd)
		{
			addUnit(obj);
		}
	}


	protected void addUnit(UnitManager obj)
	{
		AllUnits.Add(obj);
		if (!UnitRoster.ContainsKey(obj.UnitName))
		{
			UnitRoster.Add(obj.UnitName, new List<UnitManager>() { obj });
		}
		else
		{
			UnitRoster[obj.UnitName].Add(obj);
		}
	}

	public abstract int UseAPM(UniqUnitAI.AIPhase phase, int maxAPM);
}
