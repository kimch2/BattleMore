using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SpecificUpgrade : Upgrade {

	[Tooltip("This list is applied at runtime and contains all the units the upgrade applies to")]
	public List<string> unitsToApply = new List<string>();
	[Tooltip("This is all the Untis it can possible apply to")]
	public List<RaceInfo.unitType> ApplicableUnits;
	override
	public void applyUpgrade(GameObject obj)
	{	
	}

	override
	public void unApplyUpgrade(GameObject obj)
	{
	}

	protected bool confirmUnit(GameObject obj)
	{
		UnitManager manage = obj.GetComponent<UnitManager> ();
		//Debug.Log("Confirming " + manage.UnitName);
		if (unitsToApply.Contains (manage.UnitName)) {
			return true;
		}
		return false;
	}

	public abstract float ChangeString (string name, float number);

	public virtual string AddString (string name, string ToAddOn)
	{
		return ToAddOn;
	}


}
