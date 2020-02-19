using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TrueUpgradeManager : MonoBehaviour {

	[Tooltip("Button Press Sound Effects")]
	public List<AudioClip> buttonPress;
	public AudioSource mySource;

	public AudioClip simpleButtonPress;
	public AudioClip badButtonPress;

	[Tooltip("List of all possible upgrades")]
	public List<CampaignUpgrade> CampUpRef;

	[Tooltip("All pruchased upgrades, including the two default empty ones.")]
	public List<CampaignUpgrade.UpgradesPiece> myUpgrades= new List<CampaignUpgrade.UpgradesPiece>();
	// Use this for initialization

	[Tooltip("List of gameobject signs that show the player there are unapplied upgrades")]
	public List<GameObject> UnAppliedUpgrade;

	public static TrueUpgradeManager instance;

	[Tooltip("0 should be vehicles, 1 should be buildings, 2 should be turrets")]
	public List<UnityEngine.UI.Text> UnAppliedExclam;

	public List<Upgrade> otherUpgrades = new List<Upgrade>();

	void Awake()
	{instance = this;
		
	}


	void Start()
	{
		if (this && !hasBeenToLevel) {
			if (SceneManager.GetActiveScene ().buildIndex == 1) {
				DontDestroyOnLoad (this.gameObject);
			} 

			mySource = GetComponent<AudioSource> ();
		}
	}

	public void Unused()
	{

		bool unUsed = false;

		foreach (UnityEngine.UI.Text tex in UnAppliedExclam) {
			tex.enabled = false;
		}

		foreach (CampaignUpgrade upgrade in CampUpRef) {
			if (upgrade && upgrade.myUpgrades.Count > 1 && upgrade.currentIndex == 0 && upgrade.unlocked) {
		
				if (upgrade.myTypes.Contains (CampaignUpgrade.upgradeType.Vehicle)) {
					UnAppliedExclam [0].enabled = true;
				}
				else 	if (upgrade.myTypes.Contains (CampaignUpgrade.upgradeType.Building)) {
					UnAppliedExclam [1].enabled = true;
				}
				else 	if (upgrade.myTypes.Contains (CampaignUpgrade.upgradeType.Turret)) {
					UnAppliedExclam [2].enabled = true;
				}

				unUsed = true;
				break;
			}

		}
		foreach (GameObject obj in UnAppliedUpgrade) {
			if (obj) {
				obj.SetActive (unUsed);
			}
		}
	}

	void OnEnable()
	{
		SceneManager.sceneLoaded += LevelWasLoaded;
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= LevelWasLoaded;
	}

	bool hasBeenToLevel;


	void LevelWasLoaded(Scene myScene, LoadSceneMode mode)
	{
		if (SceneManager.GetActiveScene ().buildIndex != 1 && SceneManager.GetActiveScene ().buildIndex != 0) {
			RaceManager racer = GameManager.main.activePlayer;
			hasBeenToLevel = true;
	
			foreach (CampaignUpgrade.UpgradesPiece cu in myUpgrades) {
				if (cu.pointer) {
					racer.addUpgrade (cu.pointer, "");
				}
			}

			foreach (Upgrade up in otherUpgrades) {
				racer.addUpgrade (up,"");
			}

		} else {
			if (hasBeenToLevel) {
				Destroy (this.gameObject);
			}
		}
	}

	float lastSound ;
	public void playSound ()
	{
		if (Time.timeSinceLevelLoad > 1 &&  Time.time > lastSound +1) {
			mySource.PlayOneShot (buttonPress [Random.Range (0, buttonPress.Count)]);
		}
		lastSound = Time.time;
	}

	public void playSimpleSound ()
	{
		if (Time.timeSinceLevelLoad > 1 &&  Time.time > lastSound +.5f) {
			mySource.PlayOneShot (simpleButtonPress);
		}
		lastSound = Time.time;
	}

	public void playBadSound ()
	{
		if (Time.timeSinceLevelLoad > 1 &&  Time.time > lastSound +.5f) {
			mySource.PlayOneShot (badButtonPress);
		}
		lastSound = Time.time;
	}

	public void upgradeBought(SpecificUpgrade upg, CampaignUpgrade.upgradeType t)
	{
		//Debug.Log ("Upgrade was bought" +upg.name);
		CampaignUpgrade.UpgradesPiece cpu = new CampaignUpgrade.UpgradesPiece ();
		cpu.pointer = upg;
		cpu.name = upg.Name;
		cpu.description = upg.Descripton;
		cpu.unlocked = true;
		cpu.pic = upg.iconPic;
		cpu.myType = t;

		myUpgrades.Add (cpu);

		foreach (CampaignUpgrade cu in CampUpRef) {
			cu.upgradeBought();
		
		}
		

	}


}
