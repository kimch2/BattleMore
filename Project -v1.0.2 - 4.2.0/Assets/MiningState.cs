using UnityEngine;
using System.Collections;

public class MiningState : UnitState {

	private  GameObject target;
	
	private float miningTime;

	private float resourceOneAmount;
	private float resourceTwoAmount;

	public float nextMineTime;

	private OreDispenser currentlyMining;
	ParticleSystem myEffect;

	public MiningState(OreDispenser unit, UnitManager man, float mineTime, float resourceOne, float resourceTwo, ParticleSystem effect)
	{
		myManager = man;
		myEffect = effect;
		miningTime = mineTime;
		resourceOneAmount = resourceOne;
		resourceTwoAmount= resourceTwo;
		target = unit.gameObject;

		currentlyMining = target.GetComponent<OreDispenser> ();
		currentlyMining.currentMinor = this.myManager.gameObject;
		nextMineTime = Time.time + mineTime;
	}

	public override void initialize()
	{
		myEffect.Play();
		currentlyMining = target.GetComponent<OreDispenser> ();
		myManager.cMover.resetMoveLocation (target.transform.position);
	}

	// Update is called once per frame
	override
	public void Update () {

		if (Time.time > nextMineTime)
		{
			float haul = currentlyMining.getOre(resourceOneAmount);


			GameManager.main.playerList[myManager.PlayerOwner - 1].collectOneResource(ResourceType.Ore, haul);

			nextMineTime = Time.time + miningTime;
			PopUpMaker.CreateGlobalPopUp("+" + haul, Color.white, myManager.gameObject.transform.position);
			if (myManager.getStateCount() > 0)
			{
				myManager.changeState(new DefaultState());
			}
			if (!target)
			{
				myManager.changeState(new DefaultState());
				((newWorkerInteract)myManager.interactor).findNearestOre();
			}
		}
		


		

	}

	override
	public void endState()
	{currentlyMining.currentMinor = null;
		myEffect.Stop();
	}

}
