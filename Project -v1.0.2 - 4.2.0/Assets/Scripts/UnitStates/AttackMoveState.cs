using UnityEngine;
using System.Collections;

public class AttackMoveState : UnitState {

	public enum MoveType{passive, command, patrol};
	private MoveType commandType;

	private Vector3 home;// to be used if its a passive attack move and patrol;
	private Vector3 endLocation; // end location of an attack move
	private UnitManager enemy;
	private Vector3 target; // currently moving towards

	private bool enemyDead = false;



	private float nextActionTime = 0;
	float chaseRange;

		public AttackMoveState(GameObject obj, Vector3 location, MoveType type,UnitManager man, Vector3 myhome )
		{
		myManager = man;
		chaseRange = myManager.getChaseRange ();

		home = myhome;
		if (obj) {
			enemy = obj.GetComponent<UnitManager> ();
		}
		endLocation = location;
		commandType = type;
		if (enemy != null) {
			target = enemy.transform.position;
		} else {
			target = endLocation;
		}


		nextActionTime = Time.time + .1f;
		//Debug.Log("Just called th reset1" + target + "   "+ enemy);
		if (type == MoveType.passive) {
			// This is breaking stuff so I commented it out
			//target = home;
		} else if (type == MoveType.command) {
			
			home = location;
			enemyDead = true;
		} else {
			enemyDead = true;
		}
		//Debug.Log ("Target is " + obj + " locat " + target);
		}


	public UnitManager getEnemy()
	{
        return enemy;
    }

	public override void initialize()
	{
		if (myManager.cMover) {
            //Debug.Log (myManager.cMover);
           // Debug.Log("Initializing " + target);
			myManager.cMover.resetMoveLocation (target);
		}
	}

	bool EnemyTooClose = false;
	Vector3 lastEnemyLocation;
	int PathingUpdate = 0;

	// Update is called once per frame
	override
	public void Update () {
		
		if (Time.time > nextActionTime) {
			nextActionTime += .2f;

			float distance = 0;

			UnitManager temp =  myManager.findBestEnemy (out distance, enemy);
		
			if (temp) {

				if (distance >chaseRange) {
					enemy = null;
					myManager.cMover.resetMoveLocation (home);

				} else {
					enemy = temp;
					if (PathingUpdate == 0) {
						if (EnemyTooClose) {
							myManager.cMover.resetMoveLocation (myManager.transform.position - (enemy.transform.position - myManager.transform.position).normalized * 10);

						} else {
							if (Vector3.Distance( enemy.transform.position, lastEnemyLocation) > 2) {
								lastEnemyLocation = enemy.transform.position;

								myManager.cMover.resetMoveLocation (temp.transform.position);
							}
						}
						PathingUpdate = 4;
					}


					PathingUpdate--;

				}
			}
		}
		// WE FOUND AN ENEMY, MAYBE
		//still need to figure out calculation for if enemy goes out of range or if a better one comes into range


		if (enemy != null) { // && myManager.myWeapon.Count > 0) {
			enemyDead = false;
			bool attacked = false;

			foreach (IWeapon weap in myManager.myWeapon) {
				if (weap.inRange (enemy)) {

					if (myManager.cMover) {
						myManager.cMover.stop ();
					}
					attacked = true;
					if (weap.simpleCanAttack (enemy)) {
						weap.attack (enemy, myManager);
					} 
				} else {
					
					EnemyTooClose =	!weap.checkMinimumRange (enemy);
				}

			}

			if (!attacked) {

	
				if (myManager.cMover.myspeed == 0) {

					if (EnemyTooClose) {
						
						myManager.cMover.resetMoveLocation ( myManager.transform.position - ( enemy.transform.position - myManager.transform.position ).normalized *10);
					} else {
				
						myManager.cMover.resetMoveLocation (enemy.transform.position);
					}
				}

				myManager.cMover.move ();

			}
		} else {
			if (!enemyDead) {
				if (commandType == MoveType.command) {

					myManager.cMover.resetMoveLocation (endLocation);
				} else if (commandType == MoveType.passive) {//Debug.Log("going home");
					if (myManager.cMover) {
	
						myManager.cMover.resetMoveLocation (home);
					}
				} else {
					if (myManager.cMover) {

						myManager.cMover.resetMoveLocation (target);
					}
				}
				enemyDead = true;
			}

			if (myManager.cMover) {
				//Debug.Log ("MovingB");
				bool there = myManager.cMover.move ();
				// We reached the end, now where do we go?
				if (there && commandType == MoveType.patrol) {
					
					if (target == home) {
					
						target = endLocation;
					} else {
				
						target = home;
					}

					myManager.cMover.resetMoveLocation (target);
				} else if (there) {

					myManager.changeState (new DefaultState ());
				}
			}
		}


	}



	public void setHome(Vector3 input)
		{home = input;}

	override
	public void endState()
	{
	}

	bool hasCalledAide = false;

	override
	public void attackResponse(UnitManager src, float amount)
	{
		if(src && !hasCalledAide && amount > 0){

			if (src.PlayerOwner != myManager.PlayerOwner) {
				hasCalledAide = true;

					foreach (UnitManager ally in myManager.allies) {
						if (ally) {
							UnitState hisState = ally.getState ();
							if (hisState is DefaultState) {
								hisState.attackResponse (src, 0);
							}

						}
					}
			}
		}

	}




}
