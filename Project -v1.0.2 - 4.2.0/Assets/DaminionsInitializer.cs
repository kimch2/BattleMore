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

    private void Awake()
    {
        main = this;
    }

    void Start()
    {
        GameObject hero = null;
        if (DMCollectionManager.ChosenHero)
        {
            hero = Instantiate<GameObject>(DMCollectionManager.ChosenHero, GameObject.FindObjectOfType<sPoint>().transform.position, Quaternion.identity);
        }
        else
        {
            hero = Instantiate<GameObject>(DefaultHero, GameObject.FindObjectOfType<sPoint>().transform.position, Quaternion.identity);

        }
        MyHero = hero.GetComponent<UnitManager>();
        myCam.setHero(hero);
        Invoke("SelectHero", .1f);

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
                    spawner.iconPic = obj.GetComponent<UnitStats>().UnitPortrait;
                    spawner.myCost.energy = obj.GetComponent<UnitStats>().cost;
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

    }

    void SelectHero()
    {
        SelectedManager.main.AddObject(MyHero);
        SelectedManager.main.CreateUIPages(0);
    }

    private void Update()
    {
        if (MyHero && MyHero.getUnitStats().currentEnergy != lastEnergy)
        {
            lastEnergy = MyHero.getUnitStats().currentEnergy;
            for (int i = 0; i < CrystalChildren.Count; i++)
            {
                CrystalChildren[i].enabled = (lastEnergy > i);
            }
            

            //ManaSlider.value = MyHero.getUnitStats().currentEnergy / MyHero.getUnitStats().MaxEnergy;
           // ManaText.text = (int)MyHero.getUnitStats().currentEnergy + "/" + MyHero.getUnitStats().MaxEnergy;
        }
    }
}
