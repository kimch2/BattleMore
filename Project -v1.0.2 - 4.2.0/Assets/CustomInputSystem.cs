using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomInputSystem : StandaloneInputModule {



	public bool overUILayer()
	{


		GameObject obj = m_PointerData [kMouseLeftId].pointerEnter;


		//Debug.Log ("Returning " + obj + "  "+ (obj ? obj.layer.ToString() : ""));
		return(obj && obj.layer == 5);

	}


}
