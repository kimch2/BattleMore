using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissileArmer :Ability,AllySighted{

	public UnitManager manager;


	public bool shields;
	public float shieldRate;
	public GameObject shieldglobe;
	public GameObject OverchargeBoost;

	public List<DayexaShield> shieldList = new List<DayexaShield> ();


	// Use this for initialization
	void Start () {

		manager = GetComponent<UnitManager> ();
		manager.AddAllySighted (this);
		InvokeRepeating ("UpdateCharges", 1, 1.8f);
	}


	// Update is called once per frame
	void UpdateCharges () {

		if (!active) {
			return;}

			if (shields) {
				if (shieldglobe) {


					foreach (DayexaShield ds in shieldList) {
					if (!ds) {
						continue;}
						if (ds.myStats.currentEnergy < ds.myStats.MaxEnergy) {
							GameObject obj = (GameObject)Instantiate (shieldglobe, this.transform.position, Quaternion.identity);
							if (ds) {
								obj.GetComponent<ShieldGlobe> ().setInfo (ds.gameObject, false);
							
							}
							break;
							
						}
			
					}
				}
			}

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

		if (shields) {
			DayexaShield s = othermanager.gameObject.GetComponent<DayexaShield> ();
			if (s) {
				shieldList.Add (s);
			}
		}
	}

	public void allyLeft(UnitManager othermanager){
		DayexaShield s = othermanager.gameObject.GetComponent<DayexaShield> ();
		if (s) {
			shieldList.Remove (s);
		}
	}
		
}
