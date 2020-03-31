using UnityEngine;
using System.Collections;

public class UndyingArmor :  IEffect, Modifier{


	public GameObject myEffect;
	private UnitStats mystat;
	private float endtime;

	// Update is called once per frame
	void Update () {
		if (onTarget) {

			if (Time.time > endtime) {

				mystat.removeModifier (this);

				Destroy (this);
				return;
			}
		}
	}

    public override void BeginEffect()
    {
        OnTargetManager.myStats.addModifier(this);
    }

    public override void EndEffect()
    {
        OnTargetManager.myStats.removeModifier(this);
        base.EndEffect();
    }


    public override void applyTo (GameObject source, UnitManager target)
	{
        CopyIEffect(target, true, out bool AlreadyOnIt);
	}


	public float modify(float damage, GameObject source, DamageTypes.DamageType theType)
	{
		endtime -= .25f;
		damage = Mathf.Min (damage, mystat.health -1);
		return damage;
	}


	public override bool validTarget(GameObject target)
	{
		return true;
	}


 
}
