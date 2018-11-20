
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UiAbilityManager : MonoBehaviour {

	RaceManager racer;

	//public List<GameObject> UIButtons = new List<GameObject>();
	Color disabledColor = new Color(.5f,0,0,1);

	bool showAllUnits = false;
	public GameObject GridObject;
	public Image gridButton;
	public Sprite showallGrid;
	public Sprite showSomeGrid;

	public List<ButtonRef> myButtons;

	protected Lean.LeanPool myIconPool;

	public static UiAbilityManager main;

	public GameObject topDividerBar;
	public GameObject bottomBar;

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
		ExecuteEvents.Execute (myButtons[n].myButton.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
		ExecuteEvents.Execute (myButtons[n].myButton.gameObject, pointer, ExecuteEvents.pointerDownHandler);
		StartCoroutine (delayedClick(n));

	}

	IEnumerator delayedClick(int n)
	{
		//UISoundManager.interfaceClick (true);

		var pointer = new PointerEventData (EventSystem.current);
		CostBox.instance.turnOff ();
		yield return new WaitForSeconds (.06f);
		ExecuteEvents.Execute (myButtons[n].myButton.gameObject, pointer, ExecuteEvents.pointerClickHandler);
		ExecuteEvents.Execute (myButtons[n].myButton.gameObject, pointer, ExecuteEvents.pointerExitHandler);


	}


	// Use this for initialization
	void Start () {
		racer = GameManager.getInstance ().activePlayer;
		IconStartPoints [0].SetActive (false);
		IconStartPoints [1].SetActive (false);
		IconStartPoints [2].SetActive (false);

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
		nextActionTime += .03f;



		if (currentPage == null) {
			return;
		}


		for (int j = 0; j < 3; j++) {
			if (currentPage.rows [j] == null || (j > 0 && currentPage.rows [j] == currentPage.rows [j - 1])) {
				continue;
			}

			UnitManager firstManager = currentPage.rows [j] [0].getUnitManager ();

			for (int i = 0; i < firstManager.abilityList.Count; i++) {
				if (firstManager.abilityList [i] == null) {
					continue;
				}

				int buttonIndex = i + firstManager.AbilityStartingRow * 4;
	
				if (firstManager.abilityList [i].myCost) {
					if (firstManager.abilityList [i].myCost.cooldown != 0) {

						setButtonCooldown (myButtons [buttonIndex].mySlider, i, j);
					} else {
						myButtons [buttonIndex].mySlider.gameObject.SetActive (false);
					}

					myButtons [buttonIndex].MoneySign.enabled = firstManager.abilityList [i].myCost.ResourceOne > racer.ResourceOne;
				} else {
					myButtons [buttonIndex].MoneySign.enabled = false;
					myButtons [buttonIndex].mySlider.gameObject.SetActive (false);
				}

				if (firstManager.abilityList [i].chargeCount > -1) {

					int totalCharge = 0;
					foreach (RTSObject obj in currentPage.rows [j]) {
						totalCharge += obj.abilityList [i].chargeCount;
					}

					myButtons [buttonIndex].ChargeCount.text = "" + totalCharge;
				} else {
					myButtons [buttonIndex].ChargeCount.text = "";
				}

			}

		}
			/*
			UnitManager man= null;
			foreach(RTSObject obj in currentPage.rows[j]){

				if(obj){
					man = obj.getUnitManager();
					break;}
			}
			if (!man) {
				continue;
			}
*/

		

	
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
			Ability toCheck = obj.getUnitManager ().abilityList [abilityNum];
			if (toCheck.active || toCheck.myCost.cooldownProgress() != 1 ) {
				float n = toCheck.myCost.cooldownProgress ();


				if (n > maxA) {
					maxA = n;
					if (n > .995f) {
						break;
					}
				}
			}
		}

		slide.value = maxA;
		slide.gameObject.SetActive (maxA < .99 && maxA!= 0);	
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
	{//Debug.Log ("Clearing");
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
			
			foreach (ButtonRef obj in myButtons) {
				obj.myButton.gameObject.SetActive(false);

			}
		}catch(Exception){
		}

		foreach (KeyValuePair< GameObject, GameObject > del in unitIcons) {
			//del.Key.GetComponentInChildren<Text> ().text = "";
			del.Key.GetComponentInChildren<UnitIconInfo>().reset ();
			myIconPool.FastDespawn (del.Key);

			//Destroy (del.Key);
		}

		unitIcons.Clear ();

	

			currentPage = null;
		cardCreator.turnOff ();

	}


	bool shownAll = false;



	public void upDateAutoCast(Page uiPage)
	{

		for (int j = 0; j < 3; j++) {
			if (currentPage.rows [j] == null || (j > 0 && currentPage.rows[j] == currentPage.rows[j-1])) {
				continue;
			}

			UnitManager firstManager = currentPage.rows [j] [0].getUnitManager ();

			for (int i = 0; i < firstManager.abilityList.Count; i++) {
				if (firstManager.abilityList [i] == null) {
					continue;
				}

				int buttonIndex = i + firstManager.AbilityStartingRow * 4;
				myButtons [buttonIndex].AutocastAnim.enabled = firstManager.abilityList [i].autocast;
		
			}
		}

	}



	public void updateUI(Page uiPage)
	{

		for (int j = 0; j < 3; j++) {
			if (currentPage.rows [j] == null || (j > 0 && currentPage.rows [j] == currentPage.rows [j - 1])) {
				continue;
			}

			UnitManager firstManager = currentPage.rows [j] [0].getUnitManager ();

			for (int i = 0; i < firstManager.abilityList.Count; i++) {

				int buttonIndex = i + firstManager.AbilityStartingRow * 4;
				ButtonRef currButton = myButtons [buttonIndex]; 

				currButton.myButton.gameObject.SetActive (firstManager.abilityList [i] != null);

				if (firstManager.abilityList [i] == null) {
					continue;
				}
				Ability currAbil = firstManager.abilityList [i];

				if (currAbil.myCost) {
					if (currAbil.myCost.cooldown != 0) {

						setButtonCooldown (currButton.mySlider, i, j);
					} else {
						currButton.mySlider.gameObject.SetActive (false);
					}

					currButton.MoneySign.enabled = currAbil.myCost.ResourceOne > racer.ResourceOne;
				} else {
					currButton.MoneySign.enabled = false;
					currButton.mySlider.gameObject.SetActive (false);
				}

				if (firstManager.abilityList [i].chargeCount > -1) {

					int totalCharge = 0;
					foreach (RTSObject obj in currentPage.rows [j]) {
						totalCharge += obj.abilityList [i].chargeCount;
					}

					currButton.ChargeCount.text = "" + totalCharge;
				} else {
					currButton.ChargeCount.text = "";
				}

				currButton.myButton.image.sprite = currAbil.iconPic;

				currButton.abilityBox.myAbility = currAbil;

				ColorBlock cb = currButton.myButton.colors;

				bool active = false;
				foreach (RTSObject obj in currentPage.rows [j]) {
					UnitManager Uman = obj.getUnitManager ();

					if (!Uman.Silenced () && !Uman.Stunned ()) {
						active = (Uman.abilityList [i].active);
					}
					if (active) {
						break;
					}
				}


				if (active) {
					cb.disabledColor = Color.white;
					currButton.myButton.interactable = true;

				} else {
					cb.disabledColor = disabledColor;
					currButton.myButton.interactable = false;

				}

				if (currAbil.getMyType () == Ability.type.passive) {
					currButton.myHotkey.enabled = false;
					cb.disabledColor = Color.white;
					currButton.myButton.interactable = false;
				} else {
					currButton.myHotkey.enabled = true;
				}

				currButton.myButton.colors = cb;
				currButton.autocastFrame.enabled = currAbil.canAutoCast;
				currButton.AutocastAnim.enabled = currAbil.autocast;
			}
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
		for (int j = 0; j < 3; j++) {
			if (currentPage.rows [j] == null || (j > 0 && currentPage.rows [j] == currentPage.rows [j - 1])) {
				continue;
			}

			UnitManager firstManager = currentPage.rows [j] [0].getUnitManager ();

			for (int i = 0; i < firstManager.abilityList.Count; i++) {

				int buttonIndex = i + firstManager.AbilityStartingRow * 4;
				ButtonRef currButton = myButtons [buttonIndex]; 

				currButton.myButton.gameObject.SetActive (firstManager.abilityList [i] != null);

				if (firstManager.abilityList [i] == null) {
					continue;
				}
				Ability currAbil = firstManager.abilityList [i];

				ColorBlock cb = currButton.myButton.colors;

				bool active = false;
				foreach (RTSObject obj in currentPage.rows [j]) {
					UnitManager Uman = obj.getUnitManager ();

					if (!Uman.Silenced () && !Uman.Stunned ()) {
						active = (Uman.abilityList [i].active);
					}
					if (active) {
						break;
					}
				}

			
				if (active) {
					cb.disabledColor = Color.white;
					currButton.myButton.interactable = true;

				} else {
					cb.disabledColor = disabledColor;
					currButton.myButton.interactable = false;

				}

				if (currAbil.getMyType () == Ability.type.passive) {
					currButton.myHotkey.enabled = false;
					cb.disabledColor = Color.white;
					currButton.myButton.interactable = false;
				} else {
					currButton.myHotkey.enabled = true;
				}
				currButton.myButton.colors = cb;
			
			}
		}

	}


	public void callAbility(int n)
	{
		if (selectMan.ActiveObjectList () [0].getUnitManager ().PlayerOwner == 1) {
			if (Input.GetKey (KeyCode.LeftAlt)) {
				selectMan.setAutoCast (n);
				selectMan.AutoCastUI ();
				UISoundManager.interfaceClick (true);

			} else if (myButtons[n].myButton.IsInteractable () && myButtons[n].abilityBox.myAbility && myButtons[n].myButton.IsActive ()) {
				selectMan.callAbility (n);
			

			
			}
		} else {
			UISoundManager.interfaceClick (false);
		}
		//EventSystem.current.SetSelectedGameObject (null);
		
	}






	/*
	public void UpdateSingleButton(int buttonNumber, int i, string UnitName)
	{

		int j = buttonNumber / 4;
		UnitManager firstManager = currentPage.rows [j] [0].getUnitManager();
		if (firstManager.UnitName != UnitName) {
			return;
		}
			

		int buttonIndex = i + firstManager.AbilityStartingRow * 4;
		ButtonRef currButton = myButtons [buttonIndex]; 

		currButton.myButton.gameObject.SetActive (firstManager.abilityList [i] != null);

		if (firstManager.abilityList [i] == null) {
				return;
			}
		Ability currAbil = firstManager.abilityList [i];

		if (currAbil.myCost) {
				if (currAbil.myCost.cooldown != 0) {

				setButtonCooldown (currButton.mySlider, i, j);
					} else {
						currButton.mySlider.gameObject.SetActive (false);
					}

					currButton.MoneySign.enabled = currAbil.myCost.ResourceOne > racer.ResourceOne;
				} else {
					currButton.MoneySign.enabled = false;
					currButton.mySlider.gameObject.SetActive (false);
				}

				if (firstManager.abilityList [i].chargeCount > -1) {

					int totalCharge = 0;
					foreach (RTSObject obj in currentPage.rows [j]) {
						totalCharge += obj.abilityList [i].chargeCount;
					}

					currButton.ChargeCount.text = "" + totalCharge;
				} else {
					currButton.ChargeCount.text = "";
				}

				currButton.myButton.image.sprite = currAbil.iconPic;

				currButton.abilityBox.myAbility = currAbil;

				ColorBlock cb = currButton.myButton.colors;

				bool active = false;
				foreach (RTSObject obj in currentPage.rows [j]) {
					UnitManager Uman = obj.getUnitManager ();

					if (!Uman.Silenced () && !Uman.Stunned ()) {
						active = (Uman.abilityList [i].active);
					}
					if (active) {
						break;
					}
				}


				if (active) {
					cb.disabledColor = Color.white;
					currButton.myButton.interactable = true;

				} else {
					cb.disabledColor = disabledColor;
					currButton.myButton.interactable = false;

				}

				if (currAbil.getMyType () == Ability.type.passive) {
					currButton.myHotkey.enabled = false;
					cb.disabledColor = Color.white;
					currButton.myButton.interactable = false;
				} else {
					currButton.myHotkey.enabled = true;
				}

				currButton.myButton.colors = cb;
				currButton.autocastFrame.enabled = currAbil.canAutoCast;
				currButton.AutocastAnim.enabled = currAbil.autocast;
			
	}
*/




	public void loadUI(Page uiPage)
	{
		shownAll = false;
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

		foreach (ButtonRef obj in myButtons) {
			obj.myButton.gameObject.SetActive (false);
		}



		foreach (KeyValuePair< GameObject, GameObject > del in unitIcons) {
			del.Key.GetComponentInChildren<UnitIconInfo>().reset ();
			myIconPool.FastDespawn (del.Key);
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

        for (int j = 0; j < 3; j++)
        {

            if (uiPage.rows[j] == null)
            {

                continue;
            }


            if (j > 0 && uiPage.rows[j] == uiPage.rows[j - 1])
            {

                continue;
            }


            n = uiPage.rows[j][0].AbilityStartingRow;


            //Sets the unit's stats and count
            UnitManager man = uiPage.rows[j][0].gameObject.GetComponent<UnitManager>();
            Stats[n].GetComponent<StatsUI>().loadUnit(man, uiPage.rows[j].Count, man.UnitName);
            if (totalUnit > 1) { 
                IconStartPoints[j].SetActive(true);
               }
			// fill the icon panel




			//Debug.Log ("Total units " + totalUnit + "  show all " + showAllUnits + "   " + selectMan.ActiveObjectList().Count + "   " +j);
			if (totalUnit > 1 || (selectMan.ActiveObjectList().Count > 1 && showAllUnits)) {

				if (showAllUnits) {
					if (!shownAll) {
						shownAll = true;

						List<Page> currentPages = selectMan.getPageList ();
						for (int e = 0; e < currentPages.Count; e++) {

							for (int p = 0; p < currentPages[e].rows.Length; p++) {

								if (currentPages[e].rows [p] != null) {
									if (p == 0 ||currentPages[e].rows [p] != currentPages[e].rows [p - 1]) {

										for (int x = 0; x < currentPages[e].rows [p].Count; x++) {

											GameObject unit = myIconPool.FastSpawn (Vector3.zero, Quaternion.identity, GridObject.transform);
											unit.transform.Find ("BuildCount").GetComponent<Text> ().text = "";
											//GameObject unit = (GameObject)Instantiate (buttonTemplate, Vector3.zero, Quaternion.identity, GridObject.transform);

											Transform icontransform = unit.transform.Find ("UnitIconTemplate");
											icontransform.GetComponent<UnitIconInfo> ().setInfo (currentPages[e].rows [p] [x].gameObject); //.myUnit = uiPage.rows [j] [k].gameObject;
											icontransform.GetComponent<UnitIconInfo> ().changeSliderColor (currentPages[e].rows [p] [x].GetComponent<Selected>().Cooldownbar.getBarColor());
											float cooldown = currentPages[e].rows [p] [x].GetComponent<Selected>().Cooldownbar.getRatio();
									
										
											icontransform.GetComponent<UnitIconInfo> ().updateSlider (cooldown);


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
						int separation = 62;

						if (uiPage.rows [j].Count > 14) {
							separation = Mathf.Max (15, 558 / picCount);
						}

						int currentX = 145;
						for (int k = 0; k < picCount; k++) {

							Vector3 pos = IconStartPoints [j].transform.position + Vector3.down * (2 - j) * 3;
							pos.x += currentX * this.transform.localScale.x;
							GameObject unit = myIconPool.FastSpawn (Vector3.zero, Quaternion.identity);

							//GameObject unit = (GameObject)Instantiate (buttonTemplate);
							Transform icontransform = unit.transform.Find ("UnitIconTemplate");

							UnitIconInfo iconInfo = icontransform.GetComponent<UnitIconInfo> ();

							iconInfo.setInfo (uiPage.rows [j] [k].gameObject); //.myUnit = uiPage.rows [j] [k].gameObject;

							float cooldown = uiPage.rows [j] [k].GetComponent<Selected>().Cooldownbar.getRatio();
							iconInfo.changeSliderColor (uiPage.rows [j] [k].GetComponent<Selected>().Cooldownbar.getBarColor());

							iconInfo.updateSlider (cooldown);


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


			UnitManager firstManager = currentPage.rows [j] [0].getUnitManager ();

			for (int i = 0; i < firstManager.abilityList.Count; i++) {

				int buttonIndex = i + firstManager.AbilityStartingRow * 4;
				ButtonRef currButton = myButtons [buttonIndex]; 

				currButton.myButton.gameObject.SetActive (firstManager.abilityList [i] != null);
		
				if (firstManager.abilityList [i] == null) {
					continue;
				}
				Ability currAbil = firstManager.abilityList [i];
			
				if (currAbil.myCost) {
					if (currAbil.myCost.cooldown != 0) {

						setButtonCooldown (currButton.mySlider, i, j);
					}
					else {
						currButton.mySlider.gameObject.SetActive (false);
					}

					currButton.MoneySign.enabled = currAbil.myCost.ResourceOne > racer.ResourceOne;
				}else {
					currButton.MoneySign.enabled = false;
					currButton.mySlider.gameObject.SetActive (false);
				}

				if (firstManager.abilityList [i].chargeCount > -1) {

					int totalCharge = 0;
					foreach (RTSObject obj in currentPage.rows [j]) {
						totalCharge += obj.abilityList [i].chargeCount;
					}

					currButton.ChargeCount.text =  "" +totalCharge;
				} else {
					currButton.ChargeCount.text = "";
				}

				currButton.myButton.image.sprite = currAbil.iconPic;

				currButton.abilityBox.myAbility = currAbil;
	
				ColorBlock cb= currButton.myButton.colors;

				bool active = false;
				foreach (RTSObject obj in currentPage.rows [j]) {
					UnitManager Uman = obj.getUnitManager();

					if (!Uman.Silenced() && !Uman.Stunned()) {
						active = (Uman.abilityList [i].active);
					}
					if (active) {
						break;}
				}
					

				if (active) {
					cb.disabledColor = Color.white;
					currButton.myButton.interactable = true;
						
				} else {
					cb.disabledColor =disabledColor;
					currButton.myButton.interactable = false;

				}

				if (currAbil.getMyType()== Ability.type.passive) {
					currButton.myHotkey.enabled = false;
					cb.disabledColor = Color.white;
					currButton.myButton.interactable = false;
				} else {
					currButton.myHotkey.enabled = true;
				}

				currButton.myButton.colors = cb;
				currButton.autocastFrame.enabled = currAbil.canAutoCast;
				currButton.AutocastAnim.enabled = currAbil.autocast;
	
			
			}

				n++;

		}

	}

	



}

[System.Serializable]
public class ButtonRef{

	public string Key;
	public Button myButton;
	public Text myHotkey;
	public Slider mySlider;
	public Text ChargeCount;
	public Image autocastFrame;
	public Image AutocastAnim;
	public Image MoneySign;
	public AbilityBox abilityBox;
	int Row;
	int Coloumn;

	public void Update()
	{
		
	}
}