using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class NewAchievmentUI : MonoBehaviour {



	public AchievementManager AchievementMan;
	public GameObject AchievePanel;
	public GameObject NewTitle;

	void Start()
	{
		NewTitle.SetActive (false);
		foreach (Achievement ach in AchievementMan.myAchievements) {

				if (ach.IsAccomplished () && !ach.HasBeenRewarded ()) {
					ach.Reward ();

					GameObject.FindObjectOfType<LevelManager> ().changeMoney (ach.TechReward);

					GameObject obj = (GameObject)Instantiate (AchievePanel, this.transform);
					OpenAchievements.Add (obj);
					obj.transform.Find ("Title").GetComponent<Text> ().text = ach.Title;
					obj.transform.Find ("Description").GetComponent<Text> ().text = ach.GetDecription ();
					obj.transform.Find ("Icon").GetComponent<Image> ().sprite = ach.myIcon;
					obj.transform.localScale = new Vector3 (1, 1, 1);

					if (ach.TechReward > 0) {
						obj.transform.Find ("RewardDescription").GetComponent<Text> ().text = 
						"+ " + ach.TechReward;
						obj.transform.Find ("RewardPic").GetComponent<Image> ().enabled = true;
					} else {
					obj.transform.Find ("RewardDescription").GetComponent<Text> ().fontSize = 16;
						obj.transform.Find ("RewardDescription").GetComponent<Text> ().text = 
						"New Voice Pack!";
						obj.transform.Find ("RewardHelp").GetComponent<Text> ().enabled = true;
					}
					NewTitle.SetActive (true);

			}
		}
		Invoke ("closeAchievements",12);
	}

	List<GameObject> OpenAchievements = new List<GameObject>();


	public void closeAchievements()
	{
		CancelInvoke ("closeAchievements");
		StartCoroutine (CloseAchievement());
	}

	IEnumerator CloseAchievement()
	{
		GetComponent<GridLayoutGroup> ().enabled = false;
		for (float i = 0; i < 1.4f; i += Time.deltaTime) {
			foreach (GameObject ob in OpenAchievements) {
				if (ob) {
					((RectTransform)ob.transform).anchoredPosition3D += (Vector3.right * Time.deltaTime * 600);
				} 
			}
			yield return null;
		}
		foreach (GameObject ob in OpenAchievements) {
			if (ob) {
				Destroy (ob);
			}
		}
	}

}
