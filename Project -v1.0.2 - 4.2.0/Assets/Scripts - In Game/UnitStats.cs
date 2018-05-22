using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using System;



public class UnitStats : MonoBehaviour {

	[TextArea(2,10)]
	public string UnitDescription = "dude";
	public bool isHero;
	public float Maxhealth;
	public float health;
	public float HealthRegenPerSec;




	public float MaxEnergy;
	public float currentEnergy;

	public float EnergyRegenPerSec;
	public Sprite Icon;

	public float supply;    //positive gives supply, negative uses it
	public float attackPriority =1 ;


	public float armor;
	public float spellResist;
	public float cost;
	private UnitManager myManager;

	private List<Modifier> damageModifiers = new List<Modifier>();

	//BE Careful this can pass in both negative and positive numbers!
	private List<Modifier> EnergyModifiers = new List<Modifier>();
	private List<Modifier> HealModifiers = new List<Modifier>();
	public List<UnitTypes.UnitTypeTag> otherTags = new  List<UnitTypes.UnitTypeTag> ();
	private List<UnitTypes.UnitTypeTag> TotalTags = new  List<UnitTypes.UnitTypeTag> ();

	int typeBitMask;

	public UnitTypes.UnitArmorTag armorType;
	public UnitTypes.SizeTag sizeType;
	public UnitTypes.HeightType myHeight;
	private List<Modifier> deathTriggers = new List<Modifier> ();
	public List<KillModifier> killMods = new List<KillModifier> ();

	//Tags the units can have
	private Selected mySelection;

	[HideInInspector]
	public bool upgradesApplied;

	public SphereCollider visionRange;


	//private float nextActionTime;
	public VeteranStats veternStat;


	public GameObject deathCorpse;
	public GameObject deathEffect;
	public GameObject takeDamageEffect;
	public Sprite UnitPortrait;
	bool tagSet = false;

	public List<Buff> goodBuffs = new List<Buff>();
	public 	List<Buff> badBuffs = new List<Buff>();





	void Awake()
	{

		Initialize ();

	}

	public void SetTags()
	{
		tagSet = true;
		TotalTags.Clear ();
		foreach (UnitTypes.UnitTypeTag t in otherTags) {

			TotalTags.Add ((UnitTypes.UnitTypeTag)Enum.Parse(typeof(UnitTypes.UnitTypeTag) ,t.ToString()));
		}
		TotalTags.Add ((UnitTypes.UnitTypeTag)Enum.Parse(typeof(UnitTypes.UnitTypeTag) ,armorType.ToString()));
		TotalTags.Add ((UnitTypes.UnitTypeTag)Enum.Parse(typeof(UnitTypes.UnitTypeTag) ,myHeight.ToString()));
		TotalTags.Add ((UnitTypes.UnitTypeTag)Enum.Parse(typeof(UnitTypes.UnitTypeTag) ,sizeType.ToString()));

		int bitMask = 0;
		foreach (UnitTypes.UnitTypeTag t in TotalTags) {
			bitMask += (int)Mathf.Pow (2, ((int)t));
		}
		typeBitMask = bitMask;
		//Debug.Log (this.gameObject + ": "+bitMask);
	}

	public void Initialize()
	{
		if (!tagSet) {
			SetTags ();
		}
		if (!myManager) {
			myManager = this.gameObject.GetComponent<UnitManager> ();
			myManager.myStats = this;
		}

		if (isHero) {
			bool HasAName = !isUnitType (UnitTypes.UnitTypeTag.Turret) && !isUnitType (UnitTypes.UnitTypeTag.Structure);
			veternStat= new VeteranStats(HasAName, GetComponent<UnitManager>().UnitName,
				!(isUnitType(UnitTypes.UnitTypeTag.Turret))&&!(isUnitType(UnitTypes.UnitTypeTag.Worker)) && !(isUnitType(UnitTypes.UnitTypeTag.Structure)),GetComponent<UnitManager>().UnitName 
				, myManager.PlayerOwner, myManager);
		}
		else{
		veternStat= new VeteranStats(!isUnitType(UnitTypes.UnitTypeTag.Turret)&& !isUnitType(UnitTypes.UnitTypeTag.Structure), GetComponent<UnitManager>().UnitName,
				!(isUnitType(UnitTypes.UnitTypeTag.Turret))&&!(isUnitType(UnitTypes.UnitTypeTag.Worker)) && !(isUnitType(UnitTypes.UnitTypeTag.Structure)), "", myManager.PlayerOwner,myManager);}

		if (!mySelection) {
			mySelection = this.gameObject.GetComponent<Selected>();
		}
	}



	// Use this for initialization
	void Start () {
		if (!myManager) {
			myManager = GetComponent<UnitManager> ();
			myManager.myStats = this;
		}
	
		if (Clock.main.getTotalSecond()< 1 && myManager.PlayerOwner == 1) {
			
			GameManager.main.playerList[myManager.PlayerOwner-1].UnitCreated (supply);		
		}

		GameManager.main.playerList [myManager.PlayerOwner - 1].addVeteranStat (veternStat);
		if (isHero) {
			veternStat.UnitName = myManager.UnitName;
		}

		// FIX THIS REPEATING INEUMERNATER
		if (EnergyRegenPerSec > 0) {

			 setEnergyRegen (EnergyRegenPerSec);
		//	StartCoroutine (HealthEnergy());
		}
		if (HealthRegenPerSec > 0) {
			StartCoroutine (regenHealth());
		}
		Invoke ("firstHealth", .3f);
	}

	void firstHealth()
	{if (!isUnitType (UnitTypes.UnitTypeTag.Invulnerable)) {
			updateHealthBar ();
		}
	}


	Coroutine EnergyUpdater;

	public void setEnergyRegen(float amount)
	{
		EnergyRegenPerSec = amount;
		if (amount == 0) {
			if (EnergyUpdater != null) {
				StopCoroutine (EnergyUpdater);
			}
		}
		else if (EnergyUpdater == null) {
			//Debug.Log ("Starting energy regen " + Time.time);
			EnergyUpdater = StartCoroutine (EnergyReg ());
		}
	}


	IEnumerator EnergyReg()
	{
		float regenPerHalfSecond = EnergyRegenPerSec / 2;

		while(true){
			yield return new WaitForSeconds (.5f);

			//Regenerate Energy
			if (currentEnergy < MaxEnergy ) { //&& EnergyRegenPerSec >0
				
				float actual = changeEnergy (regenPerHalfSecond);
				veternStat.UpEnergy (actual);
			}
		}
	}


	IEnumerator regenHealth(){

		float regenPerHalfSecond = HealthRegenPerSec;

		float waitTime = 1;
		if (HealthRegenPerSec > 2) {
			regenPerHalfSecond = HealthRegenPerSec / 2;
			waitTime = .5f;
		}

		while(true){
			yield return new WaitForSeconds (waitTime);

		if (health < Maxhealth) { //&& HealthRegenPerSec > 0
			float actual = heal (regenPerHalfSecond);;
			veternStat.UpHealing (actual);
			}
		}
	}


	int n = 0;
	public bool isUnitType(UnitTypes.UnitTypeTag type){

		n = (1 << ((int)type));
		return (typeBitMask &  n) ==  n;
	}

	public void addBuff(Buff input, bool stack)
	{
		if (stack || !goodBuffs.Contains (input)) {
			goodBuffs.Add (input);
		}
		if (mySelection.IsSelected) {
			RaceManager.upDateSingleCard();
		}
	}

	public void addDebuff(Buff input, bool stack)
	{
		if (stack || !badBuffs.Contains (input)) {
			badBuffs.Add (input);
		}
		if (mySelection.IsSelected) {
			RaceManager.upDateSingleCard();
		}
	}

	public void removeBuff(Buff input)
	{
		if (goodBuffs.Contains (input)) {
			goodBuffs.Remove(input);
		}
		if (mySelection.IsSelected) {
			
			RaceManager.upDateSingleCard();
		}

	}

	public void removeDebuff(Buff input, bool stack)
	{
		if (badBuffs.Contains (input)) {
			badBuffs.Remove(input);
		}
		if (mySelection.IsSelected) {
			RaceManager.upDateSingleCard();
		}
	}





	public float TakeDamage(float amount, GameObject source, DamageTypes.DamageType type)
	{
		if (isUnitType(UnitTypes.UnitTypeTag.Invulnerable)) {
			return 0;
		}
			
		//bool setToZero = false;
		if (type != DamageTypes.DamageType.True) {
			foreach (Modifier mod in damageModifiers) {
				if (mod != null) {
					amount = mod.modify (amount, source, type);
					if (amount <= 0) {
						//setToZero = true;
						return 0;
						}
					}
				}
		}
	//	if (!setToZero) {

		if (type == DamageTypes.DamageType.Regular || type == DamageTypes.DamageType.Wound ) {

			amount = Mathf.Max (amount - armor, 1);
		}

		if (myManager.PlayerOwner == 1 && source != this.gameObject) {
			if (isUnitType(UnitTypes.UnitTypeTag.Structure)) {
				ErrorPrompt.instance.underBaseAttack (this.transform.position);
			} else {
				ErrorPrompt.instance.underAttack (this.transform.position);
			}
		}

		if (takeDamageEffect) {

			Instantiate (takeDamageEffect, this.gameObject.transform.position, new Quaternion ());
		}
		if (veternStat != null) {
			veternStat.UpMitigated(armor);
			veternStat.UpdamTaken (amount);
		}

			//	Debug.Log ("Actual " + amount);
		health -= amount;
			
		if ((int)health <= 0) {
			kill (source);
		} else {
			updateHealthBar ();

			if (source) {
				myManager.Attacked (source.GetComponent<UnitManager> ());
				} 
		}

		return amount;

	}


	public void SetHealth (float percent)
	{
		health = Maxhealth * percent;
		updateHealthBar ();
	}

	private void updateHealthBar()
	{
			mySelection.updateHealthBar (health / Maxhealth);
	
	}

	private void updateEnergyBar()
	{

		mySelection.updateEnergyBar(currentEnergy / MaxEnergy);

	}

	private bool dead = false;

	public void kill(GameObject deathSource)
	{
		if (dead)
			return;

		if (!isUnitType(UnitTypes.UnitTypeTag.Invulnerable)) {

			if (this) {
				if (!GameManager.main.playerList [myManager.PlayerOwner - 1].UnitDying (myManager, deathSource, true)) {
					return;
				}
			}
				
			dead = true;
			deathTriggers.RemoveAll (item => item == null);
			for (int i = deathTriggers.Count - 1; i > -1; i--) {
				if (deathTriggers [i] != null) {
					deathTriggers [i].modify (0, deathSource, DamageTypes.DamageType.Regular);
				}
			}

			

			if (deathCorpse != null) {

				Vector3 spawnLoc = this.gameObject.transform.position;
				if (!isUnitType (UnitTypes.UnitTypeTag.Air)) {
					RaycastHit objecthit;

					Physics.Raycast (this.gameObject.transform.position + Vector3.up * 10, Vector3.down, out objecthit, 1000, 1 << 8);
					spawnLoc = objecthit.point;
				}
				//Debug.Log ("Rotation is " + this.gameObject.transform.rotation);
				//GameObject corpse = 

				GameObject.Instantiate (deathCorpse, spawnLoc, this.gameObject.transform.rotation);

			}

			if (deathSource) {
				UnitStats sourceStats = deathSource.GetComponent<UnitStats> ();
				if (sourceStats) {
					sourceStats.upKills ();
				}
			}
			//fix this when we have multiplayer games, here for optimizations?
			if (myManager.PlayerOwner == 1) {
					
				GameManager.main.playerList [myManager.PlayerOwner - 1].UnitDied (supply, myManager);
			}

			if (mySelection.IsSelected) {
				SelectedManager.main.DeselectObject (myManager);
			}
			if (deathEffect) {
				Instantiate (deathEffect, this.gameObject.transform.position, Quaternion.identity);
			}
	
			SelectedManager.main.updateControlGroups (myManager);
			this.gameObject.SendMessage ("Dying", SendMessageOptions.DontRequireReceiver);
			veternStat.Died = true;
			veternStat.DeathTime = Time.timeSinceLevelLoad;
			Destroy (this.gameObject);

		}
	}

	/// <summary>
	/// Adds this amount of damage done to this guys DamageDealt or to its host if its a turret
	/// </summary>
	/// <param name="amount">Amount.</param>
	public void veteranDamage(float amount)
	{
		if (isUnitType(UnitTypes.UnitTypeTag.Turret) && transform.parent) {
			UnitManager rootMan = transform.parent.GetComponentInParent<UnitManager> ();
			if (rootMan) {
				rootMan.myStats.veternStat.damageDone += amount;
			}
		} else {
			veternStat.damageDone += amount;
		}
	}

	public void upKills()
	{
		veternStat.kills++;

		if (isUnitType(UnitTypes.UnitTypeTag.Turret) && transform.parent) {

			UnitManager rootMan = transform.parent.GetComponentInParent<UnitManager> ();
			if (rootMan) {
				rootMan.myStats.upKills();
			}
		}
		foreach (KillModifier km in killMods) {
			km.incKill ();
		}
	}

	public void addLethalTrigger()//Method method)
	{
		//lethalDamage.Add (method);
		
	}

	public void removeDeathTrigger (Modifier mod)
	{deathTriggers.Remove (mod);
	}
	public void addDeathTrigger( Modifier mod)//Method method)
	{if (!deathTriggers.Contains (mod)) {
			deathTriggers.Add (mod);
		}
		//deathTriggers.Add (method);

	}


	public void addModifier(Modifier mod)
	{
		if (!damageModifiers.Contains (mod)) {
			damageModifiers.Add (mod);
		}
	}

	public void addHighPriModifier(Modifier mod)
	{
		if (!damageModifiers.Contains (mod)) {
			damageModifiers.Insert(0,mod);
		}
	}

	public void removeModifier(Modifier mod)
	{if (damageModifiers.Contains (mod)) {
			damageModifiers.Remove (mod);
		}
	}


	public void addEnergyModifier(Modifier mod)
	{
		if (!EnergyModifiers.Contains (mod)) {
			EnergyModifiers.Add (mod);
		}
	}

	public void removeEnergyModifier(Modifier mod)
	{if (EnergyModifiers.Contains (mod)) {
			EnergyModifiers.Remove (mod);
		}
	}


	public bool atFullHealth()
	{
		if (health >= Maxhealth) {
			return true;
		} else {
			return false;
		}
	}

	public float changeEnergy(float n)
	{//Debug.Log ("Recharging " + n);
		if (MaxEnergy == 0) {
		
			return 0;}
		float amount = 0;

	
		foreach (Modifier mod in EnergyModifiers) {
			n = mod.modify (n, this.gameObject, DamageTypes.DamageType.Regular);
		}

		if (n > 0 ) {

			amount = Math.Min (n, MaxEnergy - currentEnergy);
			currentEnergy += amount;
			updateEnergyBar ();
		} 
		else if(n < 0){
			currentEnergy += n;
			if (currentEnergy < 0) {
				currentEnergy = 0;
			}
			updateEnergyBar ();
		}
			
		return amount;
	}


	public void addHealModifier(Modifier m)
	{
		HealModifiers.Add (m);
	}

	public void removeHealModifier(Modifier m)
	{
		HealModifiers.Remove(m);
	}

	/// <summary>
	/// Heal the specified n and type, True should be used for things like building Construction.
	/// </summary>
	/// <param name="n">N.</param>
	/// <param name="type">Type.</param>
	public float heal(float n, DamageTypes.DamageType type = DamageTypes.DamageType.Regular)
	{

		foreach (Modifier mod in HealModifiers) {
			n = mod.modify (n, null,type);
		}
		
		float amount = Math.Min (n, Maxhealth - health);

		health += amount;

		updateHealthBar();
		return amount;
	}

	public bool atFullEnergy()
	{	
		if (currentEnergy >= MaxEnergy) {
		
			return true;
		} else {
			return false;
		}
	}

	public void changeArmor(float amount)
	{armor += amount;}

	public int getKills()
	{
		return veternStat.kills;
	}

}
