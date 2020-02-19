using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScienceLog : MonoBehaviour {



	public List<ScienceEntry> myEntries;
	public GameObject nextButton;
	public GameObject previousButton;
	public GameObject returnButton;

	public GameObject buttonScreen;
	List<ScienceEntry> currentEntries = new List<ScienceEntry>();


	// Use this for initialization
	void Start () {


		foreach (ScienceEntry enter in myEntries) {
			if (LevelData.getsaveInfo ().hasCompletedLevel (enter.levelUnlocked)) {
				currentEntries.Add (enter);
				if (LevelData.getsaveInfo ().hasReadLog (enter.OpenButton.GetComponentInChildren<Text> ().text)) {
					setAsRead (enter);
				}

			} else {
				enter.OpenButton.SetActive (false);
			}
		}
	}


	int currentIndex;
	public void next()
	{
		currentEntries [currentIndex].Screen.SetActive (false);
		currentIndex++;
	
		currentEntries [currentIndex].Screen.SetActive (true);
		checkPrevNextButtons ();
	}

	public void previous()
	{
		currentEntries [currentIndex].Screen.SetActive (false);
		currentIndex--;
	
		currentEntries [currentIndex].Screen.SetActive (true);
		checkPrevNextButtons ();
	}

	public void Open(GameObject openButton)
	{
		foreach (ScienceEntry enter in currentEntries) {
			if (enter.OpenButton == openButton) {
				currentIndex = currentEntries.IndexOf (enter);
				enter.Screen.SetActive (true);
				break;
			}
		}
		returnButton.SetActive (true);
		checkPrevNextButtons ();
		buttonScreen.SetActive (false);
	}

	void checkPrevNextButtons()
	{
	LevelData.getsaveInfo ().readScienceLog (currentEntries [currentIndex].OpenButton.GetComponentInChildren<Text> ().text);
		setAsRead (currentEntries[currentIndex]);
	
		previousButton.SetActive (currentIndex > 0);
		nextButton.SetActive (currentIndex <currentEntries.Count - 1);
	}


	void setAsRead(ScienceEntry enter)
	{
		//enter.OpenButton.GetComponentInChildren<Text> ().fontStyle = FontStyle.Normal;
		enter.OpenButton.GetComponent<Image> ().color = new Color (.7f,.7f,.7f);
		enter.OpenButton.GetComponentInChildren<Text> ().fontSize = 26;
	}


	public void returnToMain()
	{
		currentEntries [currentIndex].Screen.SetActive (false);
		buttonScreen.SetActive (true);
		returnButton.SetActive (false);
		nextButton.SetActive (false);
		previousButton.SetActive (false);
	}



	[System.Serializable]
	public class ScienceEntry
	{
		public GameObject Screen;
		public GameObject OpenButton;
		public int levelUnlocked;

	}
}
