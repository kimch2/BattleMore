﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {



	public Dropdown difficultyBars;

	public Canvas TechTree;
	public Canvas UltTree;
 
	public List<Text> creditDisplayers = new List<Text> ();

	public Button techButton;
	public Button UltButton;
	public List<Canvas> Vehicles;
	public List<Canvas> Turrets;
	public List<Canvas> Structures;

	private List<Canvas> currentTech;
	public VoiceContainer voicePacks;
	public GameObject AllTechToggle;
	public LevelIntroMaker IntroMaker;

	public static LevelManager main;
	public LevelCompilation levelEditor;

	// Use this for initialization
	void Awake () {
		main = this;

		levelEditor = Resources.Load<GameObject>("LevelEditor").GetComponent<LevelCompilation>();
		if (LevelData.getHighestLevel() == 0) {
			techButton.interactable = false;
			UltButton.gameObject.SetActive (false);
			} 
		else if (LevelData.getHighestLevel() == 1) {
			techButton.interactable = true;
			UltButton.gameObject.SetActive (false);
		} else {
			techButton.interactable = true;
			UltButton.gameObject.SetActive (true);
		}
		
		currentTech = Vehicles;

	
	
		changeMoney (0);

	}
	void Start()
	{
		difficultyBars.value = LevelData.getDifficulty () - 1;
	}

		
	public void closeLevelIntro()
	{
		foreach (ToolTip tt in GameObject.FindObjectsOfType<ToolTip>()) {
			if (tt.toolbox) {
				tt.toolbox.enabled = false;
			} else {
				tt.ToolObj.SetActive (false);
			}
		}
	}


	public void setAllTech(Toggle  myToggle)
	{
		PlayerPrefs.SetInt("AllTech", myToggle.isOn? 1:0);
	}

	public void openLevelIntro(int n)
	{

		AllTechToggle.SetActive (LevelData.getHighestLevel () > 6 && levelEditor.MyLevels[n].showFullTechTree); // n!=0 because we dont have full tech tree on first level no matter what
		AllTechToggle.GetComponent<Toggle>().isOn = PlayerPrefs.GetInt ("AllTech",0) == 1;

		IntroMaker.LoadLevel (levelEditor.MyLevels[n]);
	}

	public void setActive(List<Canvas> canvases, bool setActive)
	{
		foreach (Canvas c in canvases) {
			c.enabled = setActive;		}
	}

	public void ToggleVehicle()
	{	//Debug.Log ("Toggling " + currentTech[0].name);
		if (currentTech != Vehicles) {
			if (currentTech != null) {
				setActive (currentTech, false);
		
			}

			currentTech = Vehicles;
			setActive (Vehicles, true);
		
		}
	}

	public void ToggleStruct()
	{if (currentTech != Structures) {
			if (currentTech != null) {
				setActive (currentTech, false);
			}

			currentTech = Structures;
			setActive (Structures, true);
		
		}
	}

	public void ToggleTurret()
	{
		if (currentTech != Turrets) {
			if (currentTech != null) {
				setActive (currentTech, false);
			}
			currentTech = Turrets;
			setActive (Turrets, true);
		}
	}


	public void MainMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene (0);
	}



	public void toggleTechTree()
	{	TrueUpgradeManager.instance.playSimpleSound ();

		TechTree.enabled = !TechTree.enabled;
	}

	public void toggleUltTree()
	{TrueUpgradeManager.instance.playSimpleSound ();
		CanvasGroup grouper = UltTree.GetComponent<CanvasGroup>();
		if (grouper.interactable) {
			grouper.interactable = false;
			grouper.alpha = 0;
			grouper.blocksRaycasts = false;
		
		} else {
			grouper.interactable = true;
			grouper.alpha = 1;
			grouper.blocksRaycasts = true;
		}
		UltTree.enabled = !UltTree.enabled;
	}

	public void setDifficulty(Dropdown i)
	{
		if (Time.timeSinceLevelLoad > 1) {
			TrueUpgradeManager.instance.playSimpleSound ();
		}
		LevelData.setDifficulty (i.value + 1);
	}


	public void changeMoney(int input)
	{
		LevelData.addMoney (input);
		foreach (Text t in creditDisplayers) {
			if (t) {
				t.text = "" + LevelData.getMoney ();
			}
		}
	}

	public void setAnnouncer(Dropdown i)
	{
		for (int j = 0; j < voicePacks.LockedVoicePacks.Count; j++) {
			if(i.options[i.value].text == voicePacks.LockedVoicePacks[j].voicePackName ){
					
				PlayerPrefs.SetInt ("VoicePack", j);

				if (Time.timeSinceLevelLoad > 2) {
					GetComponent<AudioSource>().PlayOneShot (voicePacks.LockedVoicePacks [j].getVoicePackLine ());
				}
			}
		}
	}

}
