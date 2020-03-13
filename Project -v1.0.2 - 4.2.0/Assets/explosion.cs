using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class explosion : MonoBehaviour {

    UnitManager mySrcMan;
	public int sourceInt = 1;
    VeteranStats vetStats;

    public GameObject particleEff;

	public float damageAmount;
	public DamageTypes.DamageType type;
	public float maxSize = 5.0f;

	public bool UseYFOrDetection;

    [Tooltip("The time between the spawn of this object (and the above effect) and when the damage is applied")]
    public float DamageDelay = .01f;
	float friendlyAmount;
	float sizeSquared;
	public IWeapon.bonusDamage[] extraDamage;
    public OnHitContainer MyHitContainer;
    [Tooltip("This will apply to all if this list is empty")]
    public List<int> PlayersToApplyTo;


	IEnumerator Start () {

		if (particleEff) {
			GameObject obj = 	(GameObject)Instantiate (particleEff, this.gameObject.transform.position, Quaternion.identity);
			obj.SendMessage ("setOwner", sourceInt, SendMessageOptions.DontRequireReceiver);

		}

        yield return new WaitForSeconds(DamageDelay);
		sizeSquared = maxSize;
		FindTargets();
		yield return null;

		Destroy (this.gameObject);

	}


	public void setSource(GameObject sr)
	{
		if (sr) {
			mySrcMan = sr.GetComponent<UnitManager> ();
		}
		if (mySrcMan) {
			sourceInt = mySrcMan.PlayerOwner;
		}
	}

	public void setVeteran(VeteranStats sr)
	{
		mySrcMan = sr.myUnit;
		vetStats = sr;
		sourceInt = sr.playerOwner;
	}

	public void setDamage(float amount)
	{
		damageAmount = amount;
        friendlyAmount = damageAmount * MyHitContainer.FriendlyFireRatio ;
	}

    //SOmetimes sources are global abilities which are not attached to Units, so we have to pass in additional ownershipInfo
    public void Initialize(GameObject source, VeteranStats vets, float DamageAmount, OnHitContainer container, int PlayerOwner )
    {
        mySrcMan = vets.myUnit;
        vetStats = vets;
        damageAmount = DamageAmount;
        MyHitContainer = container;
        if (!MyHitContainer)
        {
            MyHitContainer = gameObject.AddComponent<OnHitContainer>();
            Debug.LogError("No HitContainer for this object " + this.gameObject + " sourced from " + source);
        }

        friendlyAmount = damageAmount * MyHitContainer.FriendlyFireRatio;
        if (mySrcMan)
        {
            source = mySrcMan.gameObject;
        }
        sourceInt = PlayerOwner;
    }


    void FindTargets()
    {
        float TempDamageAmount =0;

        foreach (RaceManager manager in GameManager.main.playerList)
        {
            if (PlayersToApplyTo.Count == 0 || PlayersToApplyTo.Contains(manager.playerNumber))
            {
                if (manager.playerNumber == sourceInt)
                {
                    if (MyHitContainer.FriendlyFireRatio == 0)
                    {
                       // continue;
                    }
                    else
                    {
                        TempDamageAmount = damageAmount * MyHitContainer.FriendlyFireRatio;
                    }
                }
                else
                {
                    TempDamageAmount = damageAmount;
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
                        if (getDistance(unit) <= Mathf.Pow(sizeSquared + unit.CharController.radius, 2))
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
    }


	float getDistance(UnitManager unit)
	{
		if (UseYFOrDetection) {
			return new Vector2(unit.transform.position.x - transform.position.x, unit.transform.position.z - transform.position.z).sqrMagnitude;
		}
		else {
		return (unit.transform.position - transform.position).sqrMagnitude;
		}
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
        MyHitContainer.trigger(null, manager, baseAmount);
        if (baseAmount > 0)
        {
            float total = stats.TakeDamage(baseAmount, mySrcMan ? mySrcMan.gameObject : null, type, mySrcMan);
            if (mySrcMan)
            {
                mySrcMan.myStats.veteranDamage(total);
            }
        }

		
	}
}