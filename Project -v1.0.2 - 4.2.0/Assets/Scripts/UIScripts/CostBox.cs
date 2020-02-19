using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CostBox : MonoBehaviour {


	public Text MyName;
	public Text time;
	public Text description;
	public Image clocker;
	public Text Population;
	public GameObject CostPrefab;
	protected Lean.LeanPool myPool;
	public Transform CostGrid;

	List<GameObject> inUseCosts = new List<GameObject>();
	Color teal = new Color(.698f, .949f, 255);


	public static CostBox instance;
	 
	Canvas myCanvas;
	void Awake()
	{
		myCanvas = GetComponent<Canvas> ();
		instance = this;

	}

	private void Start()
	{
		myPool = Lean.LeanPool.getSpawnPool(CostPrefab);
	}

	public void turnOff()
	{
		myCanvas.enabled = false;
	}

	bool canBuild = true;
	public void setText(Ability input)
	{
		canBuild = true;
		if (input == null) {
			return;}
		continueOrder order = input.canActivate (false);
		
		MyName.text = input.Name;

	
		if (input.myCost) {
			// CLOCK ===========================================

			if (input is UnitProduction) {
				
				clocker.enabled = true;
				time.text = "" + ((UnitProduction)input).buildTime;
			}

			else if (input.myCost.cooldown == 0) {
				clocker.enabled = false;
				time.text = "";
			} else {
				if (order.reasonList.Contains (continueOrder.reason.cooldown)) {
					time.color = Color.red;
					canBuild = false;
				} else {
					
					time.color =teal;
				}
				clocker.enabled = true;

				if (input.myCost.cooldownTimer > 0) {
					time.text =  (int)(input.myCost.cooldown - input.myCost.cooldownTimer) + "/" + input.myCost.cooldown;
				} else {
					time.text = "" + input.myCost.cooldown;
				}
			}
			if (input is UnitProduction && ((UnitProduction)input).unitToBuild) {
				UnitStats mwertqert = ((UnitProduction)input).unitToBuild.GetComponent<UnitStats> ();

				float sup =	mwertqert.supply;
				if (sup > 0) {
					
						Population.text = "Pop: " + sup;

				} else {
					Population.text = "";
				}
				if (sup > GameManager.getInstance ().activePlayer.supplyMax - GameManager.getInstance ().activePlayer.currentSupply) {
					canBuild = false;
					Population.color = Color.red;
				} else {
					Population.color = teal;
				}

			} else {
				Population.text = "";
			}
			foreach (GameObject tank in inUseCosts)
			{
				myPool.FastDespawn(tank);
			}
			inUseCosts.Clear();

			foreach (ResourceTank tank in input.myCost.resourceCosts.MyResources)
			{
				CreateCostSettings(Mathf.Abs(tank.currentAmount).ToString(), UnitEquivalance.getResourceInfo(tank.resType).icon, order.InsufficientResources.Contains(tank.resType) ?  Color.red : teal);
			}

			if (input.myCost.energy > 0)
			{
				CreateCostSettings(input.myCost.energy.ToString(), UnitEquivalance.getResourceInfo(ResourceType.Energy).icon, order.reasonList.Contains(continueOrder.reason.energy) ? Color.red : teal);
			}

			if (input.myCost.health > 0)
			{
				CreateCostSettings(input.myCost.health.ToString(), UnitEquivalance.getResourceInfo(ResourceType.Life).icon, order.reasonList.Contains(continueOrder.reason.health) ? Color.red : teal);
			}

			if (input.RequiredUnit.Count > 0)
			{

				if (order.reasonList.Contains(continueOrder.reason.requirement))
				{

					string s = "Req: ";
					foreach (string n in input.RequiredUnit)
					{
						if (!s.Equals("Req: "))
						{
							s += ", ";
						}
						s += n;
					}

					CreateCostSettings(s, UnitEquivalance.getResourceInfo(ResourceType.Structure).icon, order.reasonList.Contains(continueOrder.reason.health) ? teal : Color.red);
				}
			}


		} else {
			Population.text = "";
			time.text = "";
			clocker.enabled = false;
		
		}

		if (canBuild) {
			MyName.color = teal;
		} else {
			MyName.color = Color.red;
		}
		description.text = input.Descripton;


	}


	GameObject  CreateCostSettings(string text, Sprite icon, Color c)
	{
		GameObject obj = myPool.FastSpawn(Vector3.zero, Quaternion.identity, CostGrid);
		inUseCosts.Add(obj);
		Text comp = obj.GetComponentInChildren<Text>();
		comp.text = text;
		obj.GetComponentInChildren<Image>().sprite = icon;
		comp.color = c;
		if (c == Color.red)
		{ canBuild = false; }
		return obj;
	}



}
