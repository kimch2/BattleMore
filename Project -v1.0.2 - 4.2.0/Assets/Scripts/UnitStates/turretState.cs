using UnityEngine;
using System.Collections;

public class turretState : UnitState {


	private UnitManager enemy;

	public turretState(UnitManager man)
	{
		myManager = man;

	

	}

	public override void initialize()
	{
	}

	override
	public void endState()
	{
	}

	// Update is called once per frame
	override
	public void Update () {
		
		if (myManager.myWeapon.Count ==0) {
			return;}

		if (myManager.enemies.Count > 0) {
			enemy = findBestEnemy ();

		}
	
		if (!enemy) {

			return;
		}
	
		IWeapon myWeap = myManager.canAttack (enemy);
		if (myWeap) {
				
					//myManager.gameObject.transform.LookAt(enemy.transform.position);
			myWeap.attack (enemy,myManager);

				}
			


	}


    float NextAttackResponse;

	override
	public void attackResponse(UnitManager src, float amount)
	{
   

            if (src)
            {
                //UnitManager manage = src.GetComponent<UnitManager> ();

                if (src.PlayerOwner != myManager.PlayerOwner)
                {
                // Inform other alleis to also attack
                     if (amount > 0)
                     {
                        if (Time.time < NextAttackResponse)
                            {
                             return;
                             }
                        else
                            {
                            NextAttackResponse = Time.time + 20;
                             }
                        foreach (UnitManager ally in myManager.allies)
                        {
                            if (ally)
                            {
                                if (myManager.gameObject != ally)
                                {
                                    UnitState hisState = ally.getState();
                                    if (hisState is DefaultState)
                                    {
                                        //Debug.Log ("Callign to" + ally.gameObject);
                                        hisState.attackResponse(src, 0);
                                    }
                                }
                            }
                        }
                    }


                }
            }
    }


	bool erase = false;
	public UnitManager findBestEnemy()
	{
		UnitManager best = null;

		float bestPriority = -1;

		for (int i = 0; i < myManager.enemies.Count; i ++) {
			if (myManager.enemies [i] != null) {

				//float currDistance = Vector3.Distance(myManager.enemies[i].transform.position, this.myManager.gameObject.transform.position);

				IWeapon myWeap = myManager.isValidTarget (myManager.enemies [i]);
				if (!myWeap) {

					continue;
				}
				if (!myWeap.inRange (myManager.enemies [i])) {

					continue;
				}

				if (myManager.enemies [i].myStats.attackPriority < bestPriority) {

					continue;
				} else {
					best = myManager.enemies [i];

					bestPriority = myManager.enemies [i].myStats.attackPriority;
				}

			} else {
				erase = true;
			}
		}
		if (erase) {
			myManager.enemies.RemoveAll(item => item == null);
			erase = false;
		}

		return best;
	}




}
