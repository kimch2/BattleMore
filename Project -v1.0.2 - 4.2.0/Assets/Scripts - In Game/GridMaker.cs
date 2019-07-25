using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMaker : MonoBehaviour {

	public bool triggered;

	public AudioSource src;

	GameObject child;
	public bool OnUnit;
	public bool NotNuetral;

	void Start()
	{	child = transform.GetComponentInChildren<MeshRenderer>().gameObject;
		UnitManager manager = GetComponent<UnitManager> ();
		if (manager &&  manager.PlayerOwner == 1) {
			NotNuetral = true;
		}
		if (!src) {
			src = GetComponent<AudioSource> ();
		}
	
	}

	public void MakeNotNuetral()
	{
		NotNuetral = true;
	}
	public void Trigger(GameObject source)
	{
		if (triggered) {
			return;
		}

		if (OnUnit) {
			return;}
		
		StartCoroutine (riseUp(source));

	}

	IEnumerator riseUp( GameObject source)
	{
		Vector3 startPosition = child.transform.localPosition;

		while (source && Vector3.Distance (transform.position, source.transform.position) > 56) {
			yield return null;
			yield return null;
		}
		if (!triggered) {

		
			triggered = true;
			for (float i = 0; i < 1; i += Time.deltaTime) {
		
				child.transform.localPosition = Vector3.Lerp (startPosition, Vector3.zero, i);
				yield return null;

		
			}
			child.transform.localPosition = Vector3.zero;
			if (src) {
				src.Play ();

				yield return new WaitForSeconds (1);
			}
			Destroy (this);
		}
	}

	void OnTriggerEnter(Collider col)
	{

		GridMaker gridder = col.GetComponent<GridMaker> ();
		if (gridder && !gridder.OnUnit) {
			if (NotNuetral) {
			
			
				gridder.Trigger (this.gameObject);
			}
		}
	}
}



