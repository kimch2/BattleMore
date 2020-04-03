using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CinematicCamera : SceneEventTrigger {

	// Use this for initialization

	public List<scene> myScenes = new List<scene> ();
	public bool showPoints;

	public static CinematicCamera main;
	private int currentScene = -1;
	private int currentShot = 0;
	private float shotChangeTime;
	//private float shotStartTime;

	private float nextDialogue;
	private bool hasNextDialogue;
	Vector3 previousCamPos;


	void Start () {
		main = this;

		GetComponent<Camera>().cullingMask &= ~(1 << LayerMask.NameToLayer("MinimapIcon"));
	}

	Vector3 lookPosition;

	float previousGameSpeed;

	// Update is called once per frame
	void Update () {

		if ((Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && currentScene >-1) {
			exitScene ();
		
		}


		if (currentScene > -1) {
			if (Time.time > shotChangeTime) {
				//NEW SHOT

				//shotStartTime = Time.time;
				currentShot++;
				if (currentShot == myScenes [currentScene].myShots.Count) {
					exitScene ();
					return;
				} else {
					
					shotChangeTime = Time.time + myScenes [currentScene].myShots [currentShot].duration* GameSettings.gameSpeed;
					if (myScenes [currentScene].myShots [currentShot].dialogueLength > 0 || myScenes [currentScene].myShots [currentShot].toTrigger.GetPersistentEventCount() > 0) {
						
				
						hasNextDialogue = true;
						nextDialogue = Time.time + myScenes [currentScene].myShots [currentShot].dialogueStartDelay* GameSettings.gameSpeed;
					}
				}
			}

			if (hasNextDialogue && Time.time > nextDialogue) {
				hasNextDialogue = false;
				//Debug.Log ("PLaying  " +myScenes [currentScene].myShots [currentShot].DialogueText );
				if (myScenes [currentScene].myShots [currentShot].dialogueLength > 0) {
					ExpositionDisplayer.instance.displayText (myScenes [currentScene].myShots [currentShot].DialogueText, myScenes [currentScene].myShots [currentShot].dialogueLength
						, myScenes [currentScene].myShots [currentShot].dialogueAudio, .8f, myScenes [currentScene].myShots [currentShot].dialogueImage, 7);
				}
				myScenes [currentScene].myShots [currentShot].toTrigger.Invoke ();
			
			}
			float timeThroughShot = (shotChangeTime - Time.time) / myScenes [currentScene].myShots [currentShot].duration;
		//	Debug.Log ("Shot time is " + timeThroughShot);
			float perc = timeThroughShot;// myScenes [currentScene].myShots [currentShot].myCurve.Evaluate( timeThroughShot);

	
				this.transform.position = Vector3.Lerp
				(myScenes [currentScene].myShots [currentShot].startLocation, myScenes [currentScene].myShots [currentShot].endLocation, 1 - perc);

				this.transform.LookAt (Vector3.Lerp
				(myScenes [currentScene].myShots [currentShot].startTarget, myScenes [currentScene].myShots [currentShot].endTarget, 1 - perc));
			
		
		}

	
	}
	public void playScene(int sceneNumber)
	{
		trigger (sceneNumber, 0, null, true);
	}

	public override void trigger (int index, float input, GameObject target, bool doIt){

		previousGameSpeed = Time.timeScale;
		Time.timeScale = previousGameSpeed;
		previousCamPos = MainCamera.main.gameObject.transform.position;
		MainCamera.main.GetComponent<Camera> ().enabled = false;

	GetComponent<Camera> ().enabled = true;
		currentScene = index;
		shotChangeTime = Time.time + myScenes [currentScene].myShots [currentShot].duration* GameSettings.gameSpeed;
		if (myScenes [currentScene].myShots [currentShot].dialogueLength > 0 || myScenes [currentScene].myShots [currentShot].toTrigger.GetPersistentEventCount() > 0) {
			hasNextDialogue = true;
			nextDialogue = Time.time + myScenes [currentScene].myShots [currentShot].dialogueStartDelay * GameSettings.gameSpeed;
		}

		updateStun (true);
		UISetter.main.startFade (1, false);
		if(myScenes[currentScene].myShots.Count > 1){
			//lookPosition = myScenes [currentScene].myShots [1] - myScenes [currentScene].myShots [0];
		}

		for (int i = 1; i < myScenes [currentScene].myShots.Count; i++) {
			if (myScenes [currentScene].myShots [i - 1].endLocation == myScenes [currentScene].myShots [i].startLocation) {
				//myScenes [currentScene].myShots [i - 1].isCurveMiddle = true;
			}
		}

		if (GameMenu.main) {
			GameMenu.main.disableInput ();}

	}

	public void exitScene(){
		if (currentScene > -1) {
			Time.timeScale = previousGameSpeed;
			foreach (SceneEventTrigger trig in myScenes[currentScene].nextTrig) {

				trig.trigger (0, 0, null, false);
			}
			myScenes [currentScene].onComplete.Invoke ();
		
			StartCoroutine (TweenFromScene (myScenes [currentScene].tweenFromScene,myScenes [currentScene].TweenToPosition));
			updateStun (false);
			currentScene = -1;
	
			MainCamera.main.gameObject.transform.position = previousCamPos;
			currentShot = 0;

			UISetter.main.startFade (1, true);
		}
	}

	public void updateStun(bool active)
	{
		foreach (int n in myScenes[currentScene].playersToStun) {
			foreach (KeyValuePair<string, List<UnitManager>> pairs in  GameManager.main.playerList[n-1].getFastUnitList()) {
				foreach (UnitManager manage in pairs.Value) {
                    if (manage) {
                        //Debug.Log ("Stunning " + manage.gameObject +"  " + active);
                        if (active)
                        {
                            manage.metaStatus.Stun(manage, this, true);
                        }
                        else
                        {
                            manage.metaStatus.UnStun(this);
                        }

                    }
				}
			}
		}

	}

	IEnumerator TweenFromScene(float duration ,Vector3 toPosition)
	{
		Camera cam = GetComponent<Camera> ();
		float startAngle = cam.fieldOfView;
		Vector3 startPosition = transform.position;
		Quaternion startRotation = transform.rotation;
		if (toPosition != Vector3.zero) {
			float y = previousCamPos.y;
			previousCamPos = toPosition;
			previousCamPos.y = y;
		}


		yield return null;
		for (float i = 0; i < duration; i += Time.deltaTime) {
			this.transform.position = Vector3.Lerp(startPosition, previousCamPos, i/duration);
			this.transform.rotation = Quaternion.Slerp (startRotation, MainCamera.main.gameObject.transform.rotation, i / duration);
			cam.fieldOfView = startAngle * (1 - i / duration) + MainCamera.main.GetComponent<Camera> ().fieldOfView * (i / duration);
			MainCamera.main.gameObject.transform.position = previousCamPos ;
			yield return null;
		}
		MainCamera.main.GetComponent<Camera> ().enabled = true;
		GetComponent<Camera> ().enabled = false;
		MainCamera.main.GetComponent<Camera> ().fieldOfView = startAngle;
		if (GameMenu.main) {
			GameMenu.main.EnableInput();}
	}

	public void simpleShake (float duration)
	{
		ShakeCamera (duration,10,.08f);
	}

	public void ShakeCamera(float Duration, float Intensity, float amplitude)
	{
		StartCoroutine(CameraShake(Duration, Intensity, amplitude));
	}

	IEnumerator CameraShake(float duration, float intensity, float amplitude)
	{

		float elapsed = 0.0f;
		Vector3 totalMovement = Vector3.zero;

		while (elapsed < duration) {



			float MiniShake = 0;

			Vector3 toMove = new Vector3(Random.value  - .5f, Random.value  - .5f,Random.value - .5f) * intensity;


			while(MiniShake < amplitude)
			{MiniShake += Time.deltaTime;
				transform.Translate (toMove * Time.deltaTime);
				totalMovement += toMove * Time.deltaTime;
				yield return null;
			}
			elapsed += MiniShake;

		}

		float ReturnTime = 0.0f;
		while (ReturnTime< .1f) {
			ReturnTime += Time.deltaTime;
			transform.Translate ((totalMovement * -1) * Time.deltaTime/ .1f);
			yield return null;
		}
	}

	[System.Serializable]
	public struct scene{
		public List<shot> myShots;
		public List<SceneEventTrigger> nextTrig;
		public UnityEngine.Events.UnityEvent onComplete;
		public float tweenFromScene;
		[Tooltip("Leave as vector 3 for no tween back")]
		public Vector3 TweenToPosition;
		public List<int> playersToStun;

	}
	[System.Serializable]
	public class shot{
		public Vector3 startLocation;
		public Vector3 startTarget;
		public Vector3 endLocation;
		public Vector3 endTarget;
		public float duration;
		public AnimationCurve myCurve;
		public bool isCurveMiddle;

		public Sprite dialogueImage;
		public float dialogueStartDelay;
		public float dialogueLength;
		public AudioClip dialogueAudio;
		[TextArea(1,10)]
		public string DialogueText;
		[Tooltip("This will trigger on the same time delay as the text")]
		public UnityEngine.Events.UnityEvent toTrigger;
	

	}



	public void OnDrawGizmos()
	{if (showPoints) {

			foreach (scene s in myScenes) {
				foreach(shot curr in s.myShots)
				{Gizmos.color = Color.blue;
					Gizmos.DrawLine (curr.startLocation,(curr.startTarget));
					Gizmos.DrawSphere (curr.startLocation, 3);
					Gizmos.color = Color.red;
					Gizmos.DrawSphere (curr.endLocation, 2);
					Gizmos.DrawLine (curr.endLocation, curr.endTarget);

				
				}
				Gizmos.color = Color.cyan;
				if (s.TweenToPosition != Vector3.zero) {
					Gizmos.DrawSphere (s.TweenToPosition, 3);
				}
			
			}

		
		}
	}





}
