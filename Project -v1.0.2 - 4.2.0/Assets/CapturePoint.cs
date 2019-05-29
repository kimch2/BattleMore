using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePoint : VisionTrigger
{

	public SpriteRenderer DecalRenderer;
	public SpriteRenderer MinimapDecal;
	public SpriteRenderer PlayerCircle;

	public ParticleSystem playerCapture;
	public ParticleSystem enemyCapture;

	public float shiftRatePerFifth = .2f;
	public float Balance = 0; // -1 means player has it, 1 means enemy has it
	public int InControl = 0;


	public Color playerColor = Color.blue;
	public Color enemyColor = Color.red;

	Coroutine UnitsAreIn;

	public UnityEngine.Events.UnityEvent OnPlayerCapture;
	public UnityEngine.Events.UnityEvent OnEnemyCapture;




	public override void UnitEnterTrigger(UnitManager manager)
	{
		if (UnitsAreIn == null)
		{
			UnitsAreIn = StartCoroutine(checkForControl());
		}
	}

	public override void UnitExitTrigger(UnitManager manager)
	{

	}


	IEnumerator checkForControl()
	{
		int playerNum = -1;

		while (InVision.Count > 0 || InControl != 0)
		{
			playerNum = -1;

			foreach (UnitManager man in InVision)
			{
				if (playerNum == -1)
				{
					playerNum = man.PlayerOwner;
				}
				if (man.PlayerOwner != playerNum)
				{
					playerNum = -1;
					break;
				}
			}

			if (playerNum != -1 || InControl != 0)
			{
				if (playerNum == 1)
				{
					Balance -= shiftRatePerFifth;
					if (Balance <= -1)
					{
						if (InControl != -1)
						{
							PointCaptured(1);
						}

						InControl = -1;
						Balance = -1;
					}
				}
				else if (playerNum == 2)
				{
					Balance += shiftRatePerFifth;
					if (Balance >= 1)
					{
						if (InControl != 1)
						{
							PointCaptured(2);
						}

						InControl = 1;
						Balance = 1;
					}
				}

				if (Balance == -1)
				{
				}
				else if (Balance == 1)
				{
				}
				else if (Balance < 0)
				{
					DecalRenderer.color = Color.Lerp(Color.white, playerColor, -1 * Balance);
					MinimapDecal.color = DecalRenderer.color;
				}
				else
				{
					DecalRenderer.color = Color.Lerp(Color.white, enemyColor, Balance);
					MinimapDecal.color = DecalRenderer.color;
				}
			}

			yield return new WaitForSeconds(.2f);
			InVision.RemoveAll(item => item == null);
		}
		UnitsAreIn = null;
	}

	public virtual void PointCaptured(int playerNumber)
	{

		if (playerNumber == 1)
		{
			OnPlayerCapture.Invoke();
			PlayerCircle.color = playerColor;
		}
		else
		{
			OnEnemyCapture.Invoke();
			PlayerCircle.color = enemyColor;
		}
	}
	
}
