using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	public Canvas MainMenu;
	public Canvas multiplayer;
	public Canvas campaign;
	public Canvas Credits;
	public Canvas loadCanvas;
	public Canvas OptionsCan;
	public Canvas LoadingScreen;
	public Canvas SwapOutScreen;
	public GameObject blackScreen;

	public AudioSource audioSource;
	private Canvas currentScreen;

	public static MainMenuManager main;

	// Use this for initialization
	void Start () {
		main = this;
		currentScreen = MainMenu;
		if (LevelSelectmanager.reservedChoice != null && SwapOutScreen)
		{
			SwapOutScreen.enabled = true;
			MainMenu.enabled = false;
			blackScreen.SetActive(false);
		}
	}

	public void loadScreen(Canvas can)
	{
		Cursor.lockState = CursorLockMode.Confined;
		currentScreen.enabled = false;
		currentScreen = can;
		can.enabled = true;
	}

	public void toMultiplayer()
	{	Cursor.lockState = CursorLockMode.Confined;
		multiplayer.GetComponent<RaceSelectionManger> ().initialize ();
		loadScreen (multiplayer);
		multiplayer.GetComponent<RaceSelectionManger> ().initialize ();

	}


	public void toPrologue()
	{if (LoadingScreen) {
			LoadingScreen.enabled = true;
		}
		SceneManager.LoadScene (2);
	}

	public void startMatch(Dropdown dropper)
	{
		Cursor.lockState = CursorLockMode.Confined;
		resetProgress ();
		setDifficulty (dropper);
		audioSource.Stop ();
		if (LoadingScreen) {
			LoadingScreen.enabled = true;
		}
		SceneManager.LoadScene(5);
	}

	public void toCampaignLevelSelect()
	{	if (LoadingScreen) {
			LoadingScreen.enabled = true;
		}
		Cursor.lockState = CursorLockMode.Confined;
		SceneManager.LoadScene (1);

	}

	public void toCampaign()
		{
		
		loadScreen (campaign);
		audioSource.Play ();
	
	}

	public void Exit()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
		}


	public void replay()
	{

		if (VictoryTrigger.instance)
		{
			if (RaceSwapper.main)
			{
				LevelSelectmanager.BattleModeChoice = LevelSelectmanager.reservedChoice;
			}
			VictoryTrigger.instance.replay();
		}

	}

	public void loadOption()
	{
		OptionsCan.enabled = !OptionsCan.enabled;

	}

	public void LoadLevel(int n)
	{//LevelData.currentLevel = n;
		LevelData.getsaveInfo().ComingFromLevel = false;
		if (LoadingScreen) {
			LoadingScreen.enabled = true;
		}
		Cursor.lockState = CursorLockMode.Confined;
		SceneManager.LoadScene (3);
	
	}

	public void LoadSceneNumber(int i)
	{
		LevelData.getsaveInfo().ComingFromLevel = false;
		if (LoadingScreen)
		{
			LoadingScreen.enabled = true;
		}
		Cursor.lockState = CursorLockMode.Confined;
		SceneManager.LoadScene(i);
	}



	public void toCredits()
	{loadScreen (Credits);}

	public void EndGameCredits()
	{
		SceneManager.LoadScene(17);
	}

	public void toMain()
	{loadScreen (MainMenu);
		audioSource.Stop ();
	}


	public void resetProgress()
	{PlayerPrefs.DeleteAll ();

		LevelData.reset ();
	}


	public void setDifficulty(Dropdown dropper)
	{

		LevelData.setDifficulty (dropper.value + 1);

	}


	public void GoToWebsite()
	{
		Application.OpenURL ("https://www.playbattlemore.com/");
	}
}
