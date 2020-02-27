using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class explosion : MonoBehaviour {


	public GameObject source;
	public int sourceInt = 1;
	public GameObject particleEff;
	public float friendlyFireRatio;
	public float damageAmount;
	public DamageTypes.DamageType type;
	public float maxSize= 5.0f;
	VeteranStats vetStats;
	public bool UseYFOrDetection;
	public List<Notify> triggers = new List<Notify> ();
    [Tooltip("The time between the spawn of this object (and the above effect) and when the damage is applied")]
    public float DamageDelay = .01f;
	float friendlyAmount;
	float sizeSquared;
	public IWeapon.bonusDamage[] extraDamage;

	UnitManager mySrcMan;

	// Use this for initialization
	IEnumerator Start () {

	//	Debug.Log("Starting");
		if (particleEff) {
			GameObject obj = 	(GameObject)Instantiate (particleEff, this.gameObject.transform.position, Quaternion.identity);
			obj.SendMessage ("setOwner", sourceInt, SendMessageOptions.DontRequireReceiver);

		}
        //transform.localScale = Vector3.one * maxSize;
        yield return new WaitForSeconds(DamageDelay);
		sizeSquared = maxSize;
		FindTargets();
		yield return null;

		Destroy (this.gameObject);

	}


	public void setSource(GameObject sr)
	{
		source = sr;
		if (source) {
			mySrcMan = source.GetComponent<UnitManager> ();
		}
		if (mySrcMan) {
			sourceInt = mySrcMan.PlayerOwner;
		}
	}

	public void setVeteran(VeteranStats sr)
	{

		mySrcMan = sr.myUnit;
		vetStats = sr;

		if (mySrcMan) {
			source = sr.myUnit.gameObject;
		}
		sourceInt = sr.playerOwner;
	}

	public void setDamage(float amount)
	{
		damageAmount = amount;
		friendlyAmount = damageAmount * friendlyFireRatio;
	}

	/*
	void OnTriggerEnter(Collider other)
	{

		if (other.isTrigger) {
			return;}
	
		UnitManager manager = other.gameObject.GetComponent<UnitManager> ();

		if (manager) {
			if (manager.PlayerOwner == sourceInt)
			{
				DealDamage(manager, damageAmount * friendlyFireRatio);
			}
			else
			{
				//If i use this, add back in friendly fire modifer here
				DealDamage(manager, damageAmount);
			}
		
		}

	}
	*/

	void FindTargets()
	{
		float TempDamageAmount;
		/* Commenting this out so explosions can be used to trigger status effects
		if (damageAmount == 0 && extraDamage.Length == 0)
		{
			return;
		}*/

		foreach (RaceManager manager in GameManager.main.playerList)
		{
			if (manager.playerNumber == sourceInt)
			{
				if (friendlyFireRatio == 0)
				{
					continue;
				}
				else
				{
					TempDamageAmount = damageAmount * friendlyFireRatio;
				}
			}
			else
			{
				TempDamageAmount = damageAmount;
			}

			if (TempDamageAmount == 0 )
			{
				continue;
			}

			List<UnitManager> toDamage = new List<UnitManager>();

			foreach (KeyValuePair<string, List<UnitManager>> unitList in manager.getUnitList())
			{
				foreach (UnitManager unit in unitList.Value)
				{
					if (unit == null)
					{
						continue;
					}
					
					if (getDistance(unit) <= Mathf.Pow (sizeSquared + unit.CharController.radius, 2))
					{
						toDamage.Add(unit);
					}
				}
			}
			foreach (UnitManager unit in toDamage) // Gotta do this in a separate loop so we don't get iteration errors when a guy dies
			{
				DealDamage(unit, TempDamageAmount);
			}
		}

	}



	float getDistance(UnitManager unit)
	{

		float f;
		if (UseYFOrDetection) {
			f =  new Vector2(unit.transform.position.x - transform.position.x, unit.transform.position.z - transform.position.z).sqrMagnitude;
		}
		else {
		f = (unit.transform.position - transform.position).sqrMagnitude;
		}


		//f -= (unit.CharController.radius * unit.CharController.radius);
		
		return f;
	}

	
	void DealDamage(UnitManager manager, float baseAmount)
	{
		UnitStats stats = manager.myStats;
		foreach (IWeapon.bonusDamage tag in extraDamage)
		{
			if (manager.myStats.isUnitType(tag.type))
			{
				baseAmount += tag.bonus;
			}
		}

		float total = stats.TakeDamage(baseAmount, source, type, mySrcMan);


		if (mySrcMan)
		{
			mySrcMan.myStats.veteranDamage(total);
		}

		foreach (Notify not in triggers)
		{
			not.trigger(source, null, manager, baseAmount);
		}

	}
}