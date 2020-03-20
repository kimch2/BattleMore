using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaminionsInitializer : MonoBehaviour
{

    public GameObject SpawnUnitPrefab;
    public CarbotCamera myCam;
    public GameObject DefaultHero;

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

    private void Awake()
    {
        main = this;
    }

    void Start()
    {
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
        myCam.setHero(hero);
        Invoke("SelectHero", .1f);
        if (!ControllableHero)
        {
            InvokeRepeating("GiveOrder", 2,1.5f);
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
            CrystalChildren[i].transform.parent.gameObject.SetActive(MyHero.myStats.MaxEnergy> i);
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

    private void Update()
    {
        if (MyHero && MyHero.getUnitStats().currentEnergy != lastEnergy)
        {
            lastEnergy = MyHero.getUnitStats().currentEnergy;
            for (int i = 0; i < CrystalChildren.Count; i++)
            {
                CrystalChildren[i].enabled = (lastEnergy - .5f > i);
            }


            //ManaSlider.value = MyHero.getUnitStats().currentEnergy / MyHero.getUnitStats().MaxEnergy;
            // ManaText.text = (int)MyHero.getUnitStats().currentEnergy + "/" + MyHero.getUnitStats().MaxEnergy;
        }
        else if (!MyHero)
        {
            VictoryTrigger.instance.Lose();
            this.enabled = false;
        }
    }



    SpawnPointOverride PlayerSpawnOverride;
    SpawnPointOverride EnemySpawnOverride;

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
            }
        }
        else
        {
            if (EnemySpawnOverride)
            {
                EnemySpawnOverride.myHitContainer.trigger(null, man, 0);
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
                return PlayerSpawnOverride.transform.position;
            }
        }
        else
        {
            if (EnemySpawnOverride)
            {
                return EnemySpawnOverride.transform.position;
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
