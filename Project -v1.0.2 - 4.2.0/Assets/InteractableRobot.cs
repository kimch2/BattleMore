using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableRobot : MonoBehaviour {

	[Tooltip("Priority is used to shwo when the line is played.")]
	public List<DialogLine> myLines;
	public AudioSource mySrc;

	DialogLine currentLine;
	void Start()
	{

		currentLine = myLines.Find (item => item.Priority == LevelData.getHighestLevel ());
		if (currentLine!= null) {


		} else {
		
			this.gameObject.SetActive (false);
		}

	}


	public void interact()
	{
		mySrc.PlayOneShot (currentLine.MainLine.myClip);
		currentLine = null;
		turnOffFlashing ();
	}


	public void turnOffFlashing()
	{
		
	}

}
