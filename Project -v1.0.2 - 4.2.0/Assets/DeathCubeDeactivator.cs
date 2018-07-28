using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCubeDeactivator : Ability,EnemySighted{

	UnitManager manager;


	public MultiShotParticle myEffect;
	public GameObject chargeEffect;
	public GameObject effectOne;
	public GameObject effectTwo;

	public LineRenderer linerOne;
	public LineRenderer linerTwo;

	UnitManager enemyOne = null;
	UnitManager enemyTwo = null;

	// Use this for initialization
	void Start () {
		myType = type.passive;
		manager = GetComponent<UnitManager> ();
		manager.AddEnemySighted (this);

	}


	public void EnemySpotted (UnitManager otherManager)
	{
		if (otherManager.UnitName == "Death Cube") {
			otherManager.cMover.changeSpeed (-.5f,0,false,this);
			otherManager.getUnitStats ().armor -= 99;

			if (enemyOne == null) {
				enemyOne = otherManager;
				StartCoroutine (followTargetOne ());

			} else {
				enemyTwo = otherManager;
				StartCoroutine (followTargetTwo ());
			}
		}
	}

	public void enemyLeft (UnitManager otherManager)
	{
		if (otherManager.UnitName == "Death Cube") {
			otherManager.getUnitStats ().armor += 99;
			otherManager.cMover.removeSpeedBuff(this);
			if (enemyOne == otherManager) {
				enemyOne = null;
			} else if (enemyTwo == otherManager) {
				enemyTwo = null;
			}
		}
	}



	public override void setAutoCast(bool offOn){
	}


	IEnumerator followTargetOne()
	{
		effectOne.SetActive (true);

		while (enemyOne != null) {
			effectOne.transform.position = enemyOne.transform.position;
			linerOne.SetPosition (0, transform.position+ Vector3.up *9.6f);
			linerOne.SetPosition (1, enemyOne.transform.position);
			yield return null;
		}


		if (effectOne) {
			effectOne.SetActive (false);
		}
		linerOne.SetPosition (0, Vector3.zero);
		linerOne.SetPosition (1, Vector3.zero);

	}

	IEnumerator followTargetTwo()
	{
		effectTwo.SetActive (true);
		while (enemyTwo != null) {
			effectTwo.transform.position = enemyTwo.transform.position;
			linerTwo.SetPosition (0, transform.position + Vector3.up *9.6f);
			linerTwo.SetPosition (1, enemyTwo.transform.position);
			yield return null;
		}

		if (effectTwo) {
			effectTwo.SetActive (false);
		}
		linerTwo.SetPosition (0, Vector3.zero);
		linerTwo.SetPosition (1, Vector3.zero);
	
	}




	override
	public continueOrder canActivate (bool showError)
	{

		continueOrder order = new continueOrder ();
		return order;
	}

	override
	public void Activate()
	{
	}




}
