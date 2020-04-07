using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMCollectionManager : MonoBehaviour
{
    public GameObject UnitRegistryPrefab;
    public GameObject UnitCardPrefab;
    public GameObject UnitTagPrefab;
    public Transform UnitCardScrollArea;
    public Transform TagScrollArea;
    public Transform AbilityCardScrollArea;
    public Transform RelicCardScrollArea;
    public Transform SelectedUnitsArea;
    public Transform SelectedAbilityArea;
    public Transform SelectedRelicsArea;
    public int SceneNumber;
    public static GameObject ChosenHero;
    public UnityEngine.UI.Toggle ControllableTog;

    public static List<GameObject> ChosenUnits = new List<GameObject>() { null,null,null,null};
    public static List<GameObject> ChosenAbilities = new List<GameObject>() { null, null };

    public List<DMAssigner> SelectedUnits;
    public List<DMAssigner> SelectedAbility;
    public List<DMAssigner> SelectedRelics;

    List<DMUnitCard> currentUnitCards = new List<DMUnitCard>();
    List<DMUnitCard> currentAbilityCards = new List<DMUnitCard>();
    List<DMUnitCard> currentUpgradeCards = new List<DMUnitCard>();
    List<UnitTypes.UnitTypeTag> usedTags = new List<UnitTypes.UnitTypeTag>();
    Dictionary<UnityEngine.UI.Toggle, UnitTypes.UnitTypeTag> TagToggles = new Dictionary<UnityEngine.UI.Toggle, UnitTypes.UnitTypeTag>();
    public bool ContainsAllTag;

    public static DMCollectionManager instance;
    public DMUnitCard currentDragger;
    public DMAssigner currentReceptacle;

    public void setContainsAllTag(bool isOn)
    {
        ContainsAllTag = isOn;
        ToggleHit(true);
    }
    private void Awake()
    {
        instance = this;

        ControllableTog.isOn = PlayerPrefs.GetInt("ControlHero") == 1;
    }

    public static void AssignUnit(int i, GameObject choice)
    {
        PlayerPrefs.SetString(ChosenHero.GetComponent<UnitManager>().UnitName +"Unit"+i, 
            choice ? choice.GetComponent<UnitManager>().UnitName : "");
        ChosenUnits[i] = choice;
    }

    public static void AssignAbility(int i, GameObject choice)
    {
        PlayerPrefs.SetString(ChosenHero.GetComponent<UnitManager>().UnitName + "Ability"+i,
          choice ? choice.GetComponent<Ability>().Name : "");
        ChosenAbilities[i] = choice;
    }

    public static void AssignRelic(int i, GameObject choice)
    {
        PlayerPrefs.SetString(ChosenHero.GetComponent<UnitManager>().UnitName + "Relic" + i,
          choice ? choice.GetComponent<Upgrade>().Name : "");
    }

    public void PlayDemo()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneNumber);
    }

    public void LoadHero()
    {
        foreach (DMUnitCard card in currentUnitCards)
        {
            card.DestroyMe();
            Destroy(card.gameObject);
        }
        currentUnitCards.Clear();
        foreach (DMUnitCard card in currentAbilityCards)
        {
            card.DestroyMe();
            Destroy(card.gameObject);
        }
        currentAbilityCards.Clear();
        foreach (DMUnitCard card in currentUpgradeCards)
        {
            card.DestroyMe();
            Destroy(card.gameObject);
        }
        currentUpgradeCards.Clear();
        usedTags.Clear();

        foreach (KeyValuePair<UnityEngine.UI.Toggle, UnitTypes.UnitTypeTag> pair in TagToggles)
        {
            Destroy(pair.Key.gameObject);
        }
        TagToggles.Clear();

        List<string> assignedUnits = new List<string>(); // Previous Session Choices
        for (int i = 0; i < 4; i++)
        {
            assignedUnits.Add(PlayerPrefs.GetString(ChosenHero.GetComponent<UnitManager>().UnitName + "Unit" + i));
        }
        Debug.Log("CHosen " + ChosenHero);
        foreach (RaceInfo info in UnitRegistryPrefab.GetComponents<RaceInfo>())
        {
            if (!info.UltimatePrefab || ChosenHero == info.UltimatePrefab)
            {
                foreach (GameObject obj in info.unitList)
                {
                    foreach (UnitTypes.UnitTypeTag thingy in obj.GetComponent<UnitStats>().otherTags)
                    {
                        if (!usedTags.Contains(thingy))
                        {
                            usedTags.Add(thingy);
                            GameObject newTag = Instantiate<GameObject>(UnitTagPrefab, TagScrollArea);
                            newTag.transform.SetAsFirstSibling();
                            newTag.GetComponentInChildren<UnityEngine.UI.Text>().text = thingy.ToString();
                            TagToggles.Add(newTag.GetComponent<UnityEngine.UI.Toggle>(), thingy);
                            newTag.GetComponent<UnityEngine.UI.Toggle>().onValueChanged.AddListener(ToggleHit);
                        }
                    }

                    GameObject newButton = Instantiate<GameObject>(UnitCardPrefab, UnitCardScrollArea);
                    DMUnitCard dmcard = newButton.GetComponent<DMUnitCard>();
                    currentUnitCards.Add(dmcard);
                    dmcard.LoadUnit(obj);
                    if (assignedUnits.Contains(obj.GetComponent<UnitManager>().UnitName))
                    { // We are loading our saved choices from previous sessions
                        dmcard.AssignToCard(SelectedUnits[assignedUnits.IndexOf(obj.GetComponent<UnitManager>().UnitName)]); 
                    }
                }

                List<string> assignedAbility = new List<string>();
                for (int i = 0; i < 4; i++)
                {
                    assignedAbility.Add(PlayerPrefs.GetString(ChosenHero.GetComponent<UnitManager>().UnitName + "Ability" + i));
                }

                foreach (GameObject obj in info.attachmentsList)
                {
                    GameObject newButton = Instantiate<GameObject>(UnitCardPrefab, AbilityCardScrollArea);
                    DMUnitCard dmcard = newButton.GetComponent<DMUnitCard>();
                    dmcard.LoadAbility(obj);
                    currentAbilityCards.Add(dmcard);

                    if (assignedAbility.Contains(obj.GetComponent<Ability>().Name))
                    {
                        dmcard.AssignToCard(SelectedAbility[assignedAbility.IndexOf(obj.GetComponent<Ability>().Name)]);
                    }
                }

                List<string> assignedRelics = new List<string>();
                for (int i = 0; i < 4; i++)
                {
                    assignedAbility.Add(PlayerPrefs.GetString(ChosenHero.GetComponent<UnitManager>().UnitName + "Relic" + i));
                }
                /*
                foreach (GameObject obj in info.buildingList)
                {
                    GameObject newButton = Instantiate<GameObject>(UnitCardPrefab, RelicCardScrollArea);
                    DMUnitCard dmcard = newButton.GetComponent<DMUnitCard>();
                    dmcard.LoadRelic(obj);
                    currentUpgradeCards.Add(dmcard);
                    if (assignedRelics.Contains(obj.GetComponent<Upgrade>().Name))
                    {
                        dmcard.AssignToCard(SelectedRelics[assignedRelics.IndexOf(obj.GetComponent<Upgrade>().Name)]);
                    }
                }*/
            }
        }
    }

    public void SetHero(GameObject hero)
    {
        ChosenHero = hero;
    }

    public void ToggleHit(bool isOn)
    {
        List<UnitTypes.UnitTypeTag> usedTags = new List<UnitTypes.UnitTypeTag>();
        foreach (KeyValuePair<UnityEngine.UI.Toggle, UnitTypes.UnitTypeTag> pair in TagToggles)
        {
            if (pair.Key.isOn)
            {
                usedTags.Add(pair.Value);
            }
        }
        if (usedTags.Count == 0)
        {
            foreach (DMUnitCard obj in currentUnitCards)
            {
                obj.gameObject.SetActive(true);
            }
            return;
        }

        if (!ContainsAllTag)
        {
            foreach (DMUnitCard obj in currentUnitCards)
            {
                obj.gameObject.SetActive(true);
                foreach (UnitTypes.UnitTypeTag tag in usedTags)
                 {  
                    UnitStats myStats = obj.MyUnit.GetComponent<UnitStats>();
                    if (!myStats.otherTags.Contains(tag))
                    {
                        obj.gameObject.SetActive(false);   
                    }
                }
            }
        }
        else
        {
            foreach (DMUnitCard obj in currentUnitCards)
            {
                obj.gameObject.SetActive(false);
                UnitStats myStats = obj.MyUnit.GetComponent<UnitStats>();
                foreach (UnitTypes.UnitTypeTag tag in myStats.otherTags)
                {
                    if (usedTags.Contains(tag))
                    {
                        obj.gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }
    }

    void UpdateUnitCards()
    {

    }

    public void SetHeroController(UnityEngine.UI.Toggle tog)
    {
        PlayerPrefs.SetInt("ControlHero", tog.isOn ? 1:0);
    }

}
