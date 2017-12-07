using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedLook : MonoBehaviour,Modifier  {


	public ParticleSystem particleEffect;

	[Tooltip("Put these in order of greatest Ratios, at the give ratio and up that amount will be spawned, including the starting one at full health, and one at no health")]
	public List<FireDamage> damageChangers;
	int currentIndex;
	// Use this for initialization
	UnitStats myStat;
	ParticleSystem.EmissionModule mod;

	void Start () {
		if (damageChangers.Count == 0) {
			Destroy (this);
		}
		myStat = GetComponent<UnitStats> ();
		myStat.addModifier (this);
		myStat.addHealModifier (this);
		currentIndex = 0;
		mod =particleEffect.emission;
		float ratio =	(myStat.health) / myStat.Maxhealth;

		for (int i = 0; i < damageChangers.Count; i ++){
			if (ratio > damageChangers[i].healthRatio) {
				currentIndex = i;
				break;
			}
		
		}
	
		mod.rateOverTime = damageChangers [currentIndex].EmmisionRate;
		}


	public float modify(float amount, GameObject src, DamageTypes.DamageType theType)
	{
		float ratio =	(myStat.health - amount) / myStat.Maxhealth;

		if (currentIndex != 0 && ratio > damageChangers [currentIndex - 1].healthRatio) {
				currentIndex--;
				mod.rateOverTime = damageChangers [currentIndex].EmmisionRate;
				return amount;
		}

		if (damageChangers [currentIndex].healthRatio > ratio) {
			currentIndex++;
			if (damageChangers.Count > currentIndex) {
				currentIndex = damageChangers.Count - 1;
			} else if (currentIndex < 0) {
				currentIndex = 0;
			}
			mod.rateOverTime = damageChangers [currentIndex].EmmisionRate;
			
		}

		return amount;
	}



}

[System.Serializable]
public class FireDamage{
	public float healthRatio;
	public int EmmisionRate;
}
