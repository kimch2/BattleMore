﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UltTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {


	public bool Ability;

	AbstractCost myUltCost;
	public Text cooldown;

	CanvasGroup render;
	Coroutine myFade;

	[Tooltip("Should be between 1 and 4")]
	public int UltNumber;

	public Canvas toolbox;

	Coroutine updater;

	public void OnPointerEnter(PointerEventData eventd)
	{
		if (myFade != null) {
			StopCoroutine (myFade);
		}
		myFade= StartCoroutine (toggleWindow( true));

		updater = StartCoroutine (updateCooldown());
		//toolbox.enabled = true;
		//toolbox.gameObject.GetComponentInChildren<Text> ().text = helpText;
	}

	public void OnPointerExit(PointerEventData eventd)
	{
		//toolbox.enabled = false;
		if (myFade != null) {
			StopCoroutine (myFade);
		}
		myFade =  StartCoroutine (toggleWindow( false));

		StopCoroutine (updater);
	}

	IEnumerator updateCooldown()
	{

        while (true)
        {

            if (myUltCost.cooldownTimer == 0)
            {
                cooldown.text = "" + Clock.convertToString(Mathf.Max(0, myUltCost.cooldown));
            }
            else
            {
                cooldown.text = "" + Clock.convertToString(Mathf.Max(0, (int) myUltCost.cooldownTimer));
            }
        
            
			yield return new WaitForSeconds(1);
		}

	}


	public void turnOff()
	{
		toolbox.enabled = false;
	}


	// Use this for initialization
	void Start()
	{
		Invoke("DelayedStart", .1f);
	}
		
	void DelayedStart () {
		if (toolbox == null) {
			toolbox = GameObject.Find ("ToolTipBox").GetComponent<Canvas> ();
		}

		switch(UltNumber){
		case 1:
                if(GameManager.main.playerList[0].UltOne)
			myUltCost = GameManager.main.playerList [0].UltOne.myCost;
			break;
		case 2:
                if(GameManager.main.playerList[0].UltTwo)
			myUltCost = GameManager.main.playerList [0].UltTwo.myCost;
			break;
		case 3:
                if (GameManager.main.playerList[0].UltThree)
			myUltCost = GameManager.main.playerList [0].UltThree.myCost;
			break;
		case 4:
                if(GameManager.main.playerList[0].UltFour)
			myUltCost = GameManager.main.playerList [0].UltFour.myCost;
			break;

		
		}

		render = toolbox.GetComponent<CanvasGroup> ();
		if (!render) {
			render = toolbox.gameObject.AddComponent<CanvasGroup> ();
		}
	}





	IEnumerator toggleWindow(  bool onOrOff)
	{


		if (onOrOff) {
			float startalpha = render.alpha /.15f;	
			toolbox.enabled = (onOrOff);
			for (float i = startalpha; i < .15f; i += Time.deltaTime) {

				render.alpha  = (i/.15f);
				yield return null;
			}
			render.alpha  = 1;
		} 

		else {

			for (float i = .3f ; i > 0; i -= Time.deltaTime) {

				render.alpha = (i/.3f);
				yield return null;
			}

			render.alpha  = 0;
			toolbox.enabled = (onOrOff);
		}

	}

}
