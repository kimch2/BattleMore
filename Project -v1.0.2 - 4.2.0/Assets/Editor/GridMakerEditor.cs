using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridMaker)), CanEditMultipleObjects]
public class GridMakerEditor : Editor {


	GameObject lastPlaced;
	float BoxDownAmount = 40;
	public override void OnInspectorGUI()
	{
		serializedObject.Update ();

		Object[] grid = targets;
		if (lastPlaced == null) {
			lastPlaced =((GridMaker)grid[0]).gameObject;
		}


		DrawDefaultInspector ();
		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("TopLeft")) {

			Vector3 newPosition = lastPlaced.transform.position + new Vector3 (-.5f,0,.5f) * Mathf.Sqrt((Mathf.Pow(lastPlaced.gameObject.transform.lossyScale.x,2) + Mathf.Pow(lastPlaced.gameObject.transform.lossyScale.z,2)));
			lastPlaced = Instantiate (((GridMaker)grid[0]).gameObject, newPosition, lastPlaced.transform.rotation,  ((GridMaker)grid[0]).transform.parent );
		
		}

		if (GUILayout.Button ("TopRight")) {
			Vector3 newPosition = lastPlaced.transform.position + new Vector3 (.5f,0,.5f) * Mathf.Sqrt((Mathf.Pow(lastPlaced.gameObject.transform.lossyScale.x,2) + Mathf.Pow(lastPlaced.gameObject.transform.lossyScale.z,2)));
			lastPlaced = Instantiate (((GridMaker)grid[0]).gameObject, newPosition, lastPlaced.transform.rotation,  ((GridMaker)grid[0]).transform.parent );

		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal ();

		if (GUILayout.Button ("BottomLeft")) {

			Vector3 newPosition = lastPlaced.transform.position + new Vector3 (-.5f,0,-.5f) * Mathf.Sqrt((Mathf.Pow(lastPlaced.gameObject.transform.lossyScale.x,2) + Mathf.Pow(lastPlaced.gameObject.transform.lossyScale.z,2)));
			lastPlaced = Instantiate (((GridMaker)grid[0]).gameObject, newPosition, lastPlaced.transform.rotation,  ((GridMaker)grid[0]).transform.parent );
		}
		if (GUILayout.Button ("BottomRight")) {

			Vector3 newPosition = lastPlaced.transform.position + new Vector3 (.5f,0,-.5f) * Mathf.Sqrt((Mathf.Pow(lastPlaced.gameObject.transform.lossyScale.x,2) + Mathf.Pow(lastPlaced.gameObject.transform.lossyScale.z,2)));
			lastPlaced = Instantiate (((GridMaker)grid[0]).gameObject, newPosition, lastPlaced.transform.rotation,  ((GridMaker)grid[0]).transform.parent );
		}
		GUILayout.EndHorizontal();

		if (GUILayout.Button ("Set Box Down")) {
			foreach (Object g in grid) {
				((GridMaker)g).transform.GetComponentInChildren<MeshRenderer> ().transform.position -= Vector3.up * BoxDownAmount;
			}
		}
		if (GUILayout.Button ("Set Box Normal")) {
			foreach (Object g in grid) {
				((GridMaker)g).transform.GetComponentInChildren<MeshRenderer> ().transform.localPosition = Vector3.zero;
			}
		}


		if (GUILayout.Button ("Set  ALL Boxes Down")) {
			foreach (GridMaker g in GameObject.FindObjectsOfType<GridMaker>()) {
				if (!g.OnUnit) {
					g.transform.GetComponentInChildren<MeshRenderer> ().transform.position = g.transform.position + Vector3.down * BoxDownAmount;
				}
			}
		}
		if (GUILayout.Button ("Set  ALL Boxes Up")) {
			foreach (GridMaker g in GameObject.FindObjectsOfType<GridMaker>()) {
				if (!g.OnUnit) {
					g.transform.GetComponentInChildren<MeshRenderer> ().transform.localPosition = Vector3.zero;
				}
			}
		}

		if (GUILayout.Button ("Add Audio")) {
			bool everyOther = true;
			foreach (GridMaker g in GameObject.FindObjectsOfType<GridMaker>()) {
				if (!g.OnUnit) {
					if (everyOther) {
						AudioSource s = g.gameObject.AddComponent<AudioSource> ();
						s.playOnAwake = false;
						g.src = s;
						s.clip = ((GridMaker)target).src.clip;

					}
					everyOther = !everyOther;
				}
			}
		}

		if (GUILayout.Button ("Apply new Sound")) {


			foreach (GridMaker g in GameObject.FindObjectsOfType<GridMaker>()) {

				if (g.gameObject.GetComponent<AudioSource> ()) {
					g.gameObject.GetComponent<AudioSource> ().clip = ((GridMaker)target).src.clip;
				}

			}
		}

		serializedObject.ApplyModifiedProperties ();
	}



}