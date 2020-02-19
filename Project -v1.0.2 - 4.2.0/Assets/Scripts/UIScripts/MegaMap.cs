using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
public class MegaMap : MonoBehaviour{


	public Image fogSprite;
	public RawImage UnitSprite;

	public Image OtherFogSprite;
	public Image OtherUnitSprite;
	public GameObject childParent;
	public MiniMapUIController miniController;
    public GameObject background;
	bool active;

	void Start()
	{
		InvokeRepeating ("UpdateSprite", .1f, .1f);
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return null;
        Vector2 newSize = ((RectTransform)UnitSprite.transform).sizeDelta;
        if (miniController.getAspectRatio() > 1)
        {
            newSize.y /= miniController.getAspectRatio();
        }
        else
        {
            newSize.x *= miniController.getAspectRatio();
        }

        ((RectTransform)fogSprite.transform).sizeDelta = newSize;
        ((RectTransform)UnitSprite.transform).sizeDelta = newSize;
        ((RectTransform)background.transform).sizeDelta = newSize;
    }

    void Update()
	{
		if (Input.GetKeyDown (KeyCode.M) && Time.timeScale != 0) {
			ToggleMegamap ();
		}
	}

	public void ToggleMegamap()
	{active = !active;
		childParent.SetActive (active);
		
	}


	// Update is called once per frame
	void UpdateSprite () {
		if (active) {
			fogSprite.sprite = OtherFogSprite.sprite;
			//UnitSprite.sprite = miniController.UnitSprite;// OtherUnitSprite;
		
		}
		
	}


}
