using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTag : MonoBehaviour {

	public enum AbilType
	{
		Physical,Magic,Life,Light,Hot,Cold,Null
	}


	public Vector3 startPosition;
	public float separationDistance = .45f;
	public float TagRetentionTime = 5;
	public int maxAllowed = 5;
	public float iconSize = .2f;

	public List<SpriteRenderer> mySprites;

	List<ComboQue> inQue = new List<ComboQue>();

	List<SpriteRenderer> spritesInUse = new List<SpriteRenderer>();
	List<SpriteRenderer> freeSprites = new List<SpriteRenderer>();

	[Tooltip("Should be Physical,Magic,Life,Light,Hot,Cold,Null")]
	public List<Sprite> EnumSprites;

	public Selected myHealthDisplay;

	void Awake()
	{
		foreach (SpriteRenderer rend in mySprites) {
			freeSprites.Add (rend);
			rend.gameObject.SetActive (false);
		}

	}

	public static bool CastTag(GameObject target, AbilType toAdd, List<AbilType> comboTest)
	{
		ComboTag tagger = addTounit (target);
		return tagger.addTag (toAdd, comboTest);
	}

	public static ComboTag addTounit(GameObject toAdd)
	{
		HealthDisplay display = toAdd.GetComponentInChildren<HealthDisplay> (true);

		ComboTag tagger = display.GetComponentInChildren<ComboTag> (true);
		if (tagger) {
			return tagger;
		} else {
			GameObject AddedTag = Instantiate<GameObject> (Resources.Load<GameObject>("ComboTag"), display.transform);
			DisplayBar bar = display.GetComponentInChildren<DisplayBar> (true);

			ComboTag toReturn = AddedTag.GetComponent<ComboTag> ();
			toReturn.myHealthDisplay = toAdd.GetComponentInChildren<Selected>();
			AddedTag.transform.localPosition = new Vector3(bar.BarWidth/2 , 2.1f, 0); // Edit this later to handle healthbars of differnet sizes
			AddedTag.transform.localEulerAngles = Vector3.zero;
			return toReturn;
		}
	}

	/// <summary>
	/// Adds the tag, and returns if the combo is completed
	/// </summary>
	public bool addTag(AbilType toAdd, List<AbilType> comboTest)
	{
		
		ComboQue q = new ComboQue (Time.time + TagRetentionTime, toAdd , addSprite (toAdd));
		inQue.Add (q);


		myHealthDisplay.buffDisplay.enabled = true;
		myHealthDisplay.outsideAttchement = true;

		if (inQue.Count > maxAllowed) {
			RemoveFirstItem ();
		}

		if (inQue.Count == 1) {
			StartCoroutine (RemoveTag());
		}

		if (comboTest == null || comboTest.Count >= inQue.Count) {
			return false;
		}
		for (int i = comboTest.Count-1; i  > -1; i--) {

			if (inQue [inQue.Count - (comboTest.Count - i) -1].myType != comboTest [ i]) {
				return false;
			}
		
		}

		List<ComboQue> toGrow = new List<ComboQue>();
		for (int i = comboTest.Count-1; i  > -1; i--) {
			toGrow.Add(inQue [inQue.Count - (comboTest.Count - i) -1]);

		}
		toGrow.Add (inQue[inQue.Count -1]);
		StartCoroutine(ComboHit(toGrow));
		return true;
	}

	SpriteRenderer addSprite(AbilType toAdd)
	{
		SpriteRenderer rend = freeSprites [0];
		rend.sprite = EnumSprites [(int)toAdd];
		freeSprites.RemoveAt (0);
		rend.gameObject.SetActive(true);
		rend.transform.localScale = Vector3.one * iconSize;

		if (spritesInUse.Count == 0) {
			rend.gameObject.transform.localPosition = startPosition - Vector3.right * (separationDistance * (inQue.Count - 1));
		} else {
			rend.gameObject.transform.localPosition = spritesInUse[spritesInUse.Count - 1].transform.localPosition - Vector3.right * separationDistance ;
		}
		spritesInUse.Add (rend);
		return rend;
	}

	IEnumerator RemoveTag()
	{
		if (inQue.Count > 0) {
			myHealthDisplay.buffDisplay.enabled = true;
			myHealthDisplay.outsideAttchement = true;
		}
		
		while (inQue.Count > 0) {
			while (inQue.Count > 0 && inQue [0].TimeToRemove < Time.time ) {
				RemoveFirstItem ();
			}
			yield return null;
		}
		if (inQue.Count > 0) {
			myHealthDisplay.outsideAttchement = false;
			myHealthDisplay.checkOn ();

		}
	}

	void RemoveFirstItem()
	{
		inQue.RemoveAt (0);
		StartCoroutine (DisappearItem (spritesInUse [0]));
		StartCoroutine (ShiftQue ());

	}


	IEnumerator ShiftQue()
	{
		for (float i = 0; i < .3f; i += Time.deltaTime) {
			foreach (SpriteRenderer sp in spritesInUse) {
				sp.transform.localPosition -= Vector3.left * separationDistance *( Time.deltaTime/ .3f);
			}
			yield return null;
		}
	}

	IEnumerator DisappearItem(SpriteRenderer toDisappear)
	{
		spritesInUse.Remove (toDisappear);
		for (float i = 0; i < .3f; i += Time.deltaTime) {
			toDisappear.transform.localScale = (1 - (i / .3f)) * iconSize * Vector3.one;
			yield return null;
		}
		toDisappear.gameObject.SetActive (false);
		freeSprites.Add (toDisappear);

	}


	IEnumerator ComboHit(List<ComboQue> toGrow)
	{
		Vector3 totalScale = Vector3.zero;
		Vector3 tempUse;
		for (float i = 0; i < .1f; i += Time.deltaTime) {
			tempUse = Vector3.one * 6 * Time.deltaTime;
			foreach (ComboQue que in toGrow) {
				if (que.mySprite.gameObject.activeSelf) {
					que.mySprite.transform.localScale += tempUse;

				}
			}
			totalScale += tempUse;
			yield return null;
		}

		for (float i = 0; i < .3f; i += Time.deltaTime) {
			tempUse = Vector3.one * 2 * Time.deltaTime;
			foreach (ComboQue que in toGrow) {
				if (que.mySprite.gameObject.activeSelf) {
					que.mySprite.transform.localScale -= tempUse;
				}
			}
			totalScale -= tempUse;
			yield return null;
		}
	
	
		foreach (ComboQue que in toGrow) {
			if (que.mySprite.gameObject.activeSelf) {
				que.mySprite.transform.localScale -= totalScale;
			}
		}

	}


	void OnDrawGizmos()
	{
		Gizmos.DrawSphere (transform.TransformPoint (startPosition), .1f);
	}

	struct ComboQue
	{
		public ComboQue(float time, AbilType typ, SpriteRenderer rend)
		{
			TimeToRemove = time;
			myType = typ;
			mySprite = rend;
		}


		public float TimeToRemove;
		public AbilType myType;
		public SpriteRenderer mySprite;
	}

}
	
