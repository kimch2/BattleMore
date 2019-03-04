using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class RaceUIManager : MonoBehaviour , ManagerWatcher{
	// Controls all UI Information regarding resources, supply, production, 
	// Recieves inputs for all F-Buttons commands, which can also be triggered by buttons childed under the UI objec this should be on.


	public RaceManager raceManager;

	public Text supply;
	string OneName;
	string TwoName;
	public bool runTabs;
	//tooltips are left open when they are deselected
	public List<GameObject> BuggedCans;
	public UiAbilityManager UIAbility;

	public Dropdown production; // Controls which Info panel to display - Production, Income, or Current total army

	public static RaceUIManager instance;

	private GameObject currentProdManager;
	public List<GameObject> dropdowns = new List<GameObject>();
	public List<Text> ultTexts;

	public GameObject ResourcePrefab;
	public Transform ResourceGrid;

	private Dictionary<ResourceType, Text> ResourceMap = new Dictionary<ResourceType, Text>();


	void Awake()
	{
		instance = this;
		raceManager = GameObject.FindObjectOfType<GameManager>().playerList[0];
	}

	// Use this for initialization
	void Start () {

		GameMenu.main.addDisableScript (this);
		if (raceManager == null) {

			raceManager = GameManager.main.activePlayer;
		}

		if (runTabs) {
			raceManager.addWatcher (this);

			if (raceManager.supplyMax >= raceManager.supplyCap) {
				supply.color = Color.white;
			}
			else if (raceManager.currentSupply < Mathf.Min(raceManager.supplyMax, raceManager.supplyCap) - 5  ) {
				supply.color = Color.green;
			} else if (raceManager.currentSupply >=  Mathf.Min(raceManager.supplyMax, raceManager.supplyCap) ) {
				supply.color = Color.red;
			} else {
				supply.color = Color.yellow;
			}
			supply.text = raceManager.currentSupply + "/" +  Mathf.Min(raceManager.supplyMax, raceManager.supplyCap);
			currentProdManager = dropdowns [1];
			chanageDropDown ();
		}


	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.F1)) {
			fOne ();
		} else if (Input.GetKeyDown (KeyCode.F2)) {
			fTwo ();	
		} else if (Input.GetKeyDown (KeyCode.F3)) {
			fThree ();
		} else if (Input.GetKeyDown (KeyCode.F4)) {
			fFour ();
		} else if (Input.GetKeyDown (KeyCode.F5)) {
			fFive ();
		} else if (Input.GetKeyDown (KeyCode.F6)) {
			fSix ();
		} else if (Input.GetKeyDown (KeyCode.F7)) {
			fSeven ();
		} else if (Input.GetKeyDown (KeyCode.F8)) {
			fEight ();
		} else if (Input.GetKeyDown (KeyCode.F9)) {
			fNine ();
		} else if (Input.GetKeyDown (KeyCode.F10)) {
			fTen ();
		} else if (Input.GetKeyDown (KeyCode.F11)) {
			fEleven ();
		} else if (Input.GetKeyDown (KeyCode.F12)) {
			fTwelve ();
		} else if (Input.GetKeyDown (KeyCode.P)) {
			production.value = 2;
			chanageDropDown ();
		}
		else if (Input.GetKeyDown (KeyCode.I)) {
			production.value = 1;
			chanageDropDown ();
		}
		else if (Input.GetKeyDown (KeyCode.U)) {
			production.value = 0;
			chanageDropDown ();
		} else if (Input.GetKeyDown (KeyCode.O)) {
			production.value = 3;
			chanageDropDown ();
		}
		else if (Input.GetKeyDown (KeyCode.L)) {
			production.value = 5;
			chanageDropDown ();
		}
	}



	public void chanageDropDown()
	{
		foreach (GameObject c in BuggedCans) {
			if (c.GetComponent<ToolTip> ()) {
				c.GetComponent<ToolTip> ().turnOff ();
			}
			foreach (ToolTip t in c.GetComponentsInChildren<ToolTip>()) {
				t.turnOff ();
			}
		if (currentProdManager) {
			currentProdManager.SetActive (false);
			if (currentProdManager.GetComponent<ToolTip> ()) {
				currentProdManager.GetComponent<ToolTip> ().toolbox.enabled = false;
			}
		}

		if (production.value == 0) {
			
			currentProdManager = dropdowns [0];
			currentProdManager.SetActive (true);
		}

		else if (production.value == 1) {
			
			currentProdManager = dropdowns [1];
			currentProdManager.SetActive (true);

		}
		else if (production.value == 2) {
			
			currentProdManager = dropdowns [2];
			currentProdManager.SetActive (true);

		}
		else if (production.value == 3) {
			
			currentProdManager = dropdowns [3];
			currentProdManager.SetActive (true);

		}
		
		else{
			currentProdManager.SetActive (false);
		}
		
		}
	}


	public void addResource(ResourceType resType, float startingAmount)
	{
		GameManager.main.getActivePlayer().addWatcher(this);
		GameObject obj = Instantiate<GameObject>(ResourcePrefab, ResourceGrid);
		obj.transform.SetSiblingIndex(obj.transform.parent.childCount - 2);
		obj.GetComponentInChildren<Image>().sprite = UnitEquivalance.getResourceInfo(resType).icon;
		ResourceMap.Add(resType, obj.GetComponentsInChildren<Text>()[0]);
		ResourceMap[resType].text = startingAmount + "";
	}

	
	public void updateResources(ResourceManager manager){

		foreach (ResourceTank t in manager.MyResources)
		{
			if (ResourceMap.ContainsKey(t.resType))
			{
				ResourceMap[t.resType].text = t.currentAmount + "";
			}
			else
			{
				addResource(t.resType, t.currentAmount);
				EconomyManager.main.AddResourceType(t.resType);
			}
		}
	}
	
	
	public void updateSupply( float current, float max){
		if (raceManager.supplyMax >= raceManager.supplyCap) {
			supply.color = Color.white;
		}

		else if (current < max - 5) {
			supply.color = Color.green;
		} else if (current >= max - 1) {
			supply.color = Color.red;
		} else {
			supply.color = Color.yellow;
		}
		supply.text = current + "/" + max;
	}
	
	public void updateUpgrades(){

	}

	public void fOne()
	{SelectedManager.main.globalSelect(0);
		UISoundManager.interfaceClick (true);}
	
	public void fTwo()
	{SelectedManager.main.globalSelect(1);
		UISoundManager.interfaceClick (true);}
	
	public void fThree()
	{SelectedManager.main.globalSelect(2);
		UISoundManager.interfaceClick (true);}
	
	public void fFour()
	{SelectedManager.main.globalSelect(3);
		UISoundManager.interfaceClick (true);}
	
	public void fFive()
	{raceManager.useAbilityOne ();
		UISoundManager.interfaceClick (true);}
	
	public void fSix()
	{raceManager.useAbilityTwo ();
		UISoundManager.interfaceClick (true);
		}
	
	public void fSeven()
	{raceManager.useAbilityThree ();
		UISoundManager.interfaceClick (true);
	}
	
	public void fEight()
	{raceManager.useAbilityFour ();
		UISoundManager.interfaceClick (true);
	}
	
	public void fNine()
	{SelectedManager.main.selectAllArmy ();
		UISoundManager.interfaceClick (true);
	}
	
	public void fTen()
	{SelectedManager.main.selectIdleWorker ();
		UISoundManager.interfaceClick (true);
	}
	
	public void fEleven()
	{	SelectedManager.main.selectAllBuildings ();
		UISoundManager.interfaceClick (true);
	}
	
	public void fTwelve()
	{


		SelectedManager.main.selectAllUnArmedTanks ();
		UISoundManager.interfaceClick (true);
	}



}
