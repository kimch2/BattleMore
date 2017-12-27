using UnityEngine;
using System.Collections;

public class SlowDebuff : Behavior, Notify {

	public bool OnTarget = false;
	IMover mover ;

	public float speedDecrease;
	public float duration;
	public bool stackable;
	public float percent;
	public bool applyOnStart = false;
	private float removalTime;

	void Start()
	{
		if (applyOnStart) {
			AddToWeapon ();
		}
	}

	public void AddToWeapon()
	{
		UnitManager manager = GetComponent<UnitManager> ();
		if (manager) {
			foreach (IWeapon weap in manager.myWeapon) {
				weap.addNotifyTrigger (this);
			}
		} 
	}

	public void initialize(float dur, float speed, float percentdec)
	{

		OnTarget = true;
		duration = dur;
		speedDecrease = speed;
		percent = percentdec;

		mover = this.gameObject.GetComponent<UnitManager>().cMover;
		mover.removeSpeedBuff (this);

		mover.changeSpeed (percent, speedDecrease, false, this);

		setBuffStuff (Behavior.buffType.movement, true);
		StopAllCoroutines ();
		StartCoroutine (waitForTime());
			

	}

	IEnumerator waitForTime()
	{
		yield return new WaitForSeconds (duration);
		mover.removeSpeedBuff (this);
	}



	public float trigger(GameObject source,GameObject proj, UnitManager target, float damage)
	{

		if (proj && proj.GetComponent<Projectile>().triggers.Contains(this) && target.cMover) {
			
			SlowDebuff debuff = target.gameObject.GetComponent<SlowDebuff> ();
			if (!debuff) {
				target.gameObject.AddComponent<SlowDebuff> ();
			}
			debuff.initialize (duration, speedDecrease, percent);
		} else if (proj){
			proj.GetComponent<Projectile> ().triggers.Add (this);
		}

		return damage;
	}



}
