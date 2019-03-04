using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UniqUnitAI {

	public RaceInfo.unitType myUnitType;
	public int priority; // the higher the priority the more likely the AI will use APM on this AI 0-100

	public enum AIPhase { EnterCombat, ExitCombat, inCombat, OutOfCombat }

	public abstract void apply(UnitManager man);


	public static bool applyModule(UnitManager man, Dictionary<string, UniqUnitAI> mapper)
	{
		if (!mapper.ContainsKey(man.UnitName))
		{
			switch (man.UnitName)
			{
				case "Chimera":
					mapper.Add(man.UnitName, new ChimeraAI());
					break;

				case "Vulcan":
					mapper.Add(man.UnitName, new VulcanAI());
					break;

				case "Triton":
					mapper.Add(man.UnitName, new TritonAI());
					break; 

				// ADD MORE UNIQUE UNIT AIs HERE
				default:
					return false;
			}
		}

		mapper[man.UnitName].apply(man);
		return true;
	}

	public abstract int UseAPM(UnitManager manager, AIPhase phase); // return 0 if nothing was used, return -1 if an ability was used that is shared by all like units,
													 //return above 0 for all other actions done by this unit







	public static Vector3 getRandomTargetZone(Vector3 target, float radius)
	{
	
		float radiusA = Random.Range(0, radius);
		float angle = Random.Range(0, 360);

		target.x += Mathf.Sin(Mathf.Deg2Rad * angle) * radiusA;
		target.z += Mathf.Cos(Mathf.Deg2Rad * angle) * radiusA;
		return target;
	}
}
