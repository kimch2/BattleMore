﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundOptions : MonoBehaviour {



	public GameObject masterSLiderO;
	public GameObject musicSliderO;
	public AudioSource music;

	private Slider masterSLider;
	private Slider musicSlider;
	public Slider unitResp;
	public Slider AlertFreq;
	public Toggle WaveWarning;

	// Use this for initialization
	void Start () {
		if (WaveWarning) {
			WaveWarning.isOn = GameSettings.getWaveWarning ();
		}

		masterSLider = masterSLiderO.GetComponent<Slider> ();

		musicSlider = musicSliderO.GetComponent<Slider> ();
		MainCamera mc = GameObject.FindObjectOfType<MainCamera> ();
		if (mc) {
			music = mc.GetComponent<AudioSource> ();
		}
		if (!music) {
			if (MainCamera.main) {
				music = MainCamera.main.GetComponent<AudioSource> ();
			} else {
				music = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<AudioSource> ();
			}
		}

		if (AlertFreq) {
			AlertFreq.value = (GameSettings.getBaseAlertFreq () - .2f) / 1.5f;
		}
		if (unitResp) {
			unitResp.value = (GameSettings.getUnitResponseFreq () - .35f) / 1.65f;
		}
		//if (GameSettings.getMasterVolume()> -1) {
			masterSLider.value = GameSettings.getMasterVolume();
			AudioListener.volume = masterSLider.value;
		//}
		//else{
		//	masterSLider.value = AudioListener.volume;
		//	GameSettings.setMusicVolume (masterSLider.value);
		//}


		//if (GameSettings.getMusicVolume()> -1) {
		musicSlider.value = GameSettings.getMusicVolume ();
		music.volume = musicSlider.value;
		//} else {
		//	musicSlider.value = music.volume;
		//	GameSettings.setMusicVolume (music.volume);
		//}
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.KeypadPlus)) {
			GameSettings.setMasterVolume (GameSettings.getMasterVolume () + .15f);
			masterSLider.value = GameSettings.getMasterVolume();
			AudioListener.volume = masterSLider.value;
		

		} else if (Input.GetKeyDown (KeyCode.KeypadMinus)) {
			GameSettings.setMasterVolume (GameSettings.getMasterVolume () - .15f);
			masterSLider.value = GameSettings.getMasterVolume();
			AudioListener.volume = masterSLider.value;
		}
	
	}


	public void MusicVolumeChange()
	{
		music.volume = musicSlider.value;
		GameSettings.setMusicVolume (music.volume);
	}


	public void masterVolumeChange()
	{
		AudioListener.volume = masterSLider.value;
		GameSettings.setMasterVolume (masterSLider.value);
	}

	public void UnitResponseChange()
	{
		GameSettings.setUnitResponseFreq (unitResp.value * 1.65f + .35f);
	}
	public void AlertResponseChange()
	{
		GameSettings.setBaseAlertFreq(AlertFreq.value * 1.5f + .2f);
	}

	public void toggleWaveWarning()
	{ 
		GameSettings.setWaveWarning (WaveWarning.isOn);
	}

}
