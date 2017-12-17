using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public UnityEngine.Events.UnityEvent OnHover;
	public UnityEngine.Events.UnityEvent OnExit;

	public void executeOnHover()
	{
		OnHover.Invoke ();
	}



	public void executeOnExit()
	{
		
		OnExit.Invoke ();
	}


	public void OnPointerEnter(PointerEventData eventd)
	{
		executeOnHover ();
		//toolbox.gameObject.GetComponentInChildren<Text> ().text = helpText;
	}

	public void OnPointerExit(PointerEventData eventd)
	{
		executeOnExit ();
	}


	void OnDisable()
	{//Debug.Log ("Executing " +this.gameObject);
		executeOnExit ();	
	}


}
