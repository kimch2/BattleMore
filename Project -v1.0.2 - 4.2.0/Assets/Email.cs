using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Email : MonoBehaviour {

	public List<EmailAccount> accounts;
	public List<mail> mails;

	public Text UserName;
	public Image userPic;
	EmailAccount currentAccount;
	public GameObject FirstTimeScreen;
	public GameObject LogInScreen;
	public GameObject WelcomeScreen;
	public GameObject MainEmailScreen;

	public InputField passwordField;

	public Text hintText;
	public Toggle rememberPassword;
	public GameObject hintButton;


	public GameObject EmailHeaderTemplate;
	public GameObject incorrect;

	public Dropdown UserNameDropDown;

	public GameObject inbox;
	public GameObject sentBox;
	public GameObject ReadEmailObj;



	void Awake()
	{
		currentAccount = accounts [0];
		accounts [0].Username = PlayerPrefs.GetString ("PlayerName", "");
		UserNameDropDown.options [0].text = accounts [0].Username;
		UserNameDropDown.gameObject.GetComponentInChildren<Text> ().text = accounts [0].Username;

		passwordField.gameObject.SetActive (false);
		rememberPassword.gameObject.SetActive (false);
		hintButton.SetActive (false);

		FirstTimeScreen.SetActive (true);

		if (currentAccount.Username != "") {
			FirstTimeScreen.SetActive (false);
			LogInScreen.SetActive (true);
		} else {
			FirstTimeScreen.SetActive (true);
			LogInScreen.SetActive (false);
		}

	}

	public void ToggleHint()
	{

		hintText.text = currentAccount.hint;
		hintText.transform.parent.gameObject.SetActive (!hintText.transform.parent.gameObject.activeSelf);
	}

	public void selectUser(Dropdown dropper)
	{
		hintText.transform.parent.gameObject.SetActive (false);
		currentAccount = accounts [dropper.value];
		if (PlayerPrefs.GetString (currentAccount.Username, "") != "" && PlayerPrefs.GetString (currentAccount.Username + "Rem", "false") == "true") {
			passwordField.text = PlayerPrefs.GetString (currentAccount.Username, "");
		} else {
			passwordField.text = "";
		}

		hintButton.gameObject.SetActive (dropper.value !=0);
		passwordField.gameObject.SetActive (dropper.value !=0);
		rememberPassword.gameObject.SetActive (dropper.value !=0);
	


	}


	public void FirstTimeLogIn(InputField input)
	{
		accounts [0].Username = input.text;
		PlayerPrefs.SetString ("PlayerName" , input.text);
		AttemptLogin (input);
	}


	public void ReadEmail(mail m)
	{
		ReadEmailObj.SetActive (true);
		m.Message.Replace ("{Player}", PlayerPrefs.GetString ("PlayerName") );
		ReadEmailObj.GetComponentInChildren<Text> ().text = "To: " + m.To+ "\nFrom: "+ m.From + "\n\n"+m.Message;

	}

	public void AttemptLogin(InputField input)
	{
		if (currentAccount.checkPassword (input.text) || currentAccount == accounts[0]) {

			PlayerPrefs.SetString (currentAccount.Username + "Rem", rememberPassword.isOn? "true": "false");
		
			OpenEmail ();
		} else {
			incorrect.SetActive (true);
			Invoke ("closeIncorrect", 3);

		}
		Debug.Log ("Checked");
	}

	List<mail> Sent = new List<mail> ();
	List<mail> Received = new List<mail> ();

	public void OpenEmail()
	{
		WelcomeScreen.SetActive (true);
		Invoke ("closeWelcome", 2f);
		WelcomeScreen.GetComponentInChildren<Text> ().text = "Welcome " + currentAccount.Username;
		MainEmailScreen.SetActive (true);

		sentBox.SetActive (false);
		inbox.SetActive (true);
		UserName.text = currentAccount.Username;
		if (currentAccount.Image != null) {
			userPic.sprite = currentAccount.Image;
			userPic.gameObject.SetActive (true);
		} else {
			userPic.gameObject.SetActive (false);
		}

		foreach (Transform t in inbox.transform) {
			if (t.gameObject.name.Contains ("Email")) {
				Destroy (t.gameObject);
			}
		}
		foreach (Transform t in sentBox.transform) {
			if (t.gameObject.name.Contains ("Email")) {
				Destroy (t.gameObject);
			}
		}

		bool appendMailMessage = false;
		foreach (mail m in mails) {
			if (m.To == "Player") {
				m.To = accounts [0].Username;
			}

			if (m.From == currentAccount.Username ) {
				GameObject obj = Instantiate<GameObject> (EmailHeaderTemplate, sentBox.transform);
				obj.GetComponent<Button> ().onClick.AddListener (delegate() {
					ReadEmail(m);
				});
				obj.transform.Find ("From").GetComponent<Text> ().text = "To: " +m.To;


				obj.transform.Find ("Header Line").GetComponent<Text> ().text = m.Header;
				Sent.Add (m);
			} 
			if (m.To == currentAccount.Username) {
				Received.Add (m);
				GameObject obj = Instantiate<GameObject> (EmailHeaderTemplate, inbox.transform);
				obj.GetComponent<Button> ().onClick.AddListener (delegate() {
					ReadEmail(m);
				});
				obj.transform.Find ("From").GetComponent<Text> ().text = "From: " +m.From;
				obj.transform.Find ("Header Line").GetComponent<Text> ().text = m.Header;
				if (m.LevelUnLocked == LevelData.getHighestLevel ()) {
					appendMailMessage = true;
				}
			}
		}

		if (appendMailMessage) {
			WelcomeScreen.GetComponentInChildren<Text> ().text += "\n You Got Mail!";
		}


	}

	void closeWelcome()
	{
		WelcomeScreen.SetActive (false);
	}

	void closeIncorrect()
	{
		incorrect.SetActive (false);
	}


	public void Logout()
	{
		MainEmailScreen.SetActive (false);
	}


}

[System.Serializable]
public class EmailAccount
{
	public string Username;
	public Sprite Image;
	public List<string> PasswordKeys; //beard
	public string hint;

	public bool checkPassword(string toCheck) {
		if (PlayerPrefs.GetString (Username, "") == "") {
			foreach (string pass in PasswordKeys) {
				if (toCheck.ToLower().Contains (pass.ToLower())) {
					PlayerPrefs.SetString (Username, toCheck);
					return true;
				}
			}
		}

		if (PlayerPrefs.GetString (Username, "").ToLower() == toCheck.ToLower()) {
			return true;
		}
		


		return false;
	}
}
[System.Serializable]
public class mail
{
	public string Header;
	public string From;
	public string To;
	[TextArea(2,6)]
	public string Message;

	public int LevelUnLocked;
}