using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbstractCost : MonoBehaviour {
		
		

	Coroutine currenCooldown;
	


	[HideInInspector]
	public ResourceManager resourceCosts;

		public float health;
		
		public float energy;

		public float cooldown;
		public float cooldownTimer;
		public bool StartsRefreshed = true;
		
		
		private UnitStats stats;

		private RaceManager myGame;

		//private Selected selectMan;



		public string UsedFor;
		
		
		// Use this for initialization
		void Start () {
		//selectMan = this.gameObject.GetComponent<Selected> ();
			
			if (!StartsRefreshed) {
				cooldownTimer = cooldown;
			if (currenCooldown == null)
			{
				currenCooldown = StartCoroutine(onCooldown());
			}
			}
			
			stats = this.gameObject.GetComponent<UnitStats> ();
		myGame = GameManager.main.getActivePlayer ();			
		}

	IEnumerator onCooldown()
	{	cooldownTimer = cooldown;
		//Debug.Log ("Resting cooldown " + cooldownTimer);
		while (true){
			yield return null;
		if (cooldownTimer > 0) {
			cooldownTimer -= Time.deltaTime;
			//	Debug.Log ("Colling " + cooldownTimer + "   " +UsedFor + "  " + this.gameObject);
			//selectMan.updateCoolDown (cooldownTimer / cooldown);
		}
		else
			{cooldownTimer = 0;
			break;
		}
	}

	}

	public void showCostPopUp(bool positive)
	{
		resourceCosts.showPopups(transform.position, positive);
	}


	public bool canActivate(Ability ab, continueOrder order, bool showError)
	{
		bool result = true;
		if (!ab.active) {
			result = false;
		}

		ResourceType canPay = myGame.resourceManager.canPay(resourceCosts.MyResources);

		if (canPay != ResourceType.CanPay) {

			if (showError) {
				ErrorPrompt.instance.notEnoughResource();
			}
			order.InsufficientResources.Add(canPay);
			order.nextUnitCast = false;

			result = false;
		}

	
		if (stats.health < health ) {
			order.reasonList.Add (continueOrder.reason.health);
			result =  false;
		}

		if (stats.currentEnergy < energy)
		{
			order.reasonList.Add(continueOrder.reason.energy);
			result = false;
			if (showError)
			{
				ErrorPrompt.instance.notEnoughEnergy();
			}
		}

	
		if (cooldown > 0 && cooldownTimer > 0) {
			order.reasonList.Add (continueOrder.reason.cooldown);
			result = false;
			if (showError) {
				ErrorPrompt.instance.onCooldown ();
			}
		}
			
			return result;	
	}


	public bool canActivate(Ability ab)
	{
		if (!ab.active) {
			return  false;}

		if (myGame.resourceManager.canPay(resourceCosts.MyResources) != ResourceType.CanPay){
			return false;
		}

		if (stats) {
			if (stats.health < health) {

				return false;
			}

			if (stats.currentEnergy < energy) {

				return false;
			}
		}

		if (cooldownTimer > 0) {
			
			return false;}
	
		return true;

	}



	public void resetCoolDown()
	{cooldownTimer = 0;
	}
		
	/// <summary>
	/// Cooldowns the progress. 0 = cooldown just started, 1 = its done
	/// </summary>
	/// <returns>The progress.</returns>
	public float cooldownProgress()
	{
		return (1 - cooldownTimer / cooldown);
		}
	
	public void refundCost()
	{
		//Debug.Log ("Refunding");
		myGame.collectResources(resourceCosts.MyResources, false);
	//	Debug.Log ("Refunding " + this.gameObject);
		cooldownTimer = 0;
	}


	public void startCooldown()
	{
		if (currenCooldown != null) {
			StopCoroutine (currenCooldown);
		}
		currenCooldown =  StartCoroutine (onCooldown());
	}

	public void payCost ()
	{
		myGame.PayCost(resourceCosts.MyResources);
		if (stats) {

			if (health > 0) {

				stats.TakeDamage (health, this.gameObject, DamageTypes.DamageType.True);
			}
			if (energy > 0) {
				stats.changeEnergy (-energy);

			}
		}
		startCooldown ();
	}
}
