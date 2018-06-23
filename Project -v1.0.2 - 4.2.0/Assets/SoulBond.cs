using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBond : TargetAbility, Modifier{

	UnitStats myStats;
	UnitStats brother;

	UnitManager manager;
	AugmentAttachPoint AugmentAttach;
	public GameObject AuraThingy;
	public GameObject FlyingAura;
	GameObject currentAura;
	bool currentAuraIsFLying;
	public MultiShotParticle DamageEffect;

	public LineRenderer myLine;


	void Awake()
	{audioSrc = GetComponent<AudioSource> ();
		myType = type.target;
		AugmentAttach = GetComponent<AugmentAttachPoint> ();
		myLine.SetPosition (0, transform.position+ Vector3.up *7);
		myLine.SetPosition (1, transform.position+ Vector3.up *5);
	}


	// Use this for initialization
	void Start () {
		myStats = GetComponent<UnitStats> ();
		manager = GetComponent<UnitManager> ();
	}


	public float modify(float amount, GameObject src, DamageTypes.DamageType theType)
	{
		float reduceBy = 0;
		if (!AugmentAttach.myAugment && brother) {
			if (myStats.health > 100) {


				if (Vector3.Distance (brother.transform.position, transform.position) < 65) {
					reduceBy = amount / 2;
				} else {
					reduceBy = amount / 4;
				}

				myStats.TakeDamage (reduceBy, null, DamageTypes.DamageType.Regular);
			}
		}
		return amount -reduceBy;
	}

	public override bool isValidTarget (GameObject target, Vector3 location){

		if (Vector3.Distance (target.transform.position, transform.position) > 65) {
			return false;
		}

		if (target.GetComponent<UnitStats> ().isUnitType (UnitTypes.UnitTypeTag.Structure)) {
			return false;
		}

		foreach(UnitEffectTag tag in target.GetComponents<UnitEffectTag>())
		{
			if (tag.myType == UnitEffectTag.EffectType.QuantumEntangle) {

				return false;
			}
		}
		return true;
	}


	public void turnOffAutoCast()
	{autocast = false;

	}

	public override void setAutoCast(bool offOn)
	{autocast = offOn;

	}

	void OnTriggerEnter(Collider other)
	{  
		if (other.isTrigger) {
			return;}

		if (brother && other.gameObject == brother.gameObject) {
			if (myLinerMover == null) {
				myLinerMover =	StartCoroutine (MoveLine ());
			}
		}
	}

	Coroutine myLinerMover;

	IEnumerator MoveLine()
	{

		while (brother && Vector3.Distance (brother.transform.position, transform.position) < range) {
			myLine.SetPosition (1, brother.transform.position + Vector3.up *3);

			yield return null;
		}
		myLine.SetPosition (1, transform.position+ Vector3.up *5);
		myLinerMover = null;
	
	
	}



	override
	public continueOrder canActivate (bool showError)
	{continueOrder order = new continueOrder ();

		order.nextUnitCast = false;
		order.canCast = true;

		return order;
	}

	override
	public void Activate()
	{
	}

	override
	public  bool Cast(GameObject targ, Vector3 location)
	{
		if (brother && brother.GetComponent<UnitEffectTag> ()) {
			Destroy(brother.GetComponent<UnitEffectTag> ());
		}
		UnitEffectTag tag = targ.AddComponent<UnitEffectTag> ();
		target.GetComponent<UnitStats> ().addModifier (this);
		tag.myType = UnitEffectTag.EffectType.QuantumEntangle;
		tag.SourceObject = this.gameObject;
		brother = target.GetComponent<UnitStats>();
		myLinerMover =	StartCoroutine (MoveLine ());
		return true;

	}

	override
	public void Cast(){

		if (brother && brother.GetComponent<UnitEffectTag> ()) {
			Destroy(brother.GetComponent<UnitEffectTag> ());
		}

		UnitEffectTag tag = target.AddComponent<UnitEffectTag> ();
		target.GetComponent<UnitStats> ().addModifier (this);
		tag.myType = UnitEffectTag.EffectType.QuantumEntangle;
		tag.SourceObject = this.gameObject;
		brother = target.GetComponent<UnitStats>();
		myLinerMover =	StartCoroutine (MoveLine ());
		if (currentAura) {
			if (target.GetComponent<airmover> () && currentAuraIsFLying || !target.GetComponent<airmover> () && !currentAuraIsFLying) {
				currentAura.transform.SetParent (target.transform);
				currentAura.transform.position = target.transform.position;
				currentAura.transform.rotation = target.transform.rotation;
				currentAura.transform.localScale = (target.GetComponent<UnitManager> ().UnitName == "Triton" ? 1.5f : 1) *Vector3.one;
			} else if (target.GetComponent<airmover> () && !currentAuraIsFLying) {
			
				Destroy (currentAura);
				CreateAura ();
			}
		} else {

			CreateAura ();
		}
	}

	void CreateAura()
	{
		if (target.GetComponent<airmover> ()) {
			currentAuraIsFLying = true;
			currentAura = (GameObject)Instantiate (FlyingAura, target.transform.position, target.transform.rotation, target.transform);

		} else {
			currentAuraIsFLying = false;
			currentAura = (GameObject)Instantiate (AuraThingy, target.transform.position, target.transform.rotation, target.transform);
			currentAura.transform.localScale = (target.GetComponent<UnitManager> ().UnitName == "Triton" ? 1.5f : 1) * Vector3.one;
		}
	}



}
