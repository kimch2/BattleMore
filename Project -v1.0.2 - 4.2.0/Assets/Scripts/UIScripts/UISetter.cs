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
	public List<UltimatePointer> UltimateUIs;

	public List<ImageGroup> ImageGroups;

	public GameObject EnemyArsenalButton;

	public Text SupplyText;
	public Text BuildMoreSupplyText;

	public Sprite defaultMinimapcircle;
	public Sprite defaultMinimapSquare;
	void Awake()
	{
		main = this;
	}

    public static UISetter getInstance()
    {
        if (main == null)
        {
            main = GameObject.FindObjectOfType<UISetter>();
        }
        return main;
    }



	RaceSwapper swapper;
	// Use this for initialization
	void Start() {

		int LevelNum = GameObject.FindObjectOfType<VictoryTrigger>().levelNumber;
		swapper = GameObject.FindObjectOfType<RaceSwapper>();
		LevelCompilation comp = ((GameObject)Resources.Load("LevelEditor")).GetComponent<LevelCompilation>();


			if (!comp.MyLevels[LevelNum].UIBarsNUlts.CommandsOpen && CommandMinimize)
			{
				CommandMinimize.onClick.Invoke();
			}

			if (!comp.MyLevels[LevelNum].UIBarsNUlts.LeftBarOpen && LeftMinimize)
			{
				LeftMinimize.onClick.Invoke();
			}

			if (!comp.MyLevels[LevelNum].UIBarsNUlts.RightBarOpen && RightMinimize)
			{
				RightMinimize.onClick.Invoke();
			}

			if (!comp.MyLevels[LevelNum].UIBarsNUlts.resourcesOpen && ResourceBar)
			{
				ResourceBar.SetActive(false);
			}

			RaceManager racer = GameObject.FindObjectOfType<GameManager>().playerList[0];

		if (swapper == null)
		{

			bool allTech = PlayerPrefs.GetInt("AllTech", 0) == 1;
			//Debug.Log("All Tech " + allTech);

			EnableUlt(0, allTech || comp.MyLevels[LevelNum].UIBarsNUlts.UltOneActivated);
			EnableUlt(1, allTech || comp.MyLevels[LevelNum].UIBarsNUlts.UltTwoActivated);
			EnableUlt(2, allTech || comp.MyLevels[LevelNum].UIBarsNUlts.UltThreeActivated);
			EnableUlt(3, allTech || comp.MyLevels[LevelNum].UIBarsNUlts.UltFourActivated);
		}
		else
		{
			for (int i = 0; i < swapper.Ulty.myUltimates.Count; i++)
			{
				initializeUlt(swapper.Ulty.myUltimates[i], i);
			}
		}

		foreach (Image im in minimapPics) {
			im.sprite = comp.MyLevels[LevelNum].MinimapPic;
		}


		LevelTitle.text = comp.MyLevels[LevelNum].LevelName;
		startFade(1, true);

		if (comp.MyLevels[LevelNum].ArsenalDisplayTime >= 0)
		{
            Invoke("turnOnArsenal", comp.MyLevels[LevelNum].ArsenalDisplayTime);
        }
	}

	public void initializeUlt(Ability abil, int index)
	{
		UltimateUIs[index].myAbility = abil;
		UltimateUIs[index].ultBOne.image.sprite = abil.iconPic;
		UltimateUIs[index].HelpText.text = abil.Descripton;
		UltimateUIs[index].OneCharge.gameObject.SetActive(abil == null ? false : abil.chargeCount != -1);
	}

	public void EnableUlt(int i, bool onOff)
	{
		UltimateUIs[i].ultBOne.gameObject.SetActive(onOff);

		foreach (UltimatePointer pointer in UltimateUIs)
		{
			if (pointer.ultBOne.gameObject.activeSelf)
			{
				pointer.ultBOne.gameObject.transform.parent.gameObject.SetActive(true);
				return;
			}
		}
		UltimateUIs[i].ultBOne.gameObject.transform.parent.gameObject.SetActive(false);
	}

	public void updateUlt(int ultNumber)
	{
		UltimateUIs[ultNumber].slideOne.value = UltimateUIs[ultNumber].myAbility.myCost.cooldownProgress();
		UltimateUIs[ultNumber].slideOne.gameObject.SetActive((UltimateUIs[ultNumber].slideOne.value != 1));
		UltimateUIs[ultNumber].ultBOne.interactable = (UltimateUIs[ultNumber].slideOne.value == 1 || UltimateUIs[ultNumber].myAbility.chargeCount >0);

		if (UltimateUIs[ultNumber].myAbility.chargeCount != -1)
		{
			UltimateUIs[ultNumber].OneCharge.text = UltimateUIs[ultNumber].myAbility.chargeCount + "";
		}
	}
	
	void turnOnArsenal()
	{
		if (EnemyArsenalButton) {
			EnemyArsenalButton.SetActive(true);
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
      //  InstructionHelperManager.getInstance().addBUtton("UI Setting A", 25, null);
        if (!gameObject)
		{
			return;
		}
		UnitEquivalance RacePacket = Resources.Load<GameObject>("RaceInfoPacket").GetComponent<UnitEquivalance>();
		RaceInfo myType = RacePacket.getRace(RaceInfo.raceType.SteelCrest);
		RaceInfo newRace = RacePacket.getRace(newType);

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
       // InstructionHelperManager.getInstance().addBUtton("UI Setting C", 25, null);
        foreach (GuiControls guis in gameObject.GetComponentsInChildren<GuiControls>())
		{
			guis.MaxSprite = newRace.myUIImages.Plus;
			guis.minSprite = newRace.myUIImages.Minus;
		}

		SupplyText.text = "Population:   Current / Max\nBuild "+ newRace.myUIImages.supplyUnit + "s to increase the Total,   Max 100";
		BuildMoreSupplyText.text = "Not Enough Supply.\nBuild More " + newRace.myUIImages.supplyUnit + "s";
     //   InstructionHelperManager.getInstance().addBUtton("UI Setting D", 25, null);
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


[System.Serializable]
public class UltimatePointer
{
	public Ability myAbility;
	public Slider slideOne;
	public Button ultBOne;
	public Text OneCharge;
	public Text HelpText;
	public Text Cooldown;
}