using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour, IUIManager {
	
	//Singleton
	public static UIManager main;

	public GameObject buildingPlacer;
	private GameObject tempBuildingPlacer;
	public Material goodPlacement;
	public Material badPlacement;
	public GameObject thingToBeBuilt;

	//Action Variables
	private HoverOver hoverOver = HoverOver.Terrain;
	private GameObject currentObject;
	
	//Mode Variables
	private Mode m_Mode = Mode.Normal;
	
	//Interface variables the UI needs to deal with
	private ICamera m_Camera;
	private IGUIManager m_GuiManager;

    CustomInputSystem inputSystem;

    //Building Placement variables

    public GameObject m_ObjectBeingPlaced;
	private bool m_Placed = false;

	public FogOfWar fog {
        get;set;
    }
    private RaceManager raceManager;
	private Vector3 originalPosition;
	public GameObject AbilityTargeter;
	public TargetAbility currentAbility;
	public int currentAbilityNUmber;
	public bool clickOverUI
    {
        get;set;
    }


    //Used for right click Formations
    private Vector2 rightClickOrigin;
	private Vector3 rightClickEnd;
	private bool rightClickDrag;
	private LineRenderer lineRender;
	private Vector3 rightClickOrThree = Vector3.zero;

	public bool fastCast;
    [Tooltip("Use a spriteRender for the Ability Target Reticule, like for Vector Graphics")]
    public bool UseSpriteTarget;
    public LayerMask GroundsCast = (1 << 8) | ( 1 << 16) | (1 << 11);
    public LayerMask EverythinCast = ~((1 << 4) | (1 << 5)  | (1 << 12) | (1 << 14) | (1 << 15) | (1 << 17) | (1 << 19) | (1 << 20) | (1 << 21));

    private float lastClickDouble;
	public bool IsShiftDown
	{
		get;
		set;
	}
	
	public bool IsControlDown
	{
		get;
		set;
	}
	
	public Mode CurrentMode
	{
		get
		{
			return m_Mode;
		}
	}

	void Awake()
	{
		if (!GetComponent<WorldRecharger> ()) {
			gameObject.AddComponent<WorldRecharger> ();
		}

		if (!GetComponent<PhysicsSimulator> ()) {
			gameObject.AddComponent<PhysicsSimulator> ();
		}

		inputSystem = GameObject.FindObjectOfType<CustomInputSystem> ();
		main = this;
	}

	// Use this for initialization
	void Start () 
	{	GameMenu.main.addDisableScript (this);
		lineRender = GetComponent<LineRenderer> ();
		fog = GameObject.FindObjectOfType<FogOfWar> ();
        if (!fog.enabled)
        { fog = null; }
        //Resolve interface variables
		m_Camera =  MainCamera.main;
		m_GuiManager =GameObject.FindObjectOfType<GUIManager>();
	
		//Attach Event Handlers
		EventsManager eventsManager =GameObject.FindObjectOfType<EventsManager>();

		eventsManager.MouseClick += ButtonClickedHandler;

		eventsManager.MouseScrollWheel += ScrollWheelHandler;
		eventsManager.KeyAction += KeyBoardPressedHandler;
		eventsManager.ScreenEdgeMousePosition += MouseAtScreenEdgeHandler;

		raceManager = GameManager.main.activePlayer;
		formationLight = Resources.Load<GameObject> ("FormationLight");
		fastCast  =  PlayerPrefs.GetInt ("FastCast",0) == 1;

	}

	// Chached Refs to save on Garbage COllection
	Ray rayb;


	// Update is called once per frame
	void Update () 
	{
		clickOverUI = isPointerOverUIObject ();
	
		rayb = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Input.GetMouseButtonDown (1)) {

            InitializeFormations();
		}
        CursorManager.main.normalMode();
        ModeNormalBehaviour();
        switch (m_Mode)
		{

		case Mode.Normal:			
			
			break;
			
		case Mode.Menu:
			break;

		case Mode.targetAbility:

			RaycastHit hit = TargetAbRaycast();

			if (hit.collider) {			
				try{
                     UpdateTargetReticule(SelectedManager.main.checkValidTarget(hit.point, currentObject, currentAbilityNUmber), hit.point);
				}
				catch(NullReferenceException) {					
					SwitchMode (Mode.Normal);
				}		
			}

			break;

		case Mode.globalAbility:

            hit = TargetAbRaycast();

			if (hit.collider)
			{
                UpdateTargetReticule(currentAbility.isValidTarget(currentObject, hit.point),hit.point);
			}
			break;

			
		case Mode.PlaceBuilding:
			if (Input.GetKeyUp (KeyCode.LeftShift)) {
				Destroy (m_ObjectBeingPlaced);
				m_ObjectBeingPlaced = null;
				buildingPlacer.SetActive (false);
				Destroy (tempBuildingPlacer);
				SwitchToModeNormal ();
			} else {

				ModePlaceBuildingBehaviour ();
				break;
			}
			break;
		}
	}

    RaycastHit FindTarget()
    {
        RaycastHit hit = TargetAbRaycast();

        if (hit.collider)
        {
            AbilityTargeter.transform.position = hit.point;

            if (currentObject.layer == 9 || currentObject.layer == 10 || currentObject.layer == 13)
            {

            }
            else
            {
                currentObject = null;
            }
        }
        return hit;
    }


    RaycastHit TargetAbRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayb, out hit, Mathf.Infinity, currentAbility.myTargetType == TargetAbility.targetType.ground ? GroundsCast : EverythinCast))
        {
            currentObject = hit.collider.gameObject;
            if (currentObject.transform.parent && currentObject.transform.parent.GetComponent<UnitManager>())
            {
                currentObject = currentObject.transform.parent.gameObject; // This is to get the unitmanager of the parent of a turret
            }
        }

        return hit;
    }

    RaycastHit Raycast(LayerMask layer)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            currentObject = hit.collider.gameObject;
        }
        return hit;
    }

    void UpdateTargetReticule(bool isValid, Vector3 location)
    {
        if (isValid)
        {
            AbilityTargeter.GetComponentInChildren<Light>().color = Color.green;
            CursorManager.main.targetMode();
        }
        else
        {
            AbilityTargeter.GetComponentInChildren<Light>().color = Color.red;
            CursorManager.main.invalidMode();
        }
        location.y += 60;
        AbilityTargeter.transform.position = location;
    }

    void InitializeFormations() 
    {
        if (!clickOverUI)
        {
            rightClickOrigin = Input.mousePosition;
            rightClickDrag = true;
            RaycastHit hitb;

            if (Physics.Raycast(rayb, out hitb, Mathf.Infinity, EverythinCast))
            {
                rightClickOrThree = hitb.point + Vector3.up * 2;
            }

            if (formationCoroutine != null)
            {
                StopCoroutine(formationCoroutine);
            }

            formationCoroutine = StartCoroutine(formationDisplay());
        }
    }

    InteractionState interactionState;
	private void ModeNormalBehaviour()
	{
        RaycastHit hit = Raycast(EverythinCast);

        if (hit.collider)
        { 

            if (clickOverUI)
            {
                hoverOver = HoverOver.Menu;
            }
            else
            {
                hoverOver = HoverOver.Terrain;

                if (!fog || !fog.IsInCompleteFog(hit.point))
                {
                    switch (hit.collider.gameObject.layer)
                    {
                        case 9:
                            hoverOver = HoverOver.Unit;
                            break;

                        case 10:
                            hoverOver = HoverOver.Building;
                            break;

                        case 13:
                            hoverOver = HoverOver.neutral;
                            break;
                    }
                }
            }
        }
 
		if (hoverOver == HoverOver.Menu || SelectedManager.main.ActiveObjectsCount () == 0) {

            //Nothing orderable Selected or mouse is over menu 
            CalculateInteraction (hoverOver, ref interactionState);
		} else if (SelectedManager.main.ActiveObjectsCount () > 0 && (hoverOver == HoverOver.Unit || hoverOver == HoverOver.Building)) {

            //One object selected
            CalculateInteraction (SelectedManager.main.FirstActiveObject (), hoverOver, ref interactionState);
		} else {

            interactionState = InteractionState.Nothing;
		}
	}
	
	private void CalculateInteraction(HoverOver hoveringOver, ref InteractionState interactionState)
	{
        interactionState = InteractionState.Select;
		if (hoveringOver == HoverOver.Terrain) {
			
			interactionState = InteractionState.Nothing;
		} else {
			if (! clickOverUI) {
				Selected sel = currentObject.GetComponentInParent<Selected> ();
				if (sel) {
					sel.tempSelect ();
				}
			
				CursorManager.main.selectMode ();
			}
		}
	}
	
	private void CalculateInteraction(RTSObject obj, HoverOver hoveringOver, ref InteractionState interactionState)
	{	interactionState = InteractionState.Select;

		if (currentObject == null) {
			return;
		}

		Selected select = currentObject.GetComponentInParent<Selected> ();

        if (select)
			{select.tempSelect ();}

		UnitManager manag = currentObject.GetComponentInParent<UnitManager> ();
		if (manag && manag.PlayerOwner != raceManager.playerNumber) {
			interactionState = InteractionState.Attack;
			CursorManager.main.attackMode ();
		}
	}


	private void ModePlaceBuildingBehaviour()
	{
		//Get current location and place building on that location		
		if (!clickOverUI) {
            RaycastHit hit = Raycast(GroundsCast);
            if (hit.collider) {
				m_ObjectBeingPlaced.transform.position = hit.point;
                tempBuildingPlacer.GetComponent<BuildingPlacer>().canBuild(hit.collider.gameObject);

			}		
		}
	}

    public RaycastHit getGroundCast(Vector3 location)
    {
        Ray ray = new Ray(location + Vector3.up *50, Vector3.down);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity, GroundsCast);
        return hit;
    }


	//------------------------Mouse Button Commands--------------------------------------------
	public void LeftButton_SingleClickDown(MouseEventArgs e)
	{
        clickOverUI = isPointerOverUIObject ();

		if(m_Mode == Mode.Normal && hoverOver != HoverOver.Menu)
		{
			originalPosition = Input.mousePosition;
		}		
	}

	public bool isPointerOverUIObject()
	{
		return  inputSystem.overUILayer ();
	}

	public bool isPointerOverFloatingButton()
	{
		PointerEventData eventDatacurrenPosition = new PointerEventData (EventSystem.current);
		eventDatacurrenPosition.position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);

		List<RaycastResult> results = new List<RaycastResult> ();
		EventSystem.current.RaycastAll (eventDatacurrenPosition, results);

		int count = 0;
		foreach(RaycastResult res in results){
			if (res.distance != 0) {
				count++;
			}
		}

		return (count != 0);
	}
    

	public void LeftButton_DoubleClickDown(MouseEventArgs e)
	{
        //Select all units of that type on screen
        lastClickDouble = Time.time;

			//if we're not dragging and clicked on a unit
			if (!m_GuiManager.Dragging && (currentObject.layer == 9 || currentObject.layer == 10)) {


				if (!clickOverUI) {
				
					/*  TARGET RULES
                    shift selects units without affecting others
                    control deselects units without affecting others
                */
					//deselect if none of the modifiers are being used
					if (!IsShiftDown && !IsControlDown) {
                    SelectedManager.main.DeselectAll ();
					}
                    
				if (getUnitManagerFromObject(currentObject)) {
                    SelectedManager.main.AddObjects(raceManager.getUnitOnScreen(true, currentObject.GetComponentInParent<UnitManager>().UnitName));
				}
                SelectedManager.main.CreateUIPages (0);
			} 	
		}
	}

	//Double click
	public void LeftButton_SingleClickUp(MouseEventArgs e)
	{
		if (Time.time < lastClickDouble+ .1f) {
			return;
		
		}
		clickOverUI = isPointerOverUIObject ();

		switch (m_Mode) {
			case Mode.Menu:
			
				break;

		case Mode.Normal:
			//If we've just switched from another mode, don't execute
		
			if (m_Placed) {
				m_Placed = false;
				return;
			}
			//HIGHLY DANGEROUS CODE
			if (isPointerOverFloatingButton ()) {
				return;
			}
				//We've left clicked, have we left clicked on a unit?
			if (!currentObject) {
				break;
			}

				//if we're not dragging and clicked on a unit
			if (!m_GuiManager.Dragging && (currentObject.layer == 9 || currentObject.layer == 10)) {

				if (!clickOverUI) {

				/*  TARGET RULES
                    shift selects units without affecting others
                    control deselects units without affecting others
                */
					//deselect if none of the modifiers are being used
					if (!IsShiftDown ) {
                            SelectedManager.main.DeselectAll ();
					}

					//if only shift is down, remove the unit from selection
					if (IsControlDown ) {

					
						foreach (UnitManager obj in raceManager.getUnitOnScreen(true,currentObject.GetComponentInParent<UnitManager>().UnitName)) {
							if (!Input.GetKey (KeyCode.LeftAlt)) {
                                    SelectedManager.main.AddObject (obj);
							} else {
                                    SelectedManager.main.DeselectObject (obj);
							}
						}

					}
                //if only shift is down, add the unit to selection
                else if (!IsControlDown && IsShiftDown) {

                      SelectedManager.main.AddRemoveObject (getUnitManagerFromObject (currentObject));
					} 
				else {

                      SelectedManager.main.AddObject (getUnitManagerFromObject (currentObject));
					 }
                    SelectedManager.main.CreateUIPages (0);
				}
			}
            //or if we are dragging and clicked on empty air
			 else if(m_GuiManager.Dragging){
				bool Refresh = false;
						//Get the drag area
						Vector3 upperLeft = new Vector3 ();
						upperLeft.x = Math.Min (Input.mousePosition.x, originalPosition.x);
						upperLeft.y = Math.Max (Input.mousePosition.y, originalPosition.y);
						Vector3 bottRight = new Vector3 ();
						bottRight.x = Math.Max (Input.mousePosition.x, originalPosition.x);
						bottRight.y = Math.Min (Input.mousePosition.y, originalPosition.y);
	
						//if we're control-dragging, deselect everything in the drag area
				if (IsControlDown) {
					foreach (UnitManager obj in raceManager.getUnitSelection(upperLeft, bottRight)) {
                            SelectedManager.main.DeselectObject (obj);
						Refresh = true;
					}
				}
                //if we're shift-dragging, add everything in the drag area  
                else if (IsShiftDown)
                {
                        List<UnitManager> toAdd = raceManager.getUnitSelection(upperLeft, bottRight);
                        if (toAdd.Count > 0)
                        {
                            Refresh = true;
                            SelectedManager.main.AddObjects(raceManager.getUnitSelection(upperLeft, bottRight));
                        }
					
				}
                //if we're dragging, deselect everything, then add everything in the drag area
                else {
							
					List<UnitManager> unitSel = raceManager.getUnitSelection (upperLeft, bottRight);
					if (unitSel.Count > 0) {
						Refresh = true;
                            SelectedManager.main.DeselectAll ();
                            SelectedManager.main.AddObjects(unitSel);
  
					}
				}
						//refresh GUI elements
				if (Refresh) {
                        SelectedManager.main.CreateUIPages (0);
				}
			 }
			
				break;

		case Mode.targetAbility:
			
			if (!clickOverUI) {
                RaycastHit hitB = FindTarget();
     
				if (SelectedManager.main.checkValidTarget (hitB.point, currentObject, currentAbilityNUmber)) {
                        SelectedManager.main.fireAbility (currentObject, hitB.point, currentAbilityNUmber);

					if (!Input.GetKey (KeyCode.LeftShift)) {
						SwitchMode (Mode.Normal);
					}
				}
			}
			break;

		case Mode.globalAbility:

			if (!clickOverUI) {

                    RaycastHit hitB = FindTarget();
                    if (currentAbility.isValidTarget(currentObject, hitB.point))
                    {
                        ((TargetAbility)currentAbility).Cast(currentObject, hitB.point);
                        raceManager.castedGlobal(currentAbility);
                        if (currentAbility.chargeCount > 0)
                        {
                            switch (currentAbilityNUmber)
                            {
                                case 0:
                                    raceManager.useAbilityOne();
                                    break;
                                case 1:
                                    raceManager.useAbilityTwo();
                                    break;
                                case 2:
                                    raceManager.useAbilityThree();
                                    break;
                                case 3:
                                    raceManager.useAbilityFour();
                                    break;
                            }
                        }
                        else
                        {
                            SwitchMode(Mode.Normal);
                        }
                    }
			}
				break;
			
		case Mode.PlaceBuilding:
			if (!clickOverUI) {
                    if (tempBuildingPlacer.GetComponent<BuildingPlacer>().canBuild(currentObject))
                    {

                        if (!clickOverUI)
                        {
                            RaycastHit hit = Raycast(GroundsCast);
                            {
                                if (hit.collider)
                                {

                                    SelectedManager.main.fireAbility(m_ObjectBeingPlaced, hit.point, currentAbilityNUmber);

                                    if (!Input.GetKey(KeyCode.LeftShift))
                                    {
                                        SwitchMode(Mode.Normal);
                                        m_ObjectBeingPlaced = null;
                                    }
                                    else
                                    {
                                        m_ObjectBeingPlaced = null;
                                        SwitchToModePlacingBuilding(thingToBeBuilt, null);
                                    }
                                }
                            }
                        }
                    }
			}
			break;
		}		
	}

	public void DestroyGhost(GameObject obj)
	{
		Destroy (obj);
	}

	public bool allowDrag()
	{
		switch (m_Mode) {
		case Mode.Menu:
			return false;

		case Mode.globalAbility:
			return false;
		
		case Mode.targetAbility:
			return false;
	
		case Mode.PlaceBuilding:
			return false;

		}
		return true; // normal
	}

	//Called if a fule group of units is killed to see ifthey were in target mode.
	public void checkForDeadUnit(string unitName)
	{		
		if (unitName == currentTargetUnit) {
			if (m_Mode == Mode.targetAbility) {
                SelectedManager.main.stoptarget (currentAbilityNUmber);
				SwitchMode (Mode.Normal);
			}		
		}
	}

	string currentTargetUnit;

	public void setAbility(Ability abil, int n, string UnitName)
	{
        currentTargetUnit = UnitName;
		currentAbilityNUmber = n;
		currentAbility = (TargetAbility)abil;


        if (currentAbility.myTargetType == TargetAbility.targetType.unit)
        {
            AbilityTargeter.SetActive(false); // This might break with things like the vulcans targeted abilities
        }
        else
        {
            CursorManager.main.offMode();
            AbilityTargeter.SetActive(true);
            AbilityTargeter.GetComponentInChildren<Light>().enabled = !UseSpriteTarget;
            AbilityTargeter.GetComponentInChildren<Canvas>().enabled = UseSpriteTarget;
            if (UseSpriteTarget)
            {
                AbilityTargeter.GetComponentInChildren<SVGImage>().sprite = currentAbility.targetArea;
                AbilityTargeter.GetComponentInChildren<SVGImage>().transform.localScale = Vector3.one * currentAbility.areaSize;
            }
            else
            {             
                AbilityTargeter.GetComponentInChildren<Light>().cookie = currentAbility.targetArea.texture;
                AbilityTargeter.GetComponentInChildren<Light>().spotAngle = currentAbility.areaSize;
            }
        }
		Update ();
	}



    //A safer way to get a UnitManager
    private static UnitManager getUnitManagerFromObject(GameObject obj)
    {
        if (obj.GetComponent<UnitManager>())
            return obj.GetComponent<UnitManager>();
        else
            return obj.GetComponentInParent<UnitManager>();
    }


    public void RightButton_SingleClick(MouseEventArgs e)
	{
		rightClickDrag = false;
		if (hoverOver != HoverOver.Menu) {
			
			switch (m_Mode) {
			case Mode.Menu:
				break;

			case Mode.Normal:
			//We've right clicked, have we right clicked on ground, interactable object or enemy?

				if (currentObject.layer == 8 || currentObject.layer == 16 || currentObject.layer == 11) {
					//Terrain -> Move Command

                    RaycastHit hit = Raycast(EverythinCast);		
				
					if (hit.collider) {
						Vector3 attackMovePoint = hit.point;
			

						if (Vector2.Distance (rightClickOrigin, Input.mousePosition) > 45) {

                                SelectedManager.main.GiveMoveSpread (rightClickOrThree, attackMovePoint);

						} else {
								if (Input.GetKey(KeyCode.LeftControl))
								{
                                    SelectedManager.main.attackMoveO(Vector3.zero);
								}
								else
								{
                                    SelectedManager.main.GiveOrder(Orders.CreateMoveOrder(attackMovePoint));
								}
						}
					}
				} else if (currentObject.layer == 9 || currentObject.layer == 10 || currentObject.layer == 13) {

                        SelectedManager.main.GiveOrder (Orders.CreateInteractCommand (currentObject));						
				}
		
		
				break;

			case Mode.targetAbility:
                SelectedManager.main.stoptarget (currentAbilityNUmber);
				SwitchMode (Mode.Normal);				
				break;

			case Mode.globalAbility:
				SwitchMode (Mode.Normal);
				break;

			
			case Mode.PlaceBuilding:

			//Cancel building placement
				Destroy (m_ObjectBeingPlaced);
				m_ObjectBeingPlaced = null;
				buildingPlacer.SetActive (false);
				Destroy (tempBuildingPlacer);
				SwitchToModeNormal ();

				break;
			}
		}
	}


	List<GameObject> formationLights = new List<GameObject>();
	GameObject formationLight;
	Coroutine formationCoroutine = null;

    IEnumerator formationDisplay()
	{ 		
		yield return new WaitForSeconds (.01f);

		while (rightClickDrag && SelectedManager.main.ActiveObjectList ().Count > 0) {

            if (Vector2.Distance (Input.mousePosition, rightClickOrigin) > 45) {

                RaycastHit hitb = Raycast(GroundsCast);
                if (hitb.collider)
                {
                    rightClickEnd = hitb.point + Vector3.up * 2;
                }
                lineRender.enabled = true;
                lineRender.SetPositions(new Vector3[2] { rightClickOrThree, rightClickEnd });

                Formations.SetObjectPositions(formationLights, rightClickOrThree,rightClickEnd, formationLight);
				yield return null;
		
			}
            else
            {
                lineRender.enabled = false;
                
                for (int i = 0; i < formationLights.Count; i++) {
					formationLights [i].SetActive (false);
				}
			}
			yield return null;
		}
        lineRender.enabled = false;
        formationCoroutine = null;
		for (int i = 0; i < formationLights.Count; i++) {
			formationLights [i].SetActive (false);
		}

	}
  	

	//------------------------------------------------------------------------------------------
	
	private void ScrollWheelHandler(object sender, ScrollWheelEventArgs e)
	{
		m_Camera.Zoom (sender, e);
	}
	
	private void MouseAtScreenEdgeHandler(object sender, ScreenEdgeEventArgs e)
	{	
		m_Camera.Pan (sender, e);
	}
	
	//-----------------------------------KeyBoard Handler---------------------------------
	private void KeyBoardPressedHandler(object sender, KeyBoardEventArgs e)
	{
		e.Command();
	}

    //----------------------Mouse Button Handler------------------------------------
    private void ButtonClickedHandler(object sender, MouseEventArgs e)
    {
        e.Command();
    }


    //-------------------------------------------------------------------------------------

    public bool IsCurrentUnit(RTSObject obj)
	{
		return currentObject == obj.gameObject;
	}
	

	public void UserPlacingBuilding(GameObject item, UnitProduction abil, int i)
	{
		currentAbilityNUmber = i;
		SwitchToModePlacingBuilding(item, abil);
	}
	
	public void SwitchMode(Mode mode)
	{
        if (CurrentMode == Mode.targetAbility)
        {
            if (currentAbility)
                SelectedManager.main.stoptarget(currentAbilityNUmber);
        }
        switch (mode)
		{
		case Mode.Normal:
			SwitchToModeNormal ();
			break;
			
		case Mode.Menu:
                AbilityTargeter.SetActive (false);
			break;

		case Mode.targetAbility:
               // Debug.Log("Turning off " + currentAbility);
                    
                
			CursorManager.main.targetMode ();
               
			if (fastCast) { 
				StartCoroutine(fastCastMe());
			}

			break;
		

		case Mode.globalAbility:
			AbilityTargeter.SetActive (true);
			CursorManager.main.targetMode ();
			break;

		}
        m_Mode = mode;

    }

	IEnumerator fastCastMe()
	{
		yield return null;
		LeftButton_SingleClickUp (new LeftButton_Handler((int)Input.mousePosition.x,(int)Input.mousePosition.y, 0 ));
	}



	public void SwitchToModeNormal(bool destroyPlacer =  false)
	{
		if (m_Mode != Mode.Normal) {
			AbilityTargeter.SetActive (false);
			CursorManager.main.normalMode ();
			m_Mode = Mode.Normal;
			if (destroyPlacer &&  m_ObjectBeingPlaced) {
				Destroy (m_ObjectBeingPlaced);
			}
		}
	}

    
	public void SwitchToModePlacingBuilding(GameObject item, UnitProduction abil)
	{
        Debug.Log("Switching to here");
		thingToBeBuilt = item;
		if (m_Mode == Mode.PlaceBuilding) {
			if (m_ObjectBeingPlaced) {
				Destroy (m_ObjectBeingPlaced);
			}
		}
		m_Mode = Mode.PlaceBuilding;

		//Debug.Log ("Making a " + item);
		m_ObjectBeingPlaced = (GameObject)Instantiate (item);
		tempBuildingPlacer = (GameObject)Instantiate (buildingPlacer, m_ObjectBeingPlaced.transform.position, Quaternion.identity);

		tempBuildingPlacer .SetActive (true);
		BuildingPlacer p = tempBuildingPlacer.GetComponent<BuildingPlacer> ();
       
		p.reset (m_ObjectBeingPlaced, goodPlacement,  badPlacement, currentAbilityNUmber);
        if (abil)
        {
            abil.InitializeGhostPlacer(tempBuildingPlacer);
        }
        tempBuildingPlacer .transform.SetParent (m_ObjectBeingPlaced.transform);
		p.GetComponent<SphereCollider> ().enabled = true;

        foreach (MonoBehaviour behave in p.building.GetComponents<MonoBehaviour>())
        {
            Destroy(behave);
        }
	
	}

}

public enum HoverOver
{
	Terrain,
	Menu,
	Unit,
	Building,
	CamPlane,
	neutral
}

public enum InteractionState
{
	Nothing = 0,
	Invalid = 1,
	Move = 2,
	Attack = 3,
	Select = 4,
	Interact = 6
}

public enum Mode
{
	Normal,
	Menu,
	targetAbility,
	globalAbility,
	PlaceBuilding,
}
