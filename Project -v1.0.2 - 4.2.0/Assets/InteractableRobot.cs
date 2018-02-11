using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableRobot : MonoBehaviour {

	[Tooltip("Priority is used to shwo when the line is played.")]
	public List<DialogLine> myLines;
	public AudioSource mySrc;
	public List<GameObject> glower;

	public UnityEngine.UI.Text myText;
	DialogLine currentLine;
	bool FlashingOn = true;

	public Animator myAnim;
	void Start()
	{

		Debug.Log ("Hihiest is " + LevelData.getHighestLevel ());
		currentLine = myLines.Find (item => item.Priority == LevelData.getHighestLevel ());
		if (currentLine!= null) {

			StartCoroutine (blink());
		} else {
		
			this.gameObject.SetActive (false);
		}


	}

	bool mouseOn = false;
	public void MouseOver()
	{
		foreach (GameObject obj in glower) {
			obj.GetComponent<MeshRenderer> ().materials [0].color = new Color (0,1,.75f,.6f);
		}
		mouseOn = true;
	}

	public void MouseOff()
	{
		mouseOn = false;
	}

	public void interact()
	{
		if (currentLine != null) {
			
			StartCoroutine (scrollingText(currentLine.MainLine.myText,currentLine.MainLine.myClip));
			StartCoroutine (endText (currentLine.MainLine.duration));
			Invoke ("flyAway", currentLine.MainLine.duration);
			currentLine = null;
			FlashingOn = false;
			turnOffFlashing ();
			myAnim.CrossFadeInFixedTime("SimonSHiver",.6f);

		}
	}

	void flyAway()
	{
		myAnim.CrossFadeInFixedTime ("SimonFly",1);	
	}

	IEnumerator endText(float duration)
	{
		yield return new WaitForSeconds (duration );
		myText.transform.parent.gameObject.SetActive (false);


	
	}

	IEnumerator blink()
	{
		while (FlashingOn) {
			if (!mouseOn) {
				
				for (float i = 0; i < .5f; i += Time.deltaTime) {
					if (!mouseOn) {
						foreach (GameObject obj in glower) {
							obj.GetComponent<MeshRenderer> ().materials [0].color = new Color (0, .75f, .75f, i);
						}
					}
					yield return null;
				}
				for (float i = .5f; i > 0; i -= Time.deltaTime) {
					if (!mouseOn) {
						foreach (GameObject obj in glower) {
							obj.GetComponent<MeshRenderer> ().materials [0].color = new Color (0, .75f, .75f, i);
						}
					}
					yield return null;
				}
			} else {
				yield return null;
			}
		}
		
	}

	public void turnOffFlashing()
	{
		foreach (GameObject obj in glower) {
			obj.SetActive (false);
		}
	}



	public IEnumerator scrollingText(string dialog, AudioClip clip)
	{
		yield return new WaitForSeconds (.5f);
		StartCoroutine (endText (clip.length + 2));

		mySrc.PlayOneShot (clip);
		myText.transform.parent.gameObject.SetActive (true);
		int i = 0;
		while (i <dialog.Length) {
			i++;

			myText.text = dialog.Substring(0,i);

			yield return new WaitForSeconds (.028f);
		}


	}

}
