using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorBackHeal : Ability,Modifier {

	public float ArmorAmount;
	public UnitStats myStats;
	UnitManager manager;
	public Animator myAnimator;
	bool IsArmored;
	float lastArmorTime;
	public float percentageHealSecond = .15f;

	new void Awake()
	{
		base.Awake();
		audioSrc = GetComponent<AudioSource> ();
		myType = type.passive;
	}


	// Use this for initialization
	void Start () {
		myStats = GetComponent<UnitStats> ();
		myStats.addModifier (this);
		manager = GetComponent<UnitManager> ();

	}



	public float modify(float amount, GameObject src, DamageTypes.DamageType theType)
	{

		if (!IsArmored && Time.time > lastArmorTime + 20) {
		
			if (myStats.health / myStats.Maxhealth < .25f) {

				IsArmored = true;
				myStats.armor += ArmorAmount;
				manager.setStun (true, this, false);
				myAnimator.Play ("ArmorUp");
				//myAnimator.SetInteger ("State", 0);
				lastArmorTime = Time.time;
				StartCoroutine (HealUp());
			}
		}

		return amount;
	}


	IEnumerator HealUp()
	{
		float totalTime = 10;
		myStats.heal (myStats.Maxhealth * percentageHealSecond /3);
		while (totalTime > 0 && !myStats.atFullHealth ()) {
			yield return new WaitForSeconds (.333f);
			myStats.heal (myStats.Maxhealth* percentageHealSecond/3);
			totalTime -= .333f;
		}

		manager.setStun (false, this, false);
		myAnimator.CrossFade ("BeetleIdle", 1);//.SetInteger ("State", 1);
		myStats.armor -= ArmorAmount;
		IsArmored = false;
	}


	public override void setAutoCast(bool offOn){
	}


	override
	public continueOrder canActivate (bool showError)
	{

		continueOrder order = new continueOrder ();
		return order;
	}

	override
	public void Activate()
	{
		//return true;//next unit should also do this.
	}

}
