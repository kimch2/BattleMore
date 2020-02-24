using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//this class extends RTSObject through the Unit class
public class UnitManager : Unit, IOrderable {

    //AbilityList<Ability> is in the Unit Class
    public string UnitName;

    public int PlayerOwner; // 1 = active player, 2 = enemies, 3 = nuetral
    [Tooltip("Units with a lower number will be in the front of the formation")]
    public int formationOrder;
    public Animator myAnim;

    private float chaseRange;  // how far an enemy can come into vision before I chase after him.
    public IMover cMover;      // Pathing Interface. Classes to use here : AirMover(Flying Units), cMover (ground, uses Global Astar) , RVOMover(ground, Uses Astar and unit collisions, still in testing)
    public List<IWeapon> myWeapon = new List<IWeapon>();   // IWeapon is not actually an interface but a base class with required parameters for all weapons.
    public bool MultiWeaponAttack;
    public UnitStats myStats; // Contains Unit health, regen, armor, supply, cost, etc

    public Iinteract interactor; // Passes commands to this to determine how to interact (Right click on a friendly could be a follow command or a cast spell command, based on the Unit/)

    public float visionRange;
    SphereCollider visionSphere; // Trigger Collider that respresents vision radius. Size is set in the start function based on visionRange
                                 // When Enemies enter the visionsphere, it puts them into one of these categories. They are removed when they move away or die.
                                 //[HideInInspector]
    public List<UnitManager> enemies = new List<UnitManager>();
    [HideInInspector]
    public List<UnitManager> allies = new List<UnitManager>();
    [HideInInspector]
    public List<GameObject> neutrals = new List<GameObject>();


    private LinkedList<UnitState> queuedStates = new LinkedList<UnitState>(); // Used to queue commands to be executed in succession.

    private UnitState myState;     // used for StateMachine

    private List<Object> stunSources = new List<Object>();     // Used to keep track of stun lengths and duration, to ensure the strongest one is always applied.
    private List<Object> silenceSources = new List<Object>();

    public voiceResponse myVoices;

    //List of weapons modifiers that need to be applied to weapons as they are put on this guy
    private List<Notify> potentialNotify = new List<Notify>();

    public List<Ability> myAddons = new List<Ability>(); // Currently being used so we can see Repair bays in a weapon slot
                                                         //[HideInInspector]
    public RaceManager myRacer;

    public CharacterController CharController;
    public FogOfWarUnit fogger;
    [System.Serializable]
    public struct voiceResponse
    {
        public List<AudioClip> moving;
        public List<AudioClip> attacking;

    }

    private bool isStunned;
    private bool isSilenced;

    public List<StartCommand> startingCommand;
    [Tooltip("Use this if you do not put anything in Starting COmmand List")]

    public UnitState.StateType startingState;



    new void Awake()
    {
        if (interactor == null) {
            interactor = (Iinteract)gameObject.GetComponent(typeof(Iinteract));
        }

        if (visionSphere == null) {
            foreach (SphereCollider sphere in gameObject.GetComponents<SphereCollider>()) {
                if (sphere.isTrigger) {
                    visionSphere = this.gameObject.GetComponent<SphereCollider>();
                    break;
                }
            }
        }

        if (cMover == null) {
            cMover = (IMover)gameObject.GetComponent(typeof(IMover));
        }


        if (myWeapon.Count == 0) {
            foreach (IWeapon w in GetComponents<IWeapon>()) {
                if (!myWeapon.Contains(w)) {
                    myWeapon.Add(w);
                }
            }
        }


        if (!CharController) {
            CharController = GetComponent<CharacterController>();
        }

        if (!myStats) {
            myStats = gameObject.GetComponent<UnitStats>();
        }



        GameManager man = GameManager.getInstance();
        if (PlayerOwner == man.playerNumber) {
            this.gameObject.tag = "Player";
        }

        myStats.Initialize();
        //initializeVision ();


        if (startingState == UnitState.StateType.HoldGround) {
            changeState(new HoldState(this));
        } else if (cMover != null) {
            changeState(new DefaultState());
        } else if (startingState == UnitState.StateType.Turret) {
            changeState(new turretState(this));
        }

        chaseRange = visionRange + 40;
    }

    public void initializeVision()
    {
        if (!fogger)
        {
            fogger = gameObject.GetComponent<FogOfWarUnit>();

        }

        if (!myStats)
        {
            myStats = GetComponent<UnitStats>();
        }

        if (!fogger)
        {
            if ((PlayerOwner == 1 && !myStats.isUnitType(UnitTypes.UnitTypeTag.Turret)) || myStats.getSelector().ManualFogOfWar)
            {
                fogger = gameObject.AddComponent<FogOfWarUnit>();
                if (cMover)
                {
                    cMover.myFogger = fogger;
                }
            }
        }
        else
        {
            if (PlayerOwner != 1 && !myStats.getSelector().ManualFogOfWar)
            {
                //Debug.Log("On " + this.gameObject);
                if (Application.isPlaying)
                {
                    Destroy(fogger);
                }
                //fogger.enabled = false;
                //fogger = null;
            }
        }
        float distance = visionRange + 3;
        if (CharController) {
            distance += CharController.radius;
        }

        if (!visionSphere)
        {
            foreach (SphereCollider sc in GetComponents<SphereCollider>())
            {
                if (sc.isTrigger)
                {
                    visionSphere = sc;
                    break;
                }
            }
        }

        visionSphere.radius = distance;
        if (fogger) {
            fogger.radius = distance;
            fogger.enabled = true;
        }

    }


	bool hasStarted = false;
	public void Start()
	{

		if (!hasStarted) {

			if (startingCommand.Count > 0 || cMover) {
				Invoke ("GiveStartCommand", .1f);
			}
            //Debug.Log("IN here " + (Time.timeSinceLevelLoad < 1) +"    "+!myStats.isUnitType(UnitTypes.UnitTypeTag.Structure) + "    " + myStats.isUnitType(UnitTypes.UnitTypeTag.Add_On));
			if (Time.timeSinceLevelLoad < 1 || !myStats.isUnitType (UnitTypes.UnitTypeTag.Structure) || myStats.isUnitType (UnitTypes.UnitTypeTag.Add_On)) {

                GameManager.getInstance ().playerList [PlayerOwner - 1].addUnit (this);
			}

			myStats.setAggressionPriority();
			myRacer = GameManager.main.playerList[PlayerOwner - 1];
			hasStarted = true;
			initializeVision();
            if (myAnim)
            {
                foreach (AnimatorControllerParameter parem in myAnim.parameters)
                {
                    if (parem.name == "State")
                    {
                        hasAnimState = true;
                        break;
                    }
                }
            }

        }
	}

	public RaceManager getRaceManager()
	{
		if (myRacer == null)
		{
			myRacer = GameManager.main.playerList[PlayerOwner - 1];
		}
		return myRacer;
	}


	public void addAddon(Ability toAdd)
	{
		myAddons.Add(toAdd);
	}

	public void removeAddon(Ability toAdd)
	{
		myAddons.Remove(toAdd);
	}

	void GiveStartCommand()
	{
		foreach (StartCommand command in startingCommand) {

			RaycastHit hit;
			Vector3 newLocation = transform.position;
			if (Physics.Raycast (command.location + transform.position, Vector3.down, out hit, 1000, 1 << 8)) {

				newLocation = hit.point;
			}


			if (command.myCommand == StartCommand.CommandType.AttackMove) {
				GiveOrder (Orders.CreateAttackMove (newLocation, startingCommand.Count > 1));

			} else if (command.myCommand == StartCommand.CommandType.Move) {
				GiveOrder (Orders.CreateMoveOrder (newLocation, startingCommand.Count > 1));

			} else {
				GiveOrder (Orders.CreatePatrol (newLocation, startingCommand.Count > 1));
			}
		}

		if (startingCommand.Count == 0 && PlayerOwner == 1 && Time.timeSinceLevelLoad < 1) {

			if (cMover && cMover is CustomRVO) {
				GiveOrder (Orders.CreateMoveOrder (transform.position, false));

			}
		}
	}



	//Elsewhere this command is called on the RTSObject class, which is not a monobehavior, and cannot access its gameobject.
	public new GameObject getObject()
	{return this.gameObject;}



	
	// Update is called once per frame
	void Update () {

		//System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
		//stopwatch.Start();
		if (myState != null) {
			//Debug.Log ("This " + this.gameObject  + "  " + myState);
			myState.Update ();
		} else {
			enabled = false;
		}

		//stopwatch.Stop();
		//Debug.Log(this.gameObject + "  " + stopwatch.ElapsedTicks);
	}


	/// <summary>
	/// Adds the listeners for a weapon Attack.
	/// </summary>
	/// <param name="toAdd">To add.</param>
	public void addNotify(Notify toAdd)
	{
		if (!potentialNotify.Contains (toAdd)) {
			potentialNotify.Add (toAdd);
		}

		foreach (IWeapon weap in myWeapon) {
		
			if (!weap.triggers.Contains (toAdd)) {
				weap.triggers.Add (toAdd);
			}
		}
	}


	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		foreach (StartCommand command in startingCommand) {
			Gizmos.DrawSphere (transform.position +  command.location, 1);
		}
	}


	override
	public bool UseAbility(int n, bool queue)
	{
		
		if (!isStunned && !isSilenced) {

			continueOrder order = null;
			if (abilityList [n] != null) {
				order = abilityList [n].canActivate (true);

				if (order.canCast) {

					changeState (new CastAbilityState (abilityList [n]),false,queue);

				}
			}
			return order.nextUnitCast;
		}
		return true;
	}


	override
	public bool UseTargetAbility(GameObject obj, Vector3 loc, int n, bool queue) // Either the obj - target or the location can be null.
	{//Debug.Log("Targeting " + isStunned);
		continueOrder order = new continueOrder();
		if ((!isStunned && !isSilenced) || (isStunned && queue)) { // If stuff breaks with abilities, this second bool operator may have broken it, also look in the page where the queue is forcibly set
			if (abilityList [n] != null) {
	
				order = abilityList [n].canActivate (true);
		
				if (order.canCast) {
					if (abilityList [n] is TargetAbility) {
						
						changeState (new AbilityFollowState (obj, loc, (TargetAbility)abilityList [n]), false, queue);
					} else if (abilityList [n] is Morph || abilityList [n] is BuildStructure) {
						changeState (new PlaceBuildingState (obj, loc, abilityList [n]), false, queue);
					} else if (abilityList [n] is SummonStructure) {
						((SummonStructure)abilityList [n]).setBuildSpot (loc, obj);
						((SummonStructure)abilityList [n]).Activate ();
					}
                    else if (abilityList[n] is ValhallaBuilder)
                    {
                        ((ValhallaBuilder)abilityList[n]).setBuildSpot(loc, obj);
                        ((ValhallaBuilder)abilityList[n]).Activate();
                    }

                }

			}
		} 
		return order.nextUnitCast;
	}



	override
	public void autoCast(int n, bool offOn) // Program in how it is autocast in a custom UnitState, which should be accessed/created from the interactor class
	{
		if (abilityList [n] != null) {

			abilityList [n].setAutoCast(offOn);
		}
	}



	public void setInteractor()
	{
		if(interactor == null)
		{
			interactor = (Iinteract)gameObject.GetComponent(typeof(Iinteract));
		}

		Start (); // reset some variables

	}


	public new void GiveOrder (Order order)
	{
		if (!order.queued)
		{
			if (myState is ChannelState)
			{
				//order.queued = true;
				foreach (UnitState s in queuedStates)
				{

					if (s is PlaceBuildingState)
					{
						//Debug.Log ("Cenceling");
						((PlaceBuildingState)s).cancel();
					}
				}
				if (!order.queued)
				{
					queuedStates.Clear();
				}
				//return;
			}
		}


		if (interactor != null) {
			interactor.computeInteractions (order);

		}
	}

	/// <summary>
	/// If you empy the nulls as the bonus Arg, it will shift all abilities into the left msot slots.
	/// </summary>
	/// <param name="abil">Abil.</param>
	/// <param name="emptyNulls">If set to <c>true</c> empty nulls.</param>
	public void removeAbility(Ability abil, bool emptyNulls = false)
	{
		abilityList.Remove (abil);
		if (emptyNulls) {
			abilityList.RemoveAll (item => item == null);
		}
	}


	public void AddEnemySighted(EnemySighted comp)
	{
		EnemyWatchers.Add (comp);
	}

	public void AddAllySighted(AllySighted comp)
	{
		AllyWatchers.Add (comp);
	}



	List<EnemySighted> EnemyWatchers = new List<EnemySighted> ();
	List<AllySighted> AllyWatchers = new List<AllySighted> ();

	//Other Units have entered vision
	void OnTriggerEnter(Collider other)
	{

		if (!other.isTrigger) {

			/// check if still hit
			if (other.gameObject.layer == 15) { // Its a projectile, the most common kind of trigger
				return;
			}

			UnitManager manage = other.gameObject.GetComponent<UnitManager>();

		
			if (manage) {
				if (manage.PlayerOwner == 3) {
					neutrals.Add (other.gameObject);
					return;
				}

				if (manage.PlayerOwner != PlayerOwner) {
					enemies.Add (manage);
					foreach (EnemySighted sighter in EnemyWatchers) {
						if (sighter != null) {
							sighter.EnemySpotted (manage);
						}
					}
				} else {
					allies.Add (manage);
					foreach (AllySighted sighter in AllyWatchers) {
						if (sighter != null) {
							sighter.AllySpotted (manage);
						}
					}
				}
			}
		}
	}

	// Other units have left the vision
	void OnTriggerExit(Collider other)
	{
		if (other.isTrigger) {
			return;
		}
		if (other.gameObject.layer == 15) { // Its a projectile, the most common kind of trigger
			return;
		}

		UnitManager manage = other.gameObject.GetComponent<UnitManager>();

			if(	enemies.Remove (manage)){
				foreach (EnemySighted sighter in EnemyWatchers) {
					if (sighter != null) {
						sighter.enemyLeft (manage);
					}
				}
		} else if (allies.Remove (manage)) {

				foreach (AllySighted sighter in AllyWatchers) {
					if (sighter != null) {
						sighter.allyLeft (manage);
					}
				}
			}
			else {
				neutrals.Remove (other.gameObject);
			}
	}


	bool erase = false;
	public UnitManager findClosestEnemy()
	{
		UnitManager best = null;
		float currDistance = 0;
		float distance = float.MaxValue;
	
		
		for (int i = 0; i < enemies.Count; i ++) {
			if (enemies [i] != null) {
				
				currDistance = Vector3.Distance (enemies [i].transform.position, this.gameObject.transform.position);
				if (currDistance < distance) {
					best = enemies [i];
					distance = currDistance;
				}
				
			} else {
				erase = true;
			}
		}
		if (erase) {
			enemies.RemoveAll(item => item == null);
			erase = false;
		}

		return best;
	}


	UnitManager currentIter;
	float currDistance;
	float bestPriority;

	public UnitManager findBestEnemy(out float distance, UnitManager best) // Similar to above method but takes into account attack priority (enemy soldiers should be attacked before buildings)
	{
		float currentIterPriority;
		if (best != null) {
			distance = Vector3.Distance (best.transform.position, transform.position);
			bestPriority = best.myStats.getCombatPriority(myStats.DefensePriority);
		} else {

			distance = float.MaxValue;
			bestPriority = -1;
		}

		for (int i = 0; i < enemies.Count; i ++) {

			if (enemies[i] == null) {
				continue;
			}

			currentIter = enemies [i];

			if (!isValidTarget (currentIter)) {
				continue;
			}
			currentIterPriority = currentIter.myStats.getCombatPriority(myStats.DefensePriority);
			if (currentIterPriority > bestPriority) {
				best = currentIter;
				bestPriority = currentIterPriority;
				distance = Vector3.Distance (currentIter.transform.position, this.gameObject.transform.position);
			}
			else if (currentIterPriority == bestPriority) {
			
				currDistance = Vector3.Distance (currentIter.transform.position, this.gameObject.transform.position);

				if (currDistance < distance) {
					best = currentIter;
					distance = currDistance;
				}
			}
		}

		return best;
	}

	public void setInteractor(Iinteract inter)
	{interactor = inter;
		//Start ();
	}

    public void AddUnFoggerManual()
    {
        if (!fogger)
        {
            fogger = gameObject.GetComponent<FogOfWarUnit>();
            if (!fogger)
            {
                fogger = gameObject.AddComponent<FogOfWarUnit>();
            }
        }
        fogger.radius = visionRange;

        if (cMover)
        {
            cMover.myFogger = fogger;
        }
        fogger.enabled = true;
      
    }

	private UnitState popLastState()
	{
		
		UnitState us = queuedStates.Last.Value;
		queuedStates.RemoveLast ();
		return us;
	}

	public UnitState popFirstState()
	{UnitState us = queuedStates.First.Value;
		queuedStates.RemoveFirst ();
		return us;
	}

	public void nextState() // Used when executing queued commands
	{
		if (myState is CastAbilityState) {
			if (queuedStates.Count > 0) {
				myState = popFirstState();
				if (myState != null) {
					myState.myManager = this;
					myState.initialize ();
				}
			} else {
				changeState (new DefaultState ());
			}
		} 
	}


	public void changeState(UnitState nextState)
	{
		changeState (nextState, false, false);
	}


	public void Dying()
	{
		foreach(UnitState states in queuedStates)
		{
			states.endState ();
		}
	}
	// make sure that Queue front and queueback are never both true
	public void changeState(UnitState nextState, bool Queuefront, bool QueueBack)
	{
		if (nextState != null && !isStunned) {
			enabled = true;
		} // THIS MAY BREAK THINGS - IT Did, stun no longer works

		if (myState is  ChannelState && !(nextState is DefaultState)) {
			queuedStates.AddLast (nextState);
			//Debug.Log ("Queuing " + nextState + "   " + queuedStates.Count);
			return;
			}

		//Debug.Log ("# of states  " +QueueBack + "    " + queuedStates.Count + "    " + nextState + "    " + myState);
		if (Queuefront && (!(nextState is DefaultState) && (queuedStates.Count > 0 || !(myState is DefaultState)))) {

			//Debug.Log ("Queing + " +nextState);
			queuedStates.AddFirst (myState);
			myState =interactor.computeState (nextState);
			myState.initialize ();

			return;
		}
		else if (QueueBack && (!(nextState is DefaultState) && (queuedStates.Count > 0 || !(myState is DefaultState)))){

			queuedStates.AddLast (nextState);

			return;

		}

		else if (nextState is DefaultState) {
			if (queuedStates.Count > 0) {
				if (myState != null) {
					myState.endState ();
				}
				//Debug.Log("HereA");
				myState = interactor.computeState(popFirstState());
			
				if (myState == null) {
					return;
				}
			
				myState.myManager = this;
				myState.initialize ();
				return;

			}  
		}
	
		else if (nextState is AttackMoveState ) {
			((AttackMoveState)nextState).setHome (this.gameObject.transform.position);
		}
			

		nextState.myManager = this;
	
		foreach (UnitState s in queuedStates) {

			if (s is PlaceBuildingState)
			{
				//Debug.Log ("Cenceling");
				((PlaceBuildingState)s).cancel();
			}
			else
			{
				s.endState(); // Risky line, here to make sure ore depos lose their dudes
			}
		}

			queuedStates.Clear ();


		if (nextState is CastAbilityState && ((CastAbilityState)nextState).myAbility.continueMoving) {

			queuedStates.AddLast (myState);
		}
		if (myState != null) {
			myState.endState ();
		}
		//Debug.Log("HereB");
		myState =interactor.computeState (nextState);

		if (myState!= null) {
			myState.initialize ();
		}
	

	}



	public void cancel()
	{
		//Debug.Log ("Here somehow");
		foreach (UnitState s in queuedStates) {

			if (s is PlaceBuildingState) {

				((PlaceBuildingState)s).cancel ();
			}
		}
		queuedStates.Clear ();
	}

    bool hasAnimState;
	public void animMove()
	{
        // Debug.Log("Animove");
        if (myAnim)
        {
            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Cast"))
            {
                return;
            }

            if (hasAnimState)
            {
                myAnim.SetInteger("State", 2);
            }
            else 
            {
                myAnim.Play("Move");
            }
        }
	}

	public void animAttack()
	{
		if (myAnim ) {
			myAnim.Play ("Attack");
		}
	}

	public void animStop()
	{
        if (hasAnimState)
        {
            myAnim.SetInteger("State", 1);
        }
	}

    public void LookAtTarget(Vector3 target)
    {

        target.y = this.transform.position.y;
        this.gameObject.transform.LookAt(target);
    }

	public override UnitStats getUnitStats ()
	{
        if (!myStats)
        {
            myStats = GetComponent<UnitStats>();
        }
		return myStats;
	}

	public override UnitManager getUnitManager ()
	{
		return this;
	}

    public GameObject CreateInstance(Vector3 location, int playerNumber)
    {
       
        GameObject newInstance = Instantiate<GameObject>(this.gameObject, location,Quaternion.identity);
        newInstance.GetComponent<UnitManager>().PlayerOwner = playerNumber;
        return newInstance;
    }


	public IWeapon inRange(UnitManager obj)
	{
		float min= 100000000;
		IWeapon best = null;
		foreach (IWeapon weap in myWeapon) {
			if (weap.inRange (obj)) {
				if (weap.range < min) {
					best = weap;
					min = weap.range;
				}
			}

		}
		return best;
	}

	public IWeapon isValidTarget(UnitManager obj)
	{

		foreach (IWeapon weap in myWeapon) {
			if( weap.isValidTarget(obj)){
				return weap; 
			}

		}
		return null;
	}

	public IWeapon canAttack(UnitManager obj)
	{IWeapon best = null;
		float min= 100000000;
		foreach (IWeapon weap in myWeapon) {

			if(weap.canAttack(obj)){

				if (weap.range < min) {

					best = weap;
					min = weap.range;
				}
			}
		}
		return best;
	}




	public void enQueueState(UnitState nextState)
	{queuedStates.AddLast (nextState);
	
	}


	public void cleanEnemy()
	{
		enemies.RemoveAll(item => item == null);
	}

	public void cleanAlly()
	{
		allies.RemoveAll(item => item == null);
	}
		public void setStun(bool StunOrNot, Object source,bool  showIcon)
	{

		if (StunOrNot) {
			stunSources.Add (source);
			if (cMover) {
				cMover.stop ();
			}
		} else {
			
			stunSources.Remove (source);
			stunSources.RemoveAll (item => item == null);

		}
	//	Debug.Log ("StunningBB " + StunOrNot + "   " + stunSources.Count + "   " + stunSources[0]);
		enabled = !(stunSources.Count > 0);
		isStunned = (stunSources.Count > 0);

		//Debug.Log ("Is stunned ");
		if (isStunned && StunRun == null && showIcon ) {
			StunRun = StartCoroutine (stunnedIcon());
		}
		
	}

	Coroutine StunRun;

	IEnumerator stunnedIcon()
	{
	//	Debug.Log ("Starting stun");

		GameObject icon =  PopUpMaker.CreateStunIcon (this.gameObject);
		while (isStunned) {
		
			yield return null;
		}

		Destroy (icon);
		StunRun = null;
	}

	public void StunForTime(Object source, float duration, bool showIcon = true)
	{

		StartCoroutine (stunOverTime (source, duration * (1 - myStats.Tenacity)));
		if (showIcon && isStunned && StunRun == null) {
			StunRun = StartCoroutine (stunnedIcon());
		}

	}

	IEnumerator stunOverTime(Object source, float duration)
	{
		if (cMover) {
			cMover.stop ();
		}
		stunSources.Add (source);
		isStunned = (stunSources.Count > 0);
		enabled = !(stunSources.Count > 0);
		yield return new WaitForSeconds (duration);
		if (stunSources.Contains (source)) {
			stunSources.Remove (source);
		} else {
			stunSources.RemoveAll (item => item == null);
		}

	isStunned = (stunSources.Count > 0);
		enabled = !(stunSources.Count > 0);
		
	}

    public void ExternalMove(Vector3 translationAmount, bool isFriendly)
    {
        if (!isFriendly)
        {
            translationAmount *= myStats.getTenacityMultiplier();
        }
        transform.Translate(translationAmount,Space.World);
    }


	public void setSilence(bool input, Object source)
	{
		if (input) {
			silenceSources.Add (source);
		} else {
			if (silenceSources.Contains (source)) {

				silenceSources.Remove (source);}
		}

		isSilenced= (silenceSources.Count > 0);
	}


	public bool Silenced()
	{return isSilenced;
	}

	public bool Stunned()
	{return isStunned;
	}


	public void setWeapon(IWeapon weap)
	{
		if (weap) {
			if (!myWeapon.Contains (weap)) {
				myWeapon.Add (weap);
				foreach (Notify not in potentialNotify) {
					if (!weap.triggers.Contains (not)) {
						weap.triggers.Add (not);
					}
				}
				myStats.setAggressionPriority();
			}
		}
	}

	public void removeWeapon(IWeapon weap)
	{if (!weap) {
			return;}
		
		if (myWeapon.Contains (weap)) {
			myWeapon.Remove(weap);
			myStats.setAggressionPriority();
		}
		foreach (Notify not in potentialNotify) {

			if (weap.triggers.Contains (not)) {
				weap.triggers.Remove (not);
			}
		}


	}



	public bool isIdle()
		{ // will need to be refactored if units require a custom default state
		if(myState.GetType() == typeof(DefaultState))
		{return true;	}
		return false;
	}

	public void Attacked(UnitManager src) //I have been attacked, what do i do?
	{
	//	Debug.Log (this.gameObject + " attacked by " + src + " state is " + myState);
		if (myState != null) {

			myState.attackResponse (src, 5);
		}
	}

	public SphereCollider getVisionSphere()
	{
		return visionSphere;
	}

	public UnitState getState()
	{return myState;}

	public int getStateCount()
	{return queuedStates.Count;
	}

	public UnitState checkNextState()
	{if (queuedStates.Count > 0) {
			return queuedStates.First.Value;
		} else {
		return null;}
	}

	public float getChaseRange()
	{return chaseRange;}

}

[System.Serializable]
public class StartCommand
{
	public enum CommandType{Move, AttackMove, Patrol}
	public CommandType myCommand;
	public Vector3 location;

}