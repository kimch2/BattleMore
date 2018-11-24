using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectmanager : MonoBehaviour {


	public static LevelChoice BattleModeChoice;
	public List<Button> levelButtons;
	public Image Background;

	public List<FreePlayLevel> Levels;

	public GameObject StartButton;

	public Text Title;
	public Text Description;

	private void Start()
	{
		MyRace = RaceInfo.raceType.SteelCrest;
		EnemyRace = RaceInfo.raceType.Coalition;
		LoadLevel(3);
	}

	public void resetProgress()
	{

	}

	FreePlayLevel currentLevel;
	public RaceInfo.raceType MyRace = RaceInfo.raceType.SteelCrest;
	public RaceInfo.raceType EnemyRace = RaceInfo.raceType.Coalition;

	public void LoadLevel(int i)
	{
		currentLevel = Levels[i];
		Debug.Log("Current Level is " + i + "  " + currentLevel.SceneNumber);
		Title.text = Levels[i].LevelName;
		Description.text = Levels[i].Description;
		StartButton.gameObject.SetActive(true);
		Background.sprite = currentLevel.myBackground;
	}

	public void PlayMission()
	{

		BattleModeChoice = new LevelChoice();
		BattleModeChoice.PlayingAs = MyRace;
		BattleModeChoice.PlayingAgainst = EnemyRace;
		Debug.Log("Playing   " + currentLevel.SceneNumber);
		MainMenuManager.main.LoadSceneNumber(currentLevel.SceneNumber);
	}

	public void SetRace(Dropdown d)
	{
		switch (d.value)
		{
			case 0:
				MyRace = RaceInfo.raceType.SteelCrest;	
			break;
			case 1:
				MyRace = RaceInfo.raceType.Coalition;
				break;
			case 2:
				if (UnityEngine.Random.Range(0, 2) == 0)
				{
					MyRace = RaceInfo.raceType.Coalition;
				}
				else
				{
					MyRace = RaceInfo.raceType.SteelCrest;
				}
				break;
		}
	}

	public void SetEnemy(Dropdown d)
	{
		switch (d.value)
		{
			case 0:
				EnemyRace = RaceInfo.raceType.SteelCrest;
				break;
			case 1:
				EnemyRace = RaceInfo.raceType.Coalition;
				break;
			case 2:
				if (UnityEngine.Random.Range(0, 2) == 0)
				{
					EnemyRace = RaceInfo.raceType.Coalition;
				}
				else
				{
					EnemyRace = RaceInfo.raceType.SteelCrest;
				}
				break;
		}
		Debug.Log("Enemy " + EnemyRace);
	}

}

public class LevelChoice
{
	public RaceInfo.raceType PlayingAs;
	public RaceInfo.raceType PlayingAgainst;
}


[System.Serializable]
public class FreePlayLevel
{
	public string LevelName;
	public string Description;
	public Sprite icon;
	public int SceneNumber;
	public List<RaceInfo.raceType > AllowedToPlayAs;
	public List<RaceInfo.raceType> AllowedToPlayAgainst;
	public Sprite myBackground;
}