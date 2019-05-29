using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class CombatAI : MonoBehaviour , ITask
{

	List<AIModule> myModules = new List<AIModule>();
	public List<UnitManager> myUnits;

	public Vector3 AttackMovePoint;
	Seeker mySeeker;
	bool hasGroundUnits = false;

	public Dictionary<string ,UniqUnitAI> UniqueUnitAIs = new Dictionary<string, UniqUnitAI>();

	public bool hasReachedEnd;

	public UniqUnitAI.AIPhase currentPhase = UniqUnitAI.AIPhase.OutOfCombat;
	


	private void Awake()
	{
		mySeeker = gameObject.AddComponent<Seeker>();
		//myModules.Add(new KiteAIModule(this));
		myModules.Add(new SpellCasterModule(this));
		
		InvokeRepeating("update",10, .5f);
		
	}

	public void addModule(AIModule mod)
	{
		myModules.Add(mod);
	}

	private void update()
	{ 
		CheckIfAlive();

		switch (currentPhase)
		{
			case UniqUnitAI.AIPhase.OutOfCombat:
				foreach (UnitManager man in myUnits)
				{
					if (man && man.enemies.Count > 0)
					{
						// ENTER COMBAT APM
						currentPhase = UniqUnitAI.AIPhase.inCombat;
						break;
					}
				}

				break;

			case UniqUnitAI.AIPhase.inCombat:
				bool enemiesThere= false;
				foreach (UnitManager man in myUnits)
				{
					if (man && man.enemies.Count > 0)
					{
						enemiesThere = true;
						break;
					}
				}

				if (!enemiesThere)
				{
					currentPhase = UniqUnitAI.AIPhase.OutOfCombat;
					// Exit Combat APM
				}
				else
				{
					// IN COMBAT APM
					
					foreach (AIModule mod in myModules)
					{
						mod.UseAPM(UniqUnitAI.AIPhase.inCombat, 20);
					}
				}

				break;
		}
	}


	public float GetPriority() {
		return 1;

	}
	public int Execute() {
		return 1;
	}
	



	void CheckIfAlive()
	{
		myUnits.RemoveAll(item => item == null);
		if (myUnits.Count == 0)
		{
			Destroy(this);// OR maybe destroy gameObject?
		}
	}

	public void addUnits(List<UnitManager> units, bool EnemySearch)
	{
		myUnits = units;
		foreach (UnitManager man in units)
		{
			if (!man)
			{
				continue;
			}
			UniqUnitAI.applyModule(man, UniqueUnitAIs);
			if (EnemySearch)
			{
				EnemySearchAI searcher = man.gameObject.AddComponent<EnemySearchAI>();
				searcher.combatAI = this;
			}
			if (man.cMover && !(man.cMover is airmover))
			{
				hasGroundUnits = true;
			}
		}
		foreach (AIModule mod in myModules)
		{
			mod.setUnits(myUnits);
		}
	}


	public void setAttackMovePoint(Vector3 point)
	{
		myUnits.RemoveAll(item => item == null);
		AttackMovePoint = point;
		if (hasGroundUnits)
		{
			mySeeker.StartPath(myUnits[0].transform.position, AttackMovePoint, OnPathComplete);
		}
		else
		{
			HalfPoint = Vector3.Lerp(myUnits[0].transform.position, AttackMovePoint, .7f);
			Formations.assignMoveCOmmand(myUnits, HalfPoint, HalfPoint, true, 0);
			float time = Vector3.Distance(HalfPoint, myUnits[0].transform.position) / 30;
			Invoke("FinishAttackMove", time);
		}
	}


	// Rally before reaching enemy base, then attack as one.

	public Vector3 HalfPoint;

	public void OnPathComplete(Path p)
	{
		Vector3[] RallyPoints = getRallyPoint(p, 240);
        HalfPoint = RallyPoints[0];
		Formations.assignMoveCOmmand(myUnits, RallyPoints[0], RallyPoints[1], true, 0);

		float time = Vector3.Distance(HalfPoint, myUnits[0].transform.position) / 30;
		Invoke("FinishAttackMove", time);
	}

	void FinishAttackMove()
	{ // or maybe check if they are in the default
		CheckIfAlive();

        bool FinishAttack = false;
        bool AllInRange = true;
		foreach (UnitManager man in myUnits)
		{
			if (man)
			{

                if (man.enemies.Count > 0)
                {
                    FinishAttack = true;
                    break;
                }
                if(Vector3.Distance(man.transform.position, HalfPoint) > 70 || !(man.getState() is DefaultState))
                {
                    AllInRange = false;
                }
			}
		}
        if (FinishAttack || AllInRange)
        {
            // Give the final attack command
        }
        else
        {
            Invoke("FinishAttackMove", 5);
            return;
        }

        Debug.Log("Finsihing ");
		Formations.assignMoveCOmmand(myUnits, AttackMovePoint, AttackMovePoint, true, 0);
		hasReachedEnd = true;
		foreach (UnitManager man in myUnits)
		{
			if (man)
			{
				EnemySearchAI ai = man.gameObject.GetComponent<EnemySearchAI>();
				if (ai)
				{
					ai.combatAI = null;
					ai.Start();
				}
			}
		}
	}



	Vector3[] getRallyPoint(Path p, float distanceFromTarget)
	{
		float totalDistance = 0;
		int lastInt = 0;
		for (int i = p.vectorPath.Count - 1; i > 1; i--)
		{
			totalDistance += Vector3.Distance(p.vectorPath[i], p.vectorPath[i - 1]);
			if (totalDistance > distanceFromTarget)
			{
				lastInt = i - 1;
				break;
			}
		}
		if (lastInt < 0)
		{
			lastInt = 0;
		}

		Vector3 Offset = Quaternion.Euler(0, 90, 0) * (p.vectorPath[lastInt] - p.vectorPath[lastInt +1 ]).normalized * 16;

		Vector3 totalAverage = Vector3.zero;
		int totalCount = 1;

		for (int a = -2; a < 3; a++)
		{

			if (Grid.main.isPathable(p.vectorPath[lastInt] + Offset * a))
			{
				totalAverage += (Offset * a);
				totalCount++;
			}
			else
			{
				if (a == -1)
				{
					totalAverage = Vector3.zero;
				}
				else if (a == 1)
				{
					break;
				}
			}
		}

		totalAverage /= totalCount;
		HalfPoint = totalAverage + p.vectorPath[lastInt];

		return new Vector3[] { HalfPoint + Offset / 2, HalfPoint - Offset / 2 };
	}
}
