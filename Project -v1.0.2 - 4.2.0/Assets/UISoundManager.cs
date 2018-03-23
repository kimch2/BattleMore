using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundManager : MonoBehaviour {


	public AudioSource mySrc;
	public AudioClip goodSound;
	public AudioClip badSound;
	public static UISoundManager instance;
	// Use this for initialization
	void Awake () {
		instance = this;
	}
	
	public static void interfaceClick(bool good)
	{
		instance.Click (good);
	}

	public void Click(bool good)
	{
		if (good) {
	
			mySrc.PlayOneShot (goodSound, .47f);
		} else {
			
			mySrc.PlayOneShot (badSound, .25f);
		}
	}
}
