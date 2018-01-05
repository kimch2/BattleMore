﻿
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UiAbilityManager : MonoBehaviour {

	public AudioClip ButtonPress;
	AudioSource audSrc;
	RaceManager racer;

	//public List<GameObject> UIButtons = new List<GameObject>();
	Color disabledColor = new Color(.5f,0,0,1);
	public List<Button> quickButtons = new List<Button>();
	public List<AbilityBox> quickAbility = new List<AbilityBox>();
	bool showAllUnits = false;
	public GameObject GridObject;
	public Image gridButton;
	public Sprite showallGrid;
	public Sprite showSomeGrid;


	protected Lean.LeanPool myIconPool;

	public static UiAbilityManager main;
	[System.Serializable]
	public struct buttonSet{
		public GameObject QButton;
		public Slider QSlide;
		public Text Qtext;
		public Image QAuto;
		public Image Qmoney;

		public GameObject WButton;
		public Slider WSlide;
		public Text Wtext;
		public Image WAuto;
		public Image Wmoney;

		public GameObject EButton;
		public Slider ESlide;
		public Text Etext;
		public Image EAuto;
		public Image Emoney;

		public GameObject RButton;
		public Slider RSlide;
		public Text Rtext;
		public Image RAuto;
		public Image Rmoney;
	}
	public GameObject topDividerBar;
	public GameObject bottomBar;
	public List<buttonSet> certainButtons = new List<buttonSet> ();

	public UnitCardCreater cardCreator;
	public Canvas OreCanvas;
	private int currentX;
	private int currentY;

	public GameObject buttonTemplate;

	public List<StatsUI> Stats = new List<StatsUI> ();

	public List<GameObject> IconStartPoints = new List<GameObject> ();
	//Key - Icon, Value - Unit
	private Dictionary<GameObject, GameObject> unitIcons = new Dictionary<GameObject, GameObject> ();
 	private SelectedManager selectMan;

	private Page currentPage;

	private float nextActionTime;


	void Awake()
	{
		main = this;
		if (PlayerPrefs.GetInt ("GridMode", 1) == 1) {
			toggleGridMode ();}
	}

	public void pressButton(int n)
	{
		var pointer = new PointerEventData (EventSystem.current);
		ExecuteEvents.Execute (quickButtons [n].gameObject, pointer, ExecuteEvents.pointerEnterHandler);
		ExecuteEvents.Execute (quickButtons [n].gameObject, pointer, ExecuteEvents.pointerDownHandler);
		StartCoroutine (delayedClick(n));

	}

	IEnumerator delayedClick(int n)
	{var pointer = new PointerEventData (EventSystem.current);
		CostBox.instance.turnOff ();
		yield return new WaitForSeconds (.1f);
		ExecuteEvents.Execute (quickButtons [n].gameObject, pointer, ExecuteEvents.pointerClickHandler);
		ExecuteEvents.Execute (quickButtons [n].gameObject, pointer, ExecuteEvents.pointerExitHandler);


	}


	// Use this for initialization
	void Start () {
		racer = GameManager.getInstance ().activePlayer;
		IconStartPoints [0].SetActive (false);
		IconStartPoints [1].SetActive (false);
		IconStartPoints [2].SetActive (false);
		audSrc = GameObject.FindObjectOfType<ExpositionDisplayer> ().GetComponent<AudioSource> ();
		GameMenu.main.addDisableScript (this);
		nextActionTime = Time.time;
		selectMan = SelectedManager.main; 
		myIconPool = Lean.LeanPool.getSpawnPool (buttonTemplate);
	}
	
	// Update is called once per frame
	void Update () {


		if (Input.GetKeyUp (KeyCode.Q)) {
			pressButton (0);
		} else if (Input.GetKeyUp (KeyCode.W)) {
			pressButton (1);
		} else if (Input.GetKeyUp (KeyCode.E)) {
			pressButton (2);
		} else if (Input.GetKeyUp (KeyCode.R)) {
			pressButton (3);
		} else if (Input.GetKeyUp (KeyCode.A)) {
			pressButton (4);
		} else if (Input.GetKeyUp (KeyCode.S)) {
			pressButton (5);
		} else if (Input.GetKeyUp (KeyCode.D)) {
			pressButton (6);
		} else if (Input.GetKeyUp (KeyCode.F)) {
			pressButton (7);
		} else if (Input.GetKeyUp (KeyCode.Z)) {
			pressButton (8);
		} else if (Input.GetKeyUp (KeyCode.X)) {
			pressButton (9);
		} else if (Input.GetKeyUp (KeyCode.C)) {
			pressButton (10);
		} else if (Input.GetKeyUp (KeyCode.V)) {
			pressButton (11);
		} 
			

		if (Time.time < nextActionTime) {
			return;
		} 
		nextActionTime += .08f;



		if (currentPage == null) {
			return;
		}
		int AbilityX = 0;
	

		for (int j = 0; j < 3; j++) {
			if (currentPage.rows [j] == null) {
				continue;
			}
			if (j > 0 && currentPage.rows [j] != currentPage.rows [j]) {
				AbilityX = 0;
			}
			UnitManager man= null;
			foreach(RTSObject obj in currentPage.rows[j]){

				if(obj){
					man = obj.gameObject.GetComponent<UnitManager> ();

					break;}
			}
			if (!man) {
				continue;
			}
			for (int m = 0; m < man.abilityList.Count / 4 + 1; m++) {		

					if (man.abilityList.Count > AbilityX * 4) {
						Ability abilA = man.abilityList [ AbilityX * 4];
						if (abilA != null && abilA.myCost) {
							if (abilA.myCost.cooldown != 0) {

							setButtonCooldown (certainButtons [j].QSlide,AbilityX * 4, j);
							}
						else {
							certainButtons [j].QSlide.gameObject.SetActive (false);
						}
							certainButtons [j].Qmoney.enabled = abilA.myCost.ResourceOne > racer.ResourceOne;
						}else {
							certainButtons [j].Qmoney.enabled = false;
							certainButtons [j].QSlide.gameObject.SetActive (false);
						}
					}
					if (man.abilityList.Count >1+ AbilityX * 4) {
						Ability abilB = man.abilityList [1 + AbilityX * 4];
					if (abilB != null && abilB.myCost) {

						if (abilB.myCost.cooldown != 0) {

							setButtonCooldown (certainButtons [j].WSlide, 1 + AbilityX * 4, j);
						}
						else {
							certainButtons [j].WSlide.gameObject.SetActive (false);
						}
						certainButtons [j].Wmoney.enabled = abilB.myCost.ResourceOne > racer.ResourceOne;

					} else {
						certainButtons [j].Wmoney.enabled = false;
						certainButtons [j].WSlide.gameObject.SetActive (false);
					}
					}
					if (man.abilityList.Count > 2 + AbilityX * 4) {
						Ability abilC = man.abilityList [2 + AbilityX * 4];
						if (abilC != null && abilC.myCost) {
					//	Debug.Log ("Cooldown " +abilC.myCost.cooldown);
							if (abilC.myCost.cooldown != 0) {

							setButtonCooldown (certainButtons [j].ESlide, 2 + AbilityX * 4, j);

							}
						else {
							certainButtons [j].ESlide.gameObject.SetActive (false);
						}
						certainButtons [j].Emoney.enabled = abilC.myCost.ResourceOne > racer.ResourceOne;
					}else {
						certainButtons [j].Emoney.enabled = false;
						certainButtons [j].ESlide.gameObject.SetActive (false);
					}
					}
			
					if (man.abilityList.Count > 3 + AbilityX * 4) {
					Ability abilD = man.abilityList [3 + AbilityX * 4];
					if (abilD != null && abilD.myCost) {

						if (abilD.myCost.cooldown != 0) {

							setButtonCooldown (certainButtons [j].RSlide, 3 + AbilityX * 4, j);

						} else {
							certainButtons [j].RSlide.gameObject.SetActive (false);
						}
						certainButtons [j].Rmoney.enabled = abilD.myCost.ResourceOne > racer.ResourceOne;
					}else {
						certainButtons [j].Rmoney.enabled = false;
						certainButtons [j].RSlide.gameObject.SetActive (false);
					}


					}

			}
			AbilityX++;
		}

	
	}


	public void toggleGridMode()
	{
		showAllUnits = !showAllUnits;
		if (showAllUnits) {
			gridButton.sprite = showallGrid;
		} else {
			gridButton.sprite = showSomeGrid;
		}
		PlayerPrefs.SetInt ("GridMode", showAllUnits ? 1:0);
		if (currentPage != null) {
			loadUI (currentPage );
		}
	}

	void setButtonCooldown(Slider slide, int abilityNum, int row)
	{
		float maxA = 0;
		foreach (RTSObject obj in currentPage.rows [row]) {
		//	if (obj.getUnitManager ().abilityList [abilityNum].active) {
				float n = obj.getUnitManager ().abilityList [abilityNum].myCost.cooldownProgress ();


				if (n > maxA) {
					maxA = n;
					if (n > .99f) {
						break;
					}
				}
			//}
		}

		slide.value = maxA;
		slide.gameObject.SetActive (maxA < .98);	
	}

	public void IconClick(GameObject obj)
	{

		if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl)) {
			GameObject temp = unitIcons [obj];
			selectMan.DeselectAll ();
			selectMan.AddObject (temp.GetComponent<UnitManager> ());

			selectMan.CreateUIPages (0);
		} else if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl)) {

			selectMan.DeselectObject (unitIcons [obj].GetComponent<UnitManager> ());
			selectMan.CreateUIPages (0);
		
		} else if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl)) {
			selectMan.selectAllUnitType (unitIcons [obj].GetComponent<UnitManager> ());
		
		} else {selectMan.DeSelectAllUnitType (unitIcons [obj].GetComponent<UnitManager> ());
		}
	}

	public void clearPage()
	{
		currentPage = null;

		foreach (StatsUI obj in Stats) {
			if (obj) {
				obj.clear ();
			}
		}

		if (topDividerBar) {
			topDividerBar.SetActive (false);
		}
		if (bottomBar) {
			bottomBar.SetActive (false);
		}

		try{
		foreach (buttonSet obj in certainButtons) {
			
				obj.QButton.SetActive (false);
				obj.WButton.SetActive (false);
				obj.EButton.SetActive (false);
				obj.RButton.SetActive (false);

			}}catch(Exception){
		}

		foreach (KeyValuePair< GameObject, GameObject > del in unitIcons) {
			myIconPool.FastDespawn (del.Key);
			//Destroy (del.Key);
		}

		unitIcons.Clear ();

	

			currentPage = null;
		cardCreator.turnOff ();

	}




	public void loadUI(Page uiPage)
	{
		IconStartPoints [0].SetActive (false);
		IconStartPoints [1].SetActive (false);
		IconStartPoints [2].SetActive (false);
		currentPage = uiPage;
		// Clear old info in the buttons and stats
		foreach (StatsUI obj in Stats) {
			obj.clear ();
		}
		topDividerBar.SetActive (true);
		bottomBar.SetActive (true);
		int n = 0;

		foreach (buttonSet obj in certainButtons) {
			obj.QButton.SetActive (false);
			obj.WButton.SetActive (false);
			obj.EButton.SetActive (false);
			obj.RButton.SetActive (false);
		}



		foreach (KeyValuePair< GameObject, GameObject > del in unitIcons) {
			myIconPool.FastDespawn (del.Key);
			//Destroy (del.Key);
		}

		unitIcons.Clear ();

		int totalUnit = 0;
		//Set divider bars for how many abilities the units have.
		for(int j = 0; j < 3; j ++){
			if (uiPage.rows [j] != null) {
				if (j != 0) {
					if (uiPage.rows [j] == uiPage.rows [j - 1] ) {
						if (j == 1) {
							
							topDividerBar.SetActive (false);
						} else if (j == 2) {
							
							bottomBar.SetActive (false);}
						continue;
					}
				}
						totalUnit += uiPage.rows [j].Count;
			}
		}

		if (totalUnit == 0) {
			currentPage = null;
		
			topDividerBar.SetActive (false);

			bottomBar.SetActive (false);
			cardCreator.gameObject.GetComponent<Canvas> ().enabled = false;
			return;
		}

		else if (totalUnit > 1  || (selectMan.ActiveObjectList().Count > 1 && showAllUnits)) {
			OreCanvas.enabled = false;
			cardCreator.gameObject.GetComponent<Canvas> ().enabled = false;
			if (totalUnit == 0) {
				topDividerBar.SetActive (false);

				bottomBar.SetActive (false);
				return;
			}
		} else {

			cardCreator.gameObject.GetComponent<Canvas> ().enabled = true;
		}

		for(int j = 0; j < 3; j ++){

			if (uiPage.rows [j] == null) {
				
				continue;
			}


			if (j >0 && uiPage.rows [j] == uiPage.rows[j-1]) {

				continue;
			}


			n = uiPage.rows[j][0].AbilityStartingRow;


			//Sets the unit's stats and count
			UnitManager man = uiPage.rows[j][0].gameObject.GetComponent<UnitManager> ();
			Stats[n].GetComponent<StatsUI> ().loadUnit (man,uiPage.rows[j].Count, man.UnitName);
			IconStartPoints [j].SetActive (true);
		// fill the icon panel
		
			if (totalUnit > 1 || (selectMan.ActiveObjectList().Count > 1 && showAllUnits)) {

				if (showAllUnits) {
					if (j == 0) {

						List<Page> currentPages = selectMan.getPageList ();
						for (int e = 0; e < currentPages.Count; e++) {
					
							for (int p = 0; p < currentPages[e].rows.Length; p++) {

								if (currentPages[e].rows [p] != null) {
									if (p == 0 ||currentPages[e].rows [p] != currentPages[e].rows [p - 1]) {

										for (int x = 0; x < currentPages[e].rows [p].Count; x++) {

											GameObject unit = myIconPool.FastSpawn (Vector3.zero, Quaternion.identity, GridObject.transform);
											//GameObject unit = (GameObject)Instantiate (buttonTemplate, Vector3.zero, Quaternion.identity, GridObject.transform);
										
											Transform icontransform = unit.transform.Find ("UnitIconTemplate");
											icontransform.GetComponent<UnitIconInfo> ().setInfo (currentPages[e].rows [p] [x].gameObject); //.myUnit = uiPage.rows [j] [k].gameObject;
											icontransform.GetComponent<Button> ().onClick.AddListener (delegate() {
												IconClick (unit);
											});

											unitIcons.Add (unit, currentPages[e].rows [p] [x].gameObject);
											currentPages[e].rows [p] [x].gameObject.GetComponent<Selected> ().setIcon (unit);
										}
									}
								}
							}
						}
					}
				}
				else{
					if (j == 0 || uiPage.rows [j] != uiPage.rows [j - 1]) {
						int picCount = Mathf.Min (uiPage.rows [j].Count, 18);
						int separation = 59;

						if (uiPage.rows [j].Count > 14) {
							separation = Mathf.Max (15, 558 / picCount);
						}
				
						int currentX = 140;
						for (int k = 0; k < picCount; k++) {

							Vector3 pos = IconStartPoints [j].transform.position;
							pos.x += currentX * this.transform.localScale.x;
							GameObject unit = myIconPool.FastSpawn (Vector3.zero, Quaternion.identity);
							//GameObject unit = (GameObject)Instantiate (buttonTemplate);
							Transform icontransform = unit.transform.Find ("UnitIconTemplate");
					
							icontransform.GetComponent<UnitIconInfo> ().setInfo (uiPage.rows [j] [k].gameObject); //.myUnit = uiPage.rows [j] [k].gameObject;
							unit.transform.localScale = this.transform.localScale;
					
							unit.transform.rotation = this.transform.rotation;
							unit.transform.SetParent (topDividerBar.transform.parent);

							unit.transform.position = pos;

							//unit.transform.FindChild("UnitIconTemplate").GetComponent<Image> ().sprite = uiPage.rows [j] [k].gameObject.GetComponent<UnitStats> ().Icon;
					
							currentX += separation;
							icontransform.GetComponent<Button> ().onClick.AddListener (delegate() {
								IconClick (unit);
							});
					
							unitIcons.Add (unit, uiPage.rows [j] [k].gameObject);
							uiPage.rows [j] [k].gameObject.GetComponent<Selected> ().setIcon (unit);
						}
					}
				}
			} else {
	
				if (uiPage.rows [j] != null) {

					cardCreator.CreateCard (uiPage.rows [j][0]);
				}
			}
			//Set up the command card
			int AbilityX = 0;
	
			for (int m = 0; m < man.abilityList.Count / 4 +1; m++) {


					if(man.abilityList.Count > AbilityX * 4){
						if(man.abilityList [AbilityX * 4] !=null){
						Transform trans = certainButtons [n].QButton.transform;//.Find("QButton");


				
						certainButtons [j+ AbilityX ].QSlide.gameObject.SetActive (false);
						trans.gameObject.SetActive (true);
						trans.GetComponent<Image> ().sprite = man.abilityList [AbilityX * 4].iconPic;
						trans.GetComponent<AbilityBox> ().myAbility = man.abilityList [ AbilityX * 4];

							ColorBlock cb= trans.GetComponent<Button> ().colors;
							
						bool active = false;
						foreach (RTSObject obj in currentPage.rows [j]) {
							UnitManager Uman = obj.gameObject.GetComponent<UnitManager> ();

							if (!Uman.Silenced() && !Uman.Stunned()) {
								active = (Uman.abilityList [AbilityX * 4].active);
							}
							if (active) {
								break;}
							}
					
						if (active) {
								cb.disabledColor = Color.white;
							if(trans && trans.GetComponent<Button> ())
								trans.GetComponent<Button> ().interactable = true;
							} else {
							cb.disabledColor =disabledColor;
								trans.GetComponent<Button> ().interactable = false;

							}

						if (man.abilityList [AbilityX * 4].getMyType()== Ability.type.passive) {
							certainButtons [j+ AbilityX].Qtext.enabled = false;
							cb.disabledColor = Color.white;
							trans.GetComponent<Button> ().interactable = false;
						} else {
							certainButtons [j+ AbilityX].Qtext.enabled = true;
						}

						trans.GetComponent<Button> ().colors = cb;

						trans.Find ("AutoCast").GetComponent<Image> ().enabled = man.abilityList [0 + AbilityX * 4].canAutoCast;

						certainButtons [j+ AbilityX].QAuto.enabled = man.abilityList [0 + AbilityX * 4].autocast;

						Text charger = trans.Find ("Charge1").GetComponent<Text> ();
						if (man.abilityList [AbilityX * 4].chargeCount > -1) {
						
							int totalCharge = 0;
							foreach (RTSObject rts in  uiPage.rows[j]) {
								totalCharge += rts.abilityList [AbilityX * 4].chargeCount;
							}
								
							charger.text =  "" +totalCharge;//+man.abilityList [0 + AbilityX * 4].chargeCount;
						} else {
							charger.text = "";
							}
						certainButtons [j + AbilityX].Qmoney.enabled = false;
					}
					}
			
					if(man.abilityList.Count >1+( AbilityX * 4)){
						if (man.abilityList [1 + AbilityX * 4] != null) {

					
						certainButtons [j+ AbilityX].WSlide.gameObject.SetActive (false);


						Transform trans = certainButtons [n].WButton.transform;//.Find("WButton");;

						trans.gameObject.SetActive (true);
						trans.GetComponent<Image> ().sprite = man.abilityList [1 + AbilityX * 4].iconPic;
						trans.GetComponent<AbilityBox> ().myAbility = man.abilityList [1 + AbilityX * 4];

							ColorBlock cb= trans.GetComponent<Button> ().colors;

						bool active = false;
						foreach (RTSObject obj in currentPage.rows [j]) {
							UnitManager Uman = obj.gameObject.GetComponent<UnitManager> ();

							if (!Uman.Silenced() && !Uman.Stunned()) {
								active = (Uman.abilityList [1+AbilityX * 4].active);
							}
							if (active) {
								break;}
						}



						//if (man.abilityList [AbilityX * 4].active) {
						if (active) {

							
								cb.disabledColor = Color.white;
								trans.GetComponent<Button> ().interactable = true;
							} else {
							cb.disabledColor = disabledColor;
								trans.GetComponent<Button> ().interactable = false;

							}

						if (man.abilityList [1 + AbilityX * 4].getMyType() == Ability.type.passive) {
							certainButtons [j+ AbilityX].Wtext.enabled = false;
						
							cb.disabledColor = Color.white;
							trans.GetComponent<Button> ().interactable = false;
						} else {
							certainButtons [j+ AbilityX].Wtext.enabled = true;
						}
							trans.GetComponent<Button> ().colors = cb;


						trans.Find ("AutoCast").GetComponent<Image> ().enabled = man.abilityList [1 + AbilityX * 4].canAutoCast;
						certainButtons [j + AbilityX].WAuto.enabled = man.abilityList [1 + AbilityX * 4].autocast;

							Text charger = trans.Find ("Charge2").GetComponent<Text> ();
							if (man.abilityList [1 + AbilityX * 4].chargeCount > -1) {
								
		

								int totalCharge = 0;
								foreach (RTSObject rts in  uiPage.rows[j]) {
									totalCharge += rts.abilityList [1 + AbilityX * 4].chargeCount;
								}

								charger.text =  "" +totalCharge;//+man.abilityList [1 + AbilityX * 4].chargeCount;

							//charger.text = "" + man.abilityList [1 + AbilityX * 4].chargeCount;
							} else {
								charger.text = "";
							}
						}
					certainButtons [j + AbilityX].Wmoney.enabled = false;
					}




					if(man.abilityList.Count > 2+(AbilityX * 4)){
						if (man.abilityList [2 +AbilityX * 4] != null) {

				
						certainButtons [j+ AbilityX].ESlide.gameObject.SetActive (false);


						Transform trans = certainButtons [n].EButton.transform;//.Find("EButton");;

						trans.gameObject.SetActive (true);
						trans.GetComponent<Image> ().sprite = man.abilityList [2 + AbilityX * 4].iconPic;
							trans.GetComponent<AbilityBox> ().myAbility = man.abilityList [2 + AbilityX * 4];

							ColorBlock cb= trans.GetComponent<Button> ().colors;
				

						bool active = false;
						foreach (RTSObject obj in currentPage.rows [j]) {
							UnitManager Uman = obj.gameObject.GetComponent<UnitManager> ();

							if (!Uman.Silenced() && !Uman.Stunned()) {
								active = (Uman.abilityList [2 + AbilityX * 4].active);
							}
							if (active) {
								break;}
						}

						if (active) {


								cb.disabledColor = Color.white;
								trans.GetComponent<Button> ().interactable = true;
							} else {
							cb.disabledColor =disabledColor;
								trans.GetComponent<Button> ().interactable = false;

							}

						if (man.abilityList [2 + AbilityX * 4].getMyType() == Ability.type.passive) {
							certainButtons [j+ AbilityX].Etext.enabled = false;
							cb.disabledColor = Color.white;
							trans.GetComponent<Button> ().interactable = false;
						} else {
							certainButtons [j+ AbilityX].Etext.enabled = true;
						}
							trans.GetComponent<Button> ().colors = cb;

						trans.Find ("AutoCast").GetComponent<Image> ().enabled = man.abilityList [2 + AbilityX * 4].canAutoCast;
						certainButtons [j+ AbilityX].EAuto.enabled = man.abilityList [2+ AbilityX * 4].autocast;


							Text charger = trans.Find ("Charge3").GetComponent<Text> ();
							if (man.abilityList [2 + AbilityX * 4].chargeCount > -1) {

							int totalCharge = 0;
							foreach (RTSObject rts in  uiPage.rows[j]) {
								totalCharge += rts.abilityList [2 + AbilityX * 4].chargeCount;
							}

							charger.text =  "" +totalCharge;
								//charger.text = "" + man.abilityList [2 + AbilityX * 4].chargeCount;
							} else {
								charger.text = "";
							}
						}
					certainButtons [j + AbilityX].Emoney.enabled = false;
					}

					if(man.abilityList.Count >3+( AbilityX * 4)){
						if (man.abilityList [3 +AbilityX * 4] != null) {

				
						certainButtons [j+ AbilityX].RSlide.gameObject.SetActive (false);


						Transform trans = certainButtons [n].RButton.transform;//.Find("RButton");;
	
						trans.gameObject.SetActive (true);
						trans.GetComponent<Image> ().sprite = man.abilityList [3 + AbilityX * 4].iconPic;
						trans.GetComponent<AbilityBox> ().myAbility = man.abilityList [3 + AbilityX * 4];

							ColorBlock cb= trans.GetComponent<Button> ().colors;
							

						bool active = false;
						foreach (RTSObject obj in currentPage.rows [j]) {
							UnitManager Uman = obj.gameObject.GetComponent<UnitManager> ();

							if (!Uman.Silenced() && !Uman.Stunned()) {
								active = (Uman.abilityList [3 +AbilityX * 4].active);
							}
							if (active) {
								break;}
						}


						if (active) {

								cb.disabledColor = Color.white;
								trans.GetComponent<Button> ().interactable = true;
							} else {
							cb.disabledColor = disabledColor;
								trans.GetComponent<Button> ().interactable = false;

							}

						if (man.abilityList [3 + AbilityX * 4].getMyType() == Ability.type.passive) {
							certainButtons [j+ AbilityX].Rtext.enabled = false;
							cb.disabledColor = Color.white;
							trans.GetComponent<Button> ().interactable = false;
						} else {
							certainButtons [j+ AbilityX].Rtext.enabled = true;
						}
							trans.GetComponent<Button> ().colors = cb;

						trans.Find ("AutoCast").GetComponent<Image> ().enabled = man.abilityList [3 + AbilityX * 4].canAutoCast;
						certainButtons [j+ AbilityX].RAuto.enabled = man.abilityList [3 + AbilityX * 4].autocast;



							Text charger = trans.Find ("Charge4").GetComponent<Text> ();
							if (man.abilityList [3 + AbilityX * 4].chargeCount > -1) {
							int totalCharge = 0;
							foreach (RTSObject rts in  uiPage.rows[j]) {
								totalCharge += rts.abilityList [3 + AbilityX * 4].chargeCount;
							}

							charger.text =  "" +totalCharge;


								//charger.text = "" + man.abilityList [3 + AbilityX * 4].chargeCount;
							} else {
								charger.text = "";
							}
						}
					certainButtons [j + AbilityX].Rmoney.enabled = false;
					}

				AbilityX++;
				n++;
			}
			}

		}



	public void upDateAutoCast(Page uiPage)
	{


		int n = 0;

		for(int j = 0; j < 3; j ++){
			if (uiPage.rows [j] == null) {
				continue;
			}

			n = uiPage.rows[j][0].AbilityStartingRow;

			//Sets the unit's stats and count
			UnitManager man = uiPage.rows[j][0].gameObject.GetComponent<UnitManager> ();
		

			int AbilityX = 0;

			for (int m = 0; m < man.abilityList.Count / 4 +1; m++) {


					if(man.abilityList.Count > AbilityX * 4){
						if (man.abilityList [AbilityX * 4] != null) {
			
						certainButtons [j].QAuto.enabled = man.abilityList [0 + AbilityX * 4].autocast;

						}
					}

					if(man.abilityList.Count >1+( AbilityX * 4)){
						if (man.abilityList [1 + AbilityX * 4] != null) {
	
						certainButtons [j].WAuto.enabled = man.abilityList [1 + AbilityX * 4].autocast;
					
						}
					}

					if(man.abilityList.Count > 2+(AbilityX * 4)){
						if (man.abilityList [2 + AbilityX * 4] != null) {
		
						certainButtons [j].EAuto.enabled = man.abilityList [2 + AbilityX * 4].autocast;
			
						}
					}

					if(man.abilityList.Count >3+( AbilityX * 4)){
						if (man.abilityList [3 +AbilityX * 4] != null) {

						certainButtons [j].RAuto.enabled = man.abilityList [3 + AbilityX * 4].autocast;
				
						}
					}


				}
				AbilityX++;
				n++;
			
		}



	}



	public void updateUI(Page uiPage)
	{
		int n = 0;

		for(int j = 0; j < 3; j ++){
			if (uiPage.rows [j] == null) {
				continue;
			}

			n = uiPage.rows[j][0].AbilityStartingRow;

			//Sets the unit's stats and count
			UnitManager man = uiPage.rows[j][0].gameObject.GetComponent<UnitManager> ();
			Stats[n].GetComponent<StatsUI> ().loadUnit (man, 
				uiPage.rows[j].Count, man.UnitName);

			int AbilityX = 0;

			for (int m = 0; m < man.abilityList.Count / 4 +1; m++) {
		

					if (man.abilityList.Count > AbilityX * 4) {
						if (man.abilityList [AbilityX * 4] != null) {
						Transform trans = certainButtons [n].QButton.transform;//.Find("QButton");;
							//Transform trans = UIButtons [n].transform.FindChild ("QButton");

						trans.GetComponent<Image> ().sprite = man.abilityList [AbilityX * 4].iconPic;
				
						bool active = false;
						foreach (RTSObject obj in currentPage.rows [j]) {
							UnitManager Uman = obj.getUnitManager ();
						
							if (!Uman.Silenced() && !Uman.Stunned()) {
								active = (Uman.abilityList [ AbilityX * 4].active);
							}
							if (active) {
								break;}
						}


						trans.GetComponent<Button> ().interactable = active; //man.abilityList [AbilityX * 4].active;
					

							Text charger = trans.Find ("Charge1").GetComponent<Text> ();
							if (man.abilityList [AbilityX * 4].chargeCount > -1) {

							int totalCharge = 0;
							foreach (RTSObject rts in  uiPage.rows[j]) {
								totalCharge += rts.abilityList [AbilityX * 4].chargeCount;
							}

							charger.text =  "" +totalCharge;

								//charger.text = "" + man.abilityList [AbilityX * 4].chargeCount;
							} else {
								charger.text = "";
							}
						}
					}

					if(man.abilityList.Count >1+( AbilityX * 4)){
						if (man.abilityList [1 + AbilityX * 4] != null) {
						Transform trans = certainButtons [n].WButton.transform;//.Find("WButton");;
							//Transform trans = UIButtons [n].transform.FindChild ("WButton");

						trans.GetComponent<Image> ().sprite = man.abilityList [1 + AbilityX * 4].iconPic;
					
						Text charger = trans.Find ("Charge2").GetComponent<Text> ();
							

						bool active = false;
						foreach (RTSObject obj in currentPage.rows [j]) {
							UnitManager Uman = obj.getUnitManager ();

							if (!Uman.Silenced() && !Uman.Stunned()) {
								active = (Uman.abilityList [1 +  AbilityX * 4].active);
							}
							if (active) {
								break;}
						}


						trans.GetComponent<Button> ().interactable = active;

						//trans.GetComponent<Button> ().interactable = man.abilityList [1 + AbilityX * 4].active;

							if (man.abilityList [1 + AbilityX * 4].chargeCount > -1) {
							int totalCharge = 0;
							foreach (RTSObject rts in  uiPage.rows[j]) {
								totalCharge += rts.abilityList [1 + AbilityX * 4].chargeCount;
							}

							charger.text =  "" +totalCharge;

							//charger.text = "" + man.abilityList [1 + AbilityX * 4].chargeCount;

							} else {
								charger.text = "";
							}
						}
					}

					if(man.abilityList.Count > 2+(AbilityX * 4)){
						if (man.abilityList [2 + AbilityX * 4] != null) {
						Transform trans = certainButtons [n].EButton.transform;//.Find("EButton");;
							//Transform trans = UIButtons [n].transform.FindChild ("EButton");
					
						trans.GetComponent<Image> ().sprite = man.abilityList [2 + AbilityX * 4].iconPic;


						bool active = false;
						foreach (RTSObject obj in currentPage.rows [j]) {
							UnitManager Uman = obj.getUnitManager ();

							if (!Uman.Silenced() && !Uman.Stunned()) {
								active = (Uman.abilityList [2+ AbilityX * 4].active);
							}
							if (active) {
								break;}
						}


						trans.GetComponent<Button> ().interactable = active;

						//trans.GetComponent<Button> ().interactable = man.abilityList [2 + AbilityX * 4].active;

							Text charger = trans.Find ("Charge3").GetComponent<Text> ();
							if (man.abilityList [2 + AbilityX * 4].chargeCount > -1) {
								
							int totalCharge = 0;
							foreach (RTSObject rts in  uiPage.rows[j]) {
								totalCharge += rts.abilityList [2 + AbilityX * 4].chargeCount;
							}

							charger.text =  "" +totalCharge;

							//charger.text = "" + man.abilityList [2 + AbilityX * 4].chargeCount;
							} else {
								charger.text = "";
							}
						}
					}

					if(man.abilityList.Count >3+( AbilityX * 4)){
						if (man.abilityList [3 + AbilityX * 4] != null) {
						Transform trans = certainButtons [n].RButton.transform;//.Find("RButton");;
							//Transform trans = UIButtons [n].transform.FindChild ("RButton");

						trans.GetComponent<Image> ().sprite = man.abilityList [3 + AbilityX * 4].iconPic;

						bool active = false;
						foreach (RTSObject obj in currentPage.rows [j]) {
							UnitManager Uman = obj.getUnitManager ();

							if (!Uman.Silenced() && !Uman.Stunned()) {
								active = (Uman.abilityList [3+ AbilityX * 4].active);
							}
							if (active) {
								break;}
						}


						trans.GetComponent<Button> ().interactable = active;

						//trans.GetComponent<Button> ().interactable = man.abilityList [3 + AbilityX * 4].active;
							Text charger = trans.Find ("Charge4").GetComponent<Text> ();
							if (man.abilityList [3 + AbilityX * 4].chargeCount > -1) {
							int totalCharge = 0;
							foreach (RTSObject rts in  uiPage.rows[j]) {
								totalCharge += rts.abilityList [3 + AbilityX * 4].chargeCount;
							}

							charger.text =  "" +totalCharge;	

							//charger.text = "" + man.abilityList [3 + AbilityX * 4].chargeCount;
							} else {
								charger.text = "";
							}
						}
					}


				}
				AbilityX++;
				n++;
			}
		


	}
		



	public void updateSingleCard()
	{
		for(int j = 0; j < 3; j ++){

			if (currentPage.rows [j] == null) {

				continue;
			}

			if (currentPage.rows [j] != null) {

				cardCreator.CreateCard (currentPage.rows [j][0]);
				break;
				}
			}

	}


	public void upDateActive(Page uiPage)
	{
		int n = 0;

		for (int j = 0; j < 3; j++) {
			if (uiPage.rows [j] == null) {
				continue;
			}

			n = uiPage.rows [j] [0].AbilityStartingRow;

			//Sets the unit's stats and count
		
			UnitManager man = uiPage.rows [j] [0].gameObject.GetComponent<UnitManager> ();


			int AbilityX = 0;


		for (int m = 0; m < man.abilityList.Count / 4 +1; m++) {


				if (man.abilityList.Count > AbilityX * 4) {
					if (man.abilityList [AbilityX * 4] != null) {
						Transform trans = certainButtons [n].QButton.transform;//.Find ("QButton");

						ColorBlock cb = trans.GetComponent<Button> ().colors;

						bool active = false;
						foreach (RTSObject obj in currentPage.rows [j]) {
							UnitManager Uman = obj.gameObject.GetComponent<UnitManager> ();

							if (!Uman.Silenced () && !Uman.Stunned ()) {
								active = (Uman.abilityList [AbilityX * 4].active);
							}
							if (active) {
								break;
							}
						}
		
						if (active) {
							cb.disabledColor = Color.white;
							if (trans && trans.GetComponent<Button> ())
								trans.GetComponent<Button> ().interactable = true;
						} else {
							cb.disabledColor = disabledColor;
							trans.GetComponent<Button> ().interactable = false;

						}


						trans.GetComponent<Button> ().colors = cb;

					}
				}
			if(man.abilityList.Count >1+( AbilityX * 4)){
				if (man.abilityList [1 + AbilityX * 4] != null) {

						Transform trans = certainButtons [n].WButton.transform;//.Find ("WButton");
					ColorBlock cb= trans.GetComponent<Button> ().colors;

					bool active = false;
					foreach (RTSObject obj in currentPage.rows [j]) {
						UnitManager Uman = obj.gameObject.GetComponent<UnitManager> ();

						if (!Uman.Silenced() && !Uman.Stunned()) {
							active = (Uman.abilityList [1+AbilityX * 4].active);
						}
						if (active) {
							break;}
					}



					//if (man.abilityList [AbilityX * 4].active) {
					if (active) {


						cb.disabledColor = Color.white;
						trans.GetComponent<Button> ().interactable = true;
					} else {
							cb.disabledColor =disabledColor;
						trans.GetComponent<Button> ().interactable = false;

					}

					trans.GetComponent<Button> ().colors = cb;

				}
			}




			if(man.abilityList.Count > 2+(AbilityX * 4)){
				if (man.abilityList [2 +AbilityX * 4] != null) {
						Transform trans = certainButtons [n].EButton.transform;//.Find ("EButton");
					ColorBlock cb= trans.GetComponent<Button> ().colors;


					bool active = false;
					foreach (RTSObject obj in currentPage.rows [j]) {
						UnitManager Uman = obj.gameObject.GetComponent<UnitManager> ();

						if (!Uman.Silenced() && !Uman.Stunned()) {
							active = (Uman.abilityList [2 + AbilityX * 4].active);
						}
						if (active) {
							break;}
					}

					if (active) {


						cb.disabledColor = Color.white;
						trans.GetComponent<Button> ().interactable = true;
					} else {
							cb.disabledColor =disabledColor;
						trans.GetComponent<Button> ().interactable = false;

					}
								
					trans.GetComponent<Button> ().colors = cb;

				
				}

			}

			if(man.abilityList.Count >3+( AbilityX * 4)){
				if (man.abilityList [3 +AbilityX * 4] != null) {
						Transform trans = certainButtons [n].RButton.transform;//.Find ("RButton");
					ColorBlock cb= trans.GetComponent<Button> ().colors;


					bool active = false;
					foreach (RTSObject obj in currentPage.rows [j]) {
						UnitManager Uman = obj.gameObject.GetComponent<UnitManager> ();

						if (!Uman.Silenced() && !Uman.Stunned()) {
							active = (Uman.abilityList [3 +AbilityX * 4].active);
						}
						if (active) {
							break;}
					}


					if (active) {

						cb.disabledColor = Color.white;
						trans.GetComponent<Button> ().interactable = true;
					} else {
							cb.disabledColor =disabledColor;
						trans.GetComponent<Button> ().interactable = false;

					}
							
					trans.GetComponent<Button> ().colors = cb;

				}
			}

			AbilityX++;
			n++;
		}
	}


	}


	public void callAbility(int n)
	{
		if (Input.GetKey (KeyCode.LeftAlt)) {
			selectMan.setAutoCast (n);
			selectMan.AutoCastUI ();
			audSrc.PlayOneShot (ButtonPress, .1f);

		}else if (quickButtons [n].IsInteractable() && quickAbility[n].myAbility && quickButtons[n].IsActive()){// && quickAbility[n].myAbility.active) {
			selectMan.callAbility (n);
			audSrc.PlayOneShot (ButtonPress, .1f);
			
		}
		//EventSystem.current.SetSelectedGameObject (null);
		
	}







	public void UpdateSingleButton(int buttonNumber, int abilityNum, string UnitName)
	{

		int j = buttonNumber / 4;
		UnitManager man = currentPage.rows [j] [0].gameObject.GetComponent<UnitManager> ();
		if (man.UnitName != UnitName) {
			return;
		}
	
		if(man.abilityList.Count >abilityNum){
			if (man.abilityList [abilityNum] != null) {

				Transform trans;
				Text charger;

				if (buttonNumber % 4 == 0) {
					trans = certainButtons [j].QButton.transform;//.Find("QButton");
					charger = trans.Find ("Charge1").GetComponent<Text> ();
					certainButtons [j].QAuto.enabled = man.abilityList [abilityNum].autocast;
					}
				else if (buttonNumber % 4 == 1) {
					trans = certainButtons [j].WButton.transform;//.Find("WButton");
					charger = trans.Find ("Charge2").GetComponent<Text> ();
					certainButtons [j].WAuto.enabled = man.abilityList [abilityNum].autocast;
				}
				else if (buttonNumber % 4 == 2) {
					trans = certainButtons [j].EButton.transform;//.Find("EButton");
					charger = trans.Find ("Charge3").GetComponent<Text> ();
					certainButtons [j].EAuto.enabled = man.abilityList [abilityNum].autocast;
				}
				else {
					trans = certainButtons [j].RButton.transform;//.Find("RButton");
					charger = trans.Find ("Charge4").GetComponent<Text> ();
					certainButtons [j].RAuto.enabled = man.abilityList [abilityNum].autocast;
				}

				ColorBlock cb= trans.GetComponent<Button> ().colors;


				bool active = false;
				foreach (RTSObject obj in currentPage.rows [j]) {
					UnitManager Uman = obj.gameObject.GetComponent<UnitManager> ();

					if (!Uman.Silenced() && !Uman.Stunned()) {
						active = (Uman.abilityList [abilityNum].active);
					}
					if (active) {
						break;}
				}


				if (active) {

					cb.disabledColor = Color.white;
					trans.GetComponent<Button> ().interactable = true;
				} else {
					cb.disabledColor =disabledColor;
					trans.GetComponent<Button> ().interactable = false;

				}


				if (buttonNumber % 4 == 0) {
					if (man.abilityList [abilityNum].getMyType() == Ability.type.passive) {
						certainButtons [j].Qtext.enabled = false;
						cb.disabledColor = Color.white;
						trans.GetComponent<Button> ().interactable = false;
					} else {
						certainButtons [j].Qtext.enabled = true;
					}
				}
				else if (buttonNumber % 4 == 1) {
					if (man.abilityList [abilityNum].getMyType() == Ability.type.passive) {
						certainButtons [j].Wtext.enabled = false;
						cb.disabledColor = Color.white;
						trans.GetComponent<Button> ().interactable = false;
					} else {
						certainButtons [j].Wtext.enabled = true;
					}
				}
				else if (buttonNumber % 4 == 2) {
					if (man.abilityList [abilityNum].getMyType() == Ability.type.passive) {
						certainButtons [j].Etext.enabled = false;
						cb.disabledColor = Color.white;
						trans.GetComponent<Button> ().interactable = false;
					} else {
						certainButtons [j].Etext.enabled = true;
					}
				}
				else {
					if (man.abilityList [abilityNum].getMyType() == Ability.type.passive) {
						certainButtons [j].Rtext.enabled = false;
						cb.disabledColor = Color.white;
						trans.GetComponent<Button> ().interactable = false;
					} else {
						certainButtons [j].Rtext.enabled = true;
					}
				}

				trans.GetComponent<Button> ().colors = cb;

				trans.Find ("AutoCast").GetComponent<Image> ().enabled = man.abilityList [abilityNum].canAutoCast;


				if (man.abilityList [abilityNum].chargeCount > -1) {
					int totalCharge = 0;
					foreach (RTSObject rts in currentPage.rows [j]) {
						totalCharge += rts.abilityList [ abilityNum * 4].chargeCount;
					}

					charger.text =  "" +totalCharge;

					//charger.text = "" + man.abilityList [abilityNum].chargeCount;
				} else {
					charger.text = "";
				}
			}
		}

	
	}





}
