using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class bunnyManager : Objective {

	public Text bunnyCount;
	public int currAmount;
	public int maxAmount;

	public Image myPanel;
	public Slider mySlide;

	Color green = new Color (0, 100, 0);
	Color red = new Color (120, 0, 0);
	Color yellow = new Color (100, 100, 0);

	bool flash;

	Coroutine flashing;

	public int highestAmountSoFar;
	public static bunnyManager main;

	List<bunnyPopulate> bunnyList = new List<bunnyPopulate>();
	int lastBunnyCount;

	public List<voiceLineRandomizer> lineTriggers;

	private void Awake()
	{
		main = this;
	}



	[System.Serializable]
	public class voiceLineRandomizer{
		public int triggerNumber;
		public List<int> voiceLineOptions;
		private float timeSinceLastPlayed = 0;
		int lastOneUsed = -1;


		public void playVoiceLine() {
			if (Time.time - timeSinceLastPlayed > 30) {
				timeSinceLastPlayed = Time.time;
				int rand = Random.Range (0, voiceLineOptions.Count - 1);
				if(voiceLineOptions.Count > 1) {
					while (lastOneUsed == rand) {
						rand = Random.Range (0, voiceLineOptions.Count - 1);
					}
				} 
				lastOneUsed = rand;
				dialogManager.instance.playLine (voiceLineOptions[rand]); //play that one here
			}
		}
	}

	public void addBunny(bunnyPopulate dude)
	{
		bunnyList.Add(dude);
	}

	public void BunnyDead(bunnyPopulate dude)
	{
		bunnyList.Remove(dude);
	}

	private void Update()
	{
		if (bunnyList.Count != currAmount)
		{

			mySlide.value = (float)bunnyList.Count / (float)maxAmount;

			if (bunnyList.Count > maxAmount * .85)
			{
				flash = true;
				if (flashing == null)
				{
					flashing = StartCoroutine(flashColor());
				}
				myPanel.color = red;
			}
			else if (bunnyList.Count > maxAmount * .70)
			{
				flash = true;
				myPanel.color = red;
			}
			else if (bunnyList.Count > maxAmount * .5)
			{
				flash = false;
				myPanel.color = yellow;
			}
			else if (bunnyList.Count > maxAmount * .3)
			{
				flash = false;
				myPanel.color = Color.white;
			}
			else
			{
				flash = false;
				myPanel.color = green;
			}

			if (bunnyList.Count > highestAmountSoFar)
			{
				highestAmountSoFar = bunnyList.Count;
			}

			if (highestAmountSoFar < currAmount)
			{
				foreach (voiceLineRandomizer v in lineTriggers)
				{
					if (currAmount == v.triggerNumber)
					{
						v.playVoiceLine();
						break;
					}
				}
			}

			bunnyCount.text = "White Bunnies Left \n" + bunnyList.Count + " / " + maxAmount;

			if (bunnyList.Count >= maxAmount)
			{
				VictoryTrigger.instance.Lose();
			}
			else if (bunnyList.Count <= 0)
			{
				VictoryTrigger.instance.Win();
			}
			currAmount = bunnyList.Count;
		}
	}


	IEnumerator flashColor()
	{yield return new WaitForSeconds (.5f);
		while (flash) {
			yield return new WaitForSeconds (.65f);
			myPanel.color = red;

			yield return new WaitForSeconds (.65f);
			myPanel.color = Color.white;
		}
		flashing = null;

	}
}
