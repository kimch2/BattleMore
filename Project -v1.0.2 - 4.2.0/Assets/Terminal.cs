using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour {

	public GameObject openButton;
	public Canvas toOpen;
	public Canvas toReturn;
	public int levelUnlocked;
	public bool read;

	public Canvas nextCanvas;
	public Canvas previousCan;
	public GameObject nextButton;
	public GameObject prevButton;


	void Start()
	{
		
		if (!LevelData.getsaveInfo().hasCompletedLevel (levelUnlocked)) {
			openButton.SetActive (false);
			toOpen.gameObject.SetActive (false);
		}
		if (!LevelData.getsaveInfo ().hasReadLog (openButton.GetComponentInChildren<Text> ().text)) {
			openButton.GetComponent<Image> ().color = Color.cyan;
		} else {
			openButton.GetComponent<Image> ().color = Color.white;
		}
	}

	public void close()
	{
		toOpen.enabled = false;
		toReturn.enabled = true;
	}

	public void Open()
	{
		toOpen.enabled = true;
		toReturn.enabled = false;
		LevelData.getsaveInfo ().readScienceLog(openButton.GetComponentInChildren<Text> ().text);
		openButton.GetComponent<Image> ().color = Color.white;
		
		if (!nextCanvas || !nextCanvas.gameObject.activeInHierarchy ) {
			nextButton.SetActive (false);
		}

		if (!previousCan ||  !previousCan.gameObject.activeInHierarchy ) {
			prevButton.SetActive (false);
		}
	}

	public void next()
	{
		if (nextCanvas) {
			toOpen.enabled = false;
	
			nextCanvas.GetComponent<Terminal> ().Open ();

		}
	}

	public void Previous()
	{
		if (previousCan) {
			previousCan.GetComponent<Terminal> ().Open ();
			toOpen.enabled = false;
		}
	}

	// Emails List for each headline, click on it to open,
	// Return to inbox, delete email
	// Scroll?


	// Science Entry

	//class 
	// Title
	// Text
	// CustomPage?
	// Read,
	// Deleted
	// Level unlocked
	/*
	public List<Entry> myEntries;
	public GameObject inBoxPrefab;
	public GameObject Page;
	public Text title;
	public Text description;


	[System.Serializable]
	public class Entry
	{
		public string title;
		public string fullText;
		public Canvas previousPage;
		public Canvas CustomPage;
		public bool read;
		public bool deleted;
		public int levelUnlocked;

	}
*/
}
