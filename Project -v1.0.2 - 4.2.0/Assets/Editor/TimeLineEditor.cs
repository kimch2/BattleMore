using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TimeLineEditor : EditorWindow
{

	List<WaveManager> wavers;

	[MenuItem("Window/TimeLine Manager")]
	public static void showWindow()
	{
		EditorWindow.GetWindow(typeof(TimeLineEditor));
	}

	public void Awake()
	{
		wavers = new List<WaveManager>();
		foreach (WaveManager wave in GameObject.FindObjectsOfType<WaveManager>())
		{
			wavers.Add(wave);
		}
	}


	void OnGUI()
	{

		GUILayout.BeginHorizontal();
		foreach (WaveManager wave in wavers)
		{
			GUILayout.BeginHorizontal();

			wave.FirstPlayWaveType = (WaveContainer.EnemyWave)EditorGUILayout.EnumPopup("Race: ", wave.FirstPlayWaveType, GUILayout.Width(260));
			foreach (WaveManager.waveSetting pile in wave.myWaves)
			{
				GUILayout.BeginHorizontal();
				pile.waveSpawnTime = EditorGUILayout.FloatField(pile.waveSpawnTime);
				GUILayout.EndHorizontal();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndHorizontal();
	}
}