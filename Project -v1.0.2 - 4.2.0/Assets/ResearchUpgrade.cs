using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResearchUpgrade: UnitProduction, Upgradable{

		private Selected mySelect;
		private bool researching;
		private float timer =0;

		private int currentUpgrade = 0;
	private HealthDisplay HD;
	private BuildManager buildMan;
		public List<Upgrade> upgrades;

	public bool researchingElsewhere;

	public bool requiresAddon;
	bool hasAddon;

		//public float buildTime;
		// Use this for initialization
	new void Awake () {
		base.Awake();
		
		myType = type.activated;
		mySelect = GetComponent<Selected> ();
		buildMan = GetComponent<BuildManager> ();
		HD = GetComponentInChildren<HealthDisplay>();

	}


		// Update is called once per frame
		void Update () {

			if (researching) {
			
				
				timer -= Time.deltaTime * buildRate;
				mySelect.updateCoolDown (1 - timer/buildTime);
				if(timer <=0)
					{mySelect.updateCoolDown (0);
					ErrorPrompt.instance.ResearchComplete(upgrades [currentUpgrade].Name , this.transform.position);

					myManager.myRacer.stopBuildingUnit (this);
					HD.stopBuilding ();
					buildMan.unitFinished (this);
					researching = false;
				myManager.myRacer.addUpgrade (upgrades[currentUpgrade], myManager.UnitName);
					if (requiresAddon && hasAddon || !requiresAddon) {
						active = true;
					}
					RaceManager.upDateUI ();
					//createUnit();
				}
			}

		}

	public override void DeQueueUnit()
	{
		//Debug.Log (Name + " is Dqueuing");

		researchingElsewhere = false;
		object[] temp = new object[2];
		temp [0] = false;
		myCost.refundCost ();
		if (requiresAddon && hasAddon || !requiresAddon) {
			active = true;
			temp [0] = true;
		}
			
		temp [1] = upgrades[currentUpgrade];

		//Debug.Log ("Passing out " + temp [0]);
		foreach (ResearchUpgrade ru in GameObject.FindObjectsOfType<ResearchUpgrade>()) {
			if (ru == this) {
				continue;}
			ru.commence (temp);
		}


		if (mySelect.IsSelected) {
			RaceManager.updateActivity ();
		}
	}

	public void attachAddon()
	{
		//Debug.Log ("Has addon");
		hasAddon = true;
		if (requiresAddon && !researchingElsewhere) {
			active = true;
			if (mySelect.IsSelected) {
				RaceManager.updateActivity ();
			}
		}
	}

	public void removeAddon()
	{//Debug.Log ("No addon");
		hasAddon = false;
		if (requiresAddon) {
			active = false;
			if (mySelect.IsSelected) {
				RaceManager.updateActivity ();
			}
		}
	}



		override
	public continueOrder canActivate (bool showError)
		{continueOrder order = new continueOrder();

		if (researching || !myCost.canActivate (this, order, showError)) {
			order.canCast = false;
			order.nextUnitCast = false;
				return order;}
		if (active == false) {
			order.canCast = false;
		}
	
		return order;
		}

		override
		public void Activate()
		{

			if (myCost.canActivate (this)) {
			
			researchingElsewhere = true;
			object[] temp = new object[2];
			temp [0] = false;
			temp [1] = upgrades[currentUpgrade];

			foreach (UnitManager manage in myManager.myRacer.getUnitList()[myManager.UnitName])
			{
				foreach(Ability ab in manage.abilityList)
				{
					if (ab is ResearchUpgrade)
					{
						if (ab != this)
						{
							((ResearchUpgrade)ab).commence(temp);
						}
					}
				}
			}

			if (buildMan.buildUnit (this)) {

				timer = buildTime;
				//Debug.Log ("Name " + Name + "   is false" );
			

				active = false;
				if (mySelect.IsSelected) {
					RaceManager.updateActivity ();
				}

				myCost.payCost ();
				myCost.resetCoolDown ();
				//Debug.Log (Name + " is Activating");
			}
			}

		}

	public override void setAutoCast(bool offOn){}


	public void commence(object[] incoming)
	{

		if (Name == ((Upgrade)incoming[1]).Name) {

			researchingElsewhere = !(bool)incoming[0];
			active = (bool)incoming[0];
		
		}
	}




	public void researched (Upgrade otherUpgrade)
	{
	//	Debug.Log (Name +"   "+ otherUpgrade.Name);
		if (Name == otherUpgrade.Name) {

			if (upgrades.Count > currentUpgrade + 1) {
				currentUpgrade++;

				//this is all here for replaceable or scaling upgrades
				iconPic = upgrades [currentUpgrade].iconPic;
                buildTime = upgrades[currentUpgrade].buildTime;
				Name = upgrades [currentUpgrade].Name;
				myCost = upgrades [currentUpgrade].myCost;
				Descripton = upgrades [currentUpgrade].Descripton;
				if (requiresAddon && hasAddon || !requiresAddon) {
					this.active = true;
				}
		

				if (mySelect && mySelect.IsSelected) {
					RaceManager.updateActivity();
				}

			} else {
				if (myManager.abilityList.Count > myManager.abilityList.IndexOf (this)) {
					myManager.abilityList [myManager.abilityList.IndexOf (this)] = null;
				}
			
				foreach (Upgrade up in upgrades) {

					Destroy (up);
				}

				Destroy (this);
			}
		}
	}

	public void UpdateAvailable()
	{
		foreach (ResearchUpgrade ru in GameObject.FindObjectsOfType<ResearchUpgrade>()) {
			if (ru == this) {
			
				continue;}

			if (ru.researchingElsewhere) {
				if (Name ==ru.upgrades[ru.currentUpgrade].Name) {


						researchingElsewhere = true;
					active = ru.active;

					}
			}
		
		}

	}


	public override float getProgress ()
	{return (1 - timer/buildTime);}


	public override void startBuilding(){ 
		timer = buildTime;
		HD.loadIMage(iconPic);
		myManager.myRacer.buildingUnit (this);
		myCost.resetCoolDown ();
		myManager.myRacer.commenceUpgrade (false, upgrades [currentUpgrade], myManager.UnitName);

		researching = true;
	}

	public override void cancelBuilding(){
		//Debug.Log (Name + " is canceling");
		HD.stopBuilding ();
		mySelect.updateCoolDown (0);
		myManager.myRacer.stopBuildingUnit (this);
		timer = 0;
		researching = false;
		//myCost.refundCost ();
		if (requiresAddon && hasAddon || !requiresAddon) {
			active = true;
		}

		myManager.myRacer.commenceUpgrade ((requiresAddon && hasAddon || !requiresAddon), upgrades [currentUpgrade], myManager.UnitName);
		if (mySelect.IsSelected) {
			RaceManager.updateActivity ();
		}
		//foreach (Transform obj in this.transform) {

			//obj.SendMessage ("DeactivateAnimation",SendMessageOptions.DontRequireReceiver);
		//}
	}


}
