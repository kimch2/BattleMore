using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MassCapture : SceneEventTrigger {

	public List<CapturableUnit> captures = new List<CapturableUnit>();



	public override void trigger (int index, float input,GameObject target, bool doIt){

				foreach (CapturableUnit u in captures) {
					if (u != null) {
						u.capture ();
					}
				}
			

	}

}
