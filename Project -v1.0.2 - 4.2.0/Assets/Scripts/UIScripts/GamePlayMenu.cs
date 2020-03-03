﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class GamePlayMenu : MonoBehaviour {

	public static GamePlayMenu instance;

	public Dropdown healthList;

	private MainCamera cam;
	public Slider scrollSpeed;
	public Text speedPercent;
	public Toggle showToolTip;
	public Toggle showAbility;

	public Toggle simpleToggle;
	public Toggle ControlToggle;
	public List<GameObject> SimpleObjects;
	public List<Canvas> SimpleCanvas;
	public List<GameObject> SuperSimpleObjects;
	public Toggle fastCast;

	public static GamePlayMenu getInstance()
	{
		if (!instance) {
			instance = GameObject.FindObjectOfType<GamePlayMenu> ();
		}
		return instance;
	}


	void Awake()
	{
		
		instance = this;
		simpleToggle.isOn =  PlayerPrefs.GetInt ("SimpleUI",0)  == 1;

		fastCast.isOn = PlayerPrefs.GetInt ("FastCast", 0) ==1;
		ControlToggle.isOn =  PlayerPrefs.GetInt ("SuperSimpleUI",0)  == 1;


	}

	// Use this for initialization
	void Start () {

		cam =  MainCamera.main;


		showToolTip.isOn = GameSettings.getToolTips();
		toggleTools ();

		showAbility.isOn = GameSettings.getAbility();
		toggleAbilities ();
		
		healthBarList ();
	
	}


	public void changeMouseSpeed(Slider slide)
	{
		PlayerPrefs.SetFloat("MouseScroll", slide.value);
		// NOT IMPLEMENTED
	}

	public void toggleTools()
	{

		GameSettings.setToolTips (showToolTip.isOn);


		foreach (ToolTip tool in Object.FindObjectsOfType<ToolTip> ()) {
			if (!tool.Ability) {
				tool.enabled = showToolTip.isOn;
			}
		}
	}

	public void toggleFastCast()
	{

		PlayerPrefs.SetInt ("FastCast", fastCast.isOn ? 1:0);
		if (UIManager.main) {
			UIManager.main.fastCast = fastCast.isOn;
		}
	}


	public void toggleAbilities()
	{

		GameSettings.setAbility (showAbility.isOn);

		foreach (UltTip tool in Object.FindObjectsOfType<UltTip> ()) {
			if (tool.Ability) {
				tool.enabled = showAbility.isOn;
			}
		}
	}

	public void setSimpleUI()
	{

		PlayerPrefs.SetInt ("SimpleUI", simpleToggle.isOn ? 1:0);

		foreach (GameObject obj in SimpleObjects) {
			if (obj) {
				obj.SetActive (!simpleToggle.isOn);
			}
		}
		foreach (Canvas obj in SimpleCanvas) {
			if (obj) {
				obj.enabled = !simpleToggle.isOn;
			}
		}
	}

	public void setSuperSimpleUI()
	{

		PlayerPrefs.SetInt ("SuperSimpleUI", ControlToggle.isOn ?1:0);
	

		foreach (GameObject obj in SuperSimpleObjects) {
			if (obj) {
				obj.SetActive (!ControlToggle.isOn);
			}
		}
	}


	public void healthBarList()
	{
		foreach (Selected sel in Object.FindObjectsOfType<Selected>()) {

			if (healthList.value == 0) {
				sel.setDisplayType( Selected.displayType.always);
			
			
	
			} else if (healthList.value == 1) {
				sel.setDisplayType(Selected.displayType.damaged);
			
	
			} else if (healthList.value == 2) {
				sel.setDisplayType(Selected.displayType.selected);

			} else {
			sel.setDisplayType( Selected.displayType.never);
			
			}
		
		}
	}

	public void setGameSpeed(Scrollbar theSlide)
	{

		GameSettings.gameSpeed = 1 +  ((theSlide.value - .5f)  *.4f);
		speedPercent.text = "("+(int)(GameSettings.gameSpeed * 100) + ")%";


	}

	public void resetScrollSpeed()
	{
        cam.ScrollSpeed = scrollSpeed.value * 700 + 100;
	}


	public Selected.displayType getDisplayType()
	{if (healthList.value == 0) {
			return Selected.displayType.always;
		


	} else if (healthList.value == 1) {
		return Selected.displayType.damaged;


	} else if (healthList.value == 2) {
		return Selected.displayType.selected;

	} else {
		return Selected.displayType.never;
	
	}}

}
