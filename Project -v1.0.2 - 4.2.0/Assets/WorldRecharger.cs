using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRecharger : MonoBehaviour {


	public List<UnitStats> ToHeal;
	public List<UnitStats> ToEnergize;
	public List<RoamAI> ToRoam;

	public static WorldRecharger main;

	void Awake()
	{
		main = this;
	}

	// Use this for initialization
	void Start () {
		InvokeRepeating ("Charge",.5f,.5f);
		InvokeRepeating ("Roam",1,3);
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

	public void addRoam(RoamAI target)
	{
		ToRoam.Add (target);
	}

	public void removeRoam(RoamAI target)
	{
		ToRoam.Remove (target);
	}


	void Charge()
	{

		foreach (UnitStats stat in ToHeal) {
			if (stat) {
				//Debug.Log("Healing " + stat.Maxhealth);
				stat.veternStat.UpHealing (stat.heal (stat.HealthRegenPerHalf, DamageTypes.DamageType.Regular)); 
			}
		}
		foreach (UnitStats stat in ToEnergize) {
			if (stat) {
				stat.veternStat.UpEnergy (stat.changeEnergy (stat.EnergyRegenPerHalf)); 
			}

		}
	}

	int indexMod; // Makes it so only 1/3 roam each update to give it random look

	void Roam()
	{
		for (int i = 0;i < ToRoam.Count; i++) {
			if (ToRoam[i] && i%3 == indexMod) {
				ToRoam [i].setnewLocation ();
			}
		}
		indexMod++;
		indexMod %= 3;
	}

}
