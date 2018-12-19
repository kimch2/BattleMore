using UnityEngine;
using System.Collections;

public class sawDefense : MonoBehaviour {



	public Animator myController;
	public UnitManager myManager;

	public GameObject mySaw;
	bool inAttack = true;
	float attackEndTime;
	public float attackSpeed = 5;
	int attackType;

	public GameObject targetCircle; 
	public GameObject targetSlice;
	Vector3 targetlocation;
	public float turnSpeed;

	public AudioSource myAudio;

	public AudioClip sliceSound;
	public bool slice = true;
	public bool Chop = true;

	private void Start()
	{
		targetlocation = Vector3.right;
		attackEndTime = Time.time + 1.5f; 
	}

	// Update is called once per frame
	void Update () {
		if (inAttack) {
			if (Time.time > attackEndTime) {


				inAttack = false;
			} else {
				mySaw.transform.rotation =
					Quaternion.Slerp (mySaw.transform.rotation, Quaternion.LookRotation (targetlocation), Time.deltaTime* turnSpeed);

			}
		}

		if (!inAttack) {
			if (myManager.enemies.Count > 0) {
		
				UnitManager enem = myManager.findClosestEnemy ();
				if (!enem) {
				
					return;}
				Vector3 tempEnd = enem.transform.position;
				tempEnd.y = mySaw.transform.position.y;
				targetlocation = (tempEnd- mySaw.transform.position).normalized;

				inAttack = true;
				if (attackType == 2 && Chop) {
					attackType = -2;
					myController.Play("Chop");
				}
				else{
					attackType = 2;
					myController.Play("Slice");
					}
				attackEndTime = Time.time + attackSpeed;

				if(showingInd != null) {
					StopCoroutine(showingInd);
				}

				 showingInd = StartCoroutine (showInd ());

			}
		}



	}
	Coroutine showingInd;

	IEnumerator showInd()
	{	yield return new WaitForSeconds (.5f);
		if (attackType == 2) {
			targetSlice.gameObject.SetActive (true);
			if (sliceSound) {
				//Debug.Log ("Playing slice sound");
				myAudio.PlayOneShot (sliceSound);
			}


		} else {
			Vector3 tempLoc = targetlocation * 47 + mySaw.transform.position;
			tempLoc.y -= 8;
			targetCircle.transform.position = tempLoc;
		
			targetCircle.gameObject.SetActive (true);


		}
		yield return new WaitForSeconds (Mathf.Max(.1f, 3.5f));
		targetCircle.gameObject.SetActive (false);

		targetSlice.gameObject.SetActive (false);


	}






}
