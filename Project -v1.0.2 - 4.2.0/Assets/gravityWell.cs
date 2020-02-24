using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gravityWell : MonoBehaviour, Modifier {

    // Used by Street Sweeper to clean up toxic spills
	public List<GameObject> Protecters = new List<GameObject>();
	public float duration;


	int playerOwner = 1;

	void Start()
	{

		Invoke ("End", duration);

	}

	void End()
	{
		foreach (GameObject obj in Protecters) {
			if (obj) {
			
				obj.GetComponent<UnitManager> ().myStats.removeModifier (this);
			}
		
		}
	}

	List<AreaDamage> toCleanUp = new List<AreaDamage>();

	public float modify (float amount, GameObject source, DamageTypes.DamageType theType){
	//	Debug.Log ("Modifying damage " + theType);
		if (theType == DamageTypes.DamageType.Energy) {
			return 0;
		}	
		return amount;
	}

	void OnTriggerEnter(Collider other)
	{
		explosion e = other.GetComponent<explosion> ();
		if (e && e.sourceInt != playerOwner) {
			Destroy (other.gameObject);
			return;
		}
	
		UnitManager manage = other.gameObject.GetComponent<UnitManager> ();
		if (manage && manage.PlayerOwner == playerOwner) {
			other.GetComponent<UnitStats> ().addHighPriModifier (this);
			Protecters.Add (other.gameObject);
		
		
		}

		AreaDamage area = other.GetComponent<AreaDamage> ();
		if (area && area.Owner  != playerOwner) {
			toCleanUp.Add (area);
			if (currentlyShinking == null) {
				currentlyShinking = StartCoroutine (ShrinkPool());
			}
		
		}

	}

	Coroutine currentlyShinking;
	IEnumerator ShrinkPool()
	{
		while (toCleanUp.Count > 0) {

			yield return new WaitForSeconds (.15f);
			toCleanUp.RemoveAll (item => item == null);
			foreach (AreaDamage damage in toCleanUp) {
				damage.transform.root.localScale  *= .87f;
				if (damage.transform.lossyScale.x < 6f) { // 6 is a random number based on the poisonCloud and BigPoisonCloud prefab
					Destroy (damage.transform.root.gameObject);
				}
			}
		
		
		}

		currentlyShinking = null;
		yield return null;

	}

	void OnTriggerExit(Collider other)
	{

		UnitManager manage = other.gameObject.GetComponent<UnitManager> ();
		if (manage && Protecters.Contains(other.gameObject)) {
			manage.myStats.removeModifier (this);
			Protecters.Remove (other.gameObject);
		
		}

		AreaDamage area = other.GetComponent<AreaDamage> ();
		if (area) {
			toCleanUp.Remove (area);
		}
	}



}
