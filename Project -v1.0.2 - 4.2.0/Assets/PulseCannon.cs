using UnityEngine;
using System.Collections;
using DigitalRuby.SoundManagerNamespace;
public class PulseCannon : IWeapon {

	//Like the Iweapon but this fires at every person around it 

	private float nextTime;
	public int maxPulses = 10;




	
	// Update is called once per frame
	void Update () {

		if (Time.time > nextTime) {
			


			myManager.enemies.RemoveAll (item => item == null);
			int i = 0;
			foreach (UnitManager obj in myManager.enemies) {
				StartCoroutine( AttackWave ((i * .08f ),obj.gameObject));
				i++;
				if(i >= maxPulses)
				{break;}
			}
			nextTime = Time.time + attackPeriod/3 + i *.08f;

		}


	
	}

	IEnumerator AttackWave (float time, GameObject target)
	{
		myManager.animAttack ();
		yield return new WaitForSeconds(time);

		if (target) {

			GameObject proj = null;
			if (projectile != null) {
	
				proj = createBullet ();// (GameObject)Instantiate (projectile,this.transform.position +  firePoints[originIndex].position, Quaternion.identity);
				originIndex++;
				if (originIndex == firePoints.Count) {
					originIndex = 0;}


				Projectile script = proj.GetComponent<Projectile> ();
				if (script) {
					script.Initialize (target.GetComponent<UnitManager>(), baseDamage, myManager, myHitContainer);
				} else {

					proj.SendMessage ("setSource", this.gameObject, SendMessageOptions.DontRequireReceiver);
					proj.SendMessage ("setTarget", target, SendMessageOptions.DontRequireReceiver);
					proj.SendMessage ("setDamage", baseDamage, SendMessageOptions.DontRequireReceiver);
				}



				//script.target = target.GetComponent<UnitManager>();
				//script.Source = this.gameObject;

			} else {

				//OnAttacking();
				baseDamage = target.GetComponent<UnitStats> ().TakeDamage (baseDamage, this.gameObject, DamageTypes.DamageType.Regular);
				myManager.myStats.veteranDamage (baseDamage);

			}
			if (target == null) {
				myManager.cleanEnemy ();
			}
			if (attackSoundEffect && audioSrc) {
				//Debug.Log ("Playing Sound effect");
				audioSrc.pitch = ((float)Random.Range (7, 12) / 10);
				SoundManager.PlayOneShotSound(audioSrc, attackSoundEffect);

			}

		}

	
	}

}
