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
		if (!target)
		{
			myManager.changeState(new DefaultState());
			((newWorkerInteract)myManager.interactor).findNearestOre();
			return;
		}

	
		currentlyMining = target.GetComponent<OreDispenser> ();
		myManager.cMover.resetMoveLocation (target.transform.position);

		if (!currentlyMining.MyStructure)
		{
			
			GameObject obj = GameObject.Instantiate<GameObject>(myManager.gameObject.GetComponent<newWorkerInteract>().PurifierPrefab, currentlyMining.transform.position + Vector3.up*.2f, Quaternion.identity, currentlyMining.transform);

			obj.GetComponent<BuildingInteractor>().startSelfConstruction(myManager.gameObject, 10);
			obj.GetComponentInChildren<Animator>().Play("SCExtractorConstruct");
			currentlyMining.MyStructure = obj.GetComponent<UnitManager>();
			myManager.GetComponent<newWorkerInteract>().BuildingEffect.Play();
			building = true;
		}
		else
		{ myEffect.Play();
		}
	}

	// Update is called once per frame
	override
	public void Update () {

		if (Time.time > nextMineTime)
		{
			myManager.myAnim.SetInteger("State", 1);
			if (currentlyMining.MyStructure)
			{


				if (currentlyMining.MyStructure.GetComponent<BuildingInteractor>().ConstructDone())
				{
					if (building)
					{
						myEffect.Play();
						myManager.GetComponent<newWorkerInteract>().BuildingEffect.Stop();
						building = false;
					}

					float haul = currentlyMining.getOre(resourceOneAmount);
					GameManager.main.playerList[myManager.PlayerOwner - 1].collectOneResource(ResourceType.Ore, haul);

					nextMineTime = Time.time + miningTime;
					PopUpMaker.CreateGlobalPopUp("+" + haul, Color.white, myManager.gameObject.transform.position);
					if (myManager.getStateCount() > 0)
					{
						myManager.changeState(new DefaultState());
					}
					if (!currentlyMining || currentlyMining.OreRemaining == 0)
					{
						myManager.changeState(new DefaultState());
						((newWorkerInteract)myManager.interactor).findNearestOre();
					}
				}
				else
				{

				}
			}
			else
			{
				GameObject obj = GameObject.Instantiate<GameObject>(myManager.gameObject.GetComponent<newWorkerInteract>().PurifierPrefab, currentlyMining.transform.position, Quaternion.identity, currentlyMining.transform);
				obj.GetComponent<BuildingInteractor>().startSelfConstruction(myManager.gameObject,10);
				obj.GetComponentInChildren<Animator>().Play("SCExtractorConstruct");
				currentlyMining.MyStructure = obj.GetComponent<UnitManager>();
				myManager.GetComponent<newWorkerInteract>().BuildingEffect.Play();
				building = true;
			}
		}
	}
	bool building = false;

	override
	public void endState()
	{
		//Debug.Log("Disconnecting");
		currentlyMining.currentMinor = null;
		myEffect.Stop();
		myManager.GetComponent<newWorkerInteract>().BuildingEffect.Stop();
	}

}
