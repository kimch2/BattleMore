using UnityEngine;
using System.Collections;

public abstract class IEffect : MonoBehaviour{

	public bool onTarget;
	public abstract bool canCast();

	public abstract bool validTarget(GameObject target);

	public abstract void apply (GameObject source, GameObject target);



}
