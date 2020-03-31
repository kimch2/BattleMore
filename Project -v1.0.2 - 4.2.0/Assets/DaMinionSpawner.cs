using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaMinionSpawner : MonoBehaviour
{


    public List<GameObject> TypesToSpawn;
    public float firstSpawn = 20;
    public float spawnRate = 5;

    [Tooltip("Distance from the hero to spawn units")]
    
    public float SpawnOffset = 100;
    public float ZRange = 40;

    [Tooltip("This is based on where the progress on the level is (the bar at the top)")]
    public AnimationCurve spawnCurve;

    private void Start()
    {
        Invoke("SpawnEnemy", firstSpawn);
    }

    void SpawnEnemy()
    {
        if (DaminionsInitializer.main.MyHero)
        {
            Vector3 spawnPoint = DaminionsInitializer.main.getScreenMiddle(20, 2, true);
            spawnPoint.z += Random.Range(-1 * ZRange, ZRange);

            GameObject unit = (GameObject)Instantiate(TypesToSpawn[Random.Range(0, TypesToSpawn.Count)], spawnPoint, Quaternion.identity);
            UnitManager unitMan = unit.GetComponent<UnitManager>();
            unitMan.Initialize(2, true, false);

            unitMan.GiveOrder(Orders.CreateAttackMove(spawnPoint + Vector3.left * SpawnOffset));

            Invoke("SpawnEnemy", Mathf.Max(1, (1 - spawnCurve.Evaluate(CarbotCamera.singleton.getProgress())) * spawnRate + Random.Range(-spawnRate / 3, spawnRate / 3)));
        }
    }

    int NextUnitIndex = 0;
    private void Update()
    {
        if ( NextUnitIndex < DaminionsInitializer.main.map.Units.Count)
        {
            UnitData nextUnit = DaminionsInitializer.main.map.Units[NextUnitIndex];
            Vector3 Position = nextUnit.pos;

            if (Position.x < DaminionsInitializer.main.getScreenEdge(Position, 0, 2, true).x +5)
            {
                GameObject unit = (GameObject)Instantiate(DaminionsInitializer.main.AllUnits.Find(item => item.UnitName == nextUnit.unitName).gameObject, Position, Quaternion.identity);
                UnitManager unitMan = unit.GetComponent<UnitManager>();
                unitMan.Initialize(2, true, false);
                NextUnitIndex++;
            }
        }
    }
}
