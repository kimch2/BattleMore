using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinUnlocker : MonoBehaviour {

	public List<Skin> mySkins;
	public List<MeshRenderer> ColoredSkins;
	List<Material> DefaultMaterials = new List<Material>();

	public SkinColorManager mySkinner;
	bool setFalse;

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
			UnitManager manager = GetComponent<UnitManager>();
			RaceManager racer = GameManager.main.playerList[manager.PlayerOwner - 1];
			mySkinner = racer.getColorManager();
		}
		for (int i = 0; i < ColoredSkins.Count; i++)
		{
			DefaultMaterials.Add(ColoredSkins[i].material);
			ColoredSkins[i].material = mySkinner.getSkin(DefaultMaterials[i]);
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
