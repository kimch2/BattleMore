using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapIconer : MonoBehaviour, Modifier {

	public Sprite myIcon;

	[Tooltip("Update period for changing minimap position, set to 0 so it doesn't update at all")]
	public float updateRate;
	public bool Pulsing = true;
	List<GameObject> myIcons = new List<GameObject>();
	bool alreadyAdded = false;
	// Use this for initialization
	IEnumerator Start () {

		yield return new WaitForSeconds (1);
		try{
		GetComponent<UnitStats> ().addDeathTrigger (this);
		}catch{
		}
		OnEnable ();
	}

	void OnDisable()
	{
		foreach (GameObject obj in myIcons) {
			if (GameManager.main) {
				foreach (MiniMapUIController mini in GameManager.main.MiniMaps) {
					if (obj) {
						//Debug.Log ("I died");
						if (mini) {
							mini.deleteUnitIcon (obj);
						}
					}
				}
			}
		}
	}

	void OnEnable()
	{
		StartCoroutine (waitASec ());
	}

	IEnumerator waitASec()
	{
		if (!alreadyAdded) {
			//Debug.Log ("adding from " + this.gameObject);
			yield return null;
			foreach (MiniMapUIController mini in GameManager.main.MiniMaps) {

				if (mini) {
					//	Debug.Log ("Adding MiniIcon");
					myIcons.Add (mini.showUnitIcon (this.transform.position, myIcon, Pulsing));
				}
				alreadyAdded = true;
			}

			if (updateRate > 0) {
				InvokeRepeating ("updatePosition", updateRate, updateRate);
			}
		}
	}



	public float modify ( float amount,GameObject deathSource, OnHitContainer hitSource, DamageTypes.DamageType typ)
	{
		OnDisable ();

		return amount;
	}

	void updatePosition()
	{
		//Debug.Log ("Updating Position");
		foreach (GameObject obj in myIcons) {
			foreach (MiniMapUIController mini in GameManager.main.MiniMaps) {
				if (obj && mini && mini.enabled) {
					mini.updateUnitPos(obj,this.transform.position);
				}
			}
		}

	}


}
