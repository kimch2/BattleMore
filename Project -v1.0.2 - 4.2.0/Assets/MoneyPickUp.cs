using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MoneyPickUp : MonoBehaviour {

	public float resOne;
	public float resTwo;
	public ResourceManager ToPickUp;

	public AudioClip sound;

	public void OnTriggerEnter(Collider other)
	{
		UnitManager man = other.GetComponent<UnitManager> ();
		if (man) {
			if (man.PlayerOwner == 1) {
				GameManager gm = GameObject.FindObjectOfType<GameManager> ();
				gm.activePlayer.collectResources(ToPickUp.MyResources, false);

				if (sound) {
					ExpositionDisplayer.instance.displayText ("", 1, sound, .5f, null, 0);
				}
				ToPickUp.showPopups(transform.position, true);

				Destroy (this.gameObject);
			
			}
		
		}
	}

}
