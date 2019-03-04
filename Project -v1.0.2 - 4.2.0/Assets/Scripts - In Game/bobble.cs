    using UnityEngine;
using System.Collections;


public class bobble : MonoBehaviour {

   
        public float amplitude;          //Set in Inspector 
        public float speed;                  //Set in Inspector 


	Vector3 StartPosition;
	Vector3 upVector;
	float rand;

	public void Start()
	{
		StartPosition = transform.localPosition;
		rand = Random.value *50;
		upVector = Vector3.up * amplitude * 50;
	}
        // Update is called once per frame
    void Update()
        {
		transform.localPosition = StartPosition + ( upVector * Mathf.Sin(speed * Time.time + rand));
            
        }






    }
