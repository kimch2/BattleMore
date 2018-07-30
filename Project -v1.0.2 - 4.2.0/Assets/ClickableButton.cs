using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickableButton : MonoBehaviour, IPointerClickHandler {

	public int abilityNumber;
	private SelectedManager selManager;

	void Start()
	{
		selManager = SelectedManager.main;}


	public void OnPointerClick(PointerEventData eventData)
	{	if (selManager.ActiveObjectList () [0].getUnitManager ().PlayerOwner == 1) {
			if (eventData.button == PointerEventData.InputButton.Right) {
				selManager.setAutoCast (abilityNumber);
				selManager.AutoCastUI ();

			}
		}



	}




}
