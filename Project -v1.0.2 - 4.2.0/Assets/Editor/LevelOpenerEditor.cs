using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class LevelOpenerEditor : EditorWindow
{
	
	[MenuItem("Open Level/Game Intro")]
	public static void A()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/gameCreation.unity");
	}

	[MenuItem("Open Level/Campaign Hub")]
	public static void B()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/CampaignMechanics.unity");
	}

	[MenuItem("Open Level/1 - Snow Globes")]
	public static void C()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/Level1SnowGlobes.unity");
	}

	[MenuItem("Open Level/2 - Building Bases")]
	public static void D()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/L17BaseTutorial.unity");
	}

	[MenuItem("Open Level/3 - Communication Breakdown")]
	public static void E()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/Level2.unity");
	}

	[MenuItem("Open Level/4 - Zypher Training")]
	public static void F()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/Level3ZephyrTraning.unity");
	}

	[MenuItem("Open Level/5 - Lava Land")]
	public static void G()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/Level4LavaLand.unity");
	}

	[MenuItem("Open Level/6 - Bridge Smugglers")]
	public static void H()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/Level5BridgeSmugglers.unity");
	}

	[MenuItem("Open Level/7 - Bunny Land")]
	public static void I()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/Level6BunnyLand.unity");
	}

	[MenuItem("Open Level/8 - Shape Land")]
	public static void J()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/Level7Triangles.unity");
	}

	[MenuItem("Open Level/9 - Desert Defense")]
	public static void K()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/Level8DesertDefense.unity");
	}

	[MenuItem("Open Level/10 - Money Pit")]
	public static void L()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/Level10Money.unity");
	}

	[MenuItem("Open Level/11 - MetaData")]
	public static void M()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/Level11Tron.unity");
	}

	[MenuItem("Open Level/12 - Switcheroo")]
	public static void N()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/Level16AssaultCoalition.unity");
	}

	[MenuItem("Open Level/13 - Night of the Buns")]
	public static void O()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/Level15Coalition.unity");
	}

	[MenuItem("Open Level/14 - Feathers of Freedom")]
	public static void P()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/Level12Freedom.unity");

	}

	[MenuItem("Open Level/15 - Brain of the Bugs")]
	public static void Q()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/Level13Bugs.unity");
	}

	[MenuItem("Open Level/16 - Heritage of the Null")]
	public static void R()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/Level14Null.unity");
	}

	[MenuItem("Open Level/Test Level")]
	public static void S()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/NewTestScene.unity");
	}

	[MenuItem("Open Level/SwapOut Pay Day")]
	public static void T()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/BMMoneyTime.unity");
	}

	[MenuItem("Open Level/SwapOut Locks")]
	public static void U()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/SwapOutLocks.unity");
	}

    [MenuItem("DaMinionz/ Combat Scene")]
    public static void V()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/CarbotStuff/DaMinionsMap.unity");
    }
    [MenuItem("DaMinionz/ Main Menu")]
    public static void W()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/CarbotStuff/DaMinionsMainMenu.unity");
    }
    [MenuItem("DaMinionz/ Aaron Test Level")]
    public static void Z()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/CarbotStuff/DMAaronTestLevel.unity");
    }
}