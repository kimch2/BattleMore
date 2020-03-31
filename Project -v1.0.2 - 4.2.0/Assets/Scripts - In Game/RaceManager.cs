using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using Pathfinding;

public class RaceManager : MonoBehaviour, ManagerWatcher {

    public int playerNumber;

    public ResourceManager resourceManager;

    public float supplyMax;

    public float currentSupply;
    public float supplyCap;

    public bool IgnoreUlts;
    public Ability UltOne;
    public Ability UltTwo;
    public Ability UltThree;
    public Ability UltFour;


    public bool playUltTwoWarningFirstTime = true;
    public bool playUltFourWarningFirstTime = true;
    public RaceInfo.raceType myRace;
    public GameObject upgradeBall;

    public UIManager uiManage;

    public List<Upgrade> myUpgrades = new List<Upgrade>();

    private SelectedManager selectedManager;


    private List<GameObject> resourceDropOffs = new List<GameObject>();

    private List<ManagerWatcher> myWatchers = new List<ManagerWatcher>();

    Dictionary<string, List<UnitManager>> unitRoster = new Dictionary<string, List<UnitManager>>();

    Dictionary<string, List<Ability>> UnitBuildTrigger = new Dictionary<string, List<Ability>>();

    public MVPCalculator MVP = new MVPCalculator();
    //used for unit ability validation
    private Dictionary<string, int> unitTypeCount = new Dictionary<string, int>();


    List<BuildUnitObjective> buildUnitObjectList = new List<BuildUnitObjective>();
    public void addBuildUnitObjective(BuildUnitObjective obj)
    {
        buildUnitObjectList.Add(obj);
    }

    private List<LethalDamageinterface> lethalTrigger = new List<LethalDamageinterface>();
    private List<LethalDamageinterface> deathTrigger = new List<LethalDamageinterface>();
    public RaceUIManager uiManager;
    public EconomyManager economyManager;

    //Used for end level stats
    public int unitsLost;

    private Material myDecal;

    public SkinColorManager colorManager;

    //public TechTree myTech;

    // Use this for initialization
    void Awake() {
        if (playerNumber == 1) {
            myDecal = Resources.Load<Material>("go1_1");
        } else if (playerNumber == 2) {
            myDecal = Resources.Load<Material>("EnemyDecal");
        } else if (playerNumber == 3) {
            myDecal = Resources.Load<Material>("NuetralMat");
        } else {
            myDecal = Resources.Load<Material>("go1_1");
        }
        selectedManager = SelectedManager.main;// GameObject.FindObjectOfType<SelectedManager> ();

        if (playerNumber == 1) {
            InvokeRepeating("UltUpdate", .2f, .2f);
        }

        if (!upgradeBall)
        {
            upgradeBall = new GameObject("Player" + playerNumber + " UpgradeBall");
            upgradeBall.transform.parent = transform.parent;
        }

        if (upgradeBall) {
            foreach (Upgrade grade in upgradeBall.GetComponents<Upgrade>()) {
                if (!myUpgrades.Contains(grade)) {
                    addUpgrade(grade, "");
                }
            }
        }
    }

    void Start()
    {
        uiManager = RaceUIManager.instance;
        uiManage = UIManager.main;

        if (RaceSwapper.main == null)// && myRace != RaceInfo.raceType.SteelCrest)
        {
            changeRace(myRace);
        }

        if (colorManager == null)
        {
            colorManager = gameObject.AddComponent<SkinColorManager>();
            colorManager.useColoredSkins = false;
        }
    }


    public SkinColorManager getColorManager()
    {
        if (colorManager == null)
        {
            colorManager = gameObject.AddComponent<SkinColorManager>();
            colorManager.useColoredSkins = false;
        }
        return colorManager;
    }

    public Color getColor()
    {
        if (playerNumber == 1)
        {
            return Color.blue;
        }
        else if (playerNumber == 2)
        {
            return Color.red;
        }
        else if (playerNumber == 3)
        {
            return Color.gray;
        }
        else
        {
            return Color.green;
        }
        // To do, create a color variable and set it in each level.
    }

    public void changeRace(RaceInfo.raceType newType)
    {
        myRace = newType;
        GameObject packet = ResourceLoader.getMain().getResource("RaceInfoPacket");
        if (!packet)
        {
            return;
        }
        UnitEquivalance RacePacket = packet.GetComponent<UnitEquivalance>();
       
        if (playerNumber == 1)
        {
            if (!IgnoreUlts )
            {
                UltObject Ulty = null;
                if (TerraTileController.main)
                {
                    Ulty = Instantiate<GameObject>(Resources.Load<GameObject>("TerraCrafterUlt")).GetComponent<UltObject>();
                }
                else
                {
                    Ulty = Instantiate<GameObject>(RacePacket.getRace(myRace).UltimatePrefab).GetComponent<UltObject>();
                }
                if (Application.isPlaying)
                {
                    SetUltimates(Ulty);
                }
            }

            LevelCompilation comp = ResourceLoader.getMain().getResource("LevelEditor").GetComponent<LevelCompilation>();
            ResourceManager startingTank = comp.MyLevels[GameObject.FindObjectOfType<VictoryTrigger>().levelNumber].StartingResources;

            resourceManager.MyResources.Clear();
            foreach (ResourceTank tank in RacePacket.getRace(myRace).ResourceTypes.MyResources)
            {
                AddResourceType(tank.resType, startingTank.hasResourceType(tank.resType) ? startingTank.getResource(tank.resType) : 0);
            }

            if (!UISetter.main)
            {
                GameObject.FindObjectOfType<UISetter>().SwapRaceHuds(newType);
            }
            else
            {
                 UISetter.main.SwapRaceHuds(newType); 
            }
        }
    }

    public void AddResourceType(ResourceType resType, float amount)
    {
        if (playerNumber == 1)
        {
            if (Application.isPlaying)
            {
                RaceUIManager.getInstance().addResource(resType, amount);
                EconomyManager.getInstance().AddResourceType(resType);
            }
        }
      
        resourceManager.MyResources.Add(new ResourceTank(resType, amount));
    }

    UltObject ultimateHolder;
    public void SetUltimates(UltObject Ulty)
    {
      //  InstructionHelperManager.getInstance().addBUtton("Setting Ults A", 25, null);

        ultimateHolder = Ulty;

        if (Ulty.myUltimates.Count > 0)
        {
            UltOne = Ulty.myUltimates[0];
            UltOne.myCost.cooldownTimer = Ulty.StartingCooldowns[0];
            UltOne.Start();
            UISetter.getInstance().initializeUlt(Ulty.myUltimates[0], 0);
        }
      //  InstructionHelperManager.getInstance().addBUtton("Setting Ults B", 25, null);
        if (Ulty.myUltimates.Count > 1)
        {
            UltTwo = Ulty.myUltimates[1];
            UltTwo.myCost.cooldownTimer = Ulty.StartingCooldowns[1];
            UltTwo.Start();
            UISetter.getInstance().initializeUlt(Ulty.myUltimates[1], 1);
        }
      //  InstructionHelperManager.getInstance().addBUtton("Setting Ults C", 25, null);

        if (Ulty.myUltimates.Count > 2)
        {
            UltThree = Ulty.myUltimates[2];
            UltThree.myCost.cooldownTimer = Ulty.StartingCooldowns[2];
            UltThree.Start();
            UISetter.getInstance().initializeUlt(Ulty.myUltimates[2], 2);
        }
       // InstructionHelperManager.getInstance().addBUtton("Setting Ults D", 25, null);
        if (Ulty.myUltimates.Count > 3)
        {
            UltFour = Ulty.myUltimates[3];
            UltFour.myCost.cooldownTimer = Ulty.StartingCooldowns[3];
            UltFour.Start();
            UISetter.getInstance().initializeUlt(Ulty.myUltimates[3], 3);
        }
       // InstructionHelperManager.getInstance().addBUtton("Setting Ults E", 25, null);
        Invoke("DelayedStartCooldowns", .5f);
    }

    void DelayedStartCooldowns()
    {
        if (UltOne)
        {
            UltOne.myCost.StartCustomCooldown(ultimateHolder.StartingCooldowns[0]);
        }

        if (UltTwo)
        {
            UltTwo.myCost.StartCustomCooldown(ultimateHolder.StartingCooldowns[1]);
        }

        if (UltThree)
        {
            UltThree.myCost.StartCustomCooldown(ultimateHolder.StartingCooldowns[2]);
        }

        if (UltFour)
        {
            UltFour.myCost.StartCustomCooldown(ultimateHolder.StartingCooldowns[3]);
        }
    }



    IEnumerator StartUp()
    {
        yield return null;
        if (myRace == RaceInfo.raceType.SteelCrest)
        {
            if (UltTwo && !UltTwo.myCost.StartsRefreshed && UltTwo.active)
            {
                StartCoroutine(UltTwoNotif());
            }
            if (UltFour && !UltFour.myCost.StartsRefreshed && UltFour.active)
            {
                StartCoroutine(UltFourNotif());
            }
        }
    }


    public void UltUpdate()
    {
        if (UltOne && UltOne.enabled) {
            UISetter.main.updateUlt(0);
        }
        if (UltTwo && UltTwo.enabled)
        {
            UISetter.main.updateUlt(1);
        }
        if (UltThree && UltThree.enabled)
        {
            UISetter.main.updateUlt(2);
        }
        if (UltFour && UltFour.enabled)
        {
            UISetter.main.updateUlt(3);
        }
    }


    public void commenceUpgrade(bool onOff, Upgrade upgrade, string unitname)
    {

        object[] temp = new object[2];
        temp[0] = onOff;
        temp[1] = upgrade;
        //temp [2] = unitname;

        foreach (KeyValuePair<string, List<UnitManager>> pair in unitRoster) {
            foreach (UnitManager tempMan in pair.Value) {
                if (tempMan == null) {
                    continue; }

                if (tempMan.UnitName == unitname) {

                    tempMan.gameObject.SendMessage("commence", temp);

                }
            }
        }
    }

    public void addBuildTrigger(string unitName, Ability a)
    {

        List<Ability> ab;
        if (UnitBuildTrigger.ContainsKey(unitName)) {
            ab = UnitBuildTrigger[unitName];
        } else {
            ab = new List<Ability>();
            UnitBuildTrigger.Add(unitName, ab);
        }

        ab.Add(a);

        if (unitRoster.ContainsKey(unitName) && unitRoster[unitName].Count > 0) {
            a.newUnitCreated(unitName);
        }
    }



    /// <summary>
    /// The unitname is the name of the unit that just finished researchign this, it can be blank if its some default upgrade
    /// </summary>
    /// <param name="upgrade">Upgrade.</param>
    /// <param name="unitname">Unitname.</param>
    public void addUpgrade(Upgrade upgrade, string unitname)
    {

        Component temp = upgradeBall.AddComponent(upgrade.GetType());

        foreach (FieldInfo f in temp.GetType().GetFields())
        {
            f.SetValue(temp, f.GetValue(upgrade));
        }

        myUpgrades.Add((Upgrade)temp);

        foreach (KeyValuePair<string, List<UnitManager>> pair in unitRoster) {

            foreach (UnitManager tempMan in pair.Value) {
                if (tempMan) {
                    upgrade.ApplySkin(tempMan.gameObject);
                    //if (!tempMan.myStats.isUnitType (UnitTypes.UnitTypeTag.Structure) || tempMan.myStats.isUnitType (UnitTypes.UnitTypeTag.Add_On)) {
                    upgrade.applyUpgrade(tempMan.gameObject);
                    //}
                    if (tempMan.UnitName == unitname) {

                        tempMan.gameObject.SendMessage("researched", upgrade);
                    }
                }
            }
        }
        if (Time.timeSinceLevelLoad > 1) {
            selectedManager.updateUI();
        }
    }



    public static void upDateUI()
    { SelectedManager.main.updateUI();
        //SelectedManager.main.reImageUI ();
    }

    public static void upDateSingleCard()
    {
        SelectedManager.main.RedoSingle();
    }


    public static void upDateAutocast()
    {
        SelectedManager.main.AutoCastUI();
    }

    public static void updateUIUnitcount()
    {
        SelectedManager.main.updateUI();
    }

    public static void updateActivity()
    {
        SelectedManager.main.updateUIActivity();
    }


    public static void removeUnitSelect(RTSObject man)
    {
        SelectedManager.main.DeselectObject(man); }

    public static void AddUnitSelect(RTSObject man)
    { SelectedManager.main.AddObject(man); }




    public void applyUpgrade(UnitManager obj)
    {
        foreach (Upgrade up in myUpgrades) {
            up.applyUpgrade(obj.gameObject);
            up.ApplySkin(obj.gameObject);
            obj.SendMessage("researched", up, SendMessageOptions.DontRequireReceiver);
        }
    }


    public void addWatcher(ManagerWatcher input)
    {
        if (!myWatchers.Contains(input))
        {
            myWatchers.Add(input);
        }
    }


    public void buildingUnit(UnitProduction abil)
    {
        if (playerNumber == 1)
        {
            uiManager.production.GetComponent<ProductionManager>().updateUnits(abil);
        }
    }
    public void stopBuildingUnit(UnitProduction abil)
    {
        if (playerNumber == 1)
        {
            uiManager.production.GetComponent<ProductionManager>().unitLost(abil);
        }
    }

    public bool hasSupplyAvailable(float sup)
    {
        return (sup <= (Mathf.Min(supplyCap, supplyMax) - currentSupply));

    }

    public void UnitDied(float supply, UnitManager obj)
    {
        if (obj) {
            if (unitRoster.ContainsKey(obj.UnitName)) {
                try {
                    unitRoster[obj.UnitName].Remove(obj);
                } catch (SystemException) {
                    Debug.Log("Unit does Not exist in unit roster");
                    return;
                }
            }
        }
        if (supply < 0) {

            if (obj.myStats.isUnitType(UnitTypes.UnitTypeTag.Structure))
            {
                BuildingInteractor interactor = obj.gameObject.GetComponent<BuildingInteractor>();
                if (interactor && interactor.ConstructDone())
                {
                    supplyMax += supply;
                }
            }
            else
            {
                supplyMax += supply;
            }

        } else {
            currentSupply -= supply;
        }


        updateSupply(currentSupply, Mathf.Min(supplyCap, supplyMax));
    }


    public void UnitCreated(float supply)
    {
        if (supply < 0) {

            supplyMax -= supply;
        } else {
            currentSupply += supply;
        }

        updateSupply(currentSupply, Mathf.Min(supplyCap, supplyMax));

    }



    public void addDeathWatcher(LethalDamageinterface input)
    {
        Debug.Log("Adding death watcher " + playerNumber);
        lethalTrigger.Add(input);
    }

    IEnumerator DeathRescan(GraphUpdateObject b)
    {
        yield return new WaitForSeconds(.25f);

        AstarPath.active.UpdateGraphs(b);

    }


    public void addUnit(UnitManager obj)
    {
      //  Debug.Log("adding " + obj.UnitName);
        if (!unitRoster.ContainsKey(obj.UnitName))
        {
            unitRoster.Add(obj.UnitName, new List<UnitManager>());
        }

        List<UnitManager> tempRefList = unitRoster[obj.UnitName];
        tempRefList.Add(obj);
        tempRefList.RemoveAll(item => item == null);


        if (playerNumber == 1) {
            if (FButtonManager.main == null) {
                FButtonManager.main = GameObject.FindObjectOfType<FButtonManager>();
            }
            if (FButtonManager.main != null)
            {
                FButtonManager.main.updateNumbers(unitRoster);
            }

            if (uiManager != null) {
                foreach (ArmyUIManager uiMan in uiManager.production.GetComponents<ArmyUIManager>()) {
                    uiMan.updateUnits(obj);
                }
            }
        }

        if (obj.UnitName != "Armory") {
            Selected sel = obj.GetComponent<Selected>();
            sel.decalCircle.GetComponent<MeshRenderer>().material = myDecal;
        }


        if (!obj.myStats.upgradesApplied)
        {
            obj.myStats.upgradesApplied = true;
            applyUpgrade(obj);
        }

        foreach (BuildUnitObjective objective in buildUnitObjectList) {
            objective.buildUnit(obj);
        }
        //Debug.Log ("Just built a " + unitName + "    " + unitTypeCount[unitName]);
        // new unit, call update function on units abilities
        if (tempRefList.Count == 1) {

            if (UnitBuildTrigger.ContainsKey(obj.UnitName)) {
                UnitBuildTrigger[obj.UnitName].RemoveAll(item => item == null);
                foreach (Ability a in UnitBuildTrigger[obj.UnitName]) {

                    a.newUnitCreated(obj.UnitName);

                }
            }
        }
    }


    //Truedeath applies to thing like summons and building placers. they aren't real units so they shouldnt be treated as such.
    public bool UnitDying(UnitManager Unit, GameObject deathSource, bool trueDeath)
    {
        bool finishDeath = true;
  
        if (trueDeath) {
            foreach (LethalDamageinterface trigger in lethalTrigger) {
                if (trigger != null) {
                    if (trigger.lethalDamageTrigger(Unit, deathSource) == false) {
                        finishDeath = false;
                    }
                }
            }
        }

        if (finishDeath) {


            if (uiManager != null) {
                if (playerNumber == 1) {
                    foreach (ArmyUIManager uiMan in uiManager.production.GetComponents<ArmyUIManager>()) {
                        uiMan.unitLost(Unit);
                    }
                }
            }


            string unitName = Unit.UnitName;
            List<UnitManager> tempRefList = null;
            if (unitRoster.ContainsKey(unitName)) {
                tempRefList = unitRoster[unitName];

                tempRefList.Remove(Unit);

                tempRefList.RemoveAll(item => item == null);
                if (tempRefList.Count == 0 && trueDeath) {
                    if (UnitBuildTrigger.ContainsKey(unitName)) {
                        UnitBuildTrigger[unitName].RemoveAll(item => item == null);
                        foreach (Ability a in UnitBuildTrigger[unitName]) {
                            a.UnitDied(unitName);

                        }
                    }
                }
            }

            if (Unit.gameObject.layer == 10) { //.myStats.isUnitType (UnitTypes.UnitTypeTag.Structure)) {
                                               //This rescans the Astar graph after the unit dies
                GraphUpdateObject b = new GraphUpdateObject(Unit.GetComponent<CharacterController>().bounds);

                StartCoroutine(DeathRescan(b));
            }

            if (trueDeath && !Unit.myStats.isUnitType(UnitTypes.UnitTypeTag.Turret) && !Unit.myStats.isUnitType(UnitTypes.UnitTypeTag.Summon)) {

                unitsLost++;

                bool clearList = false;
                foreach (LethalDamageinterface trigger in deathTrigger)
                {
                    if (trigger != null)
                    {
                        trigger.lethalDamageTrigger(Unit, deathSource);
                    }
                }
                if (clearList)
                {
                    deathTrigger.RemoveAll(item => item == null);
                }
            }
        }

        if (playerNumber == 1 && FButtonManager.main) {
            FButtonManager.main.updateNumbers(unitRoster);
        }
        return finishDeath;
    }

    public void updateResources(ResourceManager res) { }

    public void collectResources(List<ResourceTank> toUpdate, bool income = true)
    {
        resourceManager.CollectResources(toUpdate);
        foreach (ManagerWatcher watch in myWatchers)
        {
            if (watch != null)
            {
                watch.updateResources(resourceManager);
            }
        }
        if (income)
        {
            foreach (ResourceTank tank in toUpdate)
            {
                EconomyManager.getInstance().updateResource(tank.resType, resourceManager.getResource(tank.resType), tank.currentAmount);
            }
        }
    }

    public void PayCost(List<ResourceTank> toUpdate)
    {
        resourceManager.PayCost(toUpdate);
        foreach (ManagerWatcher watch in myWatchers)
        {
            if (watch != null)
            {
                watch.updateResources(resourceManager);
            }
        }
    }

    public void PayCost(ResourceTank toUpdate)
    {
        resourceManager.PayCost(toUpdate);
        foreach (ManagerWatcher watch in myWatchers)
        {
            if (watch != null)
            {
                watch.updateResources(resourceManager);
            }
        }
    }

    public void collectOneResource(ResourceTank toUpdate, bool income = true)
    {
        resourceManager.collectResource(toUpdate.resType, toUpdate.currentAmount);
        foreach (ManagerWatcher watch in myWatchers)
        {
            if (watch != null)
            {
                watch.updateResources(resourceManager);
            }
        }
        if (income)
        {
            EconomyManager.main.updateResource(toUpdate.resType, resourceManager.getResource(toUpdate.resType), toUpdate.currentAmount);
        }
    }

    public void collectOneResource(ResourceType theType, float amount, bool income = true)
    {
        resourceManager.collectResource(theType, amount);
        foreach (ManagerWatcher watch in myWatchers)
        {
            if (watch != null)
            {
                watch.updateResources(resourceManager);
            }
        }
        if (income)
        {
            EconomyManager.main.updateResource(theType, resourceManager.getResource(theType), amount);
        }
    }


    public void updateSupply(float current, float max) {
        bool hasNull = false;

        foreach (ManagerWatcher watch in myWatchers) {
            if (watch != null) {
                watch.updateSupply(current, max); }
            else { hasNull = true; }

        }
        if (hasNull) {
            myWatchers.RemoveAll(item => item == null); }
    }

    public void updateUpgrades() {
        bool hasNull = false;
        foreach (ManagerWatcher watch in myWatchers) {
            if (watch != null) {
                watch.updateUpgrades(); }
            else { hasNull = true; }
            if (hasNull) {
                myWatchers.RemoveAll(item => item == null); }
        }


    }


    public Dictionary<string, List<UnitManager>> getUnitList()
    {
        cleanUnitRoster();
        return unitRoster;
    }

    public Dictionary<string, List<UnitManager>> getFastUnitList()
    {
        return unitRoster;
    }

    public List<UnitManager> getUnitType(string unitName)
    {
        List<UnitManager> toReturn;
        if (!unitRoster.TryGetValue(unitName, out toReturn))
        {
            toReturn = new List<UnitManager>();
        }
        return toReturn;
    }

    public void cleanUnitRoster()
    {
        foreach (KeyValuePair<string, List<UnitManager>> pair in unitRoster) {

            pair.Value.RemoveAll(item => item == null);
        }
    }

    public void addDropOff(GameObject obj)
    { resourceDropOffs.Add(obj); }



    public GameObject getNearestDropOff(GameObject worker)
    {
        float closest = 10000000;
        resourceDropOffs.RemoveAll(item => item == null);
        if (resourceDropOffs.Count == 0) {
            return null; }
        GameObject nearest = resourceDropOffs[0];

        foreach (GameObject obj in resourceDropOffs) {
            float distance = Vector3.Distance(obj.transform.position, worker.transform.position);
            if (distance < closest)

            { closest = distance;
                nearest = obj; }

        }
        return nearest;
    }


    //Get all units in a given selection rectangle
    public List<UnitManager> getUnitSelection(Vector3 upperLeft, Vector3 bottRight)
    { cleanUnitRoster();
        bool selectBuildings = true;
        bool bDown = Input.GetKey(KeyCode.B);

        List<UnitManager> foundUnits = new List<UnitManager>();

        foreach (KeyValuePair<string, List<UnitManager>> pair in unitRoster) {
            pair.Value.RemoveAll(item => item == null);
            if (pair.Value.Count > 0) {
                if (pair.Value[0].getUnitStats().isUnitType(UnitTypes.UnitTypeTag.Turret)) {
                    continue;
                }
            }
            foreach (UnitManager obj in pair.Value) {
                Vector3 tempLocation = Camera.main.WorldToScreenPoint(obj.transform.position);
                //Debug.Log ("Checking " + tempLocation + "   within  "+ upperLeft + " bot " + bottRight);
                if (tempLocation.x + obj.GetComponent<CharacterController>().radius * 5 < upperLeft.x) {
                    continue;
                }
                if (tempLocation.x - obj.GetComponent<CharacterController>().radius * 5 > bottRight.x) {
                    continue;
                }
                if (tempLocation.y - obj.GetComponent<CharacterController>().radius * 5 > upperLeft.y) {
                    continue;
                }
                if (tempLocation.y + obj.GetComponent<CharacterController>().radius * 5 < bottRight.y) {
                    continue;
                }

                foundUnits.Add(obj);
                if (!bDown) {
                    if (!obj.myStats.isUnitType(UnitTypes.UnitTypeTag.Structure)) {
                        selectBuildings = false;
                    }

                } else {
                    if (obj.myStats.isUnitType(UnitTypes.UnitTypeTag.Structure)) {
                        selectBuildings = false;
                    }
                }
            }
        }
        for (int i = foundUnits.Count - 1; i > -1; i--) {
            if (!bDown) {
                if (!selectBuildings && foundUnits[i].myStats.isUnitType(UnitTypes.UnitTypeTag.Structure)) {
                    foundUnits.Remove(foundUnits[i]);
                }
            } else {

                if (!selectBuildings && !foundUnits[i].myStats.isUnitType(UnitTypes.UnitTypeTag.Structure)) {
                    foundUnits.Remove(foundUnits[i]);
                }

            }
        }

        return foundUnits;
    }


    public List<UnitManager> getUnitOnScreen(bool select, string Unitname)
    {
        cleanUnitRoster();

        List<UnitManager> foundUnits = new List<UnitManager>();

        if (!unitRoster.ContainsKey(Unitname)) {
            return foundUnits;
        }

        foreach (UnitManager obj in unitRoster[Unitname]) {

            Vector3 tempLocation = Camera.main.WorldToScreenPoint(obj.transform.position);
            //Debug.Log ("Checking " + tempLocation + "   within  "+ upperLeft + " bot " + bottRight);
            if (tempLocation.x + obj.CharController.radius * 5 < 0) {
                continue;
            }
            if (tempLocation.x - obj.CharController.radius * 5 > Screen.width) {
                continue;
            }
            if (tempLocation.y - obj.CharController.radius * 5 > Screen.height) {
                continue;
            }
            if (tempLocation.y + obj.CharController.radius * 5 < 0) {
                continue;
            }

            foundUnits.Add(obj);

        }


        return foundUnits;
    }


    public List<UnitManager> getAllUnitsOnScreen()
    {
        cleanUnitRoster();

        List<UnitManager> foundUnits = new List<UnitManager>();

        foreach (KeyValuePair<string, List<UnitManager>> pair in unitRoster) {
            foreach (UnitManager tempMan in pair.Value) {

                if (tempMan.myStats.isUnitType(UnitTypes.UnitTypeTag.Structure) || tempMan.myStats.isUnitType(UnitTypes.UnitTypeTag.Worker)) {
                    continue;
                }

                Vector3 tempLocation = Camera.main.WorldToScreenPoint(tempMan.transform.position);
                //Debug.Log ("Checking " + tempLocation + "   within  "+ upperLeft + " bot " + bottRight);
                if (tempLocation.x + tempMan.CharController.radius * 5 < 0) {
                    continue;
                }
                if (tempLocation.x - tempMan.CharController.radius * 5 > Screen.width) {
                    continue;
                }
                if (tempLocation.y - tempMan.CharController.radius * 5 > Screen.height) {
                    continue;
                }
                if (tempLocation.y + tempMan.CharController.radius * 5 < 0) {
                    continue;
                }

                foundUnits.Add(tempMan);

            }
        }
        return foundUnits;
    }


    public void addVeteranStat(VeteranStats input)
    { if (MVP == null) {
            MVP = new MVPCalculator(); }

        MVP.addVet(input);
    }

    public void getMVPScore()
    {

        Debug.Log(MVP.getMVP());
    }

    public List<VeteranStats> getVeteranStats()
    {
        return MVP.UnitStats();
    }

    public List<VeteranStats> getUnitStats()
    {
        List<VeteranStats> toReturn = new List<VeteranStats>();

        foreach (VeteranStats vs in MVP.UnitStats()) {
            if (vs.isWarrior) {

                toReturn.Add(vs);

            }

        }
        toReturn.Sort();


        List<VeteranStats> realReturn = new List<VeteranStats>();
        for (int i = 0; i < Mathf.Min(10, toReturn.Count); i++) {
            realReturn.Add(toReturn[i]);
        }

        return realReturn;

    }


    public void addActualDeathWatcher(LethalDamageinterface input) {
        deathTrigger.Add(input);
    }
    public void removeActualDeathWatcher(LethalDamageinterface input)
    {
        deathTrigger.Remove(input);
    }


    public void useAbilityOne()
    { if (UltOne != null) {
            Debug.Log("ult one");
            if (UltOne.active && UltOne.canActivate(true).canCast) {

                uiManage.SwitchMode(Mode.globalAbility);
                uiManage.setAbility(UltOne, 1, "");
            }
        }
    }

    public void useAbilityTwo()
    {
        if (UltTwo != null) {
            if (UltTwo.active && UltTwo.canActivate(true).canCast) {
                uiManage.SwitchMode(Mode.globalAbility);
                uiManage.setAbility(UltTwo, 1, "");
            }
        }
    }

    public void useAbilityThree()
    {
        if (UltThree != null) {
            if (UltThree.active && UltThree.canActivate(true).canCast) {
                uiManage.SwitchMode(Mode.globalAbility);
                uiManage.setAbility(UltThree, 1, "");
            }
        }
    }

    public void useAbilityFour()
    {
        if (UltFour != null) {
            if (UltFour.active && UltFour.canActivate(true).canCast) {
                uiManage.SwitchMode(Mode.globalAbility);
                uiManage.setAbility(UltFour, 1, "");
            }
        }
    }

    public void castedGlobal(TargetAbility ult)
    {
        if (ult == UltFour) {
            StartCoroutine(UltFourNotif());
        } else if (ult == UltTwo) {
            StartCoroutine(UltTwoNotif());

        }
    }

    IEnumerator UltTwoNotif()
    {
        yield return new WaitForSeconds(UltTwo.myCost.cooldown);
        if (!playUltTwoWarningFirstTime) {
            playUltTwoWarningFirstTime = true;
        } else {
            ErrorPrompt.instance.UltTwoDone();
        }
    }

    IEnumerator UltFourNotif()
    {
        yield return new WaitForSeconds(UltFour.myCost.cooldown);
        if (!playUltFourWarningFirstTime) {
            playUltFourWarningFirstTime = true;
        } else {
            ErrorPrompt.instance.UltFourDone();
        }
    }


    public int UnitsLost()
    { return unitsLost; }

    public int totalRes()
    {
        float i = 0;
        foreach (ResourceTank tank in resourceManager.MyResources)
        {
            i += tank.currentAmount;
        }

        return (int)i;
    }


	public int getArmyCount()
	{int total = 0;
		cleanUnitRoster ();

		foreach (KeyValuePair<string, List<UnitManager>> pair in unitRoster) {
			if (pair.Value.Count == 0 || pair.Value [0].myStats.isUnitType (UnitTypes.UnitTypeTag.Structure) || pair.Value [0].myStats.isUnitType (UnitTypes.UnitTypeTag.Worker)) {
				continue;}
			total += pair.Value.Count;

		}
		return total;
	}

}
