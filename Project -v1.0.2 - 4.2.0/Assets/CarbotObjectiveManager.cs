using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarbotObjectiveManager : MonoBehaviour
{
	public List<AIObjective> ToTurnOn;

	public List<AIObjective> activeObjectives;
	public List<AIObjective> contestedPoints;

	public static CarbotObjectiveManager main;


	public AudioTub EnemyCaptured;
	public AudioTub EnemyRecaptured;
	public AudioTub PlayerCaptured;
	public AudioTub FirstSpawnSoon;
	public AudioTub TwentySecondWarning;


	private void Awake()
	{
		main = this;
		foreach (AIObjective fg in ToTurnOn)
		{
			if (ToTurnOn.IndexOf(fg) != 0)
			{
				StartCoroutine(giveWarning(TwentySecondWarning, fg.number - 20));
			}
			StartCoroutine(turnOn(fg));
		}
		StartCoroutine(giveWarning(FirstSpawnSoon, .1f));
	}

	IEnumerator giveWarning(AudioTub tub, float delay)
	{
		yield return new WaitForSeconds(delay);
		AudioClip clip = tub.getRandomClip();
		ExpositionDisplayer.instance.displayText("", clip.length, clip, .93f, null, 4);
	}



	IEnumerator turnOn(AIObjective fg)
	{
		fg.point.gameObject.SetActive(false);
		yield return new WaitForSeconds(fg.number);
		fg.point.gameObject.SetActive(true);
		activeObjectives.Add(fg);
	}


	public void PointCaptured(int playerNumber)
	{
		if (playerNumber == 1)
		{
			AudioClip clip = PlayerCaptured.getRandomClip();
			ExpositionDisplayer.instance.displayText("", clip.length, clip, .93f, null, 4);
		}
		else
		{
			AudioClip clip = EnemyCaptured.getRandomClip();
			ExpositionDisplayer.instance.displayText("", clip.length, clip, .93f, null, 4);
		}
	}

	public void PointRecaptured(int playerNumber)
	{
		if (playerNumber == 1)
		{
			AudioClip clip = PlayerCaptured.getRandomClip();
			ExpositionDisplayer.instance.displayText("", clip.length, clip, .93f, null, 4);
		}
		else
		{
			AudioClip clip = EnemyCaptured.getRandomClip();
			ExpositionDisplayer.instance.displayText("", clip.length, clip, .93f, null, 4);
		}
	}
}
[System.Serializable]
public class AIObjective
{
	public float number;
	public ControlPoint point;
		
}



