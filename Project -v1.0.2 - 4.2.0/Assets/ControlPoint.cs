using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPoint : CapturePoint
{

	public float ConquerReward = 100;
	public int NexusNumber =1 ;
	public float PointsPerSec = .04f;

	private void Start()
	{
		InvokeRepeating("AwardPoints", 5,1);
	}

	

	void AwardPoints()
	{
		if (InControl == -1)
		{
			if (NexusNumber == 1)
			{
				CarbotOverlord.main.TakeFirstNexus(1, PointsPerSec);
			}
			else if (NexusNumber == 2)
			{
				CarbotOverlord.main.TakeSecondNexus(1, PointsPerSec);
			}
			else if (NexusNumber == 3)
			{
				CarbotOverlord.main.TakeThirdNexus(1, PointsPerSec);
			}

			}
		else if (InControl == 1)
		{
			if (NexusNumber == 1)
			{
				CarbotOverlord.main.TakeFirstNexus(2, PointsPerSec);
			}
			else if (NexusNumber == 2)
			{
				CarbotOverlord.main.TakeSecondNexus(2, PointsPerSec);
			}
			else if (NexusNumber == 3)
			{
				CarbotOverlord.main.TakeThirdNexus(2, PointsPerSec);
			}
		}
	}

	public override void PointCaptured(int playerNumber)
	{

		if (NexusNumber == 1)
		{
			CarbotOverlord.main.TakeFirstNexus(playerNumber, ConquerReward);
		}
		else if (NexusNumber == 2)
		{
			CarbotOverlord.main.TakeSecondNexus(playerNumber, ConquerReward);
		}
		else if (NexusNumber == 3)
		{
			CarbotOverlord.main.TakeThirdNexus(playerNumber, ConquerReward);
		}

		if (playerNumber == 1)
		{
			playerCapture.Play();
			PlayerCircle.color = Color.blue;
			capturedMessage(1);
		}
		else
		{
			enemyCapture.Play();
			capturedMessage(2);
			PlayerCircle.color = Color.red;
		}

	}

	void capturedMessage(int playerNumber)
	{
		if (InControl == 0)
		{
			CarbotObjectiveManager.main.PointCaptured(playerNumber);
		}
		else
		{
			CarbotObjectiveManager.main.PointRecaptured(playerNumber);
		}
	}


}
