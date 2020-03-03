using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using System;



public class UnitStats : MonoBehaviour {

	[TextArea(2, 10)]
	public string UnitDescription = "dude";
	public bool isHero;
	public float Maxhealth;
	public float health;
	public float HealthRegenPerSec;
	[HideInInspector]
	public float HealthRegenPerHalf;

	public float MaxEnergy;
	public float currentEnergy;

	public float EnergyRegenPerSec;
	[HideInInspector]
	public float EnergyRegenPerHalf;
	public Sprite Icon;

	public float supply;    //positive gives supply, negative uses it

    [Tooltip("The higher the attack priority, the more enemies will focus it. 3 is what most combar units are on")]
	public float attackPriority = 1;
	[HideInInspector]
	public byte DefensePriority; // Am I an Air/Ground/ Both unit , this is a bitmask to be compared against the attackers agressionPriority
    [HideInInspector]
    public byte agressionPriority; //Can I attack Air/Ground/Both units, who should I prioritize

	public float armor;
	public float spellResist;

    [Tooltip("Resistance to movement altering, stun and silence effects (not implemented yet)")]
    public float Tenacity = 0;

	public float cost;
	public ResourceManager Cost;
	[HideInInspector]
	public UnitManager myManager;

    [HideInInspector]
    public bool StatsChanged;
	private List<Modifier> damageModifiers = new List<Modifier>();

	//BE Careful this can pass in both negative and positive numbers!
	private List<Modifier> EnergyModifiers = new List<Modifier>();
	private List<Modifier> HealModifiers = new List<Modifier>();
	public List<UnitTypes.UnitTypeTag> otherTags = new List<UnitTypes.UnitTypeTag>();
	private List<UnitTypes.UnitTypeTag> TotalTags = new List<UnitTypes.UnitTypeTag>();

	int typeBitMask;

	public UnitTypes.UnitArmorTag armorType;
	public UnitTypes.SizeTag sizeType;
	public UnitTypes.HeightType myHeight;
	private List<Modifier> deathTriggers = new List<Modifier>();
    private List<Modifier> LethalTriggers = new List<Modifier>();
    public List<KillModifier> killMods = new List<KillModifier>();

    Dictionary<System.Object, float> ShieldSources = new Dictionary<System.Object, float>();

	private Selected mySelection;

	[HideInInspector]
	public bool upgradesApplied;

	public SphereCollider visionRange;


	//private float nextActionTime;
	public VeteranStats veternStat;


	public GameObject deathCorpse;
	public GameObject deathEffect;
	public Sprite UnitPortrait;
	bool tagSet = false;

    /// <summary>
    /// these show little icons in the hud.
    /// </summary>
	public List<Buff> goodBuffs = new List<Buff>();
	public List<Buff> badBuffs = new List<Buff>();

	public StatChanger statChanger; // This contains all thigns that can alter any numbers for this unit, like Range or attackSpeed


	void Awake()
	{
		statChanger = new StatChanger(this);
		Initialize();
	}

	public void SetTags()
	{
		tagSet = true;
		TotalTags.Clear();
		foreach (UnitTypes.UnitTypeTag t in otherTags) {

			TotalTags.Add((UnitTypes.UnitTypeTag)Enum.Parse(typeof(UnitTypes.UnitTypeTag), t.ToString()));
		}
		TotalTags.Add((UnitTypes.UnitTypeTag)Enum.Parse(typeof(UnitTypes.UnitTypeTag), armorType.ToString()));
		TotalTags.Add((UnitTypes.UnitTypeTag)Enum.Parse(typeof(UnitTypes.UnitTypeTag), myHeight.ToString()));
		TotalTags.Add((UnitTypes.UnitTypeTag)Enum.Parse(typeof(UnitTypes.UnitTypeTag), sizeType.ToString()));

		int bitMask = 0;
		foreach (UnitTypes.UnitTypeTag t in TotalTags) {
			bitMask += (int)Mathf.Pow(2, ((int)t));
		}
		typeBitMask = bitMask;
		setDefensePriority();

		//Debug.Log (this.gameObject + ": "+bitMask);
	}

	public void Initialize()
	{
		if (!tagSet) {
			SetTags();
		}
		if (!myManager) {
			myManager = this.gameObject.GetComponent<UnitManager>();
			myManager.myStats = this;
		}

		if (isHero) {
			bool HasAName = !isUnitType(UnitTypes.UnitTypeTag.Turret) && !isUnitType(UnitTypes.UnitTypeTag.Structure);
			veternStat = new VeteranStats(HasAName, GetComponent<UnitManager>().UnitName,
				!(isUnitType(UnitTypes.UnitTypeTag.Turret)) && !(isUnitType(UnitTypes.UnitTypeTag.Worker)) && !(isUnitType(UnitTypes.UnitTypeTag.Structure)), GetComponent<UnitManager>().UnitName
				, myManager.PlayerOwner, myManager);
		}
		else {
			veternStat = new VeteranStats(!isUnitType(UnitTypes.UnitTypeTag.Turret) && !isUnitType(UnitTypes.UnitTypeTag.Structure), GetComponent<UnitManager>().UnitName,
					!(isUnitType(UnitTypes.UnitTypeTag.Turret)) && !(isUnitType(UnitTypes.UnitTypeTag.Worker)) && !(isUnitType(UnitTypes.UnitTypeTag.Structure)), "", myManager.PlayerOwner, myManager); }

		if (!mySelection) {
			mySelection = this.gameObject.GetComponent<Selected>();
		}
		veternStat.myUnit = myManager;
	}



	// Use this for initialization
	void Start() {
		if (!myManager) {
			myManager = GetComponent<UnitManager>();
			myManager.myStats = this;
		}

		if (Clock.main.getTotalSecond() < 1  && supply != 0) {
			myManager.getRaceManager().UnitCreated(supply);
		}

		myManager.getRaceManager().addVeteranStat(veternStat);
		if (isHero) {
			veternStat.UnitName = myManager.UnitName;
		}

		if (EnergyRegenPerSec > 0 && !WorldRecharger.main.ToEnergize.Contains(this)) {
			WorldRecharger.main.addEnergy(this);
		}
		if (HealthRegenPerSec > 0 && !WorldRecharger.main.ToHeal.Contains(this)) {
			WorldRecharger.main.addHeal(this);
		}
		veternStat.playerOwner = myManager.PlayerOwner;
		HealthRegenPerHalf = HealthRegenPerSec / 2;
		EnergyRegenPerHalf = EnergyRegenPerSec / 2;
	}

	void firstHealth()
	{ if (!isUnitType(UnitTypes.UnitTypeTag.Invulnerable)) {
			updateHealthBar();
		}
	}


	public void setEnergyRegen(float amount)
	{
		if (amount == 0 && EnergyRegenPerSec > 0) {
			WorldRecharger.main.removeEnergy(this);
		}
		else if (amount > 0 && EnergyRegenPerSec == 0) {
			WorldRecharger.main.addEnergy(this);
		}

		EnergyRegenPerSec = amount;
		EnergyRegenPerHalf = amount / 2;
	}

	public void setHealRate(float amount)
	{
		if (amount == 0 && HealthRegenPerSec > 0) {
			WorldRecharger.main.removeHeal(this);
		}
		else if (amount > 0 && HealthRegenPerSec == 0) {
			WorldRecharger.main.addHeal(this);
		}

		HealthRegenPerSec = amount;
		HealthRegenPerHalf = amount / 2;
	}

    /// <summary>
    /// CUrrently only works if you have the HeroDisplay Health Bar on the Unit
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="source"></param>
    public void SetShield(float amount, System.Object source)
    {
        if (amount <= 0)
        {
            ShieldSources.Remove(source);
        }

        else if (ShieldSources.ContainsKey(source))
        {
            ShieldSources[source] = amount;
        }
        else
        {
            ShieldSources.Add(source,amount);
        }


        float total = 0;
        foreach (KeyValuePair<object, float> pair in ShieldSources)
        {
                total += pair.Value;
        }
        Debug.Log("Setting shields " + total);
        getSelector().healthBar.SetShieldAmount(total);
        
    }

    public float getTenacityMultiplier()
    {
        return 1 - Tenacity;
    }

	public Selected getSelector()
	{
		if (!mySelection)
		{
			mySelection = gameObject.GetComponent<Selected>();
		}

		return mySelection;
	}

	int n = 0;
	public bool isUnitType(UnitTypes.UnitTypeTag type){

		n = (1 << ((int)type));
		return (typeBitMask &  n) ==  n;
	}

    /// <summary>
    /// This is currently depricated
    /// </summary>
    /// <param name="input"></param>
    /// <param name="stack"></param>
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

	/// <summary>
	/// This is called at the start and when a weapon is added, so that units will prioirtize units that can target them back in combat
	/// </summary>
	public void setAggressionPriority()
	{
		agressionPriority = 0;
		bool canAttackAir = true ;
		bool canAttackGRound = true;

		if (myManager.myWeapon.Count > 0)
		{
			foreach (IWeapon weap in myManager.myWeapon)
			{
				if (weap.cantAttackTypes.Contains(UnitTypes.UnitTypeTag.Air))
				{
					canAttackAir = false;
				}
				if (weap.cantAttackTypes.Contains(UnitTypes.UnitTypeTag.Ground))
				{
					canAttackGRound = false;
				}
			}
			if (canAttackAir)
			{
				agressionPriority += 1;
			}
			if (canAttackGRound)
			{
				agressionPriority += 2;
			}
		}
	}

	public void setDefensePriority()
	{
		
		if (myHeight == UnitTypes.HeightType.Air)
		{
			DefensePriority = 1;
		}
		else if (myHeight == UnitTypes.HeightType.Ground)
		{
			DefensePriority = 2;
		}
		else if (myHeight == UnitTypes.HeightType.Both)
		{
			DefensePriority = 3;
		}
		else
		{
			DefensePriority = 0;
		}
	}

	/// <summary>
	/// pass in the defensePriority of the guy targeting this guy
	/// </summary>
	public float getCombatPriority(int defensePri)
	{
		return attackPriority + (((defensePri & agressionPriority) > 0) ? 1 :0); 
	}


	public float TakeDamage(float amount, GameObject source, DamageTypes.DamageType type, UnitManager srcManager = null)
	{

		if (isUnitType(UnitTypes.UnitTypeTag.Invulnerable)) {
			return 0;
		}
			
		if (type != DamageTypes.DamageType.True) {
			foreach (Modifier mod in damageModifiers) {
				if (mod != null) {
					amount = mod.modify (amount, source, type);
					if (amount <= 0) {
						return 0;
						}
					}
				}
		}

		if (type == DamageTypes.DamageType.Regular || type == DamageTypes.DamageType.Wound ) {

			amount = Mathf.Max (amount - armor, 1);
		}

		AttackResponse(source, srcManager);

		if (veternStat != null) {
			veternStat.UpMitigated(armor);
			veternStat.UpdamTaken (amount);
		}

		health -= amount;
		
		if (health < 1) {
            if (RunLethalDamage(amount))
            {
                kill(source, srcManager);
            }
            else
            {
                health = 1;
                updateHealthBar();
            }
		} else {
			updateHealthBar ();
		}
		
		return amount;
	}

	float nextResponseTime = 0;

	void AttackResponse(GameObject source, UnitManager srcManager)
	{
		if (nextResponseTime < Time.time)
		{
			nextResponseTime = Time.time + 15;
			if (myManager.PlayerOwner == 1 && source != this.gameObject)
			{
				mySelection.attackBlink();
				if (isUnitType(UnitTypes.UnitTypeTag.Structure))
				{
					ErrorPrompt.instance.underBaseAttack(this.transform.position);
				}
				else
				{
					ErrorPrompt.instance.underAttack(this.transform.position);
				}
			}
		
			if (srcManager)
			{
				myManager.Attacked(srcManager);
			}
		}
	}

	/// <summary>
	/// As a percent
	/// </summary>
	public void SetHealth (float percent)
	{
		health = Maxhealth * percent;
        StatsChanged = true;
        updateHealthBar ();
	}

	public void setHealthValue(float amount)
	{
		health = amount;
        StatsChanged = true;
        updateHealthBar();
	}

	public void setEnergy(float amount)
	{
		currentEnergy = amount;
        StatsChanged = true;
		updateEnergyBar();
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
		kill(deathSource, null);
	}
	
	public void kill(GameObject deathSource, UnitManager srcManager = null)
	{
		if (dead)
			return;

		if (!isUnitType(UnitTypes.UnitTypeTag.Invulnerable)) {

			if (this) {
				if (!myManager.myRacer.UnitDying (myManager, deathSource, true)) {
					return;
				}
			}
				
			mySelection.buffDisplay.DeleteMe ();

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
				
				GameObject.Instantiate (deathCorpse, spawnLoc, this.gameObject.transform.rotation);

			}
			if (EnergyRegenPerSec > 0) {
				WorldRecharger.main.removeEnergy (this);
			}
			if (HealthRegenPerSec > 0) {
				WorldRecharger.main.removeHeal(this);
			}
			
			if (srcManager) {
				srcManager.myStats.upKills ();
			}
			
				//fix this when we have multiplayer games, here for optimizations?
			myManager.myRacer.UnitDied (supply, myManager);
			if (myManager.PlayerOwner == 1)
			{
				SelectedManager.main.updateControlGroups(myManager);
			}

			if (mySelection.IsSelected) {
				SelectedManager.main.DeselectObject (myManager);
			}
			if (deathEffect) {
				Instantiate (deathEffect, this.gameObject.transform.position, Quaternion.identity);
			}
	
			
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

    bool RunLethalDamage(float damage)
    {
        foreach (Modifier mod in LethalTriggers)
        {
            if (mod.modify(damage, null, DamageTypes.DamageType.Regular) > 0)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Lethal Damage Triggers should return the damage passed in if it doesn't actually stop death. Return 0 if Death is halted
    /// </summary>
    /// <param name="mod"></param>
	public void addLethalTrigger(Modifier mod)
	{
        LethalTriggers.Add(mod);
    }

    public void removeLethalTrigger(Modifier mod)
    {
        LethalTriggers.Remove(mod);
    }

    public void removeDeathTrigger (Modifier mod)
	{
		deathTriggers.Remove (mod);
	}
	public void addDeathTrigger( Modifier mod)//Method method)
	{
		if (!deathTriggers.Contains (mod)) {
			deathTriggers.Add (mod);
		}
	}

	/// <summary>
	/// priority of 0 puts it at the front of the queue
	/// </summary>
	public void addModifier(Modifier mod, int priority = -1)
	{
		if (!damageModifiers.Contains (mod)) {
			if (priority != -1) {
				
				damageModifiers.Insert (priority, mod);
			} else {
				damageModifiers.Add (mod);
			}
		}
	}

	public void addHighPriModifier(Modifier mod)
	{
		if (!damageModifiers.Contains (mod)) {
			damageModifiers.Insert(0,mod);
		}
	}

	public void removeModifier(Modifier mod)
	{
		if (damageModifiers.Contains (mod)) {
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
	{
		if (EnergyModifiers.Contains (mod)) {
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
	{
		if (MaxEnergy == 0 ||  (n > 0 && currentEnergy == MaxEnergy)) {
			return 0;
		}
		
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
      
		if (health == Maxhealth) {
			return 0;
		}
        //Debug.Log("HEaling " + health + "  " + Maxhealth);
        foreach (Modifier mod in HealModifiers) {
			if (mod != null)
			{
				n = mod.modify(n, null, type);
			}
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


	public int getKills()
	{
		return veternStat.kills;
	}



}
