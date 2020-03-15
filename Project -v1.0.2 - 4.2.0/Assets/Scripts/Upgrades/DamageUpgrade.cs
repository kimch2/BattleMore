using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageUpgrade : Upgrade {

	[System.Serializable]
	public struct unitAmount
	{
		public string UnitName;
		public List<float> amount;
		public List<specialDamage> mySpecial;
	}

	[System.Serializable]
	public struct specialDamage
	{
		public UnitTypes.UnitTypeTag myType;
		public float amount;
	
	}

	public List<unitAmount> unitsToUpgrade = new List<unitAmount> ();
    [Tooltip("Applies a roughly 10% damage buff to units (rounded up to nearest whole number)")]
	public bool standardUpgrade = false;

	override
	public void applyUpgrade (GameObject obj){
		
        UnitManager manager = obj.GetComponent<UnitManager>();

		bool doubleTheDamage = false;
		if (obj.GetComponent<DoubleUpgradeApp> () && obj.GetComponent<DoubleUpgradeApp> ().doubleIt) {
			doubleTheDamage = true;}
		if (standardUpgrade)
		{
			for (int i = 0; i < manager.myWeapon.Count; i++)
				if (manager.myWeapon[i])
				{
					float amount = ((int)((manager.myWeapon[i].getInitialDamage() + 5) / 10)) * (doubleTheDamage ? 2 : 1);
					manager.myStats.statChanger.changeSpecWeapDamage(0, amount, null, manager.myWeapon[i], true);
					manager.myWeapon[i].incrementUpgrade();
					manager.gameObject.SendMessage("upgrade", Name, SendMessageOptions.DontRequireReceiver);
				}
		}
		else
		{

			foreach (unitAmount ua in unitsToUpgrade)
			{
				if (manager.UnitName.Contains(ua.UnitName))
				{
					for (int i = 0; i < manager.myWeapon.Count; i++)
						if (manager.myWeapon[i])
						{

							manager.myStats.statChanger.changeSpecWeapDamage(0, doubleTheDamage ? ua.amount[i] * 2 : ua.amount[i], null, manager.myWeapon[i], true);
							

							manager.myWeapon[i].incrementUpgrade();
							manager.gameObject.SendMessage("upgrade", Name, SendMessageOptions.DontRequireReceiver);
							if (ua.mySpecial.Count > 0)
							{

								IWeapon.bonusDamage foundOne = new IWeapon.bonusDamage();
								bool found = false;
								foreach (IWeapon.bonusDamage bonusA in manager.myWeapon[i].extraDamage)
								{
									if (bonusA.type == ua.mySpecial[i].myType)
									{
										foundOne = bonusA;
										found = true;
									}
								}
								if (found)
								{
									foundOne.bonus += ua.mySpecial[i].amount;
								}

								else
								{
									IWeapon.bonusDamage bonus = new IWeapon.bonusDamage();
									bonus.bonus = ua.mySpecial[i].amount;
									bonus.type = ua.mySpecial[i].myType;
									manager.myWeapon[i].extraDamage.Add(bonus);
								}
							}
						}

					if (manager.GetComponent<Selected>().IsSelected)
					{
						RaceManager.upDateUI();
					}

				}
			}

		}

	
	}

	public override void unApplyUpgrade (GameObject obj){

	}

}
