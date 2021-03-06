﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class VictoryTrigger : MonoBehaviour {

	public Canvas VictoryScreen;
	public Canvas DefeatScreen;


	public List<Objective> mainObjective = new List<Objective> ();
	public List<Objective> bonusObjective = new List<Objective> ();

	public int techRewards;
	private int totalBonusObj;
	private int completeBonusObj;

	public int levelNumber;
	//public int EnemiesDest;
	//public int ResourceAmount;
	//public string time;
	public int TechCredits;

	//public AudioClip victoryLine;
	//public AudioClip DefeatLine;

	[HideInInspector]
	public List<int> winLine;
	[HideInInspector]
	public List<int> loseLine;
	bool hasFinished;

	public static VictoryTrigger instance;
	// Use this for initialization
	void Awake () {
		instance = this;
		PlayerPrefs.SetInt ("RecentLevel", levelNumber);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Minus)) {
			if (Input.GetKey (KeyCode.Backslash)) {
				if (Input.GetKey (KeyCode.BackQuote)) {
					Win ();
				}
			}
		}
	}


	public void addObjective(Objective obj)
	{
		if (obj.bonus) {
			totalBonusObj++;
			bonusObjective.Add (obj);

			ObjectiveManager.instance.setBonusObjectives (obj);
		
		} 
		else {
			mainObjective.Add (obj);

			ObjectiveManager.instance.setObjective (obj);
		}
	
	}

	public void CompleteObject(Objective obj)
	{	TechCredits += obj.reward;

		if (!ObjectiveManager.instance.hasObjective (obj)) {
			addObjective (obj);
		}

		if (obj.bonus) {
			Debug.Log ("Completed bonus");
			completeBonusObj++;
			ObjectiveManager.instance.completeBonus (obj);

		} else {

			if (obj.UltimateObjective) {
				Win ();
				return;
			}
			ObjectiveManager.instance.completeMain (obj);

		
		}
	}

	public void unComplete(Objective obj)
	{TechCredits -= obj.reward;
		if (obj.bonus) {
			Debug.Log ("Completed bonus");
			completeBonusObj--;
			ObjectiveManager.instance.unCompleteBonus (obj);

		} else {
			ObjectiveManager.instance.UnCompleteMain(obj);
		}
	}


	public void FailObjective(Objective ob)
	{
		ObjectiveManager.instance.failObjective(ob);
	}


	public void UpdateObjective(Objective ob)
	{
		ObjectiveManager.instance.updateObjective(ob);
	}

	public void UpdateObjective(Objective ob, Color c)
	{
		ObjectiveManager.instance.updateObjective(ob,c);
	}


	void OnAnimation(Canvas target)
	{
		GameObject effect = Instantiate<GameObject>(Resources.Load<GameObject>("VcitoryEffect"), MainCamera.main.transform);
		effect.transform.localPosition = new Vector3(.3f,.3f,14.7f);
		effect.transform.localRotation = Quaternion.identity;

		target.enabled = true;
		target.GetComponent<Tweener>().GoToPose("On");
	}

	public void Win()
	{if (!hasFinished) {
			SoundTrackPlayer.main.playVictoryTrack();
            LevelSelectmanager.BattleModeChoice = null;
            hasFinished = true;
			OnAnimation(VictoryScreen);

		
			if (LevelData.getDifficulty () == 1) {
				VictoryScreen.transform.Find ("BackGround").GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> ("BronzeComplete");
			}
			else if (LevelData.getDifficulty () == 2)
			{
				VictoryScreen.transform.Find ("BackGround").GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> ("SilverComplete");

			}
			else
			{
				VictoryScreen.transform.Find ("BackGround").GetComponent<UnityEngine.UI.Image> ().sprite = Resources.Load<Sprite> ("GoldComplete");

			}

            GameObject levelEditor = ((GameObject)(Resources.Load<GameObject>("LevelEditor")));
            if (levelEditor)
            {
                LevelCompilation myComp = levelEditor.GetComponent<LevelCompilation>();
                //LevelCompilation.loadGameStatic ().ls [levelNumber].increaseCompCount ();
                if (!RaceSwapper.main)
                {
                    myComp.MyLevels[levelNumber].increaseCompCount();

                    int numTimesWon = PlayerPrefs.GetInt("L" + levelNumber + "Win");

                    PlayerPrefs.SetInt("L" + levelNumber + "Win", numTimesWon + 1);

                    int diff = LevelData.getDifficulty() - 1;
                    if (diff > PlayerPrefs.GetInt("L" + levelNumber + "Dif", -1))
                    {
                        PlayerPrefs.SetInt("L" + levelNumber + "Dif", diff);
                    }

                    GetComponent<AchievementChecker>().EndLevel();
                }
            }
			GameObject.FindObjectOfType<MainCamera>().DisableScrolling();
			UISetter.main.startFade (1.5f, false);
			StartCoroutine (WinLevel ());
		}
	}


	public void Lose()
	{if (!hasFinished) {
			hasFinished = true;
            LevelSelectmanager.BattleModeChoice = null;
            Debug.Log ("LostA");

			OnAnimation(DefeatScreen);
			GameObject.FindObjectOfType<MainCamera> ().DisableScrolling ();
			StartCoroutine (LoseLevel ());
			UISetter.main.startFade (1.5f, false);
		}
	}

	IEnumerator WinLevel ()
	{

        foreach (KeyValuePair<string, List<UnitManager>> pair in GameManager.main.playerList[0].getUnitList())
        {
            foreach (UnitManager man in pair.Value)
            {
                if (man)
                {
                    man.changeState(new VictoryState(man));
                }
            }
        }
        foreach (KeyValuePair<string, List<UnitManager>> pair in GameManager.main.playerList[1].getUnitList())
        {
            foreach (UnitManager man in pair.Value)
            {
                if (man)
                {
                    man.changeState(new StunState(man));
                }
            }
        }
        LevelData.getsaveInfo().ComingFromLevel = true;
		//ExpositionDisplayer.instance.displayText (6, victoryLine, 1);
		float totalTime = 2;

		for (int i = 0; i < 6; i++) { // Stop all currently playing messages
			ExpositionDisplayer.instance.InteruptMessage ();
		}
		foreach (int i in winLine) {
			dialogManager.instance.playLine (i);
			totalTime += dialogManager.instance.VoiceLines[i].MainLine.duration -1;
			}
		ExpositionDisplayer.instance.acceptMessages = false;
        if (CinematicCamera.main)
        {
            CinematicCamera.main.exitScene();
        }
		yield return new WaitForSeconds (totalTime);


			int bonusTech = LevelData.getDifficulty();
			if (bonusTech == 1)
			{
				bonusTech = 0;
			}
			else if (bonusTech == 2)
			{
				bonusTech = 1;
			}
			else if (bonusTech == 3)
			{
				bonusTech = 3;
			}


			int numTimesWon = PlayerPrefs.GetInt("L" + levelNumber + "Win") - 1;

			int totalReward = TechCredits + techRewards + bonusTech;
			Debug.Log("Times won " + numTimesWon);
			totalReward -= numTimesWon * 3; // Reduces the reward each time you replay a level
			if (totalReward < 0)
			{
				totalReward = bonusTech;
			}

			//Set my victory screen

			Debug.Log("COmpleting with " + TechCredits + "  " + techRewards + "  " + bonusTech);
			LevelData.levelInfo Ldata = createLevelInfo(levelNumber, GameManager.main.playerList[1].UnitsLost(), GameManager.main.playerList[0].UnitsLost(), 0, // Used to have Total Resources Harvested Here
                Clock.main.getTime(), totalReward, completeBonusObj + "/" + totalBonusObj);
			foreach (VictoryScreen vs in GameObject.FindObjectsOfType<VictoryScreen>())
			{
				vs.SetResults(Ldata, true);
			}

		if (!RaceSwapper.main)
		{
			LevelData.addLevelInfo(levelNumber, GameManager.main.playerList[1].UnitsLost(), GameManager.main.playerList[0].UnitsLost(), 0, // Used to have Total Resources Harvested Here
				Clock.main.getTime(), totalReward, completeBonusObj + "/" + totalBonusObj);
		}
	}

	IEnumerator LoseLevel ()
	{Debug.Log ("LostB");
		SoundTrackPlayer.main.playDefeatTrack();
		yield return new WaitForSeconds (1);

		foreach (int i in loseLine) {
			dialogManager.instance.playLine (i);
		}
		//ExpositionDisplayer.instance.displayText (6, DefeatLine, 1);
		yield return new WaitForSeconds (2.5f);

	

		LevelData.levelInfo Ldata = createLevelInfo(levelNumber, GameManager.main.playerList[1].UnitsLost(), GameManager.main.playerList[0].UnitsLost(), 0, // Used to have Total Resources Harvested Here
                Clock.main.getTime(), TechCredits + techRewards, completeBonusObj + "/" + totalBonusObj);
		foreach (VictoryScreen vs in GameObject.FindObjectsOfType<VictoryScreen>())
		{
			vs.SetResults(Ldata, false);
		}
		
	}

	public void loadLevel(int levelNumber)
	{
		Debug.Log ("Loading " + levelNumber);
		UnityEngine.SceneManagement.SceneManager.LoadScene (levelNumber);
	}


	public LevelData.levelInfo createLevelInfo(int levelN, int EnemiesD, int UnitsL, int Res, string timer,
		int Tech, string bonus)
	{
		LevelData.levelInfo myL = new LevelData.levelInfo ();
		myL.levelNumber = levelN;
		myL.EnemiesDest = EnemiesD;
		myL.unitsLost = UnitsL;
		myL.Resources = Res;
		myL.time = timer;
		myL.TechCredits = Tech;
		myL.bonusObj = bonus;
		return myL;
		//	Debug.Log ("Cureent Level " + currentLevel);
	}

	public void replay()
	{
		VictoryScreen.enabled = false;
		GameObject.FindObjectOfType<MainCamera> ().EnableScrolling ();
		DefeatScreen.enabled = false;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);

	}

	public void endLevel ()
	{
		VictoryScreen.enabled = false;
		GameObject.FindObjectOfType<MainCamera> ().EnableScrolling ();
		DefeatScreen.enabled = false;
		SceneManager.LoadScene (1);

	}



}
