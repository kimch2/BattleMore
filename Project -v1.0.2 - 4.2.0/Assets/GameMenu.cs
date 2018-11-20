using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class GameMenu : MonoBehaviour {


	public Canvas myCanvas;

	public Button quitB;
	public Button optionsB;
	public Button hotkeysB;
	public Button raceTipsB;
	public Button returnToGameB;

	public Button EnemyArrayB;

	public bool ispaused = false;
	public bool isDisabled = false;

	public Canvas racetipMenu;
	public Canvas OptionMenu;
	public Canvas hotkeyMenu;
	public Canvas soundMenu;
	public Canvas gameplayMenu;
	public Canvas graphicsMenu;
	public Canvas BugReporter;

	public Canvas otherHotkeys;
	public Canvas EnemyArray;

	private Canvas currentMenu;

	private UIManager uimanage;
	public KeyCode escapekey = KeyCode.Backspace;

	public ExpositionDisplayer displayer;
	//to be deactivated when the game is paused to halt their inputs.
	private List<MonoBehaviour> disableScripts = new List<MonoBehaviour>();

	public static GameMenu main;

	void Awake()
	{
		main = this;
	}

	// Use this for initialization
	void Start () {
		
		uimanage = (UIManager)FindObjectOfType<UIManager>();
		if (GameSettings.gameSpeed < 0) {
			GameSettings.gameSpeed = 1;
		} 
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyUp (escapekey) &&currentMenu != BugReporter) {
			
			openMenu ();
			if (CursorManager.main) {
				CursorManager.main.normalMode ();
			}
		}

	}

	public void addDisableScript(MonoBehaviour m)
	{
		disableScripts.Add (m);
	}




	public void setMenu(Canvas can)
	{
		closeDropdowns();
		if (currentMenu != null) {
			currentMenu.enabled = false;
		}
		currentMenu = can;
		if (can != null) {
			can.enabled = true;
		}
		
	}



	public void openMenu()
	{if(myCanvas.enabled == true)
		{returnToGame();
			uimanage.SwitchToModeNormal ();
		

		}
	else
	{setMenu (myCanvas);
			if (uimanage) {
				uimanage.setToMenu ();
			}
		pause ();
	}
		
	}

	void closeDropdowns()
	{ foreach (Dropdown d in GameObject.FindObjectsOfType<Dropdown>())
		{
			d.Hide();
		}

	}

	public void quitGame()
	{SceneManager.LoadScene (1);}

	public void restartLevel()
	{	Time.timeScale = 1;
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);}

	public void openOptions()
	{setMenu (OptionMenu);
	}

	public void openHotkeys()
	{
		setMenu (hotkeyMenu);
	}

	public void openBugReporter()
	{
		setMenu (BugReporter);
	}

	public void openRaceTips()
	{setMenu (racetipMenu);}

	public void OpenEnemyArray()
	{pause ();
		setMenu (EnemyArray);
	}


	public void pause()
	{
	//	Debug.Log ("Pauseing");
		ispaused = true;
		Cursor.lockState = CursorLockMode.None;
			Time.timeScale = 0;
		if (displayer) {
			displayer.pause ();
		}
		foreach (MonoBehaviour m in disableScripts) {
			m.enabled = false;
		}

		
	}

	public void disableInput()
	{foreach (MonoBehaviour m in disableScripts) {
			m.enabled = false;
		}

	}

	public void EnableInput()
	{foreach (MonoBehaviour m in disableScripts) {
			m.enabled =true;
		}
	}

	public void unpause()
	{
		//	Debug.Log ("Pauseing");
		ispaused = false;
		Cursor.lockState = CursorLockMode.Confined;
		if (displayer) {
			displayer.resume ();
		}
		Time.timeScale = GameSettings.gameSpeed;
		foreach (MonoBehaviour m in disableScripts) {
			m.enabled = true;
		}
	}

	public void openSoundMenu()
	{setMenu (soundMenu);
	}

	public void returnToGame()
	{setMenu (null);
		if (uimanage) {
			uimanage.SwitchToModeNormal ();
		}
		unpause ();
	}


	public void openOtherHotkeys()
	{setMenu (otherHotkeys);
	}

	public void openGamePlayMenu()
	{setMenu (gameplayMenu);
	}

	public void openGraphicsMenu()
	{
		setMenu (graphicsMenu);
	}




	public void ExitGame()
	{Application.Quit ();}

}
