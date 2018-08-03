using UnityEngine;
using System.Collections;

public class Squirrel : MonoBehaviour {
    private IEnumerator coroutine;
    Animator squirrel;

	// Use this for initialization
	void Start () {
        squirrel = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("up"))
        {
            squirrel.SetBool("run", true);
            squirrel.SetBool("idle", false);
            squirrel.SetBool("runleft", false);
            squirrel.SetBool("runright", false);
            squirrel.SetBool("walk", false);
            squirrel.SetBool("left", false);
            squirrel.SetBool("right", false);
            squirrel.SetBool("stand", false);
            squirrel.SetBool("eat", false);
        }
        if ((Input.GetKey("down")) || (Input.GetKey(KeyCode.S)))
        {
            squirrel.SetBool("idle", true);
            squirrel.SetBool("run", false);
            squirrel.SetBool("runleft", false);
            squirrel.SetBool("runright", false);
            squirrel.SetBool("walk", false);
            squirrel.SetBool("left", false);
            squirrel.SetBool("right", false);
            squirrel.SetBool("stand", false);
            squirrel.SetBool("eat", false);
        }
        if (Input.GetKey("left"))
        {
            squirrel.SetBool("runleft", true);
            squirrel.SetBool("runright", false);
            squirrel.SetBool("run", false);
            squirrel.SetBool("idle", false);
            squirrel.SetBool("walk", false);
            squirrel.SetBool("left", false);
            squirrel.SetBool("right", false);
            squirrel.SetBool("stand", false);
            squirrel.SetBool("eat", false);
            squirrel.SetBool("stand", false);
        }
        if (Input.GetKey("right"))
        {
            squirrel.SetBool("runright", true);
            squirrel.SetBool("runleft", false);
            squirrel.SetBool("run", false);
            squirrel.SetBool("idle", false);
            squirrel.SetBool("walk", false);
            squirrel.SetBool("left", false);
            squirrel.SetBool("right", false);
            squirrel.SetBool("stand", false);
            squirrel.SetBool("eat", false);
        }
        if (Input.GetKey(KeyCode.W))
        {
            squirrel.SetBool("walk", true);
            squirrel.SetBool("left", false);
            squirrel.SetBool("right", false);
            squirrel.SetBool("run", false);
            squirrel.SetBool("runleft", false);
            squirrel.SetBool("runright", false);
            squirrel.SetBool("idle", false);
            squirrel.SetBool("stand", false);
            squirrel.SetBool("eat", false);
        }
        if (Input.GetKey(KeyCode.A))
        {
            squirrel.SetBool("left", true);
            squirrel.SetBool("walk", false);
            squirrel.SetBool("right", false);
            squirrel.SetBool("run", false);
            squirrel.SetBool("runleft", false);
            squirrel.SetBool("runright", false);
            squirrel.SetBool("idle", false);
            squirrel.SetBool("stand", false);
            squirrel.SetBool("eat", false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            squirrel.SetBool("right", true);
            squirrel.SetBool("left", false);
            squirrel.SetBool("walk", false);
            squirrel.SetBool("run", false);
            squirrel.SetBool("runleft", false);
            squirrel.SetBool("runright", false);
            squirrel.SetBool("idle", false);
            squirrel.SetBool("stand", false);
            squirrel.SetBool("eat", false);
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            squirrel.SetBool("up", true);
            squirrel.SetBool("walk", false);
            squirrel.SetBool("right", false);
            squirrel.SetBool("left", false);
            squirrel.SetBool("run", false);
            squirrel.SetBool("runleft", false);
            squirrel.SetBool("runright", false);
            squirrel.SetBool("idle", false);
            StartCoroutine("stand");
            stand();
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            squirrel.SetBool("down", true);
            squirrel.SetBool("up", false);
            squirrel.SetBool("walk", false);
            squirrel.SetBool("right", false);
            squirrel.SetBool("run", false);
            squirrel.SetBool("runleft", false);
            squirrel.SetBool("runright", false);
            squirrel.SetBool("idle", false);
            squirrel.SetBool("stand", false);
            StartCoroutine("stand");
            StartCoroutine("idle");
            idle();
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            squirrel.SetBool("eat", true);
            squirrel.SetBool("down", false);
            squirrel.SetBool("up", false);
            squirrel.SetBool("walk", false);
            squirrel.SetBool("right", false);
            squirrel.SetBool("left", false);
            squirrel.SetBool("run", false);
            squirrel.SetBool("runleft", false);
            squirrel.SetBool("runright", false);
            squirrel.SetBool("idle", false);
            squirrel.SetBool("stand", false);
        }
        if (Input.GetKey(KeyCode.Alpha0))
        {
            squirrel.SetBool("die", true);
            squirrel.SetBool("run", false);
            squirrel.SetBool("runleft", false);
            squirrel.SetBool("runright", false);
        }
    }
    IEnumerator stand()
    {
       // print(Time.time);
        yield return new WaitForSeconds(0.5f);
       // print(Time.time);
        squirrel.SetBool("stand", true);
        squirrel.SetBool("up", false);
    }
    IEnumerator idle()
    {
       // print(Time.time);
        yield return new WaitForSeconds(0.5f);
      //  print(Time.time);
        squirrel.SetBool("idle", true);
        squirrel.SetBool("down", false);
    }
}
