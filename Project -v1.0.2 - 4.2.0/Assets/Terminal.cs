using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour {

	public Canvas toOpen;
	public Canvas toReturn;
	public int levelUnlocked;
	public bool read;

	public Canvas nextCanvas;
	public Canvas previousCan;

	public void close()
	{
		toOpen.enabled = false;
		toReturn.enabled = true;
	}

	public void Open()
	{
		toOpen.enabled = true;
		toReturn.enabled = false;
	}

	public void next()
	{
		if (nextCanvas) {
			nextCanvas.enabled = true;
			toOpen.enabled = false;
		}
	}

	public void Previous()
	{
		if (previousCan) {
			previousCan.enabled = true;
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
