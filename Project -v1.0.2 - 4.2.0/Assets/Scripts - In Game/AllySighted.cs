using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AllySighted {
	// this interface is used in the Unitmanager to let other scripts know that units have been sighted, in order to save them computation time on OnTriggerEnter
	void AllySpotted (UnitManager manager);
	void allyLeft(UnitManager manage);
}
