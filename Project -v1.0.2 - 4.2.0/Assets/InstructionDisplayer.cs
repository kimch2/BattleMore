using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InstructionDisplayer : MonoBehaviour {


	private Canvas myCanvas;
	private Text myText;
	public static InstructionDisplayer instance;
	public Image myImage;

	private AudioSource myAudio;

	// Use this for initialization
	void Start () {
		myText = GetComponentInChildren<Text> ();
		instance = this;
		myCanvas = GetComponent<Canvas> ();
		myCanvas.enabled = false;
		myAudio = GetComponent<AudioSource> ();
	}


	public void displayText(string input, float duration, AudioClip sound, float volume, Sprite pic)
	{
		CancelInvoke ("TurnOff");
		this.enabled = true;
		myText.text = input;
		myCanvas.enabled = true;

		Invoke ("TurnOff", duration);
		//MissionLogger.instance.AddLog (input);
		if (sound != null) {
			myAudio.volume = volume;
			myAudio.PlayOneShot (sound);
		}
		if (pic != null) {
			myImage.enabled = true;
			myImage.sprite = pic;
		} else {
			myImage.enabled = false;
		}
		Canvas.ForceUpdateCanvases ();
	}

	public void TurnOff()
	{
		this.enabled = false;
		myCanvas.enabled = false;

	}


}
