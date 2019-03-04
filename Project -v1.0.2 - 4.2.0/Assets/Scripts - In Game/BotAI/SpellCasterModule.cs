using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasterModule : AIModule
{

	public CombatAI myCombater;


	public SpellCasterModule(CombatAI com)
	{
		myCombater = com;
	}

	public override void setUnits(List<UnitManager> toSet)
	{
		AllUnits = new List<UnitManager>();
		UnitRoster = new Dictionary<string, List<UnitManager>>();

		foreach (UnitManager obj in toSet)
		{
			if (myCombater.UniqueUnitAIs.ContainsKey(obj.UnitName))
			{
				addUnit(obj);
			}
		}
	}

	public override int UseAPM(UniqUnitAI.AIPhase phase, int maxAPM)
	{
		int i = 0;
		foreach (UnitManager man in AllUnits)
		{
			i += myCombater.UniqueUnitAIs[man.UnitName].UseAPM(man, phase);
			if (i >= maxAPM)
			{
				break;
			}
		}

		return i;
	}
}
