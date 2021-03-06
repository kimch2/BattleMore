﻿using UnityEngine;
using System.Collections;

public class GameSettings {

	//public static float musicVolume = -1;
	//public static float masterVolume = -1;
	public static float gameSpeed = -1;


	public static float getUnitResponseFreq()
	{
		return PlayerPrefs.GetFloat ("UnitResponse",1);
	}
	public static float getBaseAlertFreq()
	{
		return PlayerPrefs.GetFloat ("BaseAlert",3);
	}

	public static void setUnitResponseFreq(float amount)
	{
		if (SelectedManager.main) {
			SelectedManager.main.unitResponseFrequancy = amount;
		}
		PlayerPrefs.SetFloat ("UnitResponse",amount);
	}
	public static void setBaseAlertFreq(float amount)
	{
		if (ErrorPrompt.instance) {
			ErrorPrompt.instance.setErrorFreq( amount);
		}
		PlayerPrefs.SetFloat ("BaseAlert",amount);
	}


	public static void setWaveWarning(bool active)
	{
		PlayerPrefs.SetInt ("WaveWarning", active ? 1 : 0);
	}
	public static bool getWaveWarning()
	{
		return PlayerPrefs.GetInt ("WaveWarning",1) == 1;
	}



	public static float getMusicVolume()
	{
		return PlayerPrefs.GetFloat ("MusicVolume", .5f);
	}

	public static void setMusicVolume(float amount)
	{
		PlayerPrefs.SetFloat ("MusicVolume", amount);
	}

	public static float getMasterVolume()
	{
		
			return PlayerPrefs.GetFloat ("MasterVolume", .5f);
	}

	public static void setMasterVolume(float amount)
	{PlayerPrefs.SetFloat ("MasterVolume", amount);
	}

	public static float inverseGameTime()
	{
		return 1 / gameSpeed;
	}


	public static void setToolTips(bool onOff)
	{
		if (onOff) {
			PlayerPrefs.SetInt ("ToolTip", 1);
		} else {
			PlayerPrefs.SetInt ("ToolTip", 0);
		}
	}

	public static bool getToolTips()
	{
		if (PlayerPrefs.GetInt("ToolTip", 1) == 1) {
			return true;
	
		} else {
			return false;
		}	
	}


	public static void setAbility(bool onOff)
	{
		if (onOff) {
			PlayerPrefs.SetInt ("AbilityTool", 1);
		} else {
			PlayerPrefs.SetInt ("AbilityTool", 0);
		}
	}

	public static bool getAbility()
	{
		if (PlayerPrefs.GetInt("AbilityTool", 1) == 1) {
			return true;

		} else {
			return false;
		}	
	}


}
