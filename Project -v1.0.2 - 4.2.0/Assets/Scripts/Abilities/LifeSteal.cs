using UnityEngine;
using System.Collections;

public class LifeSteal : MonoBehaviour, Notify {

	// Use this for initialization
	private UnitStats myStats;
	public float percentage = 50;
    public float MinimumPopUpAmount = 30;

	void Start () {
        OnHitContainer hitContain = gameObject.GetComponent<OnHitContainer>();   
        myStats = hitContain.myManager.myStats;
	}
	

	public float trigger(GameObject source, GameObject projectile,UnitManager target, float damage)
	{
        if (myStats)
        {
            float amount = damage * percentage * .01f;
            myStats.heal(amount);
            if (amount > MinimumPopUpAmount)
            {
                PopUpMaker.CreateGlobalPopUp("+" + (int)(amount), Color.green, myStats.transform.position);
            }
        }
		return damage;
	}


}
