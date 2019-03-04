using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


public class Selected : MonoBehaviour {
		
	public bool IsSelected
	{
		get;
		private set;
	}

	public DisplayBar healthBar;
	public DisplayBar EnergyBar;
	public DisplayBar Cooldownbar;



	public HealthDisplay buffDisplay;
	public TurretHealthDisplay turretDisplay;

	private GameObject unitIcon;
	private UnitIconInfo IconInfo;

	private Slider IconSlider;

	public GameObject RallyPoint;
	public GameObject RallyUnit;

	UnitManager manager;

	private float tempSelectTime;
	private bool tempSelectOn;
	private bool interactSelect;
	public List<SelectionNotifier> selectionNotifiers = new List<SelectionNotifier>();
	public LineRenderer myLine;

	List<GameObject> UsableBars = new List<GameObject>();
	[Tooltip("This is used if something else is piggybacking the UI display and will handle it turning on/off")]
	public bool outsideAttchement;

	public enum displayType
	{
		always,damaged,selected,never
	}


	public displayType mydisplayType = displayType.damaged;

	public GameObject decalCircle;
	private UnitStats myStats;

	[Tooltip("by default only Player 1 units have fogOfWarRevealed")]
	public bool ManualFogOfWar; 
	//private bool onCooldown = false;
	// Use this for initialization
	void Awake () 
	{		
		Initialize ();
	
	}

	public SpriteRenderer MinimapIcon;

	private void Start()
	{
		manager = GetComponent<UnitManager>();
		if (!MinimapIcon)
		{
			GameObject obj = new GameObject("MinimapIcon");
			obj.transform.parent = this.transform;
			obj.transform.localPosition = manager.CharController.center;
			MinimapIcon = obj.AddComponent<SpriteRenderer>();
			MinimapIcon.transform.LookAt(MinimapIcon.transform.position + Vector3.up);
			MinimapIcon.color = GameManager.main.playerList[manager.PlayerOwner - 1].getColor();
			MinimapIcon.gameObject.layer = 21;
			if (manager.myStats.isUnitType(UnitTypes.UnitTypeTag.Structure))
			{
				MinimapIcon.transform.localScale *= 175;
				MinimapIcon.sprite = UISetter.main.defaultMinimapSquare;
			}
			else if (manager.myStats.isUnitType(UnitTypes.UnitTypeTag.Turret))
			{
				MinimapIcon.sprite = UISetter.main.defaultMinimapcircle;
			}
			else
			{
				MinimapIcon.sprite = UISetter.main.defaultMinimapcircle;
				MinimapIcon.transform.localScale *= 100;
			}
		}
	}


	public void attackBlink()
	{
		StartCoroutine(Blinker());
	}

	IEnumerator Blinker()
	{
		MinimapIcon.transform.localScale += Vector3.one * 40;
		for (int i = 0; i < 3; i++)
		{
			MinimapIcon.color = Color.yellow;
			yield return new WaitForSeconds(.2f);
			MinimapIcon.color = Color.black;
			yield return new WaitForSeconds(.2f);
		}
		MinimapIcon.transform.localScale -= Vector3.one * 40;
		if (IsSelected)
		{
			MinimapIcon.color = Color.white;
		}
		else
		{
			MinimapIcon.color = manager.myRacer.getColor();
		}
	}









	public void Initialize()
	{if (!myLine) {
			myLine = GetComponent<LineRenderer> ();
		}
		IsSelected = false;

		myStats = this.gameObject.GetComponent<UnitStats> ();
		if (!decalCircle) {
			decalCircle = this.gameObject.transform.Find ("DecalCircle").gameObject;
		}
		if (!buffDisplay) {
			buffDisplay = GetComponentInChildren<HealthDisplay> ();
		}

		turretDisplay = GetComponentInChildren<TurretHealthDisplay> ();
		if (turretDisplay && turretDisplay.transform.parent != this.transform) {
			turretDisplay = null;
		}
		
		if (!turretDisplay) {
				
			if (healthBar) {
				UsableBars.Add (healthBar.gameObject);
			}
		} else {
			healthBar.gameObject.SetActive (false);
		}

	
		if(EnergyBar){
			UsableBars.Add(EnergyBar.gameObject);
		}

		if(Cooldownbar){
			UsableBars.Add(Cooldownbar.gameObject);
		}

		if (myStats.MaxEnergy == 0) {
			updateEnergyBar (1);
		} else {
			updateEnergyBar (myStats.currentEnergy / myStats.MaxEnergy);
		}

		updateCoolDown (1);
		updateHealthBar (myStats.health / myStats.Maxhealth);
		mydisplayType = displayType.damaged;
		//setDisplayType (GamePlayMenu.getInstance().getDisplayType ());

	}



	public void setDisplayType(displayType t)
	{mydisplayType = t;
		
		/*
		try{
		if (!turretDisplay) {


			switch (t) {
			case displayType.always: 
				buffDisplay.enabled = true;
				healthslider.enabled = true;
				energySlider.gameObject.SetActive (myStats.MaxEnergy > 0);
				
				break;

			case displayType.damaged:
				if (!myStats.atFullHealth ()) {
						
					buffDisplay.enabled = true;
					healthslider.gameObject.SetActive (true);
				} else {
					buffDisplay.enabled = false;
					healthslider.gameObject.SetActive (false);
				}

					if (myStats.MaxEnergy > 0 && !myStats.atFullEnergy()) {
					energySlider.gameObject.SetActive (true);
					buffDisplay.enabled = true;
				}
					else
					{
						energySlider.gameObject.SetActive (false);
						buffDisplay.enabled = false;
					}

				break;

			case  displayType.selected:
				if (IsSelected) {
					buffDisplay.enabled = true;
					healthslider.gameObject.SetActive (true);
					if (myStats.MaxEnergy > 0) {
						energySlider.gameObject.SetActive (true);
					}
				}
				break;

			case displayType.never:
				buffDisplay.enabled = false;
				healthslider.gameObject.SetActive (false);
				energySlider.gameObject.SetActive (false);
				break;
			}
		}
		}catch(Exception) {
		}
		*/
	}

	public void updateIconNum()
	{
		if (IconInfo) {
			IconInfo.updateNum ();
		}
	}

	public void setIcon(GameObject obj)
	{//buffDisplay.isOn = true;
		if (!obj) {
			unitIcon = null;
			return;}
		

		unitIcon = obj.transform.Find("UnitIconTemplate").gameObject;

		IconInfo = unitIcon.GetComponent<UnitIconInfo> ();
		IconSlider = obj.transform.Find ("Slider").gameObject.GetComponent<Slider>();

		updateHealthBar (myStats.health / myStats.Maxhealth);
		if (!turretDisplay) {
			//Debug.Log (this.gameObject);


			if (IconSlider) {
				IconSlider.value = Cooldownbar.getRatio();
				if (IconSlider.value <= 0 || IconSlider.value >= .99f) {
					//buffDisplay.isOn = false;

					IconSlider.gameObject.SetActive (false);
				} else {
					IconSlider.gameObject.SetActive (true);
				}
			}
		}



	}

	public void interact()
	{
		StartCoroutine (delayTurnOff ());
	
	}


	IEnumerator delayTurnOff()
	{
		decalCircle.GetComponent<MeshRenderer> ().enabled = true;
		interactSelect = true;
		yield return new WaitForSeconds(.13f);
		interactSelect = false;
		decalCircle.GetComponent<MeshRenderer> ().enabled = false;
		yield return new WaitForSeconds(.13f);
		decalCircle.GetComponent<MeshRenderer> ().enabled = true;
		yield return new WaitForSeconds(.13f);
		decalCircle.GetComponent<MeshRenderer> ().enabled = false;
		yield return new WaitForSeconds(.13f);
		decalCircle.GetComponent<MeshRenderer> ().enabled = true;
		yield return new WaitForSeconds(.13f);

		if(!IsSelected && !tempSelectOn)
		{
			decalCircle.GetComponent<MeshRenderer> ().enabled = false;
		}
	}

	public void tempSelect()
	{

		tempSelectTime = Time.time + .08f;
		if (!tempSelectOn) {

			StartCoroutine (CurrentlyTempSelect ());
			tempSelectOn = true;

			decalCircle.GetComponent<MeshRenderer> ().enabled = true;
			if (RallyPoint) {
				RallyPoint.SetActive (true);
			}
			if (myLine) {
				myLine.enabled = true;
			}


			foreach (Transform obj in this.transform) {
				
				obj.SendMessage ("setSelect", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	
	IEnumerator CurrentlyTempSelect()
	{


		while (tempSelectTime > Time.time) {
			
			if (myLine && RallyUnit) {
				myLine.SetPositions (new Vector3[]{ this.gameObject.transform.position + Vector3.up *4, RallyUnit.transform.position + Vector3.up });
				RallyPoint.transform.position = RallyUnit.transform.position;
			}
			yield return 0;
		}

		if (!IsSelected) {
	
			if (!interactSelect) {

				decalCircle.GetComponent<MeshRenderer> ().enabled = false;
			}
			if (RallyPoint) {
				RallyPoint.SetActive (false);
			}
			if (myLine) {
				myLine.enabled = false;
			}


			foreach (Transform obj in this.transform) {

				obj.SendMessage ("setDeSelect", SendMessageOptions.DontRequireReceiver);
			}


		}

			
		tempSelectOn = false;

		}

	public void updateHealthBar(float ratio)
	{
		if (!turretDisplay) {
			healthBar.updateRatio (ratio, IconInfo, null);
		} else {
			turretDisplay.updateHealth (ratio);
		}

		checkOn ();


	}

	public void updateEnergyBar(float ratio)
	{
		EnergyBar.updateRatio(ratio, null, null);
		checkOn ();
	}

	public void checkOn()
	{

		if (outsideAttchement) {
			buffDisplay.enabled = true;
			return;
		}

		foreach (GameObject obj in UsableBars) {
			if (obj.activeSelf) {
				buffDisplay.enabled = true;
				return;
			}
		}
		buffDisplay.enabled = false;
	}

	public void updateCoolDown(float ratio)
	{
		Cooldownbar.updateRatio (ratio, null, IconInfo);
		checkOn ();

	}

	public void setCooldownColor(Color c)
	{
		Cooldownbar.RatioLevels[0].HPBarColor = c;
	}


	public void SetSelected()
	{
		IsSelected = true;

		StartCoroutine (currentlySelected());
		decalCircle.GetComponent<MeshRenderer> ().enabled = true;
		MinimapIcon.color = Color.white;
		if (RallyPoint) {
			RallyPoint.SetActive (true);
			if (myLine) {
				myLine.enabled = true;
			}
		}
	}

	IEnumerator currentlySelected()
	{
		while (IsSelected) {

			if (myLine && RallyUnit) {
				myLine.SetPositions (new Vector3[]{ this.gameObject.transform.position + Vector3.up *4, RallyUnit.transform.position + Vector3.up });
				RallyPoint.transform.position = RallyUnit.transform.position;
			}
			yield return 0;
		}
	
	}
	
	public void SetDeselected()
	{
		IsSelected = false;
		unitIcon = null;
		IconSlider = null;
		IconInfo = null;
		MinimapIcon.color =manager.myRacer.getColor();
		decalCircle.GetComponent<MeshRenderer> ().enabled = false;
		if (RallyPoint) {
			RallyPoint.SetActive (false);
			if (myLine) {
				myLine.enabled = false;
			}
		}
			

		foreach (Transform obj in this.transform) {
			obj.SendMessage ("setDeSelect", SendMessageOptions.DontRequireReceiver);
		}

	}

}
	

class AlwaysHealthBar: IHealthBarState
{
	public void UpdateHealthBar (float m)
	{
		
	}
	public void UpdateCoolDown (float m)
	{
		
	}
	public void updateEnergy(float m)
	{
		
	}
}