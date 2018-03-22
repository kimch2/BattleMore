using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurviveVictory : Objective {

	public MultiShotParticle myParticles;

	public GameObject QuakeBuilding;
	public float SurvivalTime;
	int pulsesUsed;
	float startTime;
	public AudioSource PulseSound;

	string rawObjectText;
	// Use this for initialization
	new void Start () {
		rawObjectText = description;
		startTime = Time.time;
		VictoryTrigger.instance.addObjective (this);
	


		InvokeRepeating ("UpdateObj", 1,1);
	}


	public void UpdateObj()
	{
		if (SurvivalTime < 1) {
			WaitFunction ();
			CancelInvoke ("UpdateObj");
		} else {
			SurvivalTime -= 1;
			description = rawObjectText + " " + Clock.convertToString (SurvivalTime);
			VictoryTrigger.instance.UpdateObjective (this);
		}
	}


	public void increaseWait ()
	{
		pulsesUsed++;
		SurvivalTime += 10;
	}

	void WaitFunction()
	{
		if (QuakeBuilding && myParticles) {
			StartCoroutine (endEffects ());
		} else {
			complete ();
		}
	}

	IEnumerator endEffects()
	{
		MainCamera.main.ShakeCamera (7, 20, .1f);
		MainCamera.main.setCutScene (QuakeBuilding.transform.position, 100);
		yield return new WaitForSeconds (.5f);
		myParticles.playEffect ();
		if (PulseSound) {
			PulseSound.Play ();
		}
		QuakeBuilding.GetComponentInChildren<Animator> ().SetTrigger("Pulse");
		yield return new WaitForSeconds (1f);
		myParticles.playEffect ();
		if (PulseSound) {
			PulseSound.Play ();
		}
		QuakeBuilding.GetComponentInChildren<Animator> ().SetTrigger("Pulse");
		yield return new WaitForSeconds (1f);
		myParticles.playEffect ();
		if (PulseSound) {
			PulseSound.Play ();
		}
		QuakeBuilding.GetComponentInChildren<Animator> ().SetTrigger("Pulse");
		yield return new WaitForSeconds (1f);
		myParticles.playEffect ();
		if (PulseSound) {
			PulseSound.Play ();
		}
		QuakeBuilding.GetComponentInChildren<Animator> ().SetTrigger("Pulse");
		yield return new WaitForSeconds (1f);
		complete ();


	}




}
