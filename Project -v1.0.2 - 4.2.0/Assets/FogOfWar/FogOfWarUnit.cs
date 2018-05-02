using UnityEngine;
using System.Collections;

public class FogOfWarUnit : MonoBehaviour
{
    public float radius = 5.0f;

    public float updateFrequency { get { return FogOfWar.current.updateFrequency; } }
   // float _nextUpdate = 0.0f;

    public LayerMask lineOfSightMask = 0;


	public bool hasMoved = true;
	[Tooltip("Will clear fog even if a mover has not marked this thing as having moved")]
	public bool autoUpdate;
    void Start()
    {
		//Debug.Log ("Fog");
		hasMoved = true;

		if (autoUpdate) {
			InvokeRepeating ("AutoUpdate", Random.Range (0, updateFrequency), updateFrequency + .2f);
		} else {
			InvokeRepeating ("clearFog", Random.Range (.05f, updateFrequency) + .1f, updateFrequency);
		}
		//Invoke ("move", 1.9f);
		//Invoke ("clearFog", 2);

    }



	public void move (){hasMoved = true;
		}


	public void clearFog()
	{  
		if (hasMoved) {
			hasMoved = false;
			FogOfWar.current.Unfog (transform.position, radius, lineOfSightMask);
		

		}
	}

	public void AutoUpdate ()
	{
		FogOfWar.current.Unfog (transform.position, radius, lineOfSightMask);
	}

}
