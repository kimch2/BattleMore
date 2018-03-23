using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BridgeActivator : VisionTrigger {

	public GameObject Bridge;

	public GameObject DeathZone;
	public AudioClip soundEffect;
	AudioSource source;
	public MultiShotParticle myEffect;

	void Start()
	{
		source = this.gameObject.AddComponent<AudioSource> ();
		source.playOnAwake = false;
		source.loop = false;
		Bridge.SetActive (false);
		DeathZone.SetActive (true);
		StartCoroutine (DeathRescan ());
	}

	public override void UnitExitTrigger(UnitManager manager)
	{
		if (InVision.Count == 0) {
			Bridge.SetActive (false);
			DeathZone.SetActive (true);
			StartCoroutine (DeathRescan ());
			source.PlayOneShot (soundEffect,.76f);
			myEffect.playEffect ();
		}
	}

	public override void UnitEnterTrigger(UnitManager manager)
	{
		Bridge.SetActive (true);
		DeathZone.SetActive (false);
		StartCoroutine (DeathRescan ());
		if (InVision.Count == 1) {
			source.PlayOneShot (soundEffect,.76f);
			myEffect.playEffect ();
		}
	}



	IEnumerator DeathRescan()
	{	
		GraphUpdateObject b =new GraphUpdateObject( new Bounds(transform.position,Vector3.one * 75)); 
		yield return new WaitForSeconds (.05f);

		AstarPath.active.UpdateGraphs (b);

	}
}
