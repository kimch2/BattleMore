using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBattery  :Ability, AllySighted{

	public UnitManager manager;
	UnitStats myStats;

	public float shieldRate;
	DayexaShield mySHields;
	public LineRenderer myLine;
	public GameObject AuraThingy;
	public UnitStats currentTarget;
	public float range = 60;
	AugmentAttachPoint AugmentAttach;
	public List<DayexaShield> shieldList = new List<DayexaShield> ();


	// Use this for initialization
	void Start () {
		myType = type.passive;
		manager = GetComponent<UnitManager> ();
		mySHields = GetComponent<DayexaShield> ();
		myStats = GetComponent<UnitStats> ();
		manager.AddAllySighted (this);
		myLine.SetPosition (0, transform.position+ Vector3.up *7);
		myLine.SetPosition (1, transform.position+ Vector3.up *5);
		InvokeRepeating ("UpdateCharges", 1, 1);
		AugmentAttach = GetComponent<AugmentAttachPoint> ();
	}


	// Update is called once per frame
	void UpdateCharges () {

		if (!active || AugmentAttach.myAugment != null) {
			return;}

	
		if (myLineMover == null) {

			foreach (DayexaShield ds in shieldList) {
				if (!ds) {
					continue;
				}
				
				if (ds.myStats.currentEnergy < ds.myStats.MaxEnergy) {
						
					StartCoroutine (refill (ds.GetComponent<UnitStats> ()));
					break;

				}
			}
		}
	}


	public void stopRecharging()
	{
		currentTarget = null;

	}

	Coroutine myLineMover;
	IEnumerator refill(UnitStats target)
	{
		currentTarget = target;
		myLineMover =  StartCoroutine (MoveLine());
	
		while(currentTarget&& Vector3.Distance(target.transform.position, transform.position) < range && !target.atFullEnergy() && myStats.currentEnergy > shieldRate)
		{
			float amount = Mathf.Min (target.MaxEnergy - target.currentEnergy, shieldRate);
			amount = Mathf.Min (myStats.currentEnergy, amount);
			target.changeEnergy (amount);
			myStats.changeEnergy (-amount);
			mySHields.startRecharge (mySHields.RechargeDelay);
			yield return new WaitForSeconds (.7f);
		}
			
		myLine.SetPosition (1, transform.position+ Vector3.up *5);
		if (myLineMover != null) {
			StopCoroutine (myLineMover);
		}
		myLineMover = null;
		currentTarget = null;
		AuraThingy.SetActive (false);
		shieldList.RemoveAll (item => item == null);
	}

	IEnumerator MoveLine()
	{
		AuraThingy.SetActive (true);
		while (currentTarget && Vector3.Distance (currentTarget.transform.position, transform.position) < range) {
			myLine.SetPosition (1, currentTarget.transform.position + Vector3.up *3);
			AuraThingy.transform.position = currentTarget.transform.position + Vector3.up * 3;
			yield return null;
		}

		AuraThingy.SetActive (false);
		myLine.SetPosition (1, transform.position+ Vector3.up *5);
		myLineMover = null;
	}

	public  override continueOrder canActivate(bool showError){
		return new continueOrder ();
	}
	public override void Activate(){
	}
	public  override void setAutoCast(bool offOn)
	{
	}


	public void AllySpotted (UnitManager othermanager)
	{
		shieldList.RemoveAll (item => item == null);


			DayexaShield s = othermanager.gameObject.GetComponent<DayexaShield> ();
		if (s && !othermanager.gameObject.GetComponent<ShieldBattery>()) {
				shieldList.Add (s);
			}

	}

	public void allyLeft(UnitManager othermanager){
		DayexaShield s = othermanager.gameObject.GetComponent<DayexaShield> ();
		if (s) {
			shieldList.Remove (s);
		}
	}

}
