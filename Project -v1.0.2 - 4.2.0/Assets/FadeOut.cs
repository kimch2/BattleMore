using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOut : MonoBehaviour {

	public float blackTime;
	public float fadeLength;
	public Image myImage;
	public Text myText;
	private float fadeStartTime = .75f;

	public float textfadeInTime;

	public static FadeOut main;
	public bool fadeOutEveryTime = true;
	// Use this for initialization
	void Awake () {

		if (fadeOutEveryTime || Time.time < 1) {
	

			main = this;
			fadeStartTime = blackTime;
			myImage.enabled = true;
			myText.enabled = true;
			if (textfadeInTime > 0) {
				StartCoroutine (fadeInText ());
			}
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (Time.timeSinceLevelLoad +"  " +  fadeStartTime);
		if (Time.timeSinceLevelLoad > fadeStartTime) {
			
			myImage.color = new Color (0, 0, 0, 1 - (Time.timeSinceLevelLoad -  fadeStartTime) / fadeLength);
			myText.color = new Color (myText.color.r, myText.color.g, myText.color.b, 1 - (Time.timeSinceLevelLoad -  fadeStartTime +1f) / fadeLength);

			if (Time.timeSinceLevelLoad >fadeStartTime + fadeLength) {
				myImage.color = new Color (0, 0, 0, 0);
				myText.color = new Color (0, 0, 0, 0);

				this.enabled = false;
			}
		} else {
			if (MainCamera.main) {
				MainCamera.main.goToStart ();
			}
		}

	
	}

	IEnumerator fadeInText()
	{	
		for (float i = 0; i < textfadeInTime; i += Time.deltaTime) {
			yield return null;
			myText.color = new Color (myText.color.r, myText.color.g, myText.color.b, (i / textfadeInTime));
		}	
	}


	public void startFade(float length)
	{
		
	}

}
