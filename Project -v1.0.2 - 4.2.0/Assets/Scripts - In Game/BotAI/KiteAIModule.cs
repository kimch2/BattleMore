using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteAIModule : AIModule, Notify  {

	public CombatAI myCombater;
	List<string> runningAway = new List<string>();

	public KiteAIModule(CombatAI com)
	{
		myCombater = com;
	}



	public override int UseAPM(UniqUnitAI.AIPhase phase, int maxAPM)
	{
		return 0;
	}


	public override void setUnits(List<UnitManager> toSet)
	{
		AllUnits = new List<UnitManager>();
		UnitRoster = new Dictionary<string, List<UnitManager>>();

		foreach (UnitManager obj in toSet)
		{
			foreach (IWeapon weap in obj.myWeapon)
			{
				if (weap.range > 30)
				{
					//weap.addNotifyTrigger(this);
					addUnit(obj);

					break;
				}
			}
		}
	}

	public float trigger(GameObject source, GameObject projectile, UnitManager target, float damage) {

		UnitManager sourceMan = source.GetComponent<UnitManager>();
		if (runningAway.Contains(sourceMan.UnitName) || target.myWeapon.Count == 0)
		{
			return damage;
		}

		foreach (IWeapon weap in target.myWeapon)
		{
			if (weap.range + 10 > sourceMan.myWeapon[0].range)
			{
				return damage;
			}
		}

		Vector3 retreatLocation = target.transform.position + (source.transform.position - target.transform.position).normalized * sourceMan.myWeapon[0].range;
		myCombater.StartCoroutine(RunAway(UnitRoster[sourceMan.UnitName], retreatLocation, sourceMan.myWeapon[0].attackPeriod ));

		return damage;
	}

	IEnumerator RunAway(List<UnitManager> units, Vector3 location, float runTime)
	{
		runningAway.Add(units[0].UnitName);
		yield return new WaitForSeconds(.3f);
		units.RemoveAll(item => item == null);
		Formations.assignMoveCOmmand(units, location, location, false, 1);
		yield return new WaitForSeconds(runTime);
		units.RemoveAll(item => item == null);
		Formations.assignMoveCOmmand(units, myCombater.AttackMovePoint,  myCombater.AttackMovePoint, true, 1);
		runningAway.Remove(units[0].UnitName);
	}
}
