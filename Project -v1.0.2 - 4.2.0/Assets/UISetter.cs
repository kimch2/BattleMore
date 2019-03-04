using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetter : MonoBehaviour {


	public Button CommandMinimize;
	public Button LeftMinimize;
	public Button RightMinimize;
	public GameObject ResourceBar;

	public List<Image> minimapPics;
	public Text LevelTitle;

	public CanvasGroup canGroup;
	public static UISetter main;

	public List<Image> UltImages;
	public List<Text> UltHelps;

	public List<ImageGroup> ImageGroups;

	public GameObject EnemyArsenalButton;

	public Text SupplyText;
	public Text BuildMoreSupplyText;

	public Sprite defaultMinimapcircle;
	public Sprite defaultMinimapSquare;
	void Awake()
	{ main = this;

	}
	// Use this for initialization
	void Start() {

		int LevelNum = GameObject.FindObjectOfType<VictoryTrigger>().levelNumber;
		RaceSwapper swapper = GameObject.FindObjectOfType<RaceSwapper>();
		LevelCompilation comp = ((GameObject)Resources.Load("LevelEditor")).GetComponent<LevelCompilation>();


			if (!comp.MyLevels[LevelNum].UIBarsNUlts.CommandsOpen)
			{
				CommandMinimize.onClick.Invoke();
			}

			if (!comp.MyLevels[LevelNum].UIBarsNUlts.LeftBarOpen)
			{
				LeftMinimize.onClick.Invoke();
			}

			if (!comp.MyLevels[LevelNum].UIBarsNUlts.RightBarOpen)
			{
				RightMinimize.onClick.Invoke();
			}

			if (!comp.MyLevels[LevelNum].UIBarsNUlts.resourcesOpen)
			{
				ResourceBar.SetActive(false);
			}

			RaceManager racer = GameObject.FindObjectOfType<GameManager>().playerList[0];

		if (swapper == null)
		{
			racer.resourceManager.MyResources.Clear();

			UnitEquivalance RacePacket = Resources.Load<GameObject>("RaceInfoPacket").GetComponent<UnitEquivalance>();
			RaceInfo myType = RacePacket.getRace(racer.myRace);// RaceInfo.raceType.SteelCrest);

			foreach (ResourceTank tank in myType.ResourceTypes.MyResources)
			{
				racer.AddResourceType(tank.resType, 0);
			}

			racer.collectResources(comp.MyLevels[LevelNum].StartingResources.MyResources, false);
			bool allTech = PlayerPrefs.GetInt("AllTech") == 0;

			int NumOfUlts = 4;
			if (!comp.MyLevels[LevelNum].UIBarsNUlts.UltOneActivated && allTech)
			{
				racer.UltOne.enabled = false;
				racer.ultBOne.gameObject.SetActive(false);
				NumOfUlts--;
			}

			if (!comp.MyLevels[LevelNum].UIBarsNUlts.UltTwoActivated && allTech)
			{
				racer.UltTwo.enabled = false;
				racer.ultBTwo.gameObject.SetActive(false);
				NumOfUlts--;
			}

			if (!comp.MyLevels[LevelNum].UIBarsNUlts.UltThreeActivated && allTech)
			{
				racer.UltThree.enabled = false;
				racer.ultBThree.gameObject.SetActive(false);
				NumOfUlts--;
			}

			if (!comp.MyLevels[LevelNum].UIBarsNUlts.UltFourActivated && allTech)
			{
				racer.UltFour.enabled = false;
				racer.ultBFour.gameObject.SetActive(false);
				NumOfUlts--;
			}

			if (NumOfUlts == 0)
			{
				racer.ultBFour.gameObject.transform.parent.gameObject.SetActive(false);
			}
		}
		
		foreach (Image im in minimapPics) {
			im.sprite = comp.MyLevels[LevelNum].MinimapPic;
		}


		LevelTitle.text = comp.MyLevels[LevelNum].LevelName;
		startFade(1, true);

		if (comp.MyLevels[LevelNum].ArsenalDisplayTime >= 0)
		{ Invoke("turnOnArsenal", comp.MyLevels[LevelNum].ArsenalDisplayTime); }


	
		if (swapper)
		{
			for (int i = 0; i < swapper.Ulty.myUltimates.Count; i++)
			{
				UltImages[i].sprite = swapper.Ulty.myUltimates[i].iconPic;
				UltHelps[i].text = swapper.Ulty.myUltimates[i].Descripton;
			}
		}
	}


	void turnOnArsenal()
	{
		if (EnemyArsenalButton) {
			EnemyArsenalButton.SetActive(true);
			//EnemyArsenalButton.GetComponent<Tweener> ().playTransition ("Pulse");
		}
	}

	Coroutine fading;

	public void startFade(float duration, bool active)
	{
		if (fading != null)
		{ StopCoroutine(fading);
		}
		fading = StartCoroutine(fadeUI(duration, active));
	}

	IEnumerator fadeUI(float duration, bool active)
	{

		if (active) {
			for (float i = 0; i < duration; i += Time.deltaTime) {
				canGroup.alpha = i / duration;
				yield return null;
			}
			canGroup.alpha = 1;
		} else {
			for (float i = 0; i < duration; i += Time.deltaTime) {
				canGroup.alpha = 1 - (i / duration);
				yield return null;
			}
			canGroup.alpha = 0;
		}
	}

	public void SwapRaceHuds(RaceInfo.raceType newType)
	{
		if (!gameObject)
		{
			return;
		}
		UnitEquivalance RacePacket = Resources.Load<GameObject>("RaceInfoPacket").GetComponent<UnitEquivalance>();
		RaceInfo myType = RacePacket.getRace(RaceInfo.raceType.SteelCrest);
		RaceInfo newRace = RacePacket.getRace(newType);

		if (RaceSwapper.main)
		{
			RaceManager racer = GameObject.FindObjectOfType<GameManager>().playerList[0];
			racer.resourceManager.MyResources.Clear();
			Debug.Log("Race is " + newRace);
			foreach (ResourceTank tank in newRace.ResourceTypes.MyResources)
			{
				racer.AddResourceType(tank.resType, 100);
			}
		}
		SwapImage(myType.myUIImages.UltBackGround, newRace.myUIImages.UltBackGround);
		SwapImage(myType.myUIImages.FButtonbackgrounds, newRace.myUIImages.FButtonbackgrounds);
		SwapImage(myType.myUIImages.BottomLeftPanel, newRace.myUIImages.BottomLeftPanel);
		SwapImage(myType.myUIImages.BottomCenterPanel, newRace.myUIImages.BottomCenterPanel);
		SwapImage(myType.myUIImages.MinimapBackground, newRace.myUIImages.MinimapBackground);
		SwapImage(myType.myUIImages.FTwelveImage, newRace.myUIImages.FTwelveImage);
		SwapImage(myType.myUIImages.SupplyImage, newRace.myUIImages.SupplyImage);
		SwapImage(myType.myUIImages.ControlGroup, newRace.myUIImages.ControlGroup);
		SwapImage(myType.myUIImages.BasicCommands, newRace.myUIImages.BasicCommands);
		SwapImage(myType.myUIImages.PageTab, newRace.myUIImages.PageTab);
		SwapImage(myType.myUIImages.TinyButton, newRace.myUIImages.TinyButton);
		SwapImage(myType.myUIImages.GrayButton, newRace.myUIImages.GrayButton);
		SwapImage(myType.myUIImages.BlackButton, newRace.myUIImages.BlackButton);
		SwapImage(myType.myUIImages.SmallButton, newRace.myUIImages.SmallButton);
		SwapImage(myType.myUIImages.Panel, newRace.myUIImages.Panel);
		SwapImage(myType.myUIImages.SquareButton, newRace.myUIImages.SquareButton);
		SwapImage(myType.myUIImages.Plus, newRace.myUIImages.Plus);
		SwapImage(myType.myUIImages.Minus, newRace.myUIImages.Minus);
		SwapImage(myType.myUIImages.BorderlessButton, newRace.myUIImages.BorderlessButton);
		foreach (GuiControls guis in gameObject.GetComponentsInChildren<GuiControls>())
		{
			guis.MaxSprite = newRace.myUIImages.Plus;
			guis.minSprite = newRace.myUIImages.Minus;
		}

		SupplyText.text = "Population:   Current / Max\nBuild "+ newRace.myUIImages.supplyUnit + "s to increase the Total,   Max 100";
		BuildMoreSupplyText.text = "Not Enough Supply.\nBuild More " + newRace.myUIImages.supplyUnit + "s";
	}

	void SwapImage(Sprite sprite, Sprite newSprite)
	{
		ImageGroup g = ImageGroups.Find(item => item.currentSprite == sprite);
		if (g != null && newSprite != null)
		{
			foreach (Image m in g.myImages)
			{
				if (m != null)
				{
					m.sprite = newSprite;
				}
			}
		}
	}

	public void SortImages()
	{
		GameObject obj = GameObject.FindObjectOfType<UiAbilityManager>().gameObject;

		foreach (Image im in obj.GetComponentsInChildren<Image>(true))
		{
			ImageGroup findIt = ImageGroups.Find(item => item.currentSprite == im.sprite);
			if (findIt != null)
			{
				findIt.myImages.Add(im);
			}
			else
			{
				ImageGroup group = new ImageGroup();
				group.currentSprite = im.sprite;
				group.myImages = new List<Image>();
				group.myImages.Add(im);
				ImageGroups.Add(group);
			}

		}

	}

}

[System.Serializable]
public class ImageGroup
{
	public Sprite currentSprite;
	public List<Image> myImages;

}
