using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectmanager : MonoBehaviour {


	public List<Button> levelButtons;

	public List<FreePlayLevel> Levels;

	public GameObject StartButton;

	public Text Title;
	public Text Description;

	private void Start()
	{
		currentLevel = Levels[0];
	}

	public void resetProgress()
	{

	}

	FreePlayLevel currentLevel;

	public void LoadLevel(int i)
	{
		Title.text = Levels[i].LevelName;
		Description.text = Levels[i].Description;
		StartButton.gameObject.SetActive(true);
	}

	public void PlayMission()
	{
		MainMenuManager.main.LoadLevel(currentLevel.SceneNumber);
	}
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
}