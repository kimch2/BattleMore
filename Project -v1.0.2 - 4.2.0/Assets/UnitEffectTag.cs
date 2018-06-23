using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEffectTag : MonoBehaviour {

	public enum EffectType{QuantumEntangle}
	public EffectType myType;

	public GameObject SourceObject;
	public UnitManager SourceUnitmanager;
	public bool randomBool;
	public Vector3 location;


}
