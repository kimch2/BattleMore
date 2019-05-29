using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectmanager : MonoBehaviour {


	public static LevelChoice BattleModeChoice;
	public static LevelChoice reservedChoice;// This is used by the replay button so that the same race will be loaded
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

	public Text factionDescription;

	GameObject RaceInfoPacket;

    public Slider PlayerColor;
    public Slider EnemyColor;

    public Image EnemyRandom;
    public Image PlayerRandom;
    public Image DescriptionIcon;
	private void Awake()
	{
		RaceInfoPacket = Resources.Load<GameObject>("RaceInfoPacket");
	}


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
		//Debug.Log("Current Level is " + i + "  " + currentLevel.SceneNumber);
		Title.text = Levels[i].LevelName;
		Description.text = Levels[i].Description;
		StartButton.gameObject.SetActive(true);
		Background.sprite = currentLevel.myBackground;

		//resetRaces();
		
	}


	public void setDescription(string d)
	{
		UnitEquivalance RacePacket = RaceInfoPacket.GetComponent<UnitEquivalance>();

		switch (d)
		{
			case "SteelCrest":
				factionDescription.text = "The Republic of Steelcrest\n\n"+ RacePacket.getRace(RaceInfo.raceType.SteelCrest).RaceDescription;
                DescriptionIcon.sprite = RacePacket.getRace(RaceInfo.raceType.SteelCrest).factionIcon;
                break;
			case "Coalition":
				factionDescription.text = "The Coalition\n\n" + RacePacket.getRace(RaceInfo.raceType.Coalition).RaceDescription;
                DescriptionIcon.sprite = RacePacket.getRace(RaceInfo.raceType.Coalition).factionIcon;
                break;
			case "Animals":
				factionDescription.text = "Animals\n\n" + RacePacket.getRace(RaceInfo.raceType.Animals).RaceDescription;
                DescriptionIcon.sprite = RacePacket.getRace(RaceInfo.raceType.Animals).factionIcon;
                break;
			case "Random":
                factionDescription.text = "Random \n\n A powerful concoction of probability and sheer luck, this faction could be anything and nothing at the same time.";
                DescriptionIcon.sprite = EnemyRandom.sprite;
                break;
		}


	}

	public void resetRaces()
	{
        // Later on make it so you can onl play as certain races in some levels
		SetRace("SteelCrest");
		SetEnemy("Coalition");
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

	public void SetRace(string s)
	{
		switch (s)
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
		setDescription(s);
		if (MyRace == EnemyRace && (Mathf.Abs(BattleModeChoice.PlayerHueShift - BattleModeChoice.EnemyHueShift) < 40 || Mathf.Abs(BattleModeChoice.PlayerHueShift - BattleModeChoice.EnemyHueShift)  >  320))
		{
            EnemyColor.value = ((BattleModeChoice.PlayerHueShift + 90) % 360) / 360;
		}
       PlayerRandom.gameObject.SetActive(s == "Random");
        
        
		foreach (SkinPreview prev in playerSkinPreviews)
		{
			prev.parentObject.SetActive(prev.myRace == MyRace && (s != "Random"));
		}
	}

	public void setPlayerColor(Slider slide)
	{
		setPlayerColor(slide.value);
	}

	public void setPlayerColor(float value)
	{
		BattleModeChoice.PlayerHueShift = value * 360;
		playerSkinScript.resetHue(BattleModeChoice.PlayerHueShift);
		foreach (SkinUnlocker skin in playerSkinPreviews.Find(item => item.myRace == BattleModeChoice.PlayingAs).skins)
		{
			skin.Start();
		}
	}

	public void setEnemyColor(Slider slide)
	{
		setEnemyColor(slide.value);
	}

	public void setEnemyColor(float value)
	{
		BattleModeChoice.EnemyHueShift = value * 360;
		enemySkinScript.resetHue(BattleModeChoice.EnemyHueShift);
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

	void clearBattleChoice()
	{
		LevelSelectmanager.BattleModeChoice = null;
		LevelSelectmanager.reservedChoice = null;
	}

	public void SetEnemy(string d)
	{
		switch (d)
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
		setDescription(d);

		if (MyRace == EnemyRace && (Mathf.Abs(BattleModeChoice.PlayerHueShift - BattleModeChoice.EnemyHueShift) < 40 || Mathf.Abs(BattleModeChoice.PlayerHueShift - BattleModeChoice.EnemyHueShift) > 320))
		{
            EnemyColor.value = ((BattleModeChoice.PlayerHueShift + 90) % 360) / 360;
		}
        EnemyRandom.gameObject.SetActive(d == "Random");


        foreach (SkinPreview prev in EnemySkinPreviews)
		{
            prev.parentObject.SetActive(prev.myRace == EnemyRace && (d != "Random"));

		}
	}

	[System.Serializable]
	public class SkinPreview
	{
		public GameObject parentObject;
		public RaceInfo.raceType myRace;
		public List<SkinUnlocker> skins;
        public Sprite factionIcon;

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

