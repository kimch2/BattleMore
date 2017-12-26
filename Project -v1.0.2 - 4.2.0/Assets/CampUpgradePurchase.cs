using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class CampUpgradePurchase : MonoBehaviour {


	public SpecificUpgrade myUpgrade;
	public GameObject Costobject;
	public int myCost;
	public bool purchased;
	public Sprite BlueOutline;
	private LevelManager manager;
	public CampaignUpgrade.upgradeType myType;
	public List<CampUpgradePurchase> enables = new List<CampUpgradePurchase>();
	// Use this for initialization
	void Start () {
		StartCoroutine (DelayedStarted ());
	}

	IEnumerator DelayedStarted()
	{

		yield return null;
		manager = GameObject.FindObjectOfType<LevelManager> ();

		if( myUpgrade != null){

			foreach (string up in LevelData.getsaveInfo().purchasedUpgrades) {
				//Debug.Log ("Checking " + up + "   " + myUpgrade.GetType());
				if (up == myUpgrade.Name) {
					psuedoPurchase ();

				}
			}
		}
	}
		
	public void purchase()
	{
		
		if (!purchased) {
			if (myCost <= LevelData.getMoney ()) {
				purchased = true;
				activate ();
				manager.changeMoney (-myCost);
				GetComponent<Image> ().color = Color.cyan;
				LevelData.addUpgrade (myUpgrade);

				updateCostObject ();
				DirectUpgradeApplier.instance.CreatePage (myUpgrade); 

				foreach (CampUpgradePurchase up in enables) {
					up.activate ();
				}

				if (myUpgrade) {
					TrueUpgradeManager.instance.upgradeBought (myUpgrade, myType);
				}
			} else {
				// To Do- put in some kind of audio or visual thing saying you dont have enough money
			}
		} else {
			DirectUpgradeApplier.instance.CreatePage (myUpgrade); 
		}
		
	}

	private void psuedoPurchase()
	{
		purchased = true;
		foreach (CampUpgradePurchase up in enables) {
			up.activate ();
		}

		if (myUpgrade) {
			
			GameObject.FindObjectOfType<TrueUpgradeManager> ().upgradeBought (myUpgrade, myType);
		}
		activate ();
		GetComponent<Image> ().color = Color.blue;
		updateCostObject ();

	}

	public void updateCostObject()
	{
		Costobject.GetComponentInChildren<Text>().text = "Purchased";//.SetActive (false);
		Costobject.GetComponentInChildren<Text>().fontSize = 24;
		Costobject.transform.Find ("Image (1)").gameObject.SetActive (false);

		Costobject.GetComponent<Image> ().sprite = BlueOutline;
		GetComponent<Image> ().sprite = BlueOutline;

		Costobject.GetComponent<Image> ().material = null;
		GetComponent<Image> ().material = null;
	}


	public void activate()
	{
			Costobject.GetComponent<Image> ().material = null;
			GetComponent<Button> ().interactable = true;
			GetComponent<Image> ().material = null;
			GetComponent<MouseOver> ().enabled = true;

	}
}
