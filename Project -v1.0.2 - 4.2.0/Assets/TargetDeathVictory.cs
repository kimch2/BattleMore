using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetDeathVictory : Objective {

	public List<GameObject> targets = new List<GameObject> ();
	public List<VoiceTrigger> VoiceTriggers;
    public bool RevealFogOfWar = true;

	[System.Serializable]
	public struct VoiceTrigger{
		public int numDied;
		public int VoiceLine;
		public UnityEngine.Events.UnityEvent triggerMe;
	}

	string initialDescription;
	int totalTargetCount;
	// Use this for initialization
	new void Start () {
		base.Start ();
		//Debug.Log ("Death" + this.gameObject);
		foreach (GameObject obj in targets) {
			DeathWinTrigger trigger = obj.AddComponent<DeathWinTrigger> ();
			trigger.myTargetDeath = this;
		}
		totalTargetCount = targets.Count;
		initialDescription = description;
		if (targets.Count > 1) {
			description += "  0/" + targets.Count;
		}
	
	}
	


	public override void trigger (int index, float input,GameObject target, bool doIt){

		startObjective ();
	}

	public void startObjective()
	{
		BeginObjective ();

        //	VictoryTrigger.instance.addObjective (this);
        if (RevealFogOfWar) { 
        foreach (GameObject go in targets)
        {
            if (go)
            {
                FogOfWarUnit[] Foggers = go.GetComponents<FogOfWarUnit>();
                if (Foggers.Length == 0)
                {
                    FogOfWarUnit newFogger = go.AddComponent<FogOfWarUnit>();
                    UnitManager manager = go.GetComponent<UnitManager>();
                    if (manager)
                    {
                        manager.AddUnFoggerManual();
                    }
                }
                else
                {
                    foreach (FogOfWarUnit fog in Foggers)
                    {
                        fog.enabled = true;
                    }
                }
            }
        }
		}
	}

	public void IDied(GameObject obj)
	{
		targets.RemoveAll(item => item == null);
		if (targets.Contains (obj)) {
			targets.Remove (obj);

		}
		if (totalTargetCount > 1) {
			int targetsKilled = totalTargetCount - targets.Count;

			foreach (VoiceTrigger trig in VoiceTriggers) {

				if (targetsKilled == trig.numDied) {
					if (trig.VoiceLine != -1) {
						dialogManager.instance.playLine (trig.VoiceLine);
					}
					trig.triggerMe.Invoke ();
					break;
				}
			}

			description = initialDescription + "  " +targetsKilled + "/" + totalTargetCount;
			VictoryTrigger.instance.UpdateObjective (this);
		}

		if (targets.Count == 0) {
			complete ();
			Destroy (this);
		}

	}



}
