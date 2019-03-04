using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectmanager : MonoBehaviour {


	public static LevelChoice BattleModeChoice;
	public List<Button> levelButtons;
	public Image Background;

	public List<FreePlayLevel> Levels;

	public GameObject StartButton;

	public Text Title;
	public Text Description;

	public SkinColorManager playerSkinScript;
	public List<SkinPreview> playerSkinPreviews;
	public SkinColorManager enemySkinScript;
	public List<SkinPreview> EnemySkinPreviews;

	public Dropdown PlayAsDrop;
	public Dropdown PlayAgainstDrop;

	private void Start()
	{

		MyRace = RaceInfo.raceType.SteelCrest;
		EnemyRace = RaceInfo.raceType.Coalition;
		BattleModeChoice = new LevelChoice();
		LoadLevel(3);
		foreach (SkinPreview prev in playerSkinPreviews)
		{
			prev.InitializePositions();
			prev.parentObject.SetActive(prev.myRace == MyRace);
			
		}
		foreach (SkinPreview prev in EnemySkinPreviews)
		{
			prev.InitializePositions();
			prev.parentObject.SetActive(prev.myRace == EnemyRace);
		}
	}

	public void resetProgress()
	{

	}

	FreePlayLevel currentLevel;
	public RaceInfo.raceType MyRace = RaceInfo.raceType.SteelCrest;
	public RaceInfo.raceType EnemyRace = RaceInfo.raceType.Coalition;

	public void LoadLevel(int i)
	{
		currentLevel = Levels[i];
		Debug.Log("Current Level is " + i + "  " + currentLevel.SceneNumber);
		Title.text = Levels[i].LevelName;
		Description.text = Levels[i].Description;
		StartButton.gameObject.SetActive(true);
		Background.sprite = currentLevel.myBackground;

		resetRaces();
		
	}

	public void resetRaces()
	{
		Debug.Log("Clearing ");
		PlayAsDrop.ClearOptions();
		foreach (RaceInfo.raceType racer in currentLevel.AllowedToPlayAs)
		{
			PlayAsDrop.options.Add(new Dropdown.OptionData(racer.ToString()));
		}
		if (currentLevel.AllowedToPlayAs.Count > 1)
		{
			PlayAsDrop.options.Add(new Dropdown.OptionData("Random"));
		}
		if (!currentLevel.AllowedToPlayAs.Contains(MyRace))
		{
			PlayAsDrop.value = 0;
		}
		else
		{
			PlayAsDrop.value = currentLevel.AllowedToPlayAs.IndexOf(MyRace);
		}
		SetRace(PlayAsDrop);
		PlayAsDrop.RefreshShownValue();

		PlayAgainstDrop.ClearOptions();
		foreach (RaceInfo.raceType racer in currentLevel.AllowedToPlayAgainst)
		{
			PlayAgainstDrop.options.Add(new Dropdown.OptionData(racer.ToString()));
		}
		if (currentLevel.AllowedToPlayAgainst.Count > 1)
		{
			PlayAgainstDrop.options.Add(new Dropdown.OptionData("Random"));
		}
		if (!currentLevel.AllowedToPlayAgainst.Contains(EnemyRace))
		{
			PlayAgainstDrop.value = 0;
		}
		else
		{
			PlayAgainstDrop.value = currentLevel.AllowedToPlayAgainst.IndexOf(EnemyRace);
		}
		PlayAgainstDrop.RefreshShownValue();
		SetEnemy(PlayAgainstDrop);
	}

	public void PlayMission()
	{

		//BattleModeChoice = new LevelChoice();
		BattleModeChoice.PlayingAs = MyRace;
		BattleModeChoice.PlayingAgainst = EnemyRace;
		Debug.Log("Playing   " + currentLevel.SceneNumber);
		MainMenuManager.main.LoadSceneNumber(currentLevel.SceneNumber);
	}

	public void RotatePlayerUnits(bool Right)
	{
		SkinPreview preview = playerSkinPreviews.Find(item => item.myRace == MyRace);
		StartCoroutine(Spin(preview, Right));

	}
	public void RotateEnemyUnits(bool Right)
	{
		SkinPreview preview = EnemySkinPreviews.Find(item => item.myRace == EnemyRace);
		StartCoroutine(Spin(preview, Right));
	}

	IEnumerator Spin(SkinPreview preview, bool right)
	{
	
		float SpinAmount = (360 / preview.skins.Count) * (right? 1 : -1);
		float total = SpinAmount;
		for (float i = 0; i < .333f; i += Time.deltaTime)
		{
			total -= Time.deltaTime * 3 * SpinAmount;
			preview.parentObject.transform.Rotate(new Vector3(0, Time.deltaTime * 3 * SpinAmount, 0));
			if ( right && total < 0 || !right && total > 0)
			{ break; }
			yield return null;
		}
		preview.parentObject.transform.Rotate(new Vector3(0, total, 0));
	}

	public void SetRace(Dropdown d)
	{
		switch (d.options[d.value].text)
		{
			case "SteelCrest":
				MyRace = RaceInfo.raceType.SteelCrest;
			break;
			case "Coalition":
				MyRace = RaceInfo.raceType.Coalition;
				break;
			case "Animals":
				MyRace = RaceInfo.raceType.Animals;
				break;
			case "Random":
				MyRace = currentLevel.AllowedToPlayAs[UnityEngine.Random.Range(0, currentLevel.AllowedToPlayAs.Count)];
				break;
		}
		foreach (SkinPreview prev in playerSkinPreviews)
		{
			prev.parentObject.SetActive(prev.myRace == MyRace);
		}
	}

	public void setPlayerColor(Slider slide)
	{
		BattleModeChoice.PlayerHueShift = slide.value * 360;
		playerSkinScript.resetHue(BattleModeChoice.PlayerHueShift);
		foreach (SkinUnlocker skin in playerSkinPreviews.Find(item=>item.myRace == BattleModeChoice.PlayingAs).skins)
		{
			skin.Start();
		}
	}

	public void setEnemyColor(Slider slide)
	{
		BattleModeChoice.EnemyHueShift = slide.value * 360;
		enemySkinScript.resetHue (BattleModeChoice.EnemyHueShift);
		foreach (SkinUnlocker skin in EnemySkinPreviews.Find(item => item.myRace == BattleModeChoice.PlayingAgainst).skins)
		{
			skin.Start();
		}
	}

	public void setEnemySaturation(Slider slide)
	{
		BattleModeChoice.EnemySaturation = slide.value * 3;
		enemySkinScript.resetSaturation(BattleModeChoice.EnemySaturation);
		foreach (SkinUnlocker skin in EnemySkinPreviews.Find(item => item.myRace == BattleModeChoice.PlayingAgainst).skins)
		{
			skin.Start();
		}
	}

	public void setPlayerSaturation(Slider slide)
	{
		BattleModeChoice.PlayerSaturation = slide.value * 3;
		playerSkinScript.resetSaturation(BattleModeChoice.PlayerSaturation);
		foreach (SkinUnlocker skin in EnemySkinPreviews.Find(item => item.myRace == BattleModeChoice.PlayingAs).skins)
		{
			skin.Start();
		}
	}

	public void SetEnemy(Dropdown d)
	{
		switch (d.options[d.value].text)
		{
			case "SteelCrest":
				EnemyRace = RaceInfo.raceType.SteelCrest;
				break;
			case "Coalition":
				EnemyRace = RaceInfo.raceType.Coalition;
				break;
			case "Animals":
				EnemyRace = RaceInfo.raceType.Animals;
				break;
			case "Random":
				EnemyRace = currentLevel.AllowedToPlayAgainst[UnityEngine.Random.Range(0, currentLevel.AllowedToPlayAgainst.Count)];
				break;
		}
		foreach (SkinPreview prev in EnemySkinPreviews)
		{
			prev.parentObject.SetActive(prev.myRace == EnemyRace);
		}
	}

	[System.Serializable]
	public class SkinPreview
	{
		public GameObject parentObject;
		public RaceInfo.raceType myRace;
		public List<SkinUnlocker> skins;

		public void InitializePositions()
		{
			
			float angle = 360 / skins.Count;
			for (int i = 0; i < skins.Count; i++)
			{
				Vector3 location = new Vector3(0, 0, 0);
				location.x += Mathf.Sin(Mathf.Deg2Rad * angle * i) * -17;
				location.z += Mathf.Cos(Mathf.Deg2Rad * angle * i) * -17;
				skins[i].transform.localPosition = location;
			}
		}
	}

}

public class LevelChoice
{
	public RaceInfo.raceType PlayingAs;
	public float PlayerHueShift = 0;
	public float PlayerSaturation = 1;
	public RaceInfo.raceType PlayingAgainst;
	public float EnemyHueShift = 0;
	public float EnemySaturation = 1;
}


[System.Serializable]
public class FreePlayLevel
{
	public string LevelName;
	public string Description;
	public Sprite icon;
	public int SceneNumber;
	public List<RaceInfo.raceType > AllowedToPlayAs;
	public List<RaceInfo.raceType> AllowedToPlayAgainst;
	public Sprite myBackground;
}

