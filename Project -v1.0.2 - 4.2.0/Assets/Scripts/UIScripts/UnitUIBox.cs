
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitUIBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {


		public Canvas toolbox;

		
	private bool inBox;
	public int index;

	public Text Title;
	public Text Range;
	public Text Damage;
	public Text AttackSpeed;
	//public Text AttackNum;
	public Text CantAttack;
	public UnitCardCreater myUnit;


		public void OnPointerEnter(PointerEventData eventd)
		{
			inBox = true;
			toolbox.enabled = true;

			initialize ();
			StartCoroutine(UpdateStats());
			
		}

		public void OnPointerExit(PointerEventData eventd)
		{
			inBox = false;
			toolbox.enabled = false;
		}


	IEnumerator UpdateStats()
	{
		while (inBox)
		{
			yield return null;
			AttackSpeed.text = "Attack Speed: " + Mathf.Round(100 * myUnit.currentUnit.myWeapon[index].attackPeriod) / 100f;
			Damage.text = "Damage: " + Mathf.Round(100 * myUnit.currentUnit.myWeapon[index].baseDamage) / 100f;

			foreach (IWeapon.bonusDamage bd in myUnit.currentUnit.myWeapon[index].extraDamage)
			{
				Damage.text += " (+" + bd.bonus + " vs " + bd.type + ")";
			}
			if (myUnit.currentUnit.myWeapon[index].numOfAttacks > 1)
			{
				Damage.text += "  Attacks X " + myUnit.currentUnit.myWeapon[index].numOfAttacks;
			}
		}
	}

	public void initialize()
	{
		if (myUnit.currentUnit)
		{
			if (myUnit.currentUnit.myWeapon.Count > index)
			{
				IWeapon myWeap = myUnit.currentUnit.myWeapon[index];
				Title.text = myWeap.Title;
				Range.text = "Range: " + myWeap.range;
				AttackSpeed.text = "Attack Speed: " + Mathf.Round(100 * myWeap.attackPeriod) / 100f;
				Damage.text = "Damage: " + Mathf.Round(100 * myWeap.baseDamage) / 100f;

				foreach (IWeapon.bonusDamage bd in myWeap.extraDamage)
				{
					Damage.text += " (+" + bd.bonus + " vs " + bd.type + ")";
				}
				if (myWeap.numOfAttacks > 1)
				{
					Damage.text += "  Attacks X " + myWeap.numOfAttacks;
				}


				string n = "";
				if (myWeap.cantAttackTypes.Count > 0)
				{
					n += "Can't Attack: ";
				}
				foreach (UnitTypes.UnitTypeTag t in myWeap.cantAttackTypes)
				{
					n += t.ToString() + ",";
				}
				if (n.EndsWith(","))
				{
					n = n.Substring(0, n.Length - 1);
				}
				CantAttack.text = n;
			}
			else
			{
				Ability ab = myUnit.currentUnit.myAddons[index - myUnit.currentUnit.myWeapon.Count];
				Title.text = ab.Name;
				AttackSpeed.text = " Speed: 1";
				Damage.text = " Heal Amount: 10";
				Range.text = " Range: 45";
				CantAttack.text = "";
			}
		}
	}




}
