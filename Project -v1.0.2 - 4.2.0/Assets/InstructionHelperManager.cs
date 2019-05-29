using UnityEngine;
using System.Collections;

public class InstructionHelperManager : MonoBehaviour {


	public GameObject button;
	public static InstructionHelperManager instance;
	// Use this for initialization
	void Awake () {
		instance = this;
	
	}

    public static InstructionHelperManager getInstance()
    {
        if (instance == null)
        {
            instance = GameObject.FindObjectOfType<InstructionHelperManager>();

        }
        return instance;
    }

	public  void addBUtton( string text, float duration, Sprite pic)
	{
		GameObject obj = (GameObject)Instantiate (instance.button, instance.transform);

		obj.GetComponent<InstructionHelper> ().text = text;
		obj.GetComponent<InstructionHelper> ().duration =duration;

		obj.GetComponent<InstructionHelper> ().myPic = pic;
		obj.GetComponent<InstructionHelper> ().buttonImage.sprite = pic;

	}


}
