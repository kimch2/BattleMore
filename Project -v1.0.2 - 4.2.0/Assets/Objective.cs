﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Objective : SceneEventTrigger {
	[TextArea(2,10)]
	public string description;
	public int reward;
	public bool ActiveOnStart;
	public bool bonus;
	public bool completed;
	public Objective nextObjective;
	public bool UltimateObjective;
	protected bool started;

	public UnityEngine.Events.UnityEvent OnStart;
	public UnityEngine.Events.UnityEvent OnComplete;

	public List<SceneEventTrigger> myEvents = new List<SceneEventTrigger>();
	// Use this for initialization
	public void Start () {
		if (ActiveOnStart) {
            BeginObjective();
		}	
	}

	public virtual void BeginObjective()
	{
		if (!started)
		{
			started = true;
			VictoryTrigger.instance.addObjective(this);
			OnStart.Invoke();
		}
	}
	

	public override void trigger (int index, float input, GameObject target, bool doIt){
		BeginObjective ();
	}

	public void complete()
	{
		if (!completed) {
			completed = true;

            if (!started)
            {
                BeginObjective();
            }

			foreach (SceneEventTrigger trig in myEvents) {
				if (trig) {
					trig.trigger (0, 0,  null, false);
				}
			}

			if (nextObjective) {
				nextObjective.trigger (0, 0,  null, false);
			}
			VictoryTrigger.instance.CompleteObject (this);
	
			OnComplete.Invoke ();

		}
	}

	public void unComplete()
	{completed = false;
		VictoryTrigger.instance.unComplete (this);
	}

	public void fail()
	{
		VictoryTrigger.instance.FailObjective (this);

	}



}
