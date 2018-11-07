using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GraphicsMenu : MonoBehaviour {

	public GameObject qualityDropObj;
	private Dropdown qualityDrop;
	private bool shadows = true; 
	// Use this for initialization

	public GameObject GraphicsButton;
	public GameObject GraphicsWarningPanel;
	float totalFrame;
	int frameCount;
	int badFrameCount;

	static bool IgnoreMe = false;

	void Start () {
		qualityDrop = qualityDropObj.GetComponent<Dropdown> ();
		qualityDrop.value = QualitySettings.GetQualityLevel ();

		if (qualityDrop.value > 0 && !IgnoreMe && GraphicsButton) {
			InvokeRepeating ("updateAverage", 2, 2);
		}
	
	}


	public void updateAverage()
	{
		frameCount++;
		totalFrame += Time.smoothDeltaTime;

		if ((1 / Time.smoothDeltaTime < 15 || (1/ (totalFrame/frameCount)) < 35 ) && !GraphicsButton.activeSelf) {
			Debug.Log ((1 / Time.smoothDeltaTime ) + "  " + (1/ (totalFrame/frameCount)));
			badFrameCount++;
			if(badFrameCount >4){
				GraphicsButton.SetActive (true);
				if (qualityDrop.value == 1) {
					CancelInvoke ("updateAverage");
				}
			}
		}
	}

	public void Ignore()
	{
		IgnoreMe = true;
		CancelInvoke ("updateAverage");
	}

	public void TurnDownGraphhics()
	{
		totalFrame = 0;
		frameCount = 0;
		badFrameCount = 0;
		QualitySettings.SetQualityLevel (qualityDrop.value-1);

	}

	public void toggleShadows()
	{ shadows = !shadows;
		
		foreach (Light light in GameObject.FindObjectsOfType<Light>()) {
			if (shadows) {
				light.shadows = LightShadows.Soft;

			} else {
				light.shadows = LightShadows.None;
			}
		}

		}


	public void resetQuality()
		{
		QualitySettings.SetQualityLevel (qualityDrop.value);
		
	}
		


}
