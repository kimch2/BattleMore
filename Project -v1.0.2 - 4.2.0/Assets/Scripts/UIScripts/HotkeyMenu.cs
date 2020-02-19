using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HotkeyMenu : MonoBehaviour {


	public RaceInfo raceInfo;

	public GameObject grid;

	public Text GroupOne;

	public Text GroupTwo;

	public Text GroupThree;

	public Text GroupFour;


	public List<Image> myPics;



	private List<GameObject> objectList;
	private SelectedManager selectMan;

	private List<GameObject> toggles = new List<GameObject> ();

	//	private FButtonManager fManager;
	public Material grayScale;
	public static HotkeyMenu main;

	public GameObject RaceInfoPacket;
	private void Awake()
	{
		main = this;
		grayScale = Resources.Load<Material>("GrayScaleUI");
	}


	// Use this for initialization
	void Start () {

		if (RaceSwapper.main)
		{
			raceInfo = RaceSwapper.main.getPlayerRaceInfo();
		}
		else
		{
			if (RaceInfoPacket == null)
			{
				RaceInfoPacket = Resources.Load<GameObject>("RaceInfoPacket");
			}
			if (GameManager.main)
			{
				raceInfo = RaceInfoPacket.GetComponent<UnitEquivalance>().getRace(GameManager.main.activePlayer.myRace);
			}
			else
			{
				raceInfo = RaceInfoPacket.GetComponent<UnitEquivalance>().getRace(RaceInfo.raceType.SteelCrest);
			}
		}

		selectMan = GameObject.FindObjectOfType<SelectedManager> ();
		this.gameObject.GetComponent<Canvas> ().enabled = true;
		StartCoroutine(MyCoroutine());
	
	}




	public void apply()
		{
		List<List<string>> selected = new List<List<string>> ();

		for (int i = 0; i < 4; i++) {
			selected.Add (new List<string> ());
		}

		int n = 0;
		foreach (GameObject obj in toggles) {
			
			if (obj.GetComponent<Toggle>().isOn) {

				selected [n % 4].Add (objectList[(int) n /4].GetComponent<UnitManager> ().UnitName);
				if (myPics [n % 4] != null) {
					myPics [n % 4].sprite = objectList [(int)n / 4].GetComponent<UnitStats> ().Icon;
				}
			}
			n++;
		}

		string toSave = "";
		if (GroupOne) {
			GroupOne.text = "Select all:\n";
		}
		foreach (string s in selected[0]) {
			if (GroupOne) {
				GroupOne.text += s + "s, ";
			}
			toSave += s;
		}
		if (GroupOne)
			GroupOne.text = GroupOne.text.Substring(0,GroupOne.text.Length-2);

		toSave +=";";
		if (GroupTwo) {
			GroupTwo.text = "Select all:\n";
		}
		foreach (string s in selected[1]) {
			if (GroupTwo) {
				GroupTwo.text += s + "s, ";
			}
			toSave += s;
		}
		if (GroupTwo)
			GroupTwo.text =GroupTwo.text.Substring(0,GroupTwo.text.Length-2);
		toSave +=";";

		if (GroupThree) {
			GroupThree.text = "Select all:\n";
		}
		foreach (string s in selected[2]) {
			if (GroupThree) {
				GroupThree.text += s + "s, ";
			}
			toSave += s;
		}
		if (GroupThree)
			GroupThree.text = GroupThree.text.Substring(0,GroupThree.text.Length-2);
		toSave +=";";

		if (GroupFour) {
			GroupFour.text = "Select all:\n";
		}
		foreach (string s in selected[3]) {
			if (GroupFour) {
				GroupFour.text += s + "s, ";
			}
			toSave += s;
		}
		if (GroupFour)
			GroupFour.text = GroupFour.text.Substring(0,GroupFour.text.Length-2);
		toSave +=";";

		// Change this when future levels are added.
		PlayerPrefs.SetString ("FHotkey", toSave);
		if (selectMan) {
			selectMan.applyGlobalSelection (selected);
		}

		}


	IEnumerator MyCoroutine ()
	{
		yield return new WaitForSeconds(.05f);

		char[] separator = {';'};
		//fManager = GameObject.Find ("F-Buttons").GetComponent<FButtonManager>();

		string loaded = "";
		if (RaceSwapper.main)
		{
			loaded = RaceSwapper.main.getPlayerRaceInfo().getFHotkeyString();
		}
		else {
	
			
			 loaded = PlayerPrefs.GetString("FHotkey" + raceInfo.race.ToString(), raceInfo.FHotkeyString);//+ Mathf.Min(3, VictoryTrigger.instance.levelNumber), "");
			
		}
			string[] separated = loaded.Split (separator,System.StringSplitOptions.RemoveEmptyEntries);

	
			selectMan = GameObject.FindObjectOfType<SelectedManager> ();

			objectList = new List<GameObject> ();


		foreach (GameObject obj in raceInfo.unitList)
		{
			if (!(objectList.Find(item => item.GetComponent<UnitManager>().UnitName == obj.GetComponent<UnitManager>().UnitName)))
			{
				objectList.Add(obj);
			}
		}

			foreach (GameObject t in raceInfo.attachmentsList) {
				objectList.Add (t);
				}
			foreach (GameObject build in raceInfo.buildingList) {
				objectList.Add (build);
			}


				
			

			GameObject toggle = transform.Find ("UseIt").gameObject;
			GameObject name = transform.Find ("UnitName").gameObject;
			int n = 0;
			foreach (GameObject obj in objectList) {
				GameObject tempName = (GameObject)Instantiate (name, this.gameObject.transform.position, Quaternion.identity);
				tempName.transform.SetParent (grid.transform);
	
				tempName.GetComponent<Text> ().text = obj.GetComponent<UnitManager> ().UnitName + "    ";

				for (int i = 0; i < 4; i++) {

					GameObject tog = (GameObject)Instantiate (toggle, this.gameObject.transform.position, Quaternion.identity);
					tog.transform.SetParent (grid.transform);
					toggles.Add (tog);



				if (loaded == "" && n != i) {
					tog.GetComponent<Toggle> ().isOn = false;

				} else if (separated.Length == 4 && !separated [i].Contains (obj.GetComponent<UnitManager> ().UnitName)) {
					tog.GetComponent<Toggle> ().isOn = false;
				}
				}

				n++;
			}




		apply ();

		this.gameObject.GetComponent<Canvas> ().enabled = false;
	}

}
