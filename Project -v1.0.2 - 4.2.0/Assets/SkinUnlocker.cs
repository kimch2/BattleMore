using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinUnlocker : MonoBehaviour {

    int Owner = -1;
    public List<Skin> mySkins;
	public List<MeshRenderer> ColoredSkins;
    public List<SpriteRenderer> ColoredSprites;
    List<Material> DefaultMaterials = new List<Material>();

	public SkinColorManager mySkinner;
	bool setFalse;

    [Tooltip("X and Y are min and Max, Z is the Natural Offset to account for different base colors.")]
    public Vector3 HueThreshold = new Vector3(0,.05f, 0);

    public void SetSource(int i)
    {      
        Owner = i;
    }

	void Awake()
	{	if (!setFalse) {
			setFalse = true;
		
			foreach (Skin s in mySkins) {
				foreach (GameObject obj in s.myPieces) {
					obj.SetActive (false);
				}
			}
		}
	}

    public void Start()
    {
        if (!mySkinner)
        {
            if (Owner >= 0)
            {
                RaceManager racer = GameManager.main.playerList[Owner - 1];
                mySkinner = racer.getColorManager();
            }
            else
            {
                UnitManager manager = GetComponent<UnitManager>();
                RaceManager racer = GameManager.main.playerList[manager.PlayerOwner - 1];
                mySkinner = racer.getColorManager();
            }
        }
        for (int i = 0; i < ColoredSkins.Count; i++)
        {
            DefaultMaterials.Add(ColoredSkins[i].material);
            ColoredSkins[i].material = mySkinner.getSkin(DefaultMaterials[i]);
        }

        for (int i = 0; i < ColoredSprites.Count; i++)
        {
            ColoredSprites[i].material = mySkinner.getSpriteMaterial();
            ColoredSprites[i].material.SetFloat("_HSVRangeMin", HueThreshold.x);
            ColoredSprites[i].material.SetFloat("_HSVRangeMax", HueThreshold.y);
            float m = ColoredSprites[i].material.GetVector("_HSVAAdjust").x;
            ColoredSprites[i].material.SetVector("_HSVAAdjust", new Vector4(m + HueThreshold.z, 0, 0, 0));
        }
    }

	[System.Serializable]
	public class Skin
	{
		public string name;
		public List<GameObject> myPieces;
	
	}



	public void unlockSkin(string name)
	{
		if (!setFalse) {
			Awake ();
		}
		
		foreach (Skin s in mySkins) {
			if (name == s.name) {
			
				foreach (GameObject obj in s.myPieces) {
					obj.SetActive (true);
				}
			}
		}
	}



}
