using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Tweener : MonoBehaviour {

	public int startingStateIndex;
	public List<TweenState> MyStates;
	TweenState currenState;
	public float defaultTweenTime = .85f;
	void Start()
	{
		//GoToPose (myPoses[0].PoseName, 5);
		if (MyStates.Count > 0) {
			currenState = MyStates [0];
		}
	}

	public enum TweenScope
	{
		Local,World,WorldStart,LocalStart
	}
	[Tooltip("Everything in this list will be created into a new KeyFrame Animation Pose when the button below is created.")]

	public List<Transform> quickList;
	[HideInInspector]
	public List<AnimationPose> myPoses  = new List<AnimationPose>();
	[HideInInspector]
	public List<transition> myTransitions;
	Dictionary<string,Coroutine> currentTweens = new Dictionary<string,Coroutine> ();

	public void TriggerTweenState(string input)
	{
		if (currenState != null) {
			TweenPath toMove = currenState.myPaths.Find (itemB => itemB.Input == input);
			string nextStateName = toMove.nextState;
			currenState = MyStates.Find (ii => ii.StateName == nextStateName);
			GoToPose (nextStateName, toMove.transitionTime);
		} else {
			GoToPose(input,defaultTweenTime);
		}
		// MyStates.Find(item => item == nextStateName);



	}

	public void goStraightToState(string stateName)
	{
		foreach (TweenState stat in MyStates) {
			if (stat.StateName == stateName) {
				currenState = stat;
				GoToPose (stateName);
			}
		}
	}

	public void GoToPose(string nextPoseName)
	{//Debug.Log ("Going to pose " + nextPoseName + "  " + this.gameObject + "  " + currentTweens.Count);

		GoToPose (nextPoseName,defaultTweenTime);
	}

	public void GoToPose(string nextPoseName,float tweenTime =1)
	{//Debug.Log ("Going to pose " + nextPoseName + "  " + this.gameObject + "  " +tweenTime);
		foreach (AnimationPose pose in myPoses) {
			
			if (pose.PoseName == nextPoseName) {

				ShiftToPose (pose,tweenTime);
			}	
		}
	}

	void ShiftToPose(AnimationPose pose, float tweenTime)
	{
		StopAllTweens ();
		Coroutine myCorout = null;
		if (this.gameObject.activeInHierarchy) {
			myCorout = StartCoroutine (Tween (pose, tweenTime));
		}
		//Debug.Log ("adding " + nextPoseName);
		currentTweens.Add (pose.PoseName, myCorout);
	}

	public void playTransition(string TransitionName)
	{
		foreach (transition trans in myTransitions) {

			if (trans.TransitionName == TransitionName) {
				playTransition (TransitionName, trans.TweenTime);
			}	
		}
	}

	public void playTransition(string TransitionName,float tweenTime)
	{
		foreach (transition trans in myTransitions) {
			
			if (trans.TransitionName == TransitionName) {
				StopAllTweens ();
				myPoses [trans.StartPosIndex].GoToPose ();
				Debug.Log ("Went to pose");
				ShiftToPose (myPoses [trans.EndPosIndex], tweenTime);
			}	
		}
	}

	public void StopAllTweens()
	{
		foreach (KeyValuePair<string,Coroutine> pair in currentTweens) {
			Coroutine temp = pair.Value;
			StopCoroutine (temp);
			//StopCoroutine (pair.value);
			//Debug.Log ("Removing " + name);
		
		}
		currentTweens.Clear ();
	}

	public void stopTween(string name)
	{
		if (currentTweens.ContainsKey (name)) {
			StopCoroutine (currentTweens [name]);
			//Debug.Log ("Removing " + name);
			currentTweens.Remove (name);
		}
	
	}

	IEnumerator Tween(AnimationPose pose, float tweenTime)
	{
		pose.setStartPoses ();

		//Time.unscaledDeltaTime.captureFramerate
		for (float i = 0; i < tweenTime; i += Time.timeScale == 0 ?  (1/Time.unscaledDeltaTime) : Time.deltaTime) {
			pose.updateTween (i / tweenTime);
		
			yield return 0;
		}
		pose.updateTween (1);
		//Debug.Log ("Ending tween " + pose.PoseName);
		stopTween (pose.PoseName);
	}


	IEnumerator TransitionTween(AnimationPose pose, float tweenTime)
	{
		
		pose.setStartPoses ();
		for (float i = 0; i < tweenTime; i += Time.deltaTime) {
			pose.updateTween (i / tweenTime);
			yield return null;
		}
		pose.updateTween (1);
	}

}


#if UNITY_EDITOR

[CustomEditor(typeof(Tweener)),CanEditMultipleObjects]
public class TweenerEditor : Editor {

	bool ShowAnimations;
	bool ShowTransitions;
	public override void OnInspectorGUI()
	{
		Tweener targ = ((Tweener)target);
		DrawDefaultInspector ();

		if (GUILayout.Button ("Quick Key Frame Maker!")) {
			AnimationPose newPose = new AnimationPose ();
			foreach (Transform t in targ.quickList) {
				ObjectPose newObj = new ObjectPose (targ.transform);
				newObj.myObject = t;
				newObj.setCurrentVectors (newPose.myScope);
				newPose.objPoses.Add (newObj);
				newObj.PoseName = t.gameObject.name;
			}
			targ.myPoses.Add (newPose);
		}

		if (GUILayout.Button ("Add New Animation Pose")) {
			targ.myPoses.Add( new AnimationPose ());

		}
		ShowAnimations = EditorGUILayout.Foldout (ShowAnimations, "AnimationPoses ("+targ.myPoses.Count+"):");
		if (ShowAnimations) {
			EditorGUI.indentLevel++;
			foreach (AnimationPose animPose in targ.myPoses) {
				DisplayAnimationPose (animPose, targ.myPoses);
			}
			EditorGUI.indentLevel--;
		}


		GUILayout.Space (15);

		if (((Tweener)target).myPoses.Count > 1) {

			if (GUILayout.Button ("Add new Transition")) {
				transition trans = new transition ();
				trans.tweenParent = (Tweener)target;
				trans.StartPosIndex = 0;//.startPose = ((Tweener)target).myPoses [0];
				trans.EndPosIndex = 1; //.endPose = ((Tweener)target).myPoses [1];
				((Tweener)target).myTransitions.Add (trans);
			}



			List<string> optionList = new List<string> ();
			foreach (AnimationPose p in ((Tweener)target).myPoses) {
				optionList.Add (p.PoseName);
			}
			ShowTransitions = EditorGUILayout.Foldout (ShowTransitions, "Transitions ("+((Tweener)target).myTransitions.Count+"):");
			if(ShowTransitions){
				EditorGUI.indentLevel++;
				foreach (transition trans in ((Tweener)target).myTransitions) {
					showTransition (trans, optionList.ToArray());

					if (trans.showTransition && GUILayout.Button ("Delete Transition", GUILayout.Width(200))) {
						((Tweener)target).myTransitions.Remove (trans);
					}
				}
				EditorGUI.indentLevel--;
			}
		
		}

	}

	public void showTransition(transition trans, string[] optionList)
	{	trans.showTransition = EditorGUILayout.Foldout (trans.showTransition, "Transition: ");
		if (trans.showTransition) {
			EditorGUI.indentLevel++;
			trans.TransitionName = EditorGUILayout.TextField ("Name: ", trans.TransitionName);
			trans.TweenTime = EditorGUILayout.FloatField ("Tween Time: ", trans.TweenTime);

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Start: ");

			trans.StartPosIndex = EditorGUILayout.Popup (trans.StartPosIndex, optionList);
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("End: ");
			trans.EndPosIndex = EditorGUILayout.Popup (trans.EndPosIndex, optionList);
			GUILayout.EndHorizontal ();
			EditorGUI.indentLevel--;
		
		}
	}


	public void DisplayAnimationPose(AnimationPose pose, List<AnimationPose> poseList)
	{	
		pose.ShowPoseInEditor = EditorGUILayout.Foldout (pose.ShowPoseInEditor, "Pose " + pose.PoseName);

		if (pose.ShowPoseInEditor) {
			EditorGUI.indentLevel++;
			pose.PoseName = EditorGUILayout.TextField ("Name: ", pose.PoseName);
			pose.myScope =  (Tweener.TweenScope)EditorGUILayout.EnumPopup ("Scope", pose.myScope);

	

			if (GUILayout.Button ("Copy Transform Values", GUILayout.Width(200))) {
				foreach (ObjectPose p in pose.objPoses) {
					p.setCurrentVectors (pose.myScope);
				}
			}

			if (GUILayout.Button ("Add New ObjectPose", GUILayout.Width(200))) {
				pose.objPoses.Add (new ObjectPose(((Tweener) target).transform));
			}
			if (GUILayout.Button ("Go To Pose", GUILayout.Width(200))) {
				pose.GoToPose ();
			}
			if (GUILayout.Button ("Delete Animation", GUILayout.Width(200))) {
				poseList.Remove (pose);
			}
			foreach (ObjectPose obj in pose.objPoses) {
				DisplayObjectPose (obj, pose.objPoses);
			}

			EditorGUI.indentLevel--;
		}
	}

	public void DisplayObjectPose(ObjectPose pose, List<ObjectPose> poseList)
	{
		pose.ShowPoseInEditor = EditorGUILayout.Foldout (pose.ShowPoseInEditor, "Object: " + pose.PoseName);
		if (pose.ShowPoseInEditor) {
			EditorGUI.indentLevel++;
			pose.PoseName = EditorGUILayout.TextField ("Name: ", pose.PoseName);
			pose.myObject = EditorGUILayout.ObjectField ("ToMove: ", pose.myObject, typeof(Transform), true) as Transform;

			GUILayout.BeginHorizontal ();
			pose.usesPosition = EditorGUILayout.ToggleLeft ("Position", pose.usesPosition,GUILayout.Width(109));
			pose.usesRotation = EditorGUILayout.ToggleLeft ("Rotation", pose.usesRotation,GUILayout.Width(109));
			pose.usesScale = EditorGUILayout.ToggleLeft ("Scale", pose.usesScale,GUILayout.Width(109));
			GUILayout.EndHorizontal ();
			if (pose.usesPosition) {
				pose.position = EditorGUILayout.Vector3Field ("Position: ", pose.position);
			}
			if (pose.usesRotation) {
				pose.rotation = EditorGUILayout.Vector3Field ("Rotation: ", pose.rotation);
			}
			if (pose.usesScale) {
				pose.scale = EditorGUILayout.Vector3Field ("Scale: ", pose.scale);
			}
				

			if (GUILayout.Button ("Delete Animation")) {
				poseList.Remove (pose);
			}
			EditorGUI.indentLevel--;
		}
	}
}
#endif

//==============================================================================================================================


[System.Serializable]
public class AnimationPose
{	public bool ShowPoseInEditor;
	public string PoseName = "Poser";
	public Tweener.TweenScope myScope;

	public List<ObjectPose> objPoses = new List<ObjectPose>();

	List<ObjectPose> startPoses = new List<ObjectPose>();

	public void setStartPoses()
	{startPoses.Clear ();
		foreach (ObjectPose pose in objPoses) {
			ObjectPose temp = new ObjectPose (pose.myObject);
			temp.setCurrentVectors (myScope);
			startPoses.Add (temp);
		}
	}

	public void GoToPose()
	{
		switch(myScope){
		case Tweener.TweenScope.Local:
			for (int i = 0; i < objPoses.Count; i++) {
				if (objPoses [i].usesScale) {
					objPoses [i].myObject.localScale = objPoses [i].scale;
				}	
				if (objPoses [i].usesRotation) {
					objPoses [i].myObject.localEulerAngles = objPoses [i].rotation;
				}
				if (objPoses [i].usesPosition) {
					objPoses [i].myObject.localPosition = objPoses [i].position;
				}
			}

			break;

		case Tweener.TweenScope.LocalStart:
			for (int i = 0; i < objPoses.Count; i++) {
				if (objPoses [i].usesScale) {
					objPoses [i].myObject.localScale +=  objPoses [i].scale;
				}
				if (objPoses [i].usesRotation) {
					objPoses [i].myObject.localEulerAngles +=  objPoses [i].rotation;
				}
				if (objPoses [i].usesPosition) {
					objPoses [i].myObject.localPosition += objPoses [i].position;
				}
			}
			break;

		case Tweener.TweenScope.World:
			for (int i = 0; i < objPoses.Count; i++) {
				if (objPoses [i].usesRotation) {
					objPoses [i].myObject.eulerAngles = objPoses [i].rotation;
				}
				if (objPoses [i].usesPosition) {
					objPoses [i].myObject.position = objPoses [i].position;
				}
			}
			break;

		case Tweener.TweenScope.WorldStart:
			for (int i = 0; i < objPoses.Count; i++) {

				if (objPoses [i].usesRotation) {
					objPoses [i].myObject.eulerAngles += objPoses [i].rotation;
				}
				if (objPoses [i].usesPosition) {
					objPoses [i].myObject.position += objPoses [i].position;
				}
			}
			break;
		}
	}

	public void updateTween(float lerpPosition)
	{
		switch(myScope){
		case Tweener.TweenScope.Local:
			for (int i = 0; i < objPoses.Count; i++) {
				if (objPoses [i].usesScale) {
					objPoses [i].myObject.localScale = Vector3.Lerp (startPoses [i].scale, objPoses [i].scale, lerpPosition);
				}	
				if (objPoses [i].usesRotation) {
					//Debug.Log ("Changing Rotation  " +objPoses[i].myObject+ " : "  + startPoses [i].rotation +"    "+ objPoses [i].rotation + "   " +WrapEnd(startPoses [i].rotation,  objPoses [i].rotation) );
					objPoses [i].myObject.localEulerAngles = Vector3.Lerp(startPoses [i].rotation,  WrapEnd(startPoses [i].rotation,  objPoses [i].rotation), lerpPosition);
				}
				if (objPoses [i].usesPosition) {
			
					objPoses [i].myObject.localPosition = Vector3.Lerp (startPoses [i].position, objPoses [i].position, lerpPosition);
				}
			}

			break;

		case Tweener.TweenScope.LocalStart:
			for (int i = 0; i < objPoses.Count; i++) {
				if (objPoses [i].usesScale) {
					objPoses [i].myObject.localScale = startPoses [i].scale + Vector3.Lerp (Vector3.zero, objPoses [i].scale, lerpPosition);
				}
				if (objPoses [i].usesRotation) {
					objPoses [i].myObject.localEulerAngles = startPoses [i].rotation + Vector3.Lerp (Vector3.zero, WrapEnd(Vector3.zero,  objPoses [i].rotation), lerpPosition);
					}
				if (objPoses [i].usesPosition) {
							objPoses [i].myObject.localPosition = startPoses [i].position + Vector3.Lerp (Vector3.zero, objPoses [i].position, lerpPosition);
					}
					}
			break;

		case Tweener.TweenScope.World:
			for (int i = 0; i < objPoses.Count; i++) {
				if (objPoses [i].usesScale) {
					objPoses [i].myObject.localScale = Vector3.Lerp (startPoses [i].scale, objPoses [i].scale, lerpPosition);
				}
				if (objPoses [i].usesRotation) {
					objPoses [i].myObject.eulerAngles = Vector3.Lerp(startPoses [i].rotation, WrapEnd(startPoses [i].rotation,  objPoses [i].rotation), lerpPosition);
				}
				if (objPoses [i].usesPosition) {
					objPoses [i].myObject.position = Vector3.Lerp (startPoses [i].position, objPoses [i].position, lerpPosition);
				}
				}
			break;

		case Tweener.TweenScope.WorldStart:
			for (int i = 0; i < objPoses.Count; i++) {
				if (objPoses [i].usesScale) {
					objPoses [i].myObject.localScale = startPoses [i].scale + Vector3.Lerp (Vector3.zero, objPoses [i].scale, lerpPosition);
				}
				if (objPoses [i].usesRotation) {
					objPoses [i].myObject.eulerAngles = startPoses [i].rotation + Vector3.Lerp (Vector3.zero, WrapEnd(Vector3.zero,  objPoses [i].rotation), lerpPosition);
				}
				if (objPoses [i].usesPosition) {
					objPoses [i].myObject.position = startPoses [i].position + Vector3.Lerp (Vector3.zero, objPoses [i].position, lerpPosition);
				}
			}
			break;
		}
	}

	public Vector3 WrapEnd(Vector3 start, Vector3 end)
	{
		if (Mathf.Abs (start.x - end.x) > 180) {
			if (start.x > end.x) {
				end.x += 360;
			} else {
				end.x -= 360;
			}
		}
		if (Mathf.Abs (start.y - end.y) > 180) {
			if (start.y > end.y) {
				end.y += 360;
			} else {
				end.y -= 360;
			}
		}
		if (Mathf.Abs (start.z - end.z) > 180) {
			if (start.z > end.z) {
				end.z += 360;
			} else {
				end.z -= 360;
			}
		}
		return end;
	}
}

//====================================================================================================
[System.Serializable]
public class ObjectPose
{

	public ObjectPose()
	{}
	public ObjectPose(Transform t)
	{
		myObject = t;
	}

	public void setCurrentVectors(Tweener.TweenScope scope)
	{if (scope == Tweener.TweenScope.Local || scope == Tweener.TweenScope.LocalStart ) {
			scale = myObject.localScale;
			position = myObject.localPosition;
			rotation = myObject.localEulerAngles;
	} else if (scope == Tweener.TweenScope.World || scope == Tweener.TweenScope.WorldStart) {
			scale = myObject.lossyScale;
			position = myObject.position;
		rotation = myObject.eulerAngles;
		}

	}
	public bool ShowPoseInEditor;
	public string PoseName;
	public Transform myObject;
	public Vector3 position;
	public Vector3 rotation;	
	public Vector3 scale = Vector3.one;

	public bool usesPosition = true;
	public bool usesRotation = true;
	public bool usesScale;

}




//=====================================================================================
[System.Serializable]
public class TweenState
{public string StateName;
	public List<TweenPath> myPaths;

}

[System.Serializable]
public class TweenPath
{
	public string Input;
	public string nextState;
	public float transitionTime;
}


[System.Serializable]
public class transition
{
	public bool showTransition;
	public string TransitionName;
	//[HideInInspector]
	[HideInInspector]
	public Tweener tweenParent;
	//[HideInInspector]
	//public AnimationPose startPose;
	//[HideInInspector]
	//public AnimationPose endPose;

	public int StartPosIndex;
	public int EndPosIndex;
	public float TweenTime;

}

/*
#if UNITY_EDITOR

[CustomEditor(typeof(transition))]
public class TransitionEditor : Editor {


	public override void OnInspectorGUI()
	{
		transition targ = (transition)target;
		List<string> optionList = new List<string> ();
		foreach (AnimationPose p in targ.tweenParent.myPoses) {
			optionList.Add (p.PoseName);
		}

		DrawDefaultInspector ();
		GUILayout.Label ("Start: ");
		targ.startPose = targ.tweenParent.myPoses[EditorGUILayout.Popup (targ.tweenParent.myPoses.IndexOf(targ.startPose),optionList.ToArray())];

		GUILayout.Label ("End: ");
		targ.endPose = targ.tweenParent.myPoses[EditorGUILayout.Popup (targ.tweenParent.myPoses.IndexOf(targ.endPose),optionList.ToArray())];

	}
}
#endif
*/


