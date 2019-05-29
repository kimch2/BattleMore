using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateDisabler : MonoBehaviour
{

	public int month = 3;
	public int day = 25;
	public int year = 2019;
	// Start is called before the first frame update
	public SteamManager steamer;

	private void Awake()
	{
		if (steamer.steamEnabled)
		{
			Destroy(this.gameObject);
		}

	}

	void Start()
    {
		
		System.DateTime timer = System.DateTime.Today;
		System.DateTime disableDay = new System.DateTime(year, month, day);
		int i = System.DateTime.Compare(timer, disableDay);
		if (i > 0)
		{
#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
		}
	}


}
