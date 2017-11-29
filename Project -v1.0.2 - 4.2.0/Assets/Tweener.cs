using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour {

	void Start()
	{
		GoToPose ("Onstart",5);
	}

	public enum TweenScope
	{
		Local,World,WorldStart,LocalStart
	}
	public List<AnimationPose> myPoses;
	Dictionary<string,Coroutine> currentTweens = new Dictionary<string,Coroutine> ();


	public void GoToPose(string nextPoseName,float tweenTime)
	{
		foreach (AnimationPose pose in myPoses) {

			if (pose.PoseName == nextPoseName) {
				stopTween (nextPoseName);

				Coroutine myCorout = StartCoroutine (Tween(pose, tweenTime));
				currentTweens.Add (nextPoseName, myCorout);
			}	
		}
	}

	public void stopTween(string name)
	{
		if (currentTweens.ContainsKey (name)) {
			StopCoroutine (currentTweens [name]);
		}
		currentTweens.Remove (name);
	}

	IEnumerator Tween(AnimationPose pose, float tweenTime)
	{
		pose.setStartPoses ();
		for (float i = 0; i < tweenTime; i += Time.deltaTime) {
			pose.updateTween (i / tweenTime);
			yield return null;
		}
		pose.updateTween (1);
	}

}



[System.Serializable]
public class AnimationPose
{
	public string PoseName;
	public Tweener.TweenScope myScope;
	public List<ObjectPose> myPoses;

	List<ObjectPose> startPoses = new List<ObjectPose>();

	public void setStartPoses()
	{startPoses.Clear ();
		foreach (ObjectPose pose in myPoses) {
			ObjectPose temp = new ObjectPose (pose.myObject);
			temp.setCurrentVectors (myScope);
			startPoses.Add (temp);
		}
	}


	public void updateTween(float lerpPosition)
	{
		switch(myScope){
		case Tweener.TweenScope.Local:
			for (int i = 0; i < myPoses.Count; i++) {
				myPoses [i].myObject.localScale = Vector3.Lerp (startPoses[i].scale, myPoses[i].scale,lerpPosition);
				myPoses [i].myObject.localRotation = Quaternion.Lerp(Quaternion.Euler(startPoses[i].rotation), Quaternion.Euler( myPoses[i].rotation),lerpPosition);
				myPoses [i].myObject.localPosition = Vector3.Lerp (startPoses[i].position, myPoses[i].position,lerpPosition);
			}

			break;

		case Tweener.TweenScope.LocalStart:
			
			break;

		case Tweener.TweenScope.World:
			for (int i = 0; i < myPoses.Count; i++) {
				myPoses [i].myObject.localScale = Vector3.Lerp (startPoses[i].scale, myPoses[i].scale,lerpPosition);
				myPoses [i].myObject.rotation = Quaternion.Lerp(Quaternion.Euler(startPoses[i].rotation), Quaternion.Euler( myPoses[i].rotation),lerpPosition);
				myPoses [i].myObject.position = Vector3.Lerp (startPoses[i].position, myPoses[i].position,lerpPosition);
			}
			break;

		case Tweener.TweenScope.WorldStart:
			break;
		}
	}

}
[System.Serializable]
public class ObjectPose
{
	public ObjectPose(Transform t)
	{
		myObject = t;
	}

	public void setCurrentVectors(Tweener.TweenScope scope)
	{if (scope == Tweener.TweenScope.Local) {
			scale = myObject.localScale;
			position = myObject.localPosition;
			rotation = myObject.localEulerAngles;
		} else if (scope == Tweener.TweenScope.World) {
			scale = myObject.lossyScale;
			position = myObject.position;
		rotation = myObject.eulerAngles;
		}
	}

	public Transform myObject;
	public Vector3 position;
	public Vector3 rotation;	
	public Vector3 scale = Vector3.one;

}
