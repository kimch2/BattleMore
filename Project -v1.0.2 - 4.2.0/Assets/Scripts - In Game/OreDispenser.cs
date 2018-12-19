using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class OreDispenser : MonoBehaviour {


	public float OreRemaining;


	public GameObject currentMinor;
	
	// used for increased mining
	public float returnRate = 1;
	public float efficiency = 1;

	public UnitManager MyStructure;


	public bool requestWork(GameObject obj)
	{
		if (!currentMinor) {
			currentMinor = obj;
			return true;
		}
		return false;
	}

	public void assignBuilding(UnitManager manager)
	{
		MyStructure = manager;
	}

	public bool confirmCanMine(string structureName)
	{
		if (string.IsNullOrEmpty(structureName) && MyStructure == null)
		{
			return true;
		}
		else if (string.IsNullOrEmpty(structureName))
		{
			return false;
		}
		else if (MyStructure.UnitName == structureName)
		{
			return true;
		}
		else
		{
			return false;
		}
	}



	public float getOre( float amount)
	{
		float giveBack = Mathf.Min (OreRemaining, amount * returnRate);
		OreRemaining -= giveBack;
		if (OreRemaining <= .5) {

			ErrorPrompt.instance.OreDepleted(transform.position);
			SelectedManager.main.DeselectObject (GetComponent<UnitManager> ());
			AugmentAttachPoint AAP = GetComponent<AugmentAttachPoint> ();
			if (AAP && AAP.myAugment) {
			AAP.myAugment.GetComponent<Augmentor> ().Unattach ();
			}
			Destroy (this.gameObject);
		}
		return Mathf.Min (OreRemaining, giveBack) * efficiency;

	}

}
