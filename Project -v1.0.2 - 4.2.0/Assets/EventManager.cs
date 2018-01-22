using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

	public List<EventState> myStates;

	EventState currentstate;

	public void EnterState(string stateName)
	{
		if (currentstate != null) {
			currentstate.OnExit.Invoke ();
		}
		currentstate = myStates.Find (item => item.StateName == stateName);
		if (currentstate != null) {
			currentstate.OnEnter.Invoke ();
		}
	}

	public void TriggerStateEntry(string stateName)
	{
		myStates.Find (item => item.StateName == stateName).OnEnter.Invoke();
	}




	[System.Serializable]
	public class EventState{
		public string StateName;
		public UnityEngine.Events.UnityEvent OnEnter;
		public UnityEngine.Events.UnityEvent OnExit;

	}
}
