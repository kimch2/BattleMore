using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class explosion : MonoBehaviour {


    public GameObject particleEff;

	public float damageAmount;
	public DamageTypes.DamageType type;
	public float maxSize = 5.0f;

	public bool UseYFOrDetection;

    [Tooltip("The time between the spawn of this object (and the above effect) and when the damage is applied")]
    public float DamageDelay = .01f;
	float friendlyAmount;
	protected float sizeSquared;
	public IWeapon.bonusDamage[] extraDamage;
    public OnHitContainer MyHitContainer;
    [Tooltip("This will apply to all if this list is empty")]
    public List<int> PlayersToApplyTo; // Maybe get rid of this since there are bools for it?
    public bool AppliesToEnemy = true;
    public bool AppliesToAlly;

    IEnumerator Start () {

		if (particleEff) {
			GameObject obj = 	(GameObject)Instantiate (particleEff, this.gameObject.transform.position, Quaternion.identity);

            if (!MyHitContainer.SetOnHitContainer(obj, 0, null))
            {
                obj.SendMessage("setOwner", MyHitContainer.playerNumber, SendMessageOptions.DontRequireReceiver);
            }
		}
        //yield return null;
        yield return new WaitForSeconds(DamageDelay);
		sizeSquared = maxSize;
		FindTargets();
		yield return null;

		Destroy (this.gameObject);

	}


	public void setSource(GameObject sr)
	{
		if (sr) {
            MyHitContainer = sr.GetComponent<OnHitContainer>();
		}
	}


	public void setDamage(float amount)
	{
		damageAmount = amount;
        friendlyAmount = damageAmount * MyHitContainer.FriendlyFireRatio ;
	}

    //SOmetimes sources are global abilities which are not attached to Units, so we have to pass in additional ownershipInfo
    public void Initialize(float DamageAmount, OnHitContainer container)
    {
        damageAmount = DamageAmount;
        MyHitContainer = container;
        if (!MyHitContainer)
        {
            MyHitContainer = gameObject.AddComponent<OnHitContainer>();
            Debug.LogError("No HitContainer for this object " + this.gameObject + " sourced from " + container.name);
        }
        friendlyAmount = damageAmount * MyHitContainer.FriendlyFireRatio;  
    }


    protected virtual void FindTargets()
    {
        float TempDamageAmount =0;

        foreach (RaceManager manager in GameManager.main.playerList)
        {
            if (manager.playerNumber == MyHitContainer.playerNumber)
                {
                    if (!AppliesToAlly)
                    {
                        continue;
                    }
                    else
                    {
                        TempDamageAmount = damageAmount * MyHitContainer.FriendlyFireRatio;
                    }
                }
                else if (manager.playerNumber != MyHitContainer.playerNumber && !AppliesToEnemy)
                {
                    continue;
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


	protected float getDistance(UnitManager unit)
	{
		if (!UseYFOrDetection) {
            return new Vector2(unit.transform.position.x - transform.position.x, unit.transform.position.z - transform.position.z).sqrMagnitude;
		}
		else {
		    return (unit.transform.position - transform.position).sqrMagnitude;
		}
	}

	
	protected void DealDamage(UnitManager manager, float baseAmount)
	{
		UnitStats stats = manager.myStats;

		foreach (IWeapon.bonusDamage tag in extraDamage)
		{
			if (manager.myStats.isUnitType(tag.type))
			{
				baseAmount += tag.bonus;
			}
		}
        MyHitContainer.trigger(this.gameObject, manager, baseAmount);
        if (baseAmount > 0)
        {
            stats.TakeDamage(baseAmount, MyHitContainer.source, type,MyHitContainer);
        }	
	}
}