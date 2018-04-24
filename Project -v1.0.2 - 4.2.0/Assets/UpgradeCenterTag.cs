using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCenterTag : MonoBehaviour {




	public void Dying()
	{
		GameObject.FindObjectOfType<DifficultyManager> ().UpgradeDied ();
	}

	// Use this for initialization
	void Start () {
		GameObject.FindObjectOfType<DifficultyManager> ().UpgradeCounter ();
	}

}
