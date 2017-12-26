using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	AudioSource mySource;
	public AudioClip buttonPress;

	public GameObject MainScreen;

	public GameObject levelIntros;
	public List<GameObject> Expositions = new List<GameObject> ();

	public Dropdown difficultyBars;

	public Canvas Technology;
	public Canvas TechTree;
	public Canvas UltTree;
 
	public List<Text> creditDisplayers = new List<Text> ();

	public Button techButton;
	public Button UltButton;
	public List<Canvas> Vehicles;
	public List<Canvas> Turrets;
	public List<Canvas> Structures;

	private List<Canvas> currentTech;
	public GameObject LevelSelector;

	public GameObject defaultStructure;
	public GameObject defaultVehicle;
	public GameObject defaultTurret;

	public VoiceContainer voicePacks;
	public GameObject AllTechToggle;
	public LevelIntroMaker IntroMaker;

	public static LevelManager main;
	// Use this for initialization
	void Awake () {
		mySource = GetComponent<AudioSource> ();
		main = this;


		foreach (GameObject ob in Expositions) {
			ob.SetActive (false);
		}

			currentTech = Vehicles;
		if (LevelData.getHighestLevel() == 0) {
			techButton.interactable = false;
			UltButton.gameObject.SetActive (false);//.interactable = false;
			} 
		else if (LevelData.getHighestLevel() == 1) {
			techButton.interactable = true;
			UltButton.gameObject.SetActive (false);
			//UltButton.interactable = false;
		} else {
			techButton.interactable = true;
			UltButton.gameObject.SetActive (true);
			//UltButton.interactable = true;
		}
		
		currentTech = Vehicles;

		difficultyBars.value = LevelData.getDifficulty () - 1;
	
		changeMoney (0);

	}



	public void nextLevel()
	{
		
		//Debug.Log ("Being called");
		Expositions [LevelData.getHighestLevel()].SetActive (true);

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
		AllTechToggle.SetActive (LevelData.getHighestLevel () > 6 && n < 6 && n != 0); // n!=0 because we dont have full tech tree on first level no matter what
		AllTechToggle.GetComponent<Toggle>().isOn = PlayerPrefs.GetInt ("AllTech",0) == 1;

		LevelCompilation comp = Resources.Load<GameObject> ("LevelEditor").GetComponent<LevelCompilation> ();
		IntroMaker.LoadLevel (comp.MyLevels[n]);
	}

	public void setActive(List<Canvas> canvases, bool setActive)
	{
		foreach (Canvas c in canvases) {
			c.enabled = setActive;		}
	}

	public void ToggleVehicle()
	{	if (currentTech != Vehicles) {
			if (currentTech != null) {
				setActive (currentTech, false);
		
			}

			currentTech = Vehicles;
			setActive (Vehicles, true);
			GameObject.FindObjectOfType<CampTechCamManager> ().loadTech ("Manticore");
		}
	}

	public void ToggleStruct()
	{if (currentTech != Structures) {
			if (currentTech != null) {
				setActive (currentTech, false);
			}

			currentTech = Structures;
			setActive (Structures, true);
			GameObject.FindObjectOfType<CampTechCamManager> ().loadTech ("Armory");
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

			GameObject.FindObjectOfType<CampTechCamManager> ().loadTech ("Minigun");
		}
	}


	public void MainMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene (0);
	}

	public void showMainScreen()
	{


	}


	public void ToggleTech()
	{
		mySource.PlayOneShot (buttonPress);
		LevelSelector.GetComponent<Canvas>().enabled = !LevelSelector.GetComponent<Canvas>().enabled;

	}

	public void toggleTechTree()
	{mySource.PlayOneShot (buttonPress);
		TechTree.enabled = !TechTree.enabled;
	}

	public void toggleUltTree()
	{mySource.PlayOneShot (buttonPress);
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
			mySource.PlayOneShot (buttonPress);
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

		//Debug.Log ("money " + LevelData.totalXP);
	}

	public void setAnnouncer(Dropdown i)
	{
		for (int j = 0; j < voicePacks.LockedVoicePacks.Count; j++) {
			if(i.options[i.value].text == voicePacks.LockedVoicePacks[j].voicePackName ){
					
				PlayerPrefs.SetInt ("VoicePack", j);

				if (Time.timeSinceLevelLoad > 2) {
					mySource.PlayOneShot (voicePacks.LockedVoicePacks [j].getVoicePackLine ());
				}
			}
		}
	}

}
