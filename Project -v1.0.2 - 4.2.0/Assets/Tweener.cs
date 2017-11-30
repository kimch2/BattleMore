﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Tweener : MonoBehaviour {

	void Start()
	{
		GoToPose (myPoses[0].PoseName, 5);
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


	public void GoToPose(string nextPoseName,float tweenTime)
	{
		foreach (AnimationPose pose in myPoses) {
			Debug.Log ("Checking ");
			if (pose.PoseName == nextPoseName) {
				stopTween (nextPoseName);

				Coroutine myCorout = StartCoroutine (Tween(pose, tweenTime));
				currentTweens.Add (nextPoseName, myCorout);
			}	
		}
	}


	public void playTransition(string TransitionName,float tweenTime)
	{
		foreach (transition trans in myTransitions) {
			Debug.Log ("Checking " );
			if (trans.TransitionName == TransitionName) {
				//stopTween (TransitionName);

				//Coroutine myCorout = StartCoroutine (Tween(pose, tweenTime));
				//currentTweens.Add (nextPoseName, myCorout);
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

[CustomEditor(typeof(Tweener))]
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
				EditorGUILayout.Vector3Field ("Position: ", pose.position);
			}
			if (pose.usesRotation) {
				EditorGUILayout.Vector3Field ("Rotation: ", pose.rotation);
			}
			if (pose.usesScale) {
				EditorGUILayout.Vector3Field ("Scale: ", pose.scale);
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


	public void updateTween(float lerpPosition)
	{
		switch(myScope){
		case Tweener.TweenScope.Local:
			for (int i = 0; i < objPoses.Count; i++) {
				if (objPoses [i].usesScale) {
					objPoses [i].myObject.localScale = Vector3.Lerp (startPoses [i].scale, objPoses [i].scale, lerpPosition);
				}	
				if (objPoses [i].usesRotation) {
					objPoses [i].myObject.localRotation = Quaternion.Lerp (Quaternion.Euler (startPoses [i].rotation), Quaternion.Euler (objPoses [i].rotation), lerpPosition);
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
						objPoses [i].myObject.localEulerAngles = startPoses [i].rotation + Vector3.Lerp (Vector3.zero, objPoses [i].rotation, lerpPosition);
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
					objPoses [i].myObject.rotation = Quaternion.Lerp (Quaternion.Euler (startPoses [i].rotation), Quaternion.Euler (objPoses [i].rotation), lerpPosition);
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
					objPoses [i].myObject.eulerAngles = startPoses [i].rotation + Vector3.Lerp (Vector3.zero, objPoses [i].rotation, lerpPosition);
				}
				if (objPoses [i].usesPosition) {
					objPoses [i].myObject.position = startPoses [i].position + Vector3.Lerp (Vector3.zero, objPoses [i].position, lerpPosition);
				}
			}
			break;
		}
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

	public bool usesPosition;
	public bool usesRotation;
	public bool usesScale;

}




//=====================================================================================



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


