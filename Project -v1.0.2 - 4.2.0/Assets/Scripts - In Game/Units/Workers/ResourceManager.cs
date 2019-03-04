using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ResourceType { Ore, Waste, Life, Energy, Structure, CanPay } // Can Pay is used as a way of lettign the UI know that a specific kind of type is needed (by saying none of them are needed)

[System.Serializable]
public class ResourceManager {

	public List<ResourceTank> MyResources = new List<ResourceTank>();

	public void showPopups(Vector3 location, bool positive)
	{
		int i = 0;
		foreach (ResourceTank tank in MyResources)
		{
			if (tank.currentAmount != 0)
			{
				PopUpMaker.CreateGlobalPopUp((positive ? "+" : "-") + tank.currentAmount, UnitEquivalance.getResourceInfo(tank.resType).ResourceColor, location - Vector3.up * (-8 + i * 4));

				i++;
			}
		}
	}


	/// <summary>
	/// Reduce cost by 25% by inputing .25
	/// </summary>
	/// <param name="amount"></param>
	public void reduceCostPercentage(float amount)
	{
		foreach (ResourceTank tank in MyResources)
		{
			tank.currentAmount -= tank.currentAmount * amount;
		}
	}

	public void CollectResources(List<ResourceTank> myTypes )
	{
		foreach (ResourceTank tank in myTypes)
		{
			collectResource(tank.resType, tank.currentAmount);
		}
	}

	public void PayCost(List<ResourceTank> myTypes)
	{
		foreach (ResourceTank tank in myTypes)
		{
			PayCost(tank);
		}
	}

	public void PayCost(ResourceTank myType)
	{
		MyResources.Find(item => item.resType == myType.resType).payCost(myType.currentAmount);
	}



	public void RefundResources(List<ResourceTank> myTypes)
	{
		foreach (ResourceTank tank in myTypes)
		{
			refundResource(tank.resType, tank.currentAmount);
		}
	}

	public void collectResource(ResourceType resType, float amount)
	{
		ResourceTank tank = MyResources.Find(item => item.resType == resType);
		if (tank != null)
		{
			tank.HarvestResource(amount);
		}
		else {
			MyResources.Add(new ResourceTank(resType, amount));
			MyResources.Find(item => item.resType == resType).HarvestResource(amount);
		}
	}

	public void refundResource(ResourceType resType, float amount)
	{
		ResourceTank tank = MyResources.Find(item => item.resType == resType);
		if (tank != null)
		{
			tank.refundResource(amount);
		}
		else
		{
			MyResources.Add(new ResourceTank(resType, amount));
		}
	}

	public static string ResourceToString(ResourceType myType)
	{
		string s = myType.ToString();
		s = s.Replace('_', ' ');
		return s;
	}


	public float getResource(ResourceType myType)
	{
		return MyResources.Find(item => item.resType == myType).currentAmount;
	}

	public bool ICanPay()
	{
		foreach (ResourceTank tank in MyResources)
		{
			if (GameManager.main.getActivePlayer().resourceManager.canPay(MyResources).Count > 0)
				{ return false; }

		}

		return true;
	}

	public List<ResourceType>  canPay(List<ResourceTank> myCosts)
	{
		List<ResourceType> cantPay = new List<ResourceType>();
		foreach (ResourceTank tank in myCosts)
		{
			if (MyResources.Find(item => item.resType == tank.resType).currentAmount < tank.currentAmount)
			{
				cantPay.Add(tank.resType);
}
			
		}

		return cantPay;
	}

	public bool canPay(ResourceType myType, float amount)
	{
		return MyResources.Find(item => item.resType == myType).currentAmount >= amount;
	}

}

[System.Serializable]
public class ResourceTank
{
	public ResourceType resType;
	public float currentAmount;
	[HideInInspector]
	public float totalHarvested;

	public ResourceTank(ResourceType typ, float amount)
	{
		resType = typ;
		currentAmount = amount;
	}

	public void HarvestResource(float amount)
	{
		currentAmount += amount;
		totalHarvested += amount;
	}

	public void refundResource(float Amount)
	{
		currentAmount += Amount;
	}

	public void payCost(float amount)
	{
		currentAmount -= amount;
	}

}
