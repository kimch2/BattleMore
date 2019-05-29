using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarbotAI : MonoBehaviour
{

	public RaceManager myRacer;
	public UnitManager ProductionBuilding;

	public List<UnitManager> availableUnits = new List<UnitManager>();
	List<UnitManager> usedUnits = new List<UnitManager>();
	List<UnitManager> Buildings = new List<UnitManager>();

	BuildManager builder;
	public List<int> unitUnLocks = new List<int>();
	public List<int> ultUnLocks = new List<int>() { 0,1,2,3};
	public List<int> upgradesUnLocks = new List<int>() { 0, 1, 2, 3,4,5,6,7,8,9,10,11,12,13,14 };
	public List<int> usableUnits = new List<int>() { 0};

	private void Start()
	{
		builder = ProductionBuilding.GetComponent<BuildManager>();
		for (int i = 1; i < ProductionBuilding.abilityList.Count; i++)
		{
			unitUnLocks.Add(i);
		}
		 InvokeRepeating("spawnUnits", 3,1);
	}

	List<CombatAI> currentCombatters = new List<CombatAI>();


	public void addNewUnit(UnitManager manage)
	{
		if (manage.myStats.isUnitType(UnitTypes.UnitTypeTag.Structure))
		{
			Buildings.Add(manage);
		}
		else { 
			availableUnits.Add(manage);
			if (availableUnits.Count > 3  && CarbotObjectiveManager.main.activeObjectives.Count > 0)//|| myRacer.currentSupply > myRacer.supplyMax - 4
			{
				sendAvailableUnits();
			}
		}
	}


	void spawnUnits()
	{
		if(builder.buildOrder.Count < 1) { 
			ProductionBuilding.abilityList[usableUnits[Random.Range(0, usableUnits.Count)] ].Activate();
		}
	}

	void sendAvailableUnits()
	{
		GameObject AIGO = new GameObject("Combat AI");
		CombatAI AI = AIGO.AddComponent<CombatAI>();
		AI.addUnits(availableUnits, false);
		AI.setAttackMovePoint(CarbotObjectiveManager.main.activeObjectives[Random.Range(0, CarbotObjectiveManager.main.activeObjectives.Count)].point.transform.position);
		availableUnits = new List<UnitManager>();
		AI.addModule(new KiteAIModule(AI));

	}

	void OrganizeUnits()
	{

	}

	
	
	public void checkForUpgrades()
	{
		if (upgradesUnLocks.Count > 0)
		{
			int i = upgradesUnLocks[Random.Range(0, upgradesUnLocks.Count)];
			CarbotOverlord.main.PlayerList[1].purchaseUpgrade(i);
			upgradesUnLocks.Remove(i);
		}
	}

	

	public void checkNewUnits()
	{
		if (unitUnLocks.Count > 0)
		{
			int i = unitUnLocks[Random.Range(0, unitUnLocks.Count)];
			CarbotOverlord.main.PlayerList[1].purchaseUnit(i);
			unitUnLocks.Remove(i);
			usableUnits.Add(i);
		}
	}


	public void checkNewUlt()
	{
		if (ultUnLocks.Count > 0)
		{
			int i = ultUnLocks[Random.Range(0, ultUnLocks.Count)];
			CarbotOverlord.main.PlayerList[1].purchaseUlt(i);
			ultUnLocks.Remove(i);
		}
	}





}
