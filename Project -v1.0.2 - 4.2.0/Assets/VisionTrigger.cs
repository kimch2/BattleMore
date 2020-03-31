using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VisionTrigger : MonoBehaviour {

    public string ZoneName;
    [HideInInspector]
    public int PlayerOwner;

    public List<int> PlayersToLookFor = new List<int>() {1};
    [HideInInspector]
    public List<UnitManager> InVision;
    public bool CheckForDeaths = false;
    public bool AppliesToEnemies;
    public bool AppliesToAllies;
    public bool StacksEffect;


    public abstract void UnitEnterTrigger(UnitManager manager);
    public abstract void UnitExitTrigger(UnitManager manager);
    
    protected static Dictionary<string, List<UnitManager>> Unstackables = new Dictionary<string, List<UnitManager>>();


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
		if (otherManager && PlayersToLookFor.Contains(otherManager.PlayerOwner)) {

            InVision.Add(otherManager);
            if (!StacksEffect)
            {
                List<UnitManager> toReturn = null;
                Unstackables.TryGetValue(ZoneName, out toReturn);
                if (toReturn == null)
                {
                    toReturn = new List<UnitManager>();
                    Unstackables.Add(ZoneName, toReturn);
                    UnitEnterTrigger(otherManager);
                }
                else if (!toReturn.Contains(otherManager))
                {
                    UnitEnterTrigger(otherManager);
                }
                toReturn.Add(otherManager);
            }
            else
            {
                UnitEnterTrigger(otherManager);
            }         
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.isTrigger) {
			return;
		}
		UnitManager otherManager = other.gameObject.GetComponent<UnitManager> ();
        if (otherManager && InVision.Contains(otherManager))
        {
            InVision.Remove(otherManager);
            if (!StacksEffect)
            {
                Unstackables[ZoneName].Remove(otherManager);
                if (!Unstackables[ZoneName].Contains(otherManager))
                {
                    UnitExitTrigger(otherManager);
                }
            }
            else
            {
                UnitExitTrigger(otherManager);
            }
        }
	}
}