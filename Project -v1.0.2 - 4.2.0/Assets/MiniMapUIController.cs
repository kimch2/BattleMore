using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.EventSystems;

public class MiniMapUIController : MonoBehaviour, IPointerDownHandler , IPointerUpHandler
{

	public Image img;
	public Image ScreenTrapz;
	private bool[,] virtTrapezoid;
	private Texture2D screenTrapzoidTex;

	public GameObject warningSymbol;
	public Sprite defaultWarning;
	private Texture2D UnitTexture;

	public RaceManager raceMan;
	public float minimapUpdateRate;
	public Image fogger;

    public LayerMask MiniMapRender = (1 << 21);
	float Left = 726f;
  
	private bool dragging;
	float top = 1400f;
   
	float Right = 1447f;
	float bottom = 730f;

	public Sprite lineArrow;
	private float WorldHeight;
	private float WorldWidth;
 
	private int textureHeight = 160, textureWidth = 160;

 
	public int scalingfactor = 1;

	public float heightOffset;
   
	public Sprite UnitSprite;

	private float nextActionTimec;

	//Used for detecting MinimapClicks
	private float minimapWidth;
	private float minimapHeight;
	private RectTransform myRect;

	public GameObject megaMap;
	private Point[,] PointArray;

	private FogOfWar fog;
	Texture2D _texture;
	GUIStyle _panelStyle;

	public static MiniMapUIController main;
	Ray ray1;

	//Top left
	Ray ray2;

	//Top right
	Ray ray3;

	//Bottom right
	Ray ray4;

	Texture2D panelTex;
	RectTransform newParent;

	MainCamera myCam;


	float UIWidth;
	float UIHeight;


	bool wasOn = true;

	public void DubAwake ()
	{ 
		wasOn = transform.parent.gameObject.activeSelf;
		transform.parent.gameObject.SetActive (true);
		newParent = (RectTransform)this.transform.parent.Find ("ScreenTrap");
		UIWidth = newParent.rect.width;
		UIHeight = newParent.rect.height;
	
	}

	void Awake()
	{

		main = this;
	}

	public void Initialize ()
	{
		main = this;

		myCam = GameObject.FindObjectOfType<MainCamera> ();
		Left = myCam.getBoundries ().xMin;
		Right = myCam.getBoundries ().xMax;
		top = myCam.getBoundries ().yMax;
		bottom = myCam.getBoundries ().yMin;
		
		// Use for Fog of War
		Texture2D panelTex = new Texture2D (1, 1);
		panelTex.SetPixels32 (new Color32[] { new Color32 (255, 255, 255, 64) });
		panelTex.Apply ();
		_panelStyle = new GUIStyle ();
		_panelStyle.normal.background = panelTex;

		WorldHeight = top - bottom;
		WorldWidth = Right - Left;
		fog = GameObject.FindObjectOfType<FogOfWar> ();
        

		textureWidth *= scalingfactor;
		textureHeight *= scalingfactor;
		virtTrapezoid = new bool[textureWidth, textureHeight];
		screenTrapzoidTex = InitialTexture (screenTrapzoidTex);


		UnitTexture = InitialTexture (UnitTexture);

		usedTriangleList = new List<Point> ();
		UnitSprite = Sprite.Create (UnitTexture as Texture2D, new Rect (0f, 0f, textureWidth, textureHeight), Vector2.zero);
		img.sprite = UnitSprite;



		_texture = fog.getTexture ();
		setFog ();
		fogger.sprite = Sprite.Create (_texture as Texture2D, new Rect (0f, 0f, fog.texture.width, fog.texture.height), Vector2.zero);

		GameMenu.main.addDisableScript (this);
		ScreenTrapz.sprite = Sprite.Create (screenTrapzoidTex as Texture2D, new Rect (0f, 0f, textureWidth, textureHeight), Vector2.zero);

		PointArray = new Point[textureWidth, textureHeight];
		for (int i = 0; i < textureWidth; i++) {
			for (int j = 0; j < textureHeight; j++) {
				PointArray [i, j] = new Point (i, j);
			}
		}
		WidthScale = textureWidth / WorldWidth;
		HeightScale = textureHeight / WorldHeight;
   
		InvokeRepeating ("updateScreenRect", .1f, minimapUpdateRate);
		InvokeRepeating ("setFog", .08f, minimapUpdateRate);
		//InvokeRepeating ("UpdateMiniMapA", .7f, minimapUpdateRate);

		transform.parent.gameObject.SetActive (wasOn);

        myRect = GetComponent<RectTransform>();
        minimapWidth = myRect.rect.width;
        minimapHeight = myRect.rect.height;

        GameObject newCamera = new GameObject("Minimap Camera");
		Camera cam = newCamera.AddComponent<Camera>();
		cam.cullingMask = MiniMapRender;
		cam.orthographic = true;
		cam.orthographicSize = WorldHeight/2;
		cam.transform.position = Vector3.Lerp(MainCamera.main.BottomLeftBorder, MainCamera.main.TopRightBorder, .5f) + Vector3.up * 120;
        cam.aspect = WorldWidth / WorldHeight;
        cam.transform.eulerAngles = new Vector3(90,0,0);
		cam.clearFlags = CameraClearFlags.Color;
		cam.backgroundColor = Color.clear;

        Vector2 newSize = ((RectTransform)img.transform).sizeDelta;
        if (WorldWidth > WorldHeight)
        {
            minimapHeight = 237 / cam.aspect;
            newSize.y = minimapHeight;
        }
        else
        {
            minimapWidth = 237 / cam.aspect;
            newSize.x = minimapWidth;
        }

        ((RectTransform)img.transform).sizeDelta = newSize;
        ((RectTransform)fogger.transform).sizeDelta = newSize;
        ((RectTransform)transform).sizeDelta = newSize;
        ((RectTransform)ScreenTrapz.transform).sizeDelta = newSize;

        cam.renderingPath = RenderingPath.Forward;
		cam.allowDynamicResolution = false;
		cam.allowHDR = false;
		cam.allowMSAA = false;

		GameObject rawGameobject = img.gameObject;

		DestroyImmediate(img);
		RawImage raw = rawGameobject.gameObject.AddComponent<RawImage>();
		raw.texture = Resources.Load<RenderTexture>("MinimapTexture");
		cam.targetTexture = (RenderTexture)raw.texture;
		raw.raycastTarget = false;
		
	}

    public float getAspectRatio()
    {
        return minimapWidth / minimapHeight;
    }

	public void showWarning (Vector3 location)
	{
		if (this.gameObject.activeInHierarchy) {
			StartCoroutine (displayWarning (location, defaultWarning, 10));
		}
	}

	public void showWarning (Vector3 location, Sprite myImage, float time = 10)
	{
		if (this.gameObject.activeInHierarchy) {
			StartCoroutine (displayWarning (location, myImage, time));
		}
	}

	IEnumerator displayWarning (Vector3 location, Sprite symbol, float time)
	{
		int iCoord = (int)(((location.x - Left) / (WorldWidth)) * UIWidth);
		int jCoord = (int)(((location.z - bottom) / (WorldHeight)) * UIHeight);

		GameObject obj = (GameObject)Instantiate (warningSymbol, new Vector2 (iCoord, jCoord), Quaternion.identity, newParent);
		obj.name += symbol.name;
		obj.GetComponent<Image> ().sprite = symbol;
		obj.transform.localPosition = new Vector2 (iCoord - UIWidth / 2, jCoord - UIHeight / 2);
		yield return new WaitForSeconds (time);
		Destroy (obj);
	}

	List<GameObject> currentIcons = new List<GameObject> ();

	public GameObject showUnitIcon (Vector3 location, Sprite symbol, bool pulsing = false)
	{
		//RectTransform newParent = (RectTransform)this.transform.parent.FindChild ("ScreenTrap");
		int iCoord = (int)(((location.x - Left) / (WorldWidth)) * UIWidth);
		int jCoord = (int)(((location.z - bottom) / (WorldHeight)) * UIHeight);
        
		GameObject obj = (GameObject)Instantiate (warningSymbol, new Vector2 (iCoord, jCoord), Quaternion.identity, newParent);
		if (pulsing) {
			//UIPulse pulser = 
				obj.AddComponent<UIPulse> ();
		}
		obj.GetComponent<Image> ().sprite = symbol;
		obj.transform.localPosition = new Vector2 (iCoord - UIWidth / 2, jCoord - UIHeight / 2);
		currentIcons.Add (obj);
		return obj;
	}

	public void deleteUnitIcon (GameObject iconObj)
	{
		if (currentIcons.Contains (iconObj)) {
			currentIcons.Remove (iconObj);
			Destroy (iconObj);
		}
	}

	public void updateUnitPos (GameObject sprite, Vector3 location)
	{
		if (currentIcons.Contains (sprite)) {
			
			int iCoord = (int)(((location.x - Left) / (WorldWidth)) * UIWidth);
			int jCoord = (int)(((location.z - bottom) / (WorldHeight)) * UIHeight);
			sprite.transform.localPosition = new Vector2 (iCoord - UIWidth / 2, jCoord - UIHeight / 2);
		}
	}



	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyUp (KeyCode.T)) {
			attackMoveMinimap ();
		}
	}

	public void toggleMegaMap ()
	{
		if (megaMap) {
			megaMap.SetActive (!megaMap.activeSelf);
		}
	}

	private Texture2D InitialTexture (Texture2D tex)
	{
		tex = new Texture2D (textureWidth, textureHeight, TextureFormat.ARGB32, false);

		for (int i = 0; i < textureWidth; i++) {
			for (int j = 0; j < textureHeight; j++) {
				tex.SetPixel (i, j, Color.clear);
			}
		}
		tex.Apply ();
		return tex;
	}


 
	private void clearTexture (Texture2D tex, bool[,] virtMap, bool apply, List<Point> used)
	{
		foreach (Point p in used) {
			tex.SetPixel (p.x, p.y, Color.clear);
		}
		used.Clear ();

		if (apply)
			tex.Apply ();
	}

	class Point
	{
		public Point (int X, int Y)
		{
			x = X;
			y = Y;
		}
		public int x, y;
	}

	List<Point> usedTriangleList;
	float WidthScale;
	float HeightScale;

	int ChitX;
	int ChitY;

	int iCoord;
	int jCoord;
	int chitSize;

	public Vector2 worldToMinimap(Vector3 location)
	{
		//int iCoord = (int)(((location.x - Left) / (WorldWidth)) * UIWidth);
		//int jCoord = (int)(((location.z - bottom) / (WorldHeight)) * UIHeight);
		return new Vector2((((location.x - Left) / (WorldWidth)) * UIWidth),(((location.z - bottom) / (WorldHeight)) * UIHeight));
	}
    
	Vector2 topLeftP;
	Vector2 topRightP;
	Vector2 botLeftP;
	Vector2 botRightP;
	RaycastHit hit;

	private void updateScreenRect ()
	{
		clearTexture (screenTrapzoidTex, virtTrapezoid, false, usedTriangleList);
        
		// DRAWING CAMERA TRAPEZOID
		topLeftP = Vector2.zero;
		topRightP = Vector2.zero;
		botLeftP = Vector2.zero;
		botRightP = Vector2.zero;

		ray1 = MainCamera.main.GetComponent<Camera>().ScreenPointToRay (new Vector3 (0, 150, 0));

		//Top left
		ray2 = MainCamera.main.GetComponent<Camera>().ScreenPointToRay (new Vector3 (0, Screen.height - 1, 0));

		//Top right
		ray3 = MainCamera.main.GetComponent<Camera>().ScreenPointToRay (new Vector3 (Screen.width, Screen.height - 1, 0));

		//Bottom right
		ray4 = MainCamera.main.GetComponent<Camera>().ScreenPointToRay (new Vector3 (Screen.width, 150, 0));

		// LOTS OF COMMENTED OUT STUFF FOR OPTIMIZATIONS!
	
		//Find world co-ordinates

		Physics.Raycast (ray1, out hit, Mathf.Infinity, 1 << 11);
		Vector3 v1 = hit.point;

		botLeftP = new Vector2 ((int)(((v1.x - Left) / (WorldWidth)) * textureWidth), (int)(((v1.z - bottom) / (WorldHeight)) * textureHeight));


		Physics.Raycast (ray2, out hit, Mathf.Infinity, 1 << 11);
		//Vector3 
		v1 = hit.point;
	
		topLeftP = new Vector2 ((int)(((v1.x - Left) / (WorldWidth)) * textureWidth), (int)(((v1.z - bottom) / (WorldHeight)) * textureHeight));

		Physics.Raycast (ray3, out hit, Mathf.Infinity, 1 << 11);
		//Vector3
		v1 = hit.point;
	
		topRightP = new Vector2 ((int)(((v1.x - Left) / (WorldWidth)) * textureWidth), (int)(((v1.z - bottom) / (WorldHeight)) * textureHeight));


		Physics.Raycast (ray4, out hit, Mathf.Infinity, 1 << 11);
		//Vector3
		v1 = hit.point;

		botRightP = new Vector2 ((int)(((v1.x - Left) / (WorldWidth)) * textureWidth), (int)(((v1.z - bottom) / (WorldHeight)) * textureHeight));

		drawLine (screenTrapzoidTex, virtTrapezoid, topLeftP, topRightP);
		drawLine (screenTrapzoidTex, virtTrapezoid, botLeftP, botRightP);
		drawLine (screenTrapzoidTex, virtTrapezoid, botLeftP, topLeftP);
		drawLine (screenTrapzoidTex, virtTrapezoid, botRightP, topRightP);

		screenTrapzoidTex.Apply ();
	}


	public void drawLine (Texture2D tex, bool[,] virtMap, Vector2 p1, Vector2 p2)
	{
		Vector2 t = p1;
	
		float Iterate = Mathf.Max (Mathf.Abs (p1.x - p2.x), Mathf.Abs (p1.y - p2.y));
		float ctr = 0;
		int counter = 0;
		while (((int)t.x != (int)p2.x || (int)t.y != (int)p2.y) && counter <= Iterate)
		{
			counter++;
			t = Vector2.Lerp(p1, p2, ctr);

			ctr += 1 / Iterate;

			if (t.x > 0 && t.y < tex.height && t.x < tex.width)
			{
				try
				{
					usedTriangleList.Add(PointArray[(int)t.x, (int)t.y]);
					//virtMap [(int)t.x, (int)t.y] = true;
					tex.SetPixel((int)t.x, (int)t.y, Color.magenta);
				}
				catch (Exception)
				{
				}
			}
		}
	}

	public void drawPath(List<Vector3> points, float duration, float thickness=1)
	{
		StartCoroutine (drawPathY( points, duration,thickness));
	}

	IEnumerator drawPathY(List<Vector3> points, float duration, float thickness)
	{
		List<GameObject> myArrows = new List<GameObject> ();
		List<Vector2> newList = new List<Vector2> ();

		//RectTransform newParent = (RectTransform)this.transform.parent.FindChild ("ScreenTrap");
		for (int i = 0; i < points.Count; i++) {
			newList.Add (worldToMinimap (points[i]));
		}

		int indexA = 0;
		int indexB = 1;
		while (indexB < points.Count) {
		
			float distance = Vector2.Distance (newList [indexA], newList [indexB]);

			for (float i = 0; i < distance; i +=20) {
				GameObject obj = (GameObject)Instantiate (warningSymbol, Vector2.Lerp(newList[indexA], newList[indexB], (i/ distance)), Quaternion.identity, newParent);
				obj.transform.SetAsFirstSibling();
				((RectTransform)obj.transform).anchorMax = Vector2.zero;
				((RectTransform)obj.transform).anchorMin = Vector2.zero;
				((RectTransform)obj.transform).sizeDelta = new Vector2 (20 *thickness,20);
				obj.GetComponent<Image> ().sprite = lineArrow;
				((RectTransform) obj.transform).anchoredPosition = Vector2.Lerp(newList[indexA], newList[indexB], (i +6)/ distance);

				obj.transform.up = new Vector3 (  newList [indexB].x - newList [indexA].x , newList [indexB].y - newList [indexA].y   , 0);//  .Rotate (new Vector3());
				myArrows.Add (obj);
				yield return new WaitForSeconds (.12f);
			}
		
			indexA++;
			indexB++;
		}

		yield return new WaitForSeconds (duration);

		foreach (GameObject toDelete in myArrows) {
			Destroy (toDelete);
		}
	}



	Color32[] pixelsArray;

	int lastFrame;
	public void setFog ()
	{

		if (lastFrame == Time.frameCount) {
			return;
		}
		lastFrame = Time.frameCount;

		if (!fog.HasUnFogged) {
			return;
		}
		System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch ();
		watch.Start ();
		
		_texture = fog.getTexture ();
		_texture.Apply ();

	}

	public void mapMover ()
	{
		Vector3 clickPos = transform.InverseTransformPoint (Input.mousePosition);

		float x = ((clickPos.x) / myRect.rect.width) + .5f;// minimapWidth;
		float y = ((clickPos.y) / myRect.rect.height) + .5f;

		Vector2 toMove = new Vector2 ((x) * WorldWidth + Left, 
			                 y * WorldHeight - 65 + bottom - Mathf.Tan (Mathf.Deg2Rad * MainCamera.main.AngleOffset) * (MainCamera.main.transform.position.y - MainCamera.main.m_MinFieldOfView));

		MainCamera.main.CenterCamera(toMove);
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left) {
			dragging = false;		
		}			
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		if (!this.enabled) {
			return;
		}
		if (eventData.button == PointerEventData.InputButton.Left) {
			dragging = true;
			StartCoroutine (DragMini ());

		} else if (eventData.button == PointerEventData.InputButton.Right) {
		
			Vector3 clickPos = transform.InverseTransformPoint (eventData.pressPosition);
			float x = .5f + (clickPos.x) / minimapWidth;
			float y = .5f + (clickPos.y) / minimapHeight;
			Vector3 RayPoint = new Vector3 ((x * WorldWidth) + Left, 100, (y * WorldHeight) + bottom);

			RaycastHit hit;		

			if (Physics.Raycast (RayPoint, Vector3.down, out hit, 400,UIManager.main.EverythinCast)) {
                if (UIManager.main.CurrentMode == Mode.Normal)
                {
                    SelectedManager.main.GiveOrder(Orders.CreateMoveOrder(hit.point));
                }
                else
                {
                    MainCamera.main.CenterCamera(hit.point);
                }
			}
		}
	}


	IEnumerator DragMini ()
	{
		while (dragging) {
			mapMover ();
			yield return null;
		}
	}

	public void attackMoveMinimap ()
	{
		if (!this.enabled) {
			return;
		}

		Vector3 clickPos = transform.InverseTransformPoint (Input.mousePosition);

		float x = ((clickPos.x) / myRect.rect.width) + .5f;// minimapWidth;
		float y = ((clickPos.y) / myRect.rect.height) + .5f;

		Vector3 RayPoint = new Vector3 ((x * WorldWidth) + Left, 100, (y * WorldHeight) + bottom);

		RaycastHit hit;		

		if (Physics.Raycast (RayPoint, Vector3.down, out hit, 400,UIManager.main.EverythinCast)) {

			//Debug.Log ("moving to " + hit.point);
			SelectedManager.main.attackMoveO (hit.point);
		}
	}
   
}