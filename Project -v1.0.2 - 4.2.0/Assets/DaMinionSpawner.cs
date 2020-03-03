﻿using System.Collections;
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

    private void Start()
    {
        Invoke("SpawnEnemy", firstSpawn);
    }

    void SpawnEnemy()
    {
        Debug.Log("Hi there");
        Vector3 spawnPoint = DaminionsInitializer.main.MyHero.transform.position + Vector3.right * SpawnOffset;
        spawnPoint.z += Random.Range(-1*ZRange, ZRange);

        GameObject unit = (GameObject)Instantiate(TypesToSpawn[Random.Range(0, TypesToSpawn.Count)], spawnPoint, Quaternion.identity);
        UnitManager unitMan = unit.GetComponent<UnitManager>();
        unitMan.Initialize(2, true, false);

        unitMan.GiveOrder(Orders.CreateAttackMove(spawnPoint + Vector3.left*SpawnOffset));

        Invoke("SpawnEnemy", Mathf.Max(1, spawnRate + Random.Range(-spawnRate/3, spawnRate/3)));
    }

}
