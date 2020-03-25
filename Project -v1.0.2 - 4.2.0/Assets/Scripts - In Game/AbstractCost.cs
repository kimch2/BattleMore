using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbstractCost : MonoBehaviour {
		
	Coroutine currenCooldown;
	public bool showCooldown;
	
	//[HideInInspector]
	public ResourceManager resourceCosts;

	public float health;
		
	public float energy;

	public float cooldown;
   // [HideInInspector]
	public float cooldownTimer;
	public bool StartsRefreshed = true;
		
		
	private UnitStats stats;

	UnitManager manager;

	public string UsedFor;
		
		
		// Use this for initialization
	public void Start () {
		//selectMan = this.gameObject.GetComponent<Selected> ();
			
		if (!StartsRefreshed) {
			cooldownTimer = cooldown;
			if (currenCooldown == null)
			{
				currenCooldown = StartCoroutine(onCooldown());
			}
		}
			
		stats = this.gameObject.GetComponent<UnitStats> ();
        if (!stats)
        {
            stats = GetComponentInParent<UnitStats>();
        }
		manager = GetComponent<UnitManager>();
        if (!manager)
        {
            manager = GetComponentInParent<UnitManager>();
        }
	}

	IEnumerator onCooldown()
	{	cooldownTimer = cooldown;
		//Debug.Log ("Resting cooldown " + cooldownTimer);
		while (true){
			yield return null;
			if (cooldownTimer > 0) {
				cooldownTimer -= Time.deltaTime;
				//	Debug.Log ("Colling " + cooldownTimer + "   " +UsedFor + "  " + this.gameObject);
				if (showCooldown)
				{
					manager.myStats.getSelector().updateCoolDown(cooldownTimer / cooldown);
				}
			}
			else
			{
                cooldownTimer = 0;
             //   Debug.Log("Breaking");
				break;
			}
		}
		if (showCooldown)
		{
			manager.myStats.getSelector().updateCoolDown(cooldownTimer / cooldown);
		}
	}

	public void StartCustomCooldown(float customTime)
	{
		if (currenCooldown != null)
		{
			StopCoroutine(currenCooldown);
		}
		currenCooldown = StartCoroutine(customCooldown(customTime));
	}

	IEnumerator customCooldown(float timer)
	{
		float previousTime = cooldown;
		cooldown = timer;
		cooldownTimer = timer;
		//Debug.Log ("Resting cooldown " + cooldownTimer);
		while (true)
		{
			yield return null;
			if (cooldownTimer > 0)
			{
				cooldownTimer -= Time.deltaTime;
			//	Debug.Log ("CollingBB " + cooldownTimer + "  " + timer+  "   " +UsedFor + "  " + this.gameObject);
				if (showCooldown)
				{
					manager.myStats.getSelector().updateCoolDown(cooldownTimer / timer);
				}
			}
			else
			{
				cooldownTimer = 0;
				break;
			}
		}
		if (showCooldown)
		{
			manager.myStats.getSelector().updateCoolDown(cooldownTimer / cooldown);
		}
		cooldown = previousTime;
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

		List<ResourceType> canPay = manager.myRacer.resourceManager.canPay(resourceCosts.MyResources);

		if (canPay.Count > 0) {

			if (showError) {
				ErrorPrompt.instance.notEnoughResource(canPay[0]);
			}
			order.InsufficientResources = canPay;
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


        if (cooldown > 0 && cooldownTimer > 0)
        {

                order.reasonList.Add(continueOrder.reason.cooldown);
                result = false;
                if (showError)
                {
                    ErrorPrompt.instance.onCooldown();
                }
            
        }
			
		return result;	
	}


	public bool canActivate(Ability ab)
	{
		if (!ab.active) {
			return  false;}

		if (manager)
		{
			if (manager.myRacer.resourceManager.canPay(resourceCosts.MyResources).Count > 0)
			{
				return false;
			}
		}
		else if(GameManager.main.activePlayer.resourceManager.canPay(resourceCosts.MyResources).Count > 0) // Fix this once  I do enemy AI that use ults
		{
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

		if (cooldownTimer > 0 && ab.chargeCount <= 0)
        {			
			return false;
        }

        if(ab.chargeCount == 0 && ab.maxChargeCount > 0)
        {
            Debug.Log("Returning false");
            return false;
        }

        return true;

	}



	public void resetCoolDown()
	{cooldownTimer = 0;
        Debug.Log("Resetting cooldown");
	}

	void beginCooldown()
	{
		if (currenCooldown != null)
		{
			StopCoroutine(currenCooldown);
		}
		currenCooldown = StartCoroutine(onCooldown());
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
		manager.myRacer.collectResources(resourceCosts.MyResources, false);
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
		if (manager)
		{
			manager.myRacer.PayCost(resourceCosts.MyResources);
		}
		else
		{
			GameManager.main.activePlayer.PayCost(resourceCosts.MyResources);// Fix this once  I do enemy AI that use ults
		}
		if (stats) {
			if (health > 0) {

				stats.PayLife (health);
			}
			if (energy > 0) {
				stats.changeEnergy (-energy);

			}
		}
		startCooldown ();
	}
}
