using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Ability : MonoBehaviour {

	public string Name;
	[TextArea(2,10)]
	public string Descripton;

	public Sprite iconPic;
	public AbstractCost myCost;
	public enum type{passive, target, activated, building}
	protected type myType;
	[Tooltip("Check this if this ability should not interrupt the units movement")]
	public bool continueMoving;
	//These are seperate because Unit inspector wont show dictionaries
	public List<string> RequiredUnit = new List<string>();
	private Dictionary<string, bool> requirementList = new Dictionary<string, bool> ();
	protected UnitManager myManager;

	//public GameObject UIButton;
	protected string description;
	[Tooltip("Check this if you have programmed something for autocasting")]
	public bool canAutoCast;
	public bool autocast;
	[Tooltip("Check this if this ability should show in UI but be grayed out")]
	public bool active = false;
    public int maxChargeCount = 0;

   [Tooltip("-1 means it doesn't use charges")]
    public int chargeCount = -1;


	public abstract continueOrder canActivate(bool error);
	public abstract void Activate();  // returns whether or not the next unit in the same group should also cast it
	public abstract void setAutoCast(bool offOn);
	public AudioClip soundEffect;
	protected AudioSource audioSrc;
	protected Selected select;
    Coroutine currentCharger;
    public OnHitContainer myHitContainer;

    public virtual void Start() // We have this here so other source can call Start and any of this guy's inheriters will have it called instead
	{
		
	}

	protected void initialize()
		{
		myManager = GetComponent<UnitManager>();
        if (!myManager)
        {
            myManager = GetComponentInParent<UnitManager>();
        }

		foreach (string s in RequiredUnit) {

			requirementList.Add (s, false);
			GameManager.getInstance ().playerList [GetComponent<UnitManager> ().PlayerOwner - 1].addBuildTrigger (s, this);
		}

		if (requirementList.Count  > 0 && requirementList.ContainsValue (false)) {

		//	Debug.Log (this.gameObject.name + "  was created " + (requirementList.Count == 0) + "   " + (!requirementList.ContainsValue (false)));
			active = false;
		} 
	}


	protected void Awake()
	{
	
		myManager = GetComponent<UnitManager>();
        if (!myManager)
        {
            myManager = GetComponentInParent<UnitManager>();
        }
        InitializeSelect();
		audioSrc = GetComponent<AudioSource> ();
		StartCoroutine( delayedInitialize());
	}
	IEnumerator delayedInitialize()
	{
		yield return null;
		initialize();
	}


	public type getMyType()
	{return myType;
	}


	public void newUnitCreated(string newUnit)
	{

		if (requirementList.Count == 0) {
			return;
		}

		if (RequiredUnit.Contains(newUnit)) {

			//Debug.Log ("I have a " + newUnit);
			requirementList [newUnit] = true;
		}

		if (!requirementList.ContainsValue (false)) {

			active = true;
			if (GetComponent<Selected> ().IsSelected) {
				RaceManager.updateActivity ();
			}
		} 	
	}


	public void UnitDied(string unitname)
	{
		if (requirementList.Count == 0) {
			return;
		}

		if (RequiredUnit.Contains (unitname)) {
			requirementList [unitname] = false;
		}

		if (requirementList.ContainsValue (false)) {
			active = false;
			if (GetComponent<Selected> ().IsSelected) {
				RaceManager.updateActivity ();
			}
		}

	}

	protected void updateAutocastCommandCard()
	{
        InitializeSelect();
		if (select && select.IsSelected) {
			RaceManager.upDateAutocast ();
		}

	}


	protected void updateActiveCommandCard()
	{
        InitializeSelect();
        if (select && select.IsSelected) {
				RaceManager.updateActivity();
		}

	}

    protected void updateUICommandCard()
	{
        InitializeSelect();
        if (select && select.IsSelected) {
				RaceManager.upDateUI();
			}

    }

    void InitializeSelect()
    {
        if (!select)
        {
            if (!myManager)
            {
                myManager = GetComponent<UnitManager>();
                if (!myManager)
                {
                    myManager = GetComponentInParent<UnitManager>();
                }
            }
            if (myManager)
            {
                select = myManager.getUnitStats().getSelector();
            }
        }
    }

    public void InitializeCharges()
    {
        if (chargeCount > -1)
        {
            if (chargeCount < maxChargeCount)
            {
                currentCharger = StartCoroutine(increaseCharges());
            }
        }
    }


    IEnumerator increaseCharges()
    {
        if (chargeCount == 0)
        {
            active = false;
        }
        myCost.startCooldown();
        yield return new WaitForSeconds(myCost.cooldown - .2f);

        active = true;
        changeCharge(1);

        if (chargeCount < maxChargeCount)
        {
            currentCharger = StartCoroutine(increaseCharges());
        }
        else
        {
            currentCharger = null;
        }
    }


    public void changeCharge(int n)
    {
        if (chargeCount > -1)
        {

            chargeCount += n;
            if (chargeCount == 0)
            {
                active = false;
            }
            if (chargeCount > maxChargeCount)
            {
                chargeCount = maxChargeCount;
            }
            if (chargeCount < maxChargeCount && currentCharger == null)
            {
                currentCharger = StartCoroutine(increaseCharges());
            }
            updateUICommandCard();

            updateActiveCommandCard();
        }
    }


    public void UpMaxCharge()
    {
        maxChargeCount = 3;

        if (currentCharger == null)
        {
            currentCharger = StartCoroutine(increaseCharges());
        }
    }

    protected continueOrder BaseCanActivate(bool ShowError)
    {
        continueOrder order = new continueOrder();
        
        if (myCost && !myCost.canActivate(this))
        {
            order.canCast = false;
            if (myCost.energy == 0 && myCost.resourceCosts.MyResources.Count == 0 && chargeCount > 0)
            {
                order.canCast = true;
                order.nextUnitCast = false;
            }
        }
        else
        {
            order.nextUnitCast = false;
        }
        return order;
    }

    protected void BaseSetAutoCast(bool isOn)
    {
        if (canAutoCast)
        {
            autocast = isOn;
        }
    }

    public virtual void OnDeath() { }
}
