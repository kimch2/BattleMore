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
	private float scale = 1.0f;
	public List<Notify> triggers = new List<Notify> ();

	private List<UnitManager> hitStuff= new List<UnitManager>();


	public IWeapon.bonusDamage[] extraDamage;

	UnitManager mySrcMan;
	// Use this for initialization
	IEnumerator Start () {
		if (particleEff) {
			GameObject obj = 	(GameObject)Instantiate (particleEff, this.gameObject.transform.position, Quaternion.identity);
			obj.SendMessage ("setOwner", sourceInt, SendMessageOptions.DontRequireReceiver);

		}
		transform.localScale = Vector3.one * maxSize;
		yield return null;
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



	void OnTriggerEnter(Collider other)
	{if (other.isTrigger) {
			return;}



		UnitManager manager = other.gameObject.GetComponent<UnitManager> ();

		if (manager) {
			if(!hitStuff.Contains(manager)) {
				hitStuff.Add (manager);
		
					float amount = damageAmount	;

					if (sourceInt == manager.PlayerOwner) {
						amount *= friendlyFireRatio;
				}
				if (amount <= 0) {
					return;
				}

					UnitStats stats = manager.myStats;
					foreach ( IWeapon.bonusDamage tag in extraDamage) {
						if ( manager.myStats.isUnitType (tag.type)) {
							amount += tag.bonus;
						}
					}
				
					float total = 0;


					total = stats.TakeDamage (amount, source, type);
					

					if (mySrcMan) {
						mySrcMan.myStats.veteranDamage (total);
					}

					foreach (Notify not in triggers) {
					
						not.trigger(source,  null, manager, amount);
					}


			}
		}
	}

}