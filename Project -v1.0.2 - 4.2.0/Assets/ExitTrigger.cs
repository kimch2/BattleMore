using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : VisionTrigger {

	public UnityEngine.Events.UnityEvent OnEnter;

	public override void UnitEnterTrigger(UnitManager manager)
	{
	}

	public override void UnitExitTrigger(UnitManager manager)
	{
		InVision.RemoveAll (item => item == null);
		if (InVision.Count == 0) {
			OnEnter.Invoke ();
		}
	}

}
