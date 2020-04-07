using UnityEngine;
using System.Collections;

public class TurretDeathTrigger : MonoBehaviour, Modifier{

	public UnitManager mymanager;
	public UnitManager parentManager;
	//private IWeapon weapon;
	// Use this for initialization

	void Awake()
	{
		if (!mymanager)
		{
			mymanager = GetComponent<UnitManager>();
		}
		if (!parentManager)
		{
			parentManager = transform.parent.GetComponentInParent<UnitManager>();
		}
		mymanager.PlayerOwner = parentManager.PlayerOwner;
	}

	void Start () {
		mymanager.myStats.addDeathTrigger (this);
		mymanager.PlayerOwner = parentManager.PlayerOwner;
	}
	


	public void place( )
	{



	}

	public void Dying()
	{
		GameManager.main.playerList[mymanager.PlayerOwner-1].UnitDying (mymanager, null, true);
		if (GetComponent<Selected>().IsSelected) {

			RaceManager.removeUnitSelect(mymanager);
		}
		
		FButtonManager.main.updateTankNumber ();
		
		mymanager.myStats.kill (null);
	}



	// If I die, I need to let my parent tank know that I am gone
	public float modify(float damage, GameObject source, OnHitContainer hitSource, DamageTypes.DamageType theType)
	{ 
		if (transform.GetComponentInParent<TurretMount> ()) {
			transform.GetComponentInParent<TurretMount> ().unPlaceTurret ();
		}


		//mymanager.myStats.kill (source);



		/*
		if (weapon.myManager.myWeapon.Contains( weapon)) {
			foreach (TurretMount turr in transform.GetComponentsInParent<TurretMount> ()) {

				if (turr.turret != null) {

					// What does this do again?
					//weapon.myManager.myWeapon = turr.turret.GetComponent<IWeapon> ();
					return 0 ;
				}

			}

		


			//weapon.myManager.changeState (new DefaultState ());
		}*/
		return 0 ;
	}
}
