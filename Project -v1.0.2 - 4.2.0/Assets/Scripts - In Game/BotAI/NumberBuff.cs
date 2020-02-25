using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberBuff
{

}

public class StatChanger
{
	UnitStats myStats;
    public enum BuffType {Armor,HP,HPRegen,Energy, EnergyRegen,MoveSpeed,Damage,AttackSpeed, Range, Cooldown }

	Dictionary<string, NumberAlter> cachedAlters = new Dictionary<string, NumberAlter>();


	public NumberAlter GetNumberAlter(string name)
	{
		if (cachedAlters.ContainsKey(name))
		{
			return cachedAlters[name];
		}

		NumberAlter alter = new NumberAlter(name, myStats);
		cachedAlters.Add(name, alter);
		return alter;
	}



	public StatChanger(UnitStats stats)
	{
		myStats = stats;
	}

	/// <summary>
	///  .2f as a perent means a 20% increase in attackspeed, .2 flat slows it down. applies to all weapons
	/// </summary>
	public void changeAttackSpeed(float perc, float flat, UnityEngine.Object obj, bool isFriendly, bool stackable = false)
    {
        if (isFriendly && !myStats.myManager.metaStatus.CanBuff || !isFriendly && !myStats.myManager.metaStatus.canDebuff)
        {
            return;
        }
        NumberAlter number = GetNumberAlter("AttackSpeed");
        number.AddBuff(perc, flat, obj, stackable);   
        foreach (IWeapon weap in myStats.myManager.myWeapon)
        {
            if (cachedAlters.ContainsKey("AttackSpeed" + weap.GetHashCode()))
            {
                weap.attackPeriod = NumberAlter.adjustTotalSpeedInverse(new List<NumberAlter>() { number, GetNumberAlter("AttackSpeed" + weap.GetHashCode()) }, weap.baseAttackPeriod, .05f, 100);
            }
            else
            {
                weap.attackPeriod = number.adjustSpeedInverse(weap.getBasePeriod());
            }
        }
    }

	public void removeAttackSpeed(UnityEngine.Object obj)
	{
		NumberAlter number = GetNumberAlter("AttackSpeed");
		number.RemoveBuff(obj);
		foreach (IWeapon weap in myStats.myManager.myWeapon)
		{
			if (cachedAlters.ContainsKey("AttackSpeed" + weap.GetHashCode()))
			{
				weap.attackPeriod = NumberAlter.adjustTotalSpeedInverse(new List<NumberAlter>() { number, GetNumberAlter("AttackSpeed" + weap.GetHashCode()) }, weap.baseAttackPeriod, .05f, 100);
			}
			else
			{
				weap.attackPeriod = number.adjustSpeedInverse(weap.getBasePeriod());
			}
		}
	}




	public void changeSpecAttSpeed(float perc, float flat, UnityEngine.Object obj, IWeapon specificWeap, bool isFriendly, bool stackable = false)
    {
        if (isFriendly && !myStats.myManager.metaStatus.CanBuff || !isFriendly && !myStats.myManager.metaStatus.canDebuff)
        {
            return;
        }
        NumberAlter number = GetNumberAlter("AttackSpeed" + specificWeap.GetHashCode());
		number.AddBuff(perc, flat, obj, stackable);
		specificWeap.attackPeriod = NumberAlter.adjustTotalSpeedInverse(new List<NumberAlter>() { number, GetNumberAlter("AttackSpeed") }, specificWeap.baseAttackPeriod, .05f, 100);
	}

	public void removeSpecAttSpeed(UnityEngine.Object obj, IWeapon specificWeap)
	{
		NumberAlter number = GetNumberAlter("AttackSpeed" + specificWeap.GetHashCode());
		number.RemoveBuff(obj);
		specificWeap.attackPeriod = NumberAlter.adjustTotalSpeedInverse(new List<NumberAlter>() { number, GetNumberAlter("AttackSpeed") }, specificWeap.baseAttackPeriod, .05f, 100);
	}

	//===================================================================================================
	/// <summary>
	///  .2f as a perent means a 20% increase, This will apply to all weapons
	/// </summary>
	public void changeWeaponDamage(float perc, float flat, UnityEngine.Object obj, bool isFriendly, bool stackable = false)
    {
        if (isFriendly && !myStats.myManager.metaStatus.CanBuff || !isFriendly && !myStats.myManager.metaStatus.canDebuff)
        {
            return;
        }
        NumberAlter number = GetNumberAlter("Damage");
		number.AddBuff(perc, flat, obj, stackable);
		foreach (IWeapon weap in myStats.myManager.myWeapon)
		{
			if (cachedAlters.ContainsKey("Damage" + weap.GetHashCode()))
			{
				weap.baseDamage = NumberAlter.AdjustTotalSpeed(new List<NumberAlter>() { number, GetNumberAlter("Damage" + weap.GetHashCode()) }, weap.getInitialDamage(), .05f, 10000);
			}
			else
			{
				weap.baseDamage = number.ApplyBuffs(weap.getInitialDamage());
			}
		}
	}

	public void removeWeaponDamage(UnityEngine.Object obj)
	{
		NumberAlter number = GetNumberAlter("Damage");
		number.RemoveBuff(obj);
		foreach (IWeapon weap in myStats.myManager.myWeapon)
		{
			if (cachedAlters.ContainsKey("Damage" + weap.GetHashCode()))
			{
				weap.baseDamage = NumberAlter.AdjustTotalSpeed(new List<NumberAlter>() { number, GetNumberAlter("Damage" + weap.GetHashCode()) }, weap.getInitialDamage(), .05f, 10000);
			}
			else
			{
				weap.baseDamage = number.ApplyBuffs(weap.getInitialDamage());
			}
		}
	}


	public void changeSpecWeapDamage(float perc, float flat, UnityEngine.Object obj, IWeapon specificWeap, bool isFriendly, bool stackable = false)
	{
        if (isFriendly && !myStats.myManager.metaStatus.CanBuff || !isFriendly && !myStats.myManager.metaStatus.canDebuff)
        {
            return;
        }
        NumberAlter number = GetNumberAlter("Damage" + specificWeap.GetHashCode());
		number.AddBuff(perc, flat, obj, stackable);
		specificWeap.baseDamage = NumberAlter.AdjustTotalSpeed(new List<NumberAlter>() { number, GetNumberAlter("AttackSpeed") }, specificWeap.getInitialDamage(), .05f, 10000);
	}

	public void removeSpecWeapDamage(UnityEngine.Object obj, IWeapon specificWeap)
	{
		NumberAlter number = GetNumberAlter("Damage" + specificWeap.GetHashCode());
		number.RemoveBuff(obj);
		specificWeap.attackPeriod = NumberAlter.adjustTotalSpeedInverse(new List<NumberAlter>() { number, GetNumberAlter("Damage") }, specificWeap.getInitialDamage(), .05f, 10000);
	}


	//===================================================================================================
	/// <summary>
	///  .2f as a perent means a 20% increase, This will apply to all weapons
	/// </summary>
	public void changeWeaponRange(float perc, float flat, UnityEngine.Object obj, bool isFriendly, bool stackable = false)
    {
        if (isFriendly && !myStats.myManager.metaStatus.CanBuff || !isFriendly && !myStats.myManager.metaStatus.canDebuff)
        {
            return;
        }
        NumberAlter number = GetNumberAlter("Range");
		number.AddBuff(perc, flat, obj, stackable);
		foreach (IWeapon weap in myStats.myManager.myWeapon)
		{
			weap.range = number.ApplyBuffs(weap.range);
		}
	}

	public void removeWeaponRange(UnityEngine.Object obj)
	{
		NumberAlter number = GetNumberAlter("Range");
		number.RemoveBuff(obj);
		foreach (IWeapon weap in myStats.myManager.myWeapon)
		{
			weap.range = number.ApplyBuffs(weap.range);
		}
	}
	//===================================================================================================
	/// <summary>
	///  .2f as a perent means a 20% increase, This will apply to all weapons
	/// </summary>
	public void changeMoveSpeed(float perc, float flat, UnityEngine.Object obj, bool isFriendly, bool stackable = false)
    {
        if (isFriendly && !myStats.myManager.metaStatus.CanBuff || !isFriendly && !myStats.myManager.metaStatus.canDebuff)
        {
            return;
        }
        NumberAlter number = GetNumberAlter("MoveSpeed");
		number.AddBuff(perc, flat, obj, stackable);
		if (myStats.myManager.cMover)
		{
			//This will probably break because you also have to set it in the RVOController
			myStats.myManager.cMover.MaxSpeed = number.ApplyBuffs(myStats.myManager.cMover.initialSpeed);
			Pathfinding.RVO.RVOController rvo = myStats.gameObject.GetComponent<Pathfinding.RVO.RVOController>();
			if (rvo)
			{
				rvo.maxSpeed = myStats.myManager.cMover.MaxSpeed;
			}
            myStats.StatsChanged = true;
		}
	}

	public void removeMoveSpeed(UnityEngine.Object obj)
	{
		NumberAlter number = GetNumberAlter("MoveSpeed");
		number.RemoveBuff(obj);
		if (myStats.myManager.cMover)
		{
			//This will probably break because you also have to set it in the RVOController
			myStats.myManager.cMover.MaxSpeed = number.ApplyBuffs(myStats.myManager.cMover.initialSpeed);
			Pathfinding.RVO.RVOController rvo =  myStats.gameObject.GetComponent<Pathfinding.RVO.RVOController>();
			if (rvo)
			{
				rvo.maxSpeed = myStats.myManager.cMover.MaxSpeed;
			}
            myStats.StatsChanged = true;
        }
	}
	//===================================================================================================

	/// <summary>
	///  .2f as a perent means a 20% increase, This will apply to all weapons
	/// </summary>
	public void changeHealthMax(float perc, float flat, UnityEngine.Object obj, bool isFriendly, bool stackable = false)
    {
        if (isFriendly && !myStats.myManager.metaStatus.CanBuff || !isFriendly && !myStats.myManager.metaStatus.canDebuff)
        {
            return;
        }
        NumberAlter number = GetNumberAlter("HealthMax");
		number.AddBuff(perc, flat, obj, stackable);
		myStats.Maxhealth = number.ApplyBuffs(number.baseAmount);
        myStats.StatsChanged = true;
    }

	public void removeHealthMax(UnityEngine.Object obj)
	{
		NumberAlter number = GetNumberAlter("HealthMax");
		number.RemoveBuff(obj);
		myStats.Maxhealth = number.ApplyBuffs(number.baseAmount);
        myStats.StatsChanged = true;
    }
	//===================================================================================================

	public void changeEnergyMax(float perc, float flat, UnityEngine.Object obj, bool isFriendly, bool stackable = false)
    {
        if (isFriendly && !myStats.myManager.metaStatus.CanBuff || !isFriendly && !myStats.myManager.metaStatus.canDebuff)
        {
            return;
        }
        NumberAlter number = GetNumberAlter("EnergyMax");
		number.AddBuff(perc, flat, obj, stackable);
		myStats.MaxEnergy = number.ApplyBuffs(number.baseAmount);
        myStats.StatsChanged = true;
    }

	public void removeEnergyMax(UnityEngine.Object obj)
	{
		NumberAlter number = GetNumberAlter("EnergyMax");
		number.RemoveBuff(obj);
		myStats.MaxEnergy = number.ApplyBuffs(number.baseAmount);
        myStats.StatsChanged = true;
    }

    //===================================================================================================


    public void changeEnergyRegen(float perc, float flat, UnityEngine.Object obj, bool isFriendly, bool stackable = false)
    {
        if (isFriendly && !myStats.myManager.metaStatus.CanBuff || !isFriendly && !myStats.myManager.metaStatus.canDebuff)
        {
            return;
        }
        NumberAlter number = GetNumberAlter("EnergyRegen");
        number.AddBuff(perc, flat, obj, stackable);
        myStats.setEnergyRegen(number.ApplyBuffs(number.baseAmount));
    }

    public void removeEnergyRegen(UnityEngine.Object obj)
    {
        NumberAlter number = GetNumberAlter("EnergyMax");
        number.RemoveBuff(obj);
        myStats.setEnergyRegen(number.ApplyBuffs(number.baseAmount));
    }

    //===================================================================================================


    public void changeHealthRegen(float perc, float flat, UnityEngine.Object obj, bool isFriendly, bool stackable = false)
    {
        if (isFriendly && !myStats.myManager.metaStatus.CanBuff || !isFriendly && !myStats.myManager.metaStatus.canDebuff)
        {
            return;
        }
        NumberAlter number = GetNumberAlter("HealthRegen");
        number.AddBuff(perc, flat, obj, stackable);
        myStats.setHealRate(number.ApplyBuffs(number.baseAmount));
    }

    public void removeHealthRegen(UnityEngine.Object obj)
    {
        NumberAlter number = GetNumberAlter("HealthRegen");
        number.RemoveBuff(obj);
        myStats.setHealRate(number.ApplyBuffs(number.baseAmount));
    }
    //===================================================================================================

    /// <summary>
    ///  .2f as a perent means a 20% increase, This will apply to all weapons
    /// </summary>
    public void changeArmor(float perc, float flat, UnityEngine.Object obj, bool isFriendly, bool stackable = false)
    {
        if (isFriendly && !myStats.myManager.metaStatus.CanBuff || !isFriendly && !myStats.myManager.metaStatus.canDebuff)
        {
            return;
        }
        NumberAlter number = GetNumberAlter("Armor");
		number.AddBuff(perc, flat, obj, stackable);
		myStats.armor = number.ApplyBuffs(number.baseAmount);
        myStats.StatsChanged = true;
    }

	public void removeArmor(UnityEngine.Object obj)
	{
		NumberAlter number = GetNumberAlter("Armor");
		number.RemoveBuff(obj);
		myStats.armor = number.ApplyBuffs(number.baseAmount);
        myStats.StatsChanged = true;
    }

	//===================================================================================================
	/// <summary>
	///  .2f as a perent means a 20% increase, This will apply to all weapons
	/// </summary>
	public void changeCooldown(float perc, float flat, UnityEngine.Object obj, bool isFriendly, bool stackable = false)
    {
        if (isFriendly && !myStats.myManager.metaStatus.CanBuff || !isFriendly && !myStats.myManager.metaStatus.canDebuff)
        {
            return;
        }
        NumberAlter number = GetNumberAlter("Cooldown");
		number.AddBuff(perc, flat, obj, stackable);
		foreach (Ability ab in myStats.myManager.abilityList)
		{
			if (ab && ab.myCost)
			{
				ab.myCost.cooldown = number.ApplyBuffs(ab.myCost.cooldown); // Need to Store the base cooldown somewhere
			}
		}
	}

	public void removeCooldown(UnityEngine.Object obj)
	{
		NumberAlter number = GetNumberAlter("Cooldown");
		number.RemoveBuff(obj);
		foreach (Ability ab in myStats.myManager.abilityList)
		{
			if (ab && ab.myCost)
			{
				ab.myCost.cooldown = number.ApplyBuffs(ab.myCost.cooldown); // Need to Store the base cooldown somewhere
			}
		}
	}
	//===================================================================================================

	/// <summary>
	///  .2f as a perent means a 20% increase, This will apply to all weapons
	/// </summary>
	public void changeVisionRange(float perc, float flat, UnityEngine.Object obj, bool isFriendly, bool stackable = false)
    {
        if (isFriendly && !myStats.myManager.metaStatus.CanBuff || !isFriendly && !myStats.myManager.metaStatus.canDebuff)
        {
            return;
        }
        NumberAlter number = GetNumberAlter("VisionRange");
		number.AddBuff(perc, flat, obj, stackable);
		myStats.myManager.visionRange = number.ApplyBuffs(number.baseAmount);
		myStats.myManager.getVisionSphere().radius = myStats.myManager.visionRange;
	}

	public void removeVisionRange(UnityEngine.Object obj)
	{
		NumberAlter number = GetNumberAlter("VisionRange");
		number.RemoveBuff(obj);
		myStats.myManager.visionRange = number.ApplyBuffs(number.baseAmount);
		myStats.myManager.getVisionSphere().radius = myStats.myManager.visionRange;
	}
	//===================================================================================================
	/// <summary>
	///  .2f as a perent means a 20% increase, This will apply to all weapons
	/// </summary>
	public void changeCastRange(float perc, float flat, UnityEngine.Object obj, bool isFriendly, bool stackable = false)
	{
        if(isFriendly && !myStats.myManager.metaStatus.CanBuff || !isFriendly && !myStats.myManager.metaStatus.canDebuff)
        {
            return;
        }
		NumberAlter number = GetNumberAlter("CastRange");
		number.AddBuff(perc, flat, obj, stackable);
		foreach (Ability ab in myStats.myManager.abilityList)
		{
			if (ab && ab is TargetAbility)
			{
				((TargetAbility)ab).range = number.ApplyBuffs(((TargetAbility)ab).range); // Need to Store the base range somewhere
			}
		}
	}
	public void removeCastRange(UnityEngine.Object obj)
	{
		NumberAlter number = GetNumberAlter("CastRange");
		number.RemoveBuff(obj);
		foreach (Ability ab in myStats.myManager.abilityList)
		{
			if (ab && ab is TargetAbility)
			{
				((TargetAbility)ab).range = number.ApplyBuffs(((TargetAbility)ab).range); // Need to Store the base range somewhere
			}
		}
	}
}

public class SpeedMod
{
	public float Perc;
	public float Flat;
	public Object Source;
}

public class NumberAlter
{
	public string NumberName;
	public List<SpeedMod> speedMods = new List<SpeedMod>();
	
	public float min;
	public float max;
	public float baseAmount = 0;


	public void AddBuff(float perc, float flat, Object obj, bool stackable = false)
	{
		for (int i = 0; i < speedMods.Count; i++)
		{
			SpeedMod a = speedMods[i];
			if (a.Source == obj)
			{
				if (!stackable)
				{
					return;
				}
				else
				{
					a.Flat += flat;
					a.Perc += perc;
					return;
				}
			}
		}

		SpeedMod temp = new SpeedMod
		{
			Flat = flat,
			Perc = perc,
			Source = obj
		};
		
		speedMods.Add(temp);

	}

	public float ApplyBuffs(float baseNumber)
	{
		return AdjustSpeed(baseNumber);
	}

	public void RemoveBuff(Object obj)
	{
		speedMods.RemoveAll(item => item.Source = obj);
	}

	private float AdjustSpeed(float baseNumber)
	{
		foreach (SpeedMod a in speedMods)
		{
			baseNumber += a.Flat;
		}

		float percent = 1;
		foreach (SpeedMod a in speedMods)
		{
			percent += a.Perc;
		}

		percent = Mathf.Max(.001f, percent);
		baseNumber *= percent;

		return Mathf.Clamp(baseNumber, min, max);
	}

	public float adjustSpeedInverse(float baseNumber)
	{
		foreach (SpeedMod a in speedMods)
		{
			baseNumber += a.Flat;
		}

		float percent = 1;
		foreach (SpeedMod a in speedMods)
		{
			percent += a.Perc;
		}
		percent = Mathf.Max(.001f, percent);
		baseNumber = baseNumber / percent;

		return Mathf.Clamp(baseNumber, min, max);
	}

	public static float AdjustTotalSpeed(List<NumberAlter> allAlts, float baseNumber, float min, float max)
	{
		foreach (NumberAlter alt in allAlts)
		{
			foreach (SpeedMod a in alt.speedMods)
			{
				baseNumber += a.Flat;
			}
		}

		float percent = 1;

		foreach (NumberAlter alt in allAlts)
		{
			foreach (SpeedMod a in alt.speedMods)
			{
				percent += a.Perc;
			}
		}

		percent = Mathf.Max(.001f, percent);
		baseNumber *= percent;

		return Mathf.Clamp(baseNumber, min, max);
	}

	public static float adjustTotalSpeedInverse(List<NumberAlter> allAlts, float baseNumber, float min, float max)
	{
		foreach (NumberAlter alt in allAlts)
		{
			foreach (SpeedMod a in alt.speedMods)
			{
				baseNumber += a.Flat;
			}
		}

		float percent = 1;

		foreach (NumberAlter alt in allAlts)
		{
			foreach (SpeedMod a in alt.speedMods)
			{
				percent += a.Perc;
			}
		}
		percent = Mathf.Max(.001f, percent);
		baseNumber = baseNumber / percent;

		return Mathf.Clamp(baseNumber, min, max);
	}

	//BaseAmount doesn't work for things that have multiple targets, like weapons or abilities
	public NumberAlter(string numberName, UnitStats stats)
	{
		NumberName = numberName;
		switch (numberName)
		{
			case "Damage":
				min = 0;
				max = 2048;
				break;

			case "AttackSpeed":
				min = .05f;
				max = 2048;
				break;

			case "Range":
				min = 0;
				max = 2048;
				break;

			case "MoveSpeed":
				min = 0;
				max = 2048;
				break;

			case "HealthMax":
				min = 0;
				max = 2048;
				baseAmount = stats.Maxhealth;
				break;

			case "EnergyMax":
				min = 0;
				max = 2048;
				baseAmount = stats.MaxEnergy;
				break;

            case "EnergyRegen":
                min = 0;
                max = 2048;
                baseAmount = stats.EnergyRegenPerSec;
                break;

            case "HealthRegen":
                min = 0;
                max = 2048;
                baseAmount = stats.HealthRegenPerSec;
                break;

            case "Armor":
				min = -2048;
				max = 2048;
				baseAmount = stats.armor;
				break;

			case "Cooldown":
				min = 0;
				max = 2048;
				break;

			case "VisionRange":
				min = 4;
				max = 2048;
				baseAmount = stats.myManager.visionRange;
				break;

			case "CastRange":
				min = 10;
				max = 2048;
				break;

			default:
				min = .01f;
				max = 4096;
				//Debug.Log("Adding " + numberName + "   but it isn't spelled right or it hasn't been catalogged");
				break;

		}
	}
}
