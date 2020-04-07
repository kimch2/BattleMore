using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DMLevelEditor : MonoBehaviour
{
    public Camera myCamera;
    public Transform LeftBound;
    public Text lengthText;
    public InputField SaveName;
    public GameObject UnitRegistry;
    public GameObject EnemySelectionArea;
    public GameObject ScenerySelectionArea;
    public Dropdown SceneryAngle;
    public Transform FileArea;
    public Button DeleteButton;
    public Button LoadButton;

    public GameObject ButtonTemplate;
    public GameObject FileNameTemplate;
    public Canvas LoadCanvas;
    public GameObject EnemySelectionParent;
    public GameObject ScenerySelectionParent;
    public Slider SceneryScale;

    GameObject ToPlace;
    Sprite currentSprite;

    CustomInputSystem inputSystem;

    public DaminionMap mapData;
    List<UnitManager> AllUnits = new List<UnitManager>();
    List<Sprite> AllScenery = new List<Sprite>();
    List<GameObject> LoadSceneButtons = new List<GameObject>();

    private void Start()
    {
        inputSystem = GameObject.FindObjectOfType<CustomInputSystem>();
        foreach (RaceInfo info in UnitRegistry.GetComponents<RaceInfo>())
        {
            foreach (Sprite obj in info.myUIImages.toReplace)
            {
                GameObject newButton = Instantiate<GameObject>(ButtonTemplate, ScenerySelectionArea.transform);
                newButton.GetComponent<Image>().sprite = obj;
                newButton.GetComponent<Button>().onClick.AddListener(() => SelectScenery(obj, true));
                AllScenery.Add(obj);
            }

            foreach (GameObject manag in info.unitList)
            {
                GameObject newButton = Instantiate<GameObject>(ButtonTemplate, EnemySelectionArea.transform);
                SpriteRenderer render = newButton.GetComponent<SpriteRenderer>();
                newButton.GetComponent<Image>().sprite = manag.GetComponent<UnitStats>().Icon;
                newButton.GetComponentInChildren<Text>().text = manag.GetComponent<UnitManager>().UnitName;
                newButton.GetComponent<Button>().onClick.AddListener(()=> SelectUnit(manag, true));
                AllUnits.Add(manag.GetComponent<UnitManager>());
            }
        }
    }

    private void Update()
    {
        if (Input.mousePosition.x < 1 && myCamera.transform.position.x > LeftBound.position.x)
        {
            myCamera.transform.Translate(Vector3.left * Time.deltaTime * 45);
        }
        else if (Input.mousePosition.x > Screen.width - 1 && myCamera.transform.position.x < LeftBound.position.x + mapData.MapLength)
        {
            myCamera.transform.Translate(Vector3.right * Time.deltaTime * 45);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            SceneryScale.value += .04f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            SceneryScale.value -= .04f;
        }


        if (!ToPlace)
        {
            return;
        }
        bool IsPlacingUnit = ToPlace.GetComponent<UnitManager>();

            if (!isPointerOverUIObject())
            {
                Ray rayb = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(rayb, out hit, Mathf.Infinity, 1 << 8))
                {
                    ToPlace.transform.position = hit.point;
                    if (!IsPlacingUnit)
                    {
                        UpdateAngle();
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (IsPlacingUnit)
                    {
                        placeUnit();
                    }
                    else
                    {
                        placeScenery();
                    }
                }
                if (Input.GetMouseButtonDown(1))
                {
                    if (IsPlacingUnit)
                    {
                        RaycastHit[] hitb = Physics.RaycastAll(rayb.origin, rayb.direction, Mathf.Infinity, 1 << 9, QueryTriggerInteraction.Ignore);
                        foreach (RaycastHit hitter in hitb)
                        {
                            if (hitter.collider.gameObject != ToPlace)
                            {
                                mapData.Units.RemoveAll(item => item.ex == hitter.collider.gameObject);
                                Destroy(hitter.collider.gameObject);
                                break;
                            }
                        }
                    }
                    else
                    {
                        RaycastHit[] hitb = Physics.RaycastAll(rayb.origin, rayb.direction, Mathf.Infinity, 1 << 0, QueryTriggerInteraction.Ignore);
                        foreach (RaycastHit hitter in hitb)
                        {
                            if (hitter.collider.gameObject != ToPlace)
                            {
                                mapData.Scenery.RemoveAll(item => item.ex == hitter.collider.gameObject);
                                Destroy(hitter.collider.gameObject);
                                break;
                            }
                        }
                    }
                
            }
        }
    }

    public void ChangeMapLength(Slider slider)
    {
        bool atRightBound = (myCamera.transform.position.x > LeftBound.position.x + mapData.MapLength - 2);
        mapData.MapLength = slider.value * 600 + 100;
        lengthText.text = "Map Length " + mapData.MapLength;
        if (myCamera.transform.position.x > LeftBound.position.x + mapData.MapLength || atRightBound )
        {
            myCamera.transform.position = new Vector3(LeftBound.position.x + mapData.MapLength, myCamera.transform.position.y, myCamera.transform.position.z);
        }
    }

    public void OpenArea(UnityEngine.UI.Dropdown dropper)
    {
        EnemySelectionParent.SetActive(dropper.value == 0);
        ScenerySelectionParent.SetActive(dropper.value == 1);
        SceneryAngle.gameObject.SetActive(dropper.value == 1);
        SceneryScale.gameObject.SetActive(dropper.value == 1);
    }

    public void UpdateAngle()
    {
        if (ToPlace)
        {
            if (SceneryAngle.value == 0)
            {
                ToPlace.transform.rotation = Quaternion.identity;
            }
            else if (SceneryAngle.value == 1)
            {
                ToPlace.transform.eulerAngles = Vector3.right * 90;
            }
            else
            {
                Quaternion Rot = Quaternion.LookRotation(myCamera.transform.forward, myCamera.transform.up);
                ToPlace.transform.rotation = Rot;
            }
        }
    }

    public void UpdateSceneryScale()
    {
        UpdateScale(ToPlace);
    }

    void UpdateScale(GameObject toAdjust)
    {
        if (toAdjust)
        {
            toAdjust.transform.localScale =Vector3.one * (SceneryScale.value * 20 + .5f);
        }
    }

    void ClearMap()
    {
        foreach (UnitData data in mapData.Units)
        {
            Destroy(data.ex);
        }

        foreach (SceneryData data in mapData.Scenery)
        {
            Destroy(data.ex);
        }
    }



    public void SelectScenery(Sprite sprite, bool DestroyOriginal)
    {
        if (ToPlace && DestroyOriginal)
        {
            Destroy(ToPlace);
        }
        currentSprite = sprite;
        ToPlace = CreateScenery(sprite.name);
    }

    GameObject CreateScenery(string SpriteName)
    {
        GameObject toReturn = new GameObject();
        toReturn.AddComponent<SpriteRenderer>().sprite = AllScenery.Find(item => item.name == SpriteName);
        toReturn.AddComponent<BoxCollider>();
        UpdateScale(toReturn);
        return toReturn;
    }

    public void placeScenery()
    {
        mapData.Scenery.Add(new SceneryData(ToPlace, RoundVector(ToPlace.transform.position - LeftBound.transform.position), currentSprite.name, false));
        SelectScenery(currentSprite, false);
    }



    public GameObject CreateUnit(GameObject template)
    {
        GameObject toMake = Instantiate<GameObject>(template);
        foreach (MonoBehaviour comp in toMake.GetComponents<MonoBehaviour>())
        {
            comp.enabled = false;
        }
        return toMake;
    }

    public void placeUnit()
    {
        mapData.Units.Add(new UnitData(ToPlace, ToPlace.GetComponent<UnitManager>().UnitName, RoundVector(ToPlace.transform.position - LeftBound.transform.position)));
        SelectUnit(ToPlace, false);
    }

    public void SelectUnit(GameObject obj, bool DestroyOriginal)
    {
        if (ToPlace && DestroyOriginal)
        {
            Destroy(ToPlace);
        }
        ToPlace = CreateUnit(obj);
    }



    public bool isPointerOverUIObject()
    {
        return inputSystem.overUILayer();
    }


    public void LoadFile()
    {
        ClearMap();

        string inputJson = System.IO.File.ReadAllText("Assets/"+SaveName.text + ".dmm");
        mapData = JsonUtility.FromJson<DaminionMap>(inputJson);

        foreach (UnitData data in mapData.Units)
        {
            UnitManager foundUnit = AllUnits.Find(item => item.UnitName == data.unitName);
            GameObject created = CreateUnit(foundUnit.gameObject);
            created.transform.position = data.pos +  LeftBound.transform.position;
            data.ex = created;
        }

        foreach (SceneryData data in mapData.Scenery)
        {
            GameObject obj = CreateScenery(data.spriteName);
            obj.transform.position = LeftBound.position+ data.pos;
            obj.transform.rotation = data.rot;
            obj.transform.localScale = data.scale;
            data.ex = obj;
        }
    }

    Vector3 RoundVector(Vector3 toConvert)
    {
        return new Vector3(Mathf.Round(toConvert.x), Mathf.Round(toConvert.y), Mathf.Round(toConvert.z));
    }


    public void Save()
    {
        if (!string.IsNullOrEmpty(SaveName.text))
        {
            //Debug.Log("Saving");
            string jsonString = JsonUtility.ToJson(mapData);
            string name = SaveName.text;
            System.IO.File.WriteAllText("Assets/" +name + ".dmm", jsonString);
            updateInputField();
        }
    }

    public void UpdateLoadScreen()
    {
        foreach (GameObject obj in LoadSceneButtons)
        {
            Destroy(obj);
        }
        LoadSceneButtons.Clear();
        foreach (string file in System.IO.Directory.GetFiles(Application.dataPath,"*.dmm"))
        {
            
            string subber = file.Substring(Application.dataPath.Length + 1);
            subber = subber.Substring(0, subber.Length - 4);
            GameObject newBut = Instantiate(FileNameTemplate, FileArea);
            LoadSceneButtons.Add(newBut);
            newBut.GetComponent<Button>().onClick.AddListener(() => SaveName.text = subber);
            newBut.GetComponentInChildren<Text>().text = subber;           
        }
    }

    public void DeleteFile()
    {
       System.IO.File.Delete(Application.dataPath + "/" + SaveName.text + ".dmm");
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
        UpdateLoadScreen();
        updateInputField();
    }

    public void updateInputField()
    {
        DeleteButton.interactable = System.IO.File.Exists(Application.dataPath + "/" + SaveName.text + ".dmm");
        LoadButton.interactable = DeleteButton.interactable;

    }

    public void Play()
    {
        PlayerPrefs.SetString("PlayLevel", SaveName.text);
        Save();

    }

    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

}


[System.Serializable]
public class DaminionMap {

    public float MapLength = 500;
    public List<SceneryData> Scenery = new List<SceneryData>();
    public List<UnitData> Units = new List<UnitData>();

}

[System.Serializable]
public class SceneryData {
    public string spriteName;
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;
    public bool CamLook;
    [System.NonSerialized]
    public GameObject ex;

    public SceneryData(GameObject example, Vector3 relativePosition, string name, bool cameraLook)
    {
        ex = example;
        spriteName = name;
        pos = relativePosition;
        rot = example.transform.rotation;
        scale =example.transform.localScale;
        CamLook = cameraLook;
    }
}

[System.Serializable]
public class UnitData
{
    public string unitName;
    public Vector3 pos;
    [System.NonSerialized]
    public GameObject ex;


    public UnitData(GameObject example, string unitName, Vector3 position)
    {
        ex = example;
        this.unitName = unitName;
        this.pos = position;
    }
}