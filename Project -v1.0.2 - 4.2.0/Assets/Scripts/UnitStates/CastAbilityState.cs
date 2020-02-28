using UnityEngine;
using System.Collections;


public class CastAbilityState  : UnitState {

	public Ability myAbility;



	public CastAbilityState(Ability abil)
	{


		myAbility = abil;



	


	}

	public override void initialize()
	{
	}

	// Update is called once per frame
	override
	public void Update () {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
                                                                                                                                                                                                                                                                

		myAbility.Activate();
        WorldRecharger.main.SpellWasCast(myManager.PlayerOwner,myManager.gameObject);
        myManager.nextState ();


	}

	override
	public void endState()
	{
	}
	/*
	override
	public void attackResponse(UnitManager src, float amount)
	{
	}

*/
}
