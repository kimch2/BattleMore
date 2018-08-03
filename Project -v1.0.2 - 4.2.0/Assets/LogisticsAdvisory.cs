using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogisticsAdvisory : MonoBehaviour {


	GameObject currentPage;
	GameObject currentTab;



	public List<LogisticsAdvisor> myAdvisors;

	[System.Serializable]
	public struct LogisticsAdvisor
	{
		public string AdvisorName;
		public GameObject Page;
		public GameObject Tab;
		public Upgrade myUpgrade;
		public GameObject TabCheck;
	}

	public void Start()
	{
		PlayerPrefs.SetInt (myAdvisors[0].AdvisorName,1);
		string s = PlayerPrefs.GetString ("CurrentAdvisor", myAdvisors[0].AdvisorName);
		SelectCharacterTab (s);	
		assignAsAdvisor (s);
	}

	public void SelectCharacterTab(string tab)
	{

		foreach (LogisticsAdvisor la in myAdvisors) {
			if (la.AdvisorName == tab) {
				openWindow (la.Page,la.Tab,la.AdvisorName);
			}
		}
			
	}

	public void openWindow(GameObject page, GameObject tab,  string characterName)
	{
		if (currentPage) {
			currentPage.SetActive (false);
			UnityEngine.UI.ColorBlock cb = currentTab.GetComponent<UnityEngine.UI.Button> ().colors;
			cb.normalColor = Color.gray;
			currentTab.GetComponent<UnityEngine.UI.Button> ().colors = cb;
		}
		currentPage = page;
		currentPage.SetActive (true);
		currentTab = tab;

		UnityEngine.UI.ColorBlock cbB = tab.GetComponent<UnityEngine.UI.Button> ().colors;
		cbB.normalColor = Color.white;
		tab.GetComponent<UnityEngine.UI.Button> ().colors = cbB;

		page.transform.Find ("SelectasAdvisor").gameObject.SetActive ((PlayerPrefs.GetInt(characterName,0) != 0));
		page.transform.Find ("SelectasAdvisor").gameObject.GetComponent<UnityEngine.UI.Button>().interactable = (PlayerPrefs.GetString("CurrentAdvisor") != characterName);

		page.transform.Find ("HireAdvisor").gameObject.SetActive((PlayerPrefs.GetInt(characterName,0) == 0));
		page.transform.Find ("HireAdvisor").Find("Text (1)").GetComponent<UnityEngine.UI.Text>().color = (LevelData.getMoney () > 9) ? Color.white: Color.red;
		tab.transform.SetAsLastSibling ();
	}

	public void HireAdvisor(string tab)
	{
		if (LevelData.getMoney () > 14) {

			foreach (LogisticsAdvisor la in myAdvisors) {
				if (la.AdvisorName == tab) {
					PlayerPrefs.SetInt (la.AdvisorName, 1);
					LevelManager.main.changeMoney (-15);
					Start ();
					SelectCharacterTab (la.AdvisorName);	
					TrueUpgradeManager.instance.playSound();
					assignAsAdvisor (tab);
				}
			}
		} else {
			TrueUpgradeManager.instance.playBadSound ();
		}
	}

	public void assignAsAdvisor(string character)
	{

		foreach (LogisticsAdvisor la in myAdvisors) {
			TrueUpgradeManager.instance.otherUpgrades.Remove(la.myUpgrade);
			la.TabCheck.SetActive (false);

			if (la.AdvisorName == character) {
				PlayerPrefs.SetInt ("VoicePack", myAdvisors.IndexOf(la));
				TrueUpgradeManager.instance.otherUpgrades.Add (la.myUpgrade);
				PlayerPrefs.SetString ("CurrentAdvisor", character);			
				currentPage.transform.Find ("SelectasAdvisor").gameObject.GetComponent<UnityEngine.UI.Button>().interactable = (false);

				la.TabCheck.SetActive (true);

				if (Time.timeSinceLevelLoad > 1) {

					VoiceContainer container = Resources.Load<GameObject> ("VoiceContainer").GetComponent<VoiceContainer>();
					TrueUpgradeManager.instance.mySource.PlayOneShot (
						container.myVoicePacks[myAdvisors.IndexOf(la)].getVoicePackLine());
				}

			}
		}

	}

}
