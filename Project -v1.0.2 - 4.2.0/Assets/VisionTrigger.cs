using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VisionTrigger : MonoBehaviour {

    [Tooltip("The player number that we are looking for")]
    public int PlayerNumber;
    public List<int> AdditionaPlayerNums;
    public List<UnitManager> InVision;
    public abstract void UnitEnterTrigger(UnitManager manager);
    public abstract void UnitExitTrigger(UnitManager manager);
    public bool CheckForDeaths = false;
    public bool AppliesToEnemies;
    public bool AppliesToAllies;

    public void SetOwner(int i)
    {
        Debug.Log("Setting owner");
    }

	void OnEnable()
	{
		if (CheckForDeaths) {
			InvokeRepeating ("CheckNull", .5f, .5f);
		}
	}

	void CheckNull(){
		
		if (InVision.RemoveAll (item => item == null) > 0) {
			UnitExitTrigger (null);
		}
	}

	void OnTriggerEnter(Collider other) {

		if (other.isTrigger) {
			return;
		}

		UnitManager otherManager = other.gameObject.GetComponent<UnitManager> ();
		if (otherManager && (otherManager.PlayerOwner.Equals (PlayerNumber) || AdditionaPlayerNums.Contains(otherManager.PlayerOwner))) {

			InVision.Add (otherManager);
			UnitEnterTrigger (otherManager);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.isTrigger) {
			return;
		}
		UnitManager otherManager = other.gameObject.GetComponent<UnitManager> ();
		if (otherManager && InVision.Contains(otherManager)) {

			InVision.Remove(otherManager);
			UnitExitTrigger (otherManager);
		}
	}


}
