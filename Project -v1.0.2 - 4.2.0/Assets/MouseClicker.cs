using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClicker : MonoBehaviour {

	[TextArea(3,10)]
	public string description;
	public UnityEngine.Events.UnityEvent OnHover;
	public UnityEngine.Events.UnityEvent OnClick;

	public UnityEngine.Events.UnityEvent OnExit;

	public void executeOnHover()
	{
		OnHover.Invoke ();
	}

	public void executeOnClick()
	{
		OnClick.Invoke ();
	}

	public void executeOnExit()
	{
		OnExit.Invoke ();
	}
}
