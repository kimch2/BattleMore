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
	float badFrameCount;

	public Dropdown resolution;
	public Toggle Windowed;

	static bool IgnoreMe = false;

	void Start() {
		qualityDrop = qualityDropObj.GetComponent<Dropdown>();
		qualityDrop.value = QualitySettings.GetQualityLevel();

		if (qualityDrop.value > 0 && !IgnoreMe && GraphicsButton)
		{
			InvokeRepeating("updateAverage", 4, 2);
		}
		Windowed.isOn = PlayerPrefs.GetInt("Windowed", 0) == 0;
		Screen.fullScreen = !Windowed.isOn;
		resolution.value = PlayerPrefs.GetInt("Resolution", 10);
		setResolution(resolution.value);
	}


	public void updateAverage()
	{
		frameCount++;
		totalFrame += Time.smoothDeltaTime;
		badFrameCount -= .1f;
		if ((1 / Time.smoothDeltaTime < 15 || (1 / (totalFrame / frameCount)) < 35) && !GraphicsButton.activeSelf) {
		//	Debug.Log((1 / Time.smoothDeltaTime) + "  " + (1 / (totalFrame / frameCount)));
			badFrameCount++;
			if (badFrameCount > 4) {
				GraphicsButton.SetActive(true);
				if (qualityDrop.value == 1) {
					CancelInvoke("updateAverage");
				}
			}
		}
	}

	public void Ignore()
	{
		IgnoreMe = true;
		CancelInvoke("updateAverage");
	}

	public void TurnDownGraphhics()
	{
		totalFrame = 0;
		frameCount = 0;
		badFrameCount = 0;
		QualitySettings.SetQualityLevel(qualityDrop.value - 1);

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
		QualitySettings.SetQualityLevel(qualityDrop.value);

	}


	public void setAspectRatio()
	{
		setResolution(resolution.value);
		PlayerPrefs.SetInt("Resolution", resolution.value);
	}

	public void SetWindowed()
	{

	

		PlayerPrefs.SetInt("Windowed", (Windowed.isOn) ? 0 : 1);
		setResolution(resolution.value);
	}

	void setResolution(int i)
	{
		switch (i)
		{
			case 0:
			Screen.SetResolution(1024, 768, !Windowed);
			break;

			case 1:
				Screen.SetResolution(1280, 720, !Windowed);
				break;

			case 2:
				Screen.SetResolution(1280, 768, !Windowed);
				break;

			case 3:
				Screen.SetResolution(1280, 800, !Windowed);
				break;

			case 4:
				Screen.SetResolution(1280, 960, !Windowed);
				break;

			case 5:
				Screen.SetResolution(1280, 1024, !Windowed);
				break;

			case 6:
				Screen.SetResolution(1440, 900, !Windowed);
				break;

			case 7:
				Screen.SetResolution(1600, 1000, !Windowed);
				break;

			case 8:
				Screen.SetResolution(1600, 1024, !Windowed);
				break;

			case 9:
				Screen.SetResolution(1680, 1050, !Windowed);
				break;

			case 10:
				Screen.SetResolution(1920, 1080, !Windowed);
				break;

		}
		Screen.fullScreen = !Windowed.isOn;
	}


}
