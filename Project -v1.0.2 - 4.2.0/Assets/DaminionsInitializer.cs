using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaminionsInitializer : MonoBehaviour
{

    public GameObject SpawnUnitPrefab;
    public CarbotCamera myCam;
    public GameObject DefaultHero;
    public GameObject UnitRegistry;

    public Slider ManaSlider;
    public Text ManaText;
    public static DaminionsInitializer main;

    [Tooltip("The hero that has been spawned into the scene")]
    public UnitManager MyHero;

    public List<Image> CrystalChildren;
    public List<Text> AbilityCosts;
    float lastEnergy;
    public bool ControllableHero = true;
    public bool TravelingRight = true;
    public string LoadFileName;
    [HideInInspector]
    public DaminionMap map;
    public GameObject VictoryZone;

    private void Awake()
    {
        main = this;
    }

    void Start()
    {

        SpawnUnitListeners.Add(1, new List<LethalDamageinterface>());
        SpawnUnitListeners.Add(2, new List<LethalDamageinterface>());
        GameObject hero = null;
        if (DMCollectionManager.ChosenHero)
        {
            hero = Instantiate<GameObject>(DMCollectionManager.ChosenHero, CarbotCamera.singleton.LeftSide, Quaternion.identity);
        }
        else
        {
            hero = Instantiate<GameObject>(DefaultHero, CarbotCamera.singleton.LeftSide, Quaternion.identity);

        }
        MyHero = hero.GetComponent<UnitManager>();
        MyHero.Initialize(1, false,false);
        myCam.setHero(hero);
        Invoke("SelectHero", .1f);

        if (Time.time > 1)
        {
            ControllableHero = PlayerPrefs.GetInt("ControlHero") == 1;
        }

        if (!ControllableHero)
        {
            InvokeRepeating("GiveOrder", 2, 1.5f);
            MyHero.myStats.statChanger.changeMoveSpeed(-.6f, 0, this, true);
        }


        bool clearList = false;
        foreach (GameObject obj in DMCollectionManager.ChosenUnits)
        {
            if (obj != null)
            {
                clearList = true;
            }
        }
        if (clearList)
        {
            hero.GetComponent<UnitManager>().abilityList.Clear();
        }
        foreach (GameObject obj in DMCollectionManager.ChosenUnits)
        {
            if (obj != null)
            {
                GameObject Spawner = Instantiate<GameObject>(SpawnUnitPrefab, hero.transform);
                Spawner.transform.localPosition = Vector3.zero;
                DMSpawnUnit spawner = Spawner.GetComponent<DMSpawnUnit>();
                spawner.ToSpawn = obj;

                UnitManager man = obj.GetComponent<UnitManager>();
                spawner.Name = man.UnitName;

                UnitStats stats = obj.GetComponent<UnitStats>();
                spawner.Descripton = stats.UnitDescription;
                spawner.iconPic = stats.UnitPortrait;
                spawner.myCost.energy = stats.cost;
                spawner.spawnCount = (int)stats.supply;
                hero.GetComponent<UnitManager>().abilityList.Add(spawner);
            }
        }


        foreach (GameObject obj in DMCollectionManager.ChosenAbilities)

            if (obj != null)
            {
                GameObject Abil = Instantiate<GameObject>(obj, hero.transform);
                Abil.transform.localPosition = Vector3.zero;
                hero.GetComponent<UnitManager>().abilityList.Add(Abil.GetComponent<Ability>());
            }

        for (int i = 0; i < CrystalChildren.Count; i++)
        {
            CrystalChildren[i].transform.parent.gameObject.SetActive(MyHero.myStats.MaxEnergy > i);
        }

        for (int i = 0; i < AbilityCosts.Count; i++)
        {
            if (MyHero.abilityList[i] && MyHero.abilityList[i].myCost)
            {
                AbilityCosts[i].text = MyHero.abilityList[i].myCost.energy + "";
            }
            else
            {
                AbilityCosts[i].text = "";
            }
        }

        string LoadCustom = PlayerPrefs.GetString("PlayLevel");
        if (LoadCustom != "" && System.IO.File.Exists(Application.dataPath + "/" + LoadCustom + ".dmm"))
        {
            LoadFileName = LoadCustom;
        }

        if (LoadFileName != "")
        {
            BuildSceneFromFile(LoadFileName);
        }

    }
    [HideInInspector]
   public List<UnitManager> AllUnits = new List<UnitManager>();
    [HideInInspector]
   public List<Sprite> AllScenery = new List<Sprite>();

    public void BuildSceneFromFile(string fileName)
    {
        if (System.IO.File.Exists(Application.dataPath + "/" + fileName + ".dmm"))
        {
            foreach (RaceInfo info in UnitRegistry.GetComponents<RaceInfo>())
            {
                foreach (Sprite obj in info.myUIImages.toReplace)
                {
                    AllScenery.Add(obj);
                }

                foreach (GameObject manag in info.unitList)
                {
                    AllUnits.Add(manag.GetComponent<UnitManager>());
                }
            }
            
            map = JsonUtility.FromJson<DaminionMap>(System.IO.File.ReadAllText(Application.dataPath + "/" + fileName + ".dmm"));
            CarbotCamera.singleton.RightSide = CarbotCamera.singleton.LeftSide + Vector3.right * map.MapLength;
            VictoryZone.transform.position = CarbotCamera.singleton.RightSide;

            foreach (SceneryData data in map.Scenery)
            {
                GameObject obj = new GameObject();
                obj.AddComponent<SpriteRenderer>().sprite = AllScenery.Find(item => item.name == data.spriteName);
                obj.GetComponent<SpriteRenderer>().sortingOrder = -1;
                obj.transform.position = data.pos + CarbotCamera.singleton.LeftSide;
                obj.transform.rotation = data.rot;
                obj.transform.localScale = data.scale;
                data.ex = obj;
            }

            // Ordered the units going left to right so they can be spawned easily and in order
            if (map.Units.Count > 0)
            {
                List<UnitData> OrderedList = new List<UnitData>();
                OrderedList.Add(map.Units[0]);
                for (int j = 1; j < map.Units.Count; j++)
                {
                    map.Units[j].pos += CarbotCamera.singleton.LeftSide;
                    bool placed = false;
                    for (int i = 0; i < OrderedList.Count; i++)
                    {

                        if (!placed && map.Units[j].pos.x < OrderedList[i].pos.x)
                        {                           
                            OrderedList.Insert(i, map.Units[j]);
                            placed = true;
                        }
                    }
                    if (!placed)
                    {
                        OrderedList.Add(map.Units[j]);
                    }
                }
                map.Units = OrderedList;
            }
        }
    }

    void SelectHero()
    {
        SelectedManager.main.AddObject(MyHero);
        SelectedManager.main.CreateUIPages(0);
        if (!ControllableHero)
        {
            SelectedManager.main.CanGiveOrders = false;
        }
    }

    void GiveOrder()
    {
        if (MyHero.getState() is DefaultState)
        {
            MyHero.GiveOrder(Orders.CreateAttackMove(MyHero.transform.position + Vector3.right * 50, true));
        }
    }


    float NextEnemyCheckTime;
    public static bool MarchMode;
    [Tooltip("The speed untis move at when no enemies are on screen")]
    public float MarchSpeed = 15;
    private void Update()
    {
        CheckMarchOrders();

        if (MyHero && MyHero.getUnitStats().currentEnergy != lastEnergy)
        {
            lastEnergy = MyHero.getUnitStats().currentEnergy;
            for (int i = 0; i < CrystalChildren.Count; i++)
            {
                CrystalChildren[i].enabled = (lastEnergy - .5f > i);
            }
        }
        else if (!MyHero)
        {
            VictoryTrigger.instance.Lose();
            this.enabled = false;
        }


    }


    void CheckMarchOrders()
    {
        if (Time.time > NextEnemyCheckTime)
        {
            {
                //This function might be too slow
                bool VisibleEnemies = GameManager.main.playerList[1].getAllUnitsOnScreen().Count != 0;
                if (!VisibleEnemies && !MarchMode)
                {
                    NextEnemyCheckTime = Time.time + .5f;
                    MarchMode = true;
                    GiveMarchOrder(true);
                }
                else if (VisibleEnemies && MarchMode)
                {
                    NextEnemyCheckTime = Time.time + 2;
                    MarchMode = false;
                    GiveMarchOrder(false);
                }
                else
                {
                    NextEnemyCheckTime = Time.time + .5f;
                }
            }
        }
    }

    void GiveMarchOrder(bool MarchOn)
    {
        if (MarchOn)
        {
            foreach (KeyValuePair<string, List<UnitManager>> unit in GameManager.main.playerList[0].getUnitList())
            {
                foreach (UnitManager man in unit.Value)
                {
                    if (man.cMover)
                    {
                        man.cMover.SetMaxSpeed(MarchSpeed);
                    }
                }
            }
        }
        else
        {
            foreach (KeyValuePair<string, List<UnitManager>> unit in GameManager.main.playerList[0].getUnitList())
            {
                foreach (UnitManager man in unit.Value)
                {
                    if (man.cMover)
                    {
                        man.cMover.ResetNormalMaxSpeed();
                    }
                }
            }
        }
    }


    SpawnPointOverride PlayerSpawnOverride;
    SpawnPointOverride EnemySpawnOverride;
    public Dictionary<int, List<LethalDamageinterface>> SpawnUnitListeners = new Dictionary<int, List<LethalDamageinterface>>();

    public void AddSpawnWatcher(LethalDamageinterface mod, int playerOwner)
    {
        SpawnUnitListeners[playerOwner].Add(mod);
    }

    public void RemoveSpawnWatcher(LethalDamageinterface mod, int playerOwner)
    {
            SpawnUnitListeners[playerOwner].Remove(mod);
    }


    /// <summary>
    /// This is used when there is a SpawnOverride object in the scene
    /// </summary>
    /// <param name="man"></param>
    public void AlterUnit(UnitManager man)
    {
        if (man.PlayerOwner == 1)
        {
            if (PlayerSpawnOverride)
            {
                PlayerSpawnOverride.myHitContainer.trigger(MyHero.gameObject, man, 0);
                PlayerSpawnOverride.ModifyUnit(man);              
            }
            if (MarchMode && man.cMover)
            {
                man.cMover.SetMaxSpeed(MarchSpeed *1.3f);
            }
        }
        else
        {
            if (EnemySpawnOverride)
            {
                EnemySpawnOverride.myHitContainer.trigger(null, man, 0);
                EnemySpawnOverride.ModifyUnit(man);
            }
        }

        foreach (LethalDamageinterface mod in SpawnUnitListeners[man.PlayerOwner])
        {
            if (mod != null)
            {
                mod.lethalDamageTrigger(man, null);
            }
        }
    }

    public void SetSpawnLocation(SpawnPointOverride thingy, int playerNumber)
    {
        if (playerNumber == 1)
        {
            PlayerSpawnOverride = thingy;
        }
        else
        {
            EnemySpawnOverride = thingy;
        }
    }

    public Vector3 getSpawnLocation(int playerNumber, bool Vary)
    {
        if (playerNumber == 1)
        {
            if (PlayerSpawnOverride)
            {
                if (PlayerSpawnOverride.CheckIfOnScreen())
                {
                    return PlayerSpawnOverride.transform.position;
                }
                else
                {
                    return getScreenMiddle(Vary ? 18 : 0, playerNumber, true);
                }
            }
        }
        else
        {
            if (EnemySpawnOverride)
            {
                if (EnemySpawnOverride.CheckIfOnScreen())
                {
                    return EnemySpawnOverride.transform.position;
                }
                else
                {
                    return getScreenMiddle(Vary ? 18 : 0, playerNumber, true);
                }
            }
        }
        return getScreenMiddle(Vary? 18 : 0, playerNumber, true);
    }


    public float GetXPosition(int playerNumber)
    {
        if (playerNumber == 1) {
            if (TravelingRight)
            {
                return 0;
            }
            else
            {
                return Screen.width;
            }
        }
        else
        {
            if (TravelingRight)
            {
                return Screen.width;
            }
            else
            {
                return 0;
            }
        }
    }

    public Vector3 getScreenMiddle(float YVariance, int playerNumber, bool StartingSide)
    {
        Ray ray = MainCamera.main.myCamera.ScreenPointToRay(new Vector2(GetXPosition(playerNumber),  Screen.height/2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return CarbotCamera.singleton.ClampYLocation(hit.point + Vector3.forward * Random.Range(-1 * YVariance, YVariance));
        }
        Debug.Log(" Could not find a terrain on the side of the screen");
        return Vector3.zero;
    }

  

    public Vector3 getScreenEdge(Vector3 unitLocation, float YVariance, int playerNumber, bool StartingSide)
    {
        Vector2 screenPoint = MainCamera.main.myCamera.WorldToScreenPoint(unitLocation);// new Vector3(-0, Screen.height/2); //myCamera.WorldToScreenPoint(unitLocation);
        screenPoint.x = GetXPosition(playerNumber);
        Ray ray = MainCamera.main.myCamera.ScreenPointToRay(screenPoint);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return CarbotCamera.singleton.ClampYLocation(hit.point + Vector3.forward * Random.Range(-1 * YVariance, YVariance));
        }
        Debug.Log(" Could not find a terrain on the side of the screen");
        return unitLocation;
    }
    


}
