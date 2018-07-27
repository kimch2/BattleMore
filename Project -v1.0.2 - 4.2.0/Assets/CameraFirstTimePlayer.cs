using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFirstTimePlayer : MonoBehaviour {

	[TextArea(3,10)]
	public string OpeningLine;
	public AudioClip openingAudio;
	public InteractableRobot myRobot;
	public GameObject hadrianPic;

	// Use this for initialization
	void Awake () {

		if (LevelData.getHighestLevel () == 1) {
			hadrianPic.SetActive (true);
			myRobot.StartCoroutine (myRobot.scrollingText(OpeningLine,openingAudio));
			Invoke ("turnOff", openingAudio.length + 2.5f);

		} else {
			GetComponent<Animator> ().enabled = false;
			GetComponent<EventManager> ().EnterState ("MainArea");
			GetComponent<TimeDelayTrigger> ().enabled = false;
		}
	}

	void turnOff()
	{
		hadrianPic.SetActive (false);
	}

}
