using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRecharger : MonoBehaviour {


	public List<UnitStats> ToHeal;
	public List<UnitStats> ToEnergize;

	public static WorldRecharger main;

	void Awake()
	{
		main = this;
	}

	// Use this for initialization
	void Start () {
		InvokeRepeating ("Charge",.5f,.5f);
	}


	public void addHeal(UnitStats target)
	{
		ToHeal.Add (target);
	}

	public void removeHeal(UnitStats target)
	{
		ToHeal.Remove(target);
	}

	public void addEnergy(UnitStats target)
	{
		ToEnergize.Add (target);
	}

	public void removeEnergy(UnitStats target)
	{
		ToEnergize.Remove (target);
	}

	float healAmount = 0;
	float energyAmount = 0;

	void Charge()
	{

		foreach (UnitStats stat in ToHeal) {
			if (stat) {
				stat.veternStat.UpHealing (stat.heal (stat.HealthRegenPerHalf, DamageTypes.DamageType.Regular)); 
			}
		}
		foreach (UnitStats stat in ToEnergize) {
			if (stat) {
				stat.veternStat.UpEnergy (stat.changeEnergy (stat.EnergyRegenPerHalf)); 
			}

		}
	}

}
