using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyBuffUpgrade : Upgrade {

	public MonoBehaviour toCopy;
	public List<string> unitNames;

	override
	public void applyUpgrade (GameObject obj){

		UnitManager man = obj.GetComponent<UnitManager> ();
		if (unitNames.Contains (man.UnitName)) {

				System.Type type = toCopy.GetType ();
				Component copy = obj.AddComponent (type);
				// Copied fields can be restricted with BindingFlags
				System.Reflection.FieldInfo[] fields = type.GetFields (); 
				foreach (System.Reflection.FieldInfo field in fields) {
					field.SetValue (copy, field.GetValue (toCopy));
				}
				((MonoBehaviour)copy).enabled = true;

		}
	}

	public override void unApplyUpgrade (GameObject obj){

	}


}
