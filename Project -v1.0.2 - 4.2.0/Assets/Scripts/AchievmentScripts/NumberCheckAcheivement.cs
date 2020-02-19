using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberCheckAcheivement: Achievement{

	public float number;
	public string PlayerPrefTag;
	public bool lessThan;

	public override string GetDecription()
	{return Description ;
	}

	public override void CheckBeginning (){
	}

	public override void CheckEnd (){
		if (!IsAccomplished ()) {
			if (isCorrectLevel()) {
				if (!lessThan) {
					if (PlayerPrefs.GetFloat (PlayerPrefTag, 0) > number) {
						Accomplished ();
					}
				} else {
					if (PlayerPrefs.GetFloat (PlayerPrefTag, 0) < number) {
						Accomplished ();
					}
				}
			}
		}
		PlayerPrefs.SetFloat (PlayerPrefTag,0);

	}

	public override void Reset()
	{
		PlayerPrefs.SetFloat (Title, 0);
		PlayerPrefs.SetFloat (PlayerPrefTag, 0);

	}

}
