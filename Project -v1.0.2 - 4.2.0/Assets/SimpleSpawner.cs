using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpawner : MonoBehaviour {

	public float firstSpawn = 20;
	public float spawnRate;
	public bool increasingSpawnRate;
	public List<GameObject> enemyTypes;
	DifficultyManager difficultyM;
	Vector3 attackPoint;


	// Use this for initialization
	void Start () {
		difficultyM = GameObject.FindObjectOfType<DifficultyManager> ();
		if (enemyTypes.Count > 0) {
			Invoke ("SpawnEnemy", firstSpawn);
		}

		spawnRate -= ((LevelData.getDifficulty() -1) * 2);
		attackPoint = GameObject.FindObjectOfType<sPoint> ().transform.position;
	}

	public void setSpawnRate(float time)
	{
		spawnRate = time;
	}

	void SpawnEnemy()
	{

        if (!gameObject.activeSelf)
        {
            return;
        }
		
		GameObject unit = (GameObject)Instantiate (enemyTypes [Random.Range (0, enemyTypes.Count)], this.transform.position, Quaternion.identity);
        UnitManager unitMan = unit.GetComponent<UnitManager>();
       unitMan.PlayerOwner = 2;
        unitMan.setInteractor();
        unitMan.interactor.initializeInteractor();

        difficultyM.SetUnitStats (unit);
		unitMan.GiveOrder (Orders.CreateAttackMove (attackPoint));
		if (increasingSpawnRate && spawnRate > .8f) {
			spawnRate -= 1;
			if (spawnRate < .8f) {
				spawnRate = .8f;
			}
		}
		Invoke ("SpawnEnemy", Mathf.Max(1, spawnRate + Random.Range(-10,10)));
	}

}
