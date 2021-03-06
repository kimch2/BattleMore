﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class ErrorPrompt : MonoBehaviour {

	public AudioClip errorSound;
	public static ErrorPrompt instance;

	public VoiceContainer voiceContainer;
	VoicePack myVoicePack;

	float lastAttackAlert = -1000;
	List<Vector3> attackLocations = new List<Vector3>();
	int currentAlertIndex;

	float lastErrorTime;
	float errorFreq;
	public string supplyBuilding = "Aether Cores";

	public void showError(string err)
	{

		if (lastErrorTime < Time.time - 2.5f) {

			this.gameObject.GetComponent<Text> ().text = err;
			this.gameObject.GetComponent<Text> ().enabled = true;
			ExpositionDisplayer.instance.displayText ("", 10, errorSound, .4f, null, 3);
			//myAudio.PlayOneShot (errorSound, .4f);
			StopCoroutine (MyCoroutine ());
			StartCoroutine (MyCoroutine ());

			lastErrorTime = Time.time;
		}
	}

	public void showError(string err, AudioClip aud)
	{

		if (lastErrorTime < Time.time - 5f) {

			this.gameObject.GetComponent<Text> ().text = err;
			this.gameObject.GetComponent<Text> ().enabled = true;
			ExpositionDisplayer.instance.displayText ("", aud.length + .5f, aud, 1, null, 3);
			//myAudio.PlayOneShot (aud,1);
			StopCoroutine (MyCoroutine ());
			StartCoroutine (MyCoroutine ());

			lastErrorTime = Time.time;
		}
	}


	//Reserved for thing like building construction completion
	public void showMessage(string err, AudioClip clip)
	{
		this.gameObject.GetComponent<Text> ().text = err;
		this.gameObject.GetComponent<Text> ().enabled = true;
		ExpositionDisplayer.instance.displayText ("", clip.length + .5f, clip, 1, null, 3);
		//myAudio.PlayOneShot (clip);
		StopCoroutine (MyCoroutine ());
		StartCoroutine (MyCoroutine ());

		lastErrorTime = Time.time;

	}
	
	IEnumerator MyCoroutine ()
	{

		yield return new WaitForSeconds(3f);
		this.gameObject.GetComponent<Text> ().enabled = false;

	}//
	

	public void notEnoughResource(ResourceType resType)
	{
        UISoundManager.interfaceClick (false);
		Debug.Log("Printing ");
		showError( "Not Enough " + resType.ToString(),myVoicePack.getOreLine());
	} 


	public void ResearchComplete(string s , Vector3 location)
	{showError(  s.Substring(0, s.Length-3) + " Complete ", myVoicePack.getResearchLine(s));
		addAlertLocation( location);
	}

	public void notEnoughSupply()
	{UISoundManager.interfaceClick (false);
		showError( "Not enough Supply, Build more " + supplyBuilding, myVoicePack.getSupplyLine());
		}

	public void atMaxSupply()
	{showError( "Army Supply limit reached", myVoicePack.getMaxSupplyLine());
	}

	public void notEnoughEnergy()
	{UISoundManager.interfaceClick (false);
		showError(  "Not enough Energy");
		}

	public void populationcapHit()
	{
		showError( "Population Limited Reached", myVoicePack.getMaxSupplyLine());

	}
	public void BuildingDone(string n, Vector3 location)
	{
		showError( n + " Construction Complete", myVoicePack.getBuildingCompleteLine(n));
		addAlertLocation( location);

	}


	public void UltTwoDone()
	{
		showError( "Nimbus Deployment Ready", myVoicePack.getUltTwoComplete());

	}

	public void UltFourDone()
	{
		showError( "FireStorm Ready", myVoicePack.getUltFourComplete());
	
	}
	public void invalidGroundLocation()
	{UISoundManager.interfaceClick (false);
		showError( "Invalid Location", myVoicePack.getBadBuildingLine());
	}

	public void invalidTarget()
	{UISoundManager.interfaceClick (false);
		showError( "Invalid target.");
		}

	public void onCooldown()
	{UISoundManager.interfaceClick (false);
		showError( "Ability on Cooldown", myVoicePack.getCooldownLine());
	}

	public void OreDepleted(Vector3 location)
	{showError( "Ore Deposit Depleted", myVoicePack.getOreDepletedLine());
		addAlertLocation( location);
	}


	public void setErrorFreq(float Amount)
	{

		errorFreq = (10 / Amount) + 4;
	}
	public void underAttack(Vector3 location)
	{	

			if (lastAttackAlert + errorFreq > Time.time && !checkIfOnScreen(location)) {
			showError ("Under Attack!", myVoicePack.getTroopAttackLine());
			addAlertLocation( location);
		
			lastAttackAlert = Time.time;
		}

	}


	void addAlertLocation(Vector3 spot)
	{
		attackLocations.Insert (0, spot);
		currentAlertIndex = 0;
		if (attackLocations.Count > 4) {
			attackLocations.RemoveAt (2);
		}
	}
	
	public void underBaseAttack(Vector3 location)
	{

		if (lastAttackAlert +  errorFreq < Time.time &&!checkIfOnScreen(location) && Time.timeSinceLevelLoad > 15) {
			showError ("Base Under Attack!", myVoicePack.getbaseAttackedLine());
			addAlertLocation( location);

			lastAttackAlert = Time.time;
		}

	}


	public void EnemyWave(WaveContainer.waveWarningType waveType)
	{
		if (GameSettings.getWaveWarning ()) {
			ExpositionDisplayer.instance.displayText ("", 5, myVoicePack.getEnemyWaveLine (waveType), 1, null, 4);
		}
		//showError ("Enemy Wave Detected!", myVoicePack.getbaseAttackedLine());
	}

	public bool checkIfOnScreen(Vector3 spot)
	{Vector3 tempLocation = Camera.main.WorldToScreenPoint (spot);
		if (tempLocation.x < 0) {
			return false;
		}
		if (tempLocation.x > Screen.width) {
			return false;
		}
		if (tempLocation.y > Screen.height) {
			return false;
		}
		if (tempLocation.y  < 0) {
			return false;
		}
		return true;
	}


	// Use this for initialization
	void Start () {
		instance = this;
		GameMenu.main.addDisableScript (this);

		myVoicePack = voiceContainer.myVoicePacks[PlayerPrefs.GetInt("VoicePack",0)];
		setErrorFreq(PlayerPrefs.GetFloat ("BaseAlert",10));

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.BackQuote)) {

			if (attackLocations.Count > 0) {
				MainCamera.main.generalMove (attackLocations [currentAlertIndex]);			
				currentAlertIndex++;
				if (currentAlertIndex == attackLocations.Count) {
					currentAlertIndex = 0;
				}
			}
		}

	}



	


}
