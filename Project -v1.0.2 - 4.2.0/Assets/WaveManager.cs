using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {


	//int GeneralIndex = 0; // Hadrian = 0, Rucks = 1, Katrina =2, Carbot = 3 



	public WaveContainer.EnemyWave FirstPlayWaveType;
	public List<WaveContainer.EnemyWave> ReplayWaves;

	public List<GameObject> spawnLocations;
	public GameObject EmergencySpawnLocation;

	List<WaveSpawner.attackWave> CurrentWaves;
	public List<waveSetting> myWaves;
	public Vector3 firstRallyPoint;
	public List<Vector3> alternateRallyPoints;

	public bool giveWarnings = true;
	int currentWaveIndex;
	private float nextActionTime = 10000;
	int SpawnerCount;

	DifficultyManager difficultyM;
	RaceManager raceMan;
	float mystartTime;
	public bool SearchForEnemies = true;
	WaveContainer container = null;
	WaveContainer.WaveOption waveOption;

	[System.Serializable]
	public class waveSetting
	{
		public float waveSpawnTime;
		public GameObject SpawnObject;
		[Tooltip("usually 0-4, how late of a wave type should it be.")]
		public int WaveAdvancement;
		[Tooltip("If it must be here and it gets destroyed, it wont spawn")]
		public bool MustBeHere;
		public List<CustomWarning> CustomWarnings = new List< CustomWarning>();
		public List<SceneEventTrigger> CustomTriggers = new List<SceneEventTrigger> ();
		[Tooltip("-1 means its set to the regular point, phase these out as much as possible.")]
		public int RallyPointIndex = -1;
	}

	[System.Serializable]
	public class CustomWarning
	{
		[TextArea(2,10)]
		public List<attackWarning> warningVariants;

	}


	[System.Serializable]
	public class attackWarning
	{
		//[TextArea(2,10)]
		public string textWarning;
		public AudioClip audioWarning;
		public Sprite myPic;
	}

	public WaveContainer.WaveOption getCurrentWaveType()
	{
		return waveOption;
	}


	public void ReleaseWave(int waveDifficulty)
	{
		GameObject spawner = EmergencySpawnLocation;
		List<UnitManager> myUnits = new List<UnitManager>();
		//float delay = .1f;
		foreach (GameObject obj in CurrentWaves[0].waveType) {
			myUnits.Add( createUnit(obj, spawner));
		//	StartCoroutine (MyCoroutine (delay, obj, spawner, myWaves [currentWaveIndex].RallyPointIndex == -1 ? firstRallyPoint : alternateRallyPoints[myWaves [currentWaveIndex].RallyPointIndex]));
			//delay += .2f;

		}
	}

	void Start () {



		mystartTime = Time.timeSinceLevelLoad;
		//GeneralIndex = PlayerPrefs.GetInt ("VoicePack", 0);
		currentWaveIndex = 0;
		SpawnerCount = spawnLocations.Count;
		container = ((GameObject)(Resources.Load ("WaveContainer"))).GetComponent<WaveContainer> ();
	

		if ( PlayerPrefs.GetInt ("L" + VictoryTrigger.instance.levelNumber+"Win",0) > 0 && RaceSwapper.main == null) {

			if (ReplayWaves.Count > 0) {
				//container = ((GameObject)(Resources.Load ("WaveContainer"))).GetComponent<WaveContainer> ();
				waveOption = container.getWave (ReplayWaves [UnityEngine.Random.Range (0, ReplayWaves.Count)]);
				CurrentWaves = waveOption.waveRampUp;
			}


		} else {

			waveOption = container.getWave (FirstPlayWaveType);
            if (RaceSwapper.main != null)
            {
                waveOption = container.getWave(getSwappedRace());
            }
            CurrentWaves = container.getWave (FirstPlayWaveType).waveRampUp;

            if (RaceSwapper.main != null)
            {
                CurrentWaves = container.getWave(getSwappedRace()).waveRampUp;
            }
        }


		raceMan = GameObject.FindObjectOfType<GameManager> ().activePlayer;
		difficultyM = GameObject.FindObjectOfType<DifficultyManager> ();
	
		nextActionTime = myWaves [currentWaveIndex].waveSpawnTime;
	}

	void OnDisable()
	{
		CancelInvoke ();
	}


    WaveContainer.EnemyWave getSwappedRace()
    {
        if (RaceSwapper.main.getEnemyRace() == RaceInfo.raceType.SteelCrest)
        {
            return WaveContainer.EnemyWave.EnemySteelCrest;
        }
        if (RaceSwapper.main.getEnemyRace() == RaceInfo.raceType.Coalition)
        {
            if (Random.Range(0, 2) == 0)
            {
                return WaveContainer.EnemyWave.ScrapCrack;
            }
            return WaveContainer.EnemyWave.ScrapSkif;
        }
        return FirstPlayWaveType;
    }

	// Update is called once per frame
	void Update () {
		
		if (Time.timeSinceLevelLoad - mystartTime > nextActionTime) {

			if (spawnLocations.Count < SpawnerCount) {
				spawnLocations.RemoveAll (item => item == null);
				SpawnerCount = spawnLocations.Count;

			}
			GameObject spawner = null;


			if (currentWaveIndex >= myWaves.Count) {

				return;
			}
			if (myWaves [currentWaveIndex].MustBeHere && !myWaves [currentWaveIndex].SpawnObject) {
				setNextWave ();

				return;
			} else if ( myWaves [currentWaveIndex].SpawnObject) {
				spawner = myWaves [currentWaveIndex].SpawnObject;

			} else if (!myWaves [currentWaveIndex].MustBeHere && !myWaves [currentWaveIndex].SpawnObject && spawnLocations.Count > 0) {
				spawner = spawnLocations [Random.Range (0, spawnLocations.Count )];
			} 

			if (!spawner) {
				spawner = EmergencySpawnLocation;
			}



			if (myWaves [currentWaveIndex].CustomTriggers.Count > 0 ||
			    myWaves [currentWaveIndex].CustomWarnings.Count > 0) {
				//Display Custom Warning
				//int n = UnityEngine.Random.Range (0, nextWave.warnings.Count - 1);
				//ExpositionDisplayer.instance.displayText (nextWave.warnings [n].textWarning, 7, nextWave.warnings [n].audioWarning, .93f, nextWave.warnings[n].myPic,4);

			} else {
				if (giveWarnings) {
					ErrorPrompt.instance.EnemyWave (waveOption.warningType);
				}
			}

			if (CurrentWaves == null) {
				if (ReplayWaves.Count > 0) {
					waveOption = container.getWave (ReplayWaves [UnityEngine.Random.Range (0, ReplayWaves.Count)]);
				} else
					waveOption = container.getWave (FirstPlayWaveType);
				CurrentWaves = waveOption.waveRampUp;
			}

			foreach (MiniMapUIController mini in GameManager.main.MiniMaps) {
				if (mini) {
					mini.showWarning (spawner.transform.position,waveOption.WaveSymbol);
				}
			}

			List<UnitManager> AttackingUnits = new List<UnitManager>();

			Debug.Log ("Spawning wave " + this.gameObject + "  " + currentWaveIndex);

			Vector3 targetPoint = myWaves[currentWaveIndex].RallyPointIndex == -1 ? firstRallyPoint : alternateRallyPoints[myWaves[currentWaveIndex].RallyPointIndex];
			myWaves [currentWaveIndex].WaveAdvancement = Mathf.Clamp (myWaves [currentWaveIndex].WaveAdvancement, 0, CurrentWaves.Count - 1);

			List<UnitManager> myUnits = new List<UnitManager>();
			foreach (GameObject obj in CurrentWaves[myWaves[currentWaveIndex].WaveAdvancement].waveType) {
				myUnits.Add(createUnit(obj, spawner));
				//StartCoroutine (MyCoroutine (delay, obj, spawner, myWaves [currentWaveIndex].RallyPointIndex == -1 ? firstRallyPoint : alternateRallyPoints[myWaves [currentWaveIndex].RallyPointIndex]));
				//delay += .12f;

			}



			if (LevelData.getDifficulty () >= 2) {
				SpawnExtra (CurrentWaves [myWaves [currentWaveIndex].WaveAdvancement], spawner , myWaves [currentWaveIndex].RallyPointIndex == -1 ? firstRallyPoint : alternateRallyPoints[myWaves [currentWaveIndex].RallyPointIndex], myUnits);
				foreach (GameObject obj in CurrentWaves[myWaves[currentWaveIndex].WaveAdvancement].mediumExtra) {
					myUnits.Add(createUnit(obj, spawner));
					
					//StartCoroutine(MyCoroutine (delay, obj, spawner, myWaves [currentWaveIndex].RallyPointIndex == -1 ? firstRallyPoint : alternateRallyPoints[myWaves [currentWaveIndex].RallyPointIndex]));
					//delay += .12f;

				}
			}
			if (LevelData.getDifficulty () >= 3) {

				foreach (GameObject obj in CurrentWaves[myWaves[currentWaveIndex].WaveAdvancement].HardExtra) {

					myUnits.Add(createUnit(obj, spawner));

					//StartCoroutine(MyCoroutine (delay, obj, spawner,  myWaves [currentWaveIndex].RallyPointIndex == -1 ? firstRallyPoint : alternateRallyPoints[myWaves [currentWaveIndex].RallyPointIndex]));
					//delay += .12f;
				}
			}

			GameObject AIGO = new GameObject("Combat AI");
            AIGO.transform.position = myUnits[0].transform.position;
            CombatAI AI =  AIGO.AddComponent<CombatAI>();
           
			AI.addUnits(myUnits, SearchForEnemies);
			AI.setAttackMovePoint(targetPoint);

			setNextWave ();
		
		}

	}


	void setNextWave()
	{
		if (currentWaveIndex < myWaves.Count - 1) {
			currentWaveIndex++;
			nextActionTime = myWaves [currentWaveIndex].waveSpawnTime;
		//	Debug.Log ("Launchign"  + currentWaveIndex + "   " + nextActionTime + "    " + CurrentWaves.Count ) ;
		}
		else{
			nextActionTime += 180;
			if (LevelData.getDifficulty () == 1) {
				nextActionTime += 180;
			}
		} 

	}


	//autobalancing based on how many units the player has
	void SpawnExtra(WaveSpawner.attackWave myWave, GameObject Spawner, Vector3 spawnSpot, List<UnitManager> myList)
	{			
		if (raceMan.getArmyCount () * .50 >  myWave.waveType.Count + myWave.HardExtra.Count + myWave.mediumExtra.Count) {
			foreach (GameObject obj in myWave.HardExtra) {

				myList.Add(createUnit(obj, Spawner));

			}
		}
	}


	UnitManager createUnit(GameObject obj, GameObject spawnObject)
	{

		Vector3 hitzone = spawnObject.transform.position;

		float radius = Random.Range(12, 25);
		float angle = Random.Range(0, 360);

		hitzone.x += Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
		hitzone.z += Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
		hitzone.y -= 5;

		if (obj.GetComponent<airmover>())
		{
			hitzone.y += obj.GetComponent<airmover>().flyerHeight + 5;
		}

		GameObject unit = (GameObject)Instantiate(obj, hitzone, Quaternion.identity);

		FogOfWarUnit fogger = unit.GetComponent<FogOfWarUnit>();
		if (fogger)
		{ DestroyImmediate(fogger); }

		if (unit.GetComponent<airmover>())
		{
			unit.transform.position = unit.transform.position + Vector3.up * 10;
		}

	
		difficultyM.SetUnitStats(unit);

		UnitManager manage = unit.GetComponent<UnitManager>();
		manage.PlayerOwner = 2;
		manage.startingCommand.Clear();
		foreach (UnitManager man in unit.GetComponentsInChildren<UnitManager>())
		{
			man.PlayerOwner = 2;
		}

		return manage;
	}

	


	public void SetRallyIndexes(int n)
	{
		foreach (waveSetting setting in myWaves) {
			setting.RallyPointIndex = n;
		}
	}




	public void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		foreach (GameObject obj in spawnLocations) {
			if (obj) {

				Gizmos.DrawLine (obj.transform.position, firstRallyPoint);
			}
		}

		foreach (Vector3 alter in alternateRallyPoints) {
			Gizmos.DrawCube (alter, Vector3.one * 10);
		}
		if (EmergencySpawnLocation) {
			Gizmos.color = Color.red;
			Gizmos.DrawLine (EmergencySpawnLocation.transform.position, firstRallyPoint);
		}
	}

}
