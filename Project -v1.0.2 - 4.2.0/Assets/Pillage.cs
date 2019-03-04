using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillage : MonoBehaviour , Notify {


	public ResourceManager ToCollect;
	public float moneyPerAttack;
	public bool showPopup = true;

	public bool PillageOnlyDeposits;
	public float oreEfficency = .75f;

	UnitManager myManager;

	float DiffAmount;
	void Start () {

		myManager = GetComponent<UnitManager>();
		if (myManager.PlayerOwner == 2)
		{
			PillageOnlyDeposits = false;
		}
		if (PillageOnlyDeposits)
		{
			GetComponent<AudioSource>().volume *= .3f;
			if (myManager.enemies.Count == 0)
			{
				Invoke(	"findNewSpot", .5f);
			}
			DiffAmount = (int)((ToCollect.MyResources[0].currentAmount / oreEfficency) - ToCollect.MyResources[0].currentAmount);
		}
		this.GetComponent<IWeapon> ().triggers.Add (this);
	}




	public float trigger(GameObject source, GameObject projectile,UnitManager target, float damage)
	{
		if (!target.myStats.isUnitType(UnitTypes.UnitTypeTag.Invulnerable) && !PillageOnlyDeposits)
		{

			GameManager.main.activePlayer.collectResources(ToCollect.MyResources, false);
			if (showPopup) { 
				ToCollect.showPopups(this.transform.position + Vector3.up * 2, true);
			}
		}
	

		else
		{
			OreDispenser dispense = target.GetComponent<OreDispenser>();
			if (dispense && PillageOnlyDeposits)
			{
				
				dispense.OreRemaining -= DiffAmount;
	
				dispense.getOre((int)ToCollect.MyResources[0].currentAmount);
				GameManager.main.activePlayer.collectResources(ToCollect.MyResources, true);
				if (showPopup)
				{
				ToCollect.showPopups(this.transform.position + Vector3.up * 2, true);
				}
			
				if (!dispense || dispense.OreRemaining <= 0)
				{
					Invoke("findNewSpot", .2f);
				}
			}
		}

		//popper.CreatePopUp ("+" + (int)(damage * percentage), Color.gray); 
		return damage;
	}



	void findNewSpot()
	{
		float distance = 200;

		OreDispenser closest = null;

		foreach (KeyValuePair<string, List<UnitManager>> pair in GameManager.main.playerList[2].getUnitList())
		{
			foreach (UnitManager obj in pair.Value)
			{

				if (FogOfWar.current.IsInCompleteFog(obj.transform.position))
				{
					continue;
				}
				OreDispenser dis = obj.GetComponent<OreDispenser>();

				if (!dis)
				{
					continue;
				}

				float temp = Vector3.Distance(obj.transform.position, this.gameObject.transform.position);
				if (temp < distance)
				{
					distance = temp;
					closest = dis;
				}
			}
		}
		if (closest != null)
		{
			myManager.GiveOrder(Orders.CreateInteractCommand(closest.gameObject,true));
		}
	}


}