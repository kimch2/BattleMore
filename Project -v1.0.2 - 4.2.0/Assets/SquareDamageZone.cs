using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareDamageZone : VisionTrigger {

	public DamageTypes.DamageType myType = DamageTypes.DamageType.Regular;
    public OnHitContainer myHitContainer;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("UpdateDamage", .1f, .5f);
        if (!myHitContainer)
        {
            myHitContainer = OnHitContainer.CreateDefaultContainer(this.gameObject, null, "SquareZone");
        }
	}

	// Update is called once per frame
	void UpdateDamage () {

		if (InVision.Count > 0) {

			InVision.RemoveAll (item => item == null);
			foreach (UnitManager s in InVision) {
				s.myStats.TakeDamage (Mathf.Sqrt( s.myStats.Maxhealth), this.gameObject.gameObject.gameObject, myType, myHitContainer);
			}
		}


	}

	public override void UnitEnterTrigger(UnitManager manager)
	{

	}


	public override void UnitExitTrigger(UnitManager manager)
	{

	}



}
