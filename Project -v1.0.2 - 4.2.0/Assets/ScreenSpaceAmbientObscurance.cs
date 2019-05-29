using System;
using UnityEngine;


[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]

public class SurforgeSSAO : MonoBehaviour {

	
	[Range (0,30)]
	public float intensity = 0.5f;
	[Range (0.1f,300)]
	public float radius = 0.2f;
	[Range (0,3)]
	public int blurIterations = 1;
	[Range (0,5)]
	public float blurFilterDistance = 1.25f;
	[Range (0,1)]
	public int downsample = 0;

	public Texture2D rand = null;
	public Shader aoShader= null;

	private Material aoMaterial = null;



	protected bool  supportHDRTextures = true;
	protected bool  supportDX11 = false;
	protected bool  isSupported = true;
		
	protected Material CheckShaderAndCreateMaterial ( Shader s, Material m2Create)
	{
		if (!s)
		{
			Debug.Log("Missing shader in " + ToString ());
			enabled = false;
			return null;
		}

		if (s.isSupported && m2Create && m2Create.shader == s)
			return m2Create;
			
		if (!s.isSupported)
		{
			NotSupported ();
			Debug.Log("The shader " + s.ToString() + " on effect "+ToString()+" is not supported on this platform!");
			return null;
		}
		else
		{
			m2Create = new Material (s);
			m2Create.hideFlags = HideFlags.DontSave;
			if (m2Create)
				return m2Create;
			else return null;
		}
	}
		
		
	protected Material CreateMaterial (Shader s, Material m2Create)
	{
		if (!s)
		{
			Debug.Log ("Missing shader in " + ToString ());
			return null;
		}
			
		if (m2Create && (m2Create.shader == s) && (s.isSupported))
			return m2Create;
			
		if (!s.isSupported)
		{
			return null;
		}
		else
		{
			m2Create = new Material (s);
			m2Create.hideFlags = HideFlags.DontSave;
			if (m2Create)
				return m2Create;
			else return null;
		}
	}
		
	void OnEnable ()
	{
		isSupported = true;
	}
		
	protected bool CheckSupport ()
	{
		return CheckSupport (false);
	}
		
		
	protected void Start ()
	{
		CheckResources ();
	}
		
	protected bool CheckSupport (bool needDepth)
	{
		isSupported = true;
		supportDX11 = SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders;
			
	
			
		if (needDepth)
			GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
			
		return true;
	}
		
	protected bool CheckSupport (bool needDepth,  bool needHdr)
	{
		if (!CheckSupport(needDepth))
			return false;
			
		if (needHdr && !supportHDRTextures)
		{
			NotSupported ();
			return false;
		}
			
		return true;
	}
		
		
	public bool Dx11Support ()
	{
		return supportDX11;
	}
		
		
	protected void ReportAutoDisable ()
	{
		Debug.LogWarning ("The image effect " + ToString() + " has been disabled as it's not supported on the current platform.");
	}
		
	// deprecated but needed for old effects to survive upgrading
	bool CheckShader (Shader s)
	{
		Debug.Log("The shader " + s.ToString () + " on effect "+ ToString () + " is not part of the Unity 3.2+ effects suite anymore. For best performance and quality, please ensure you are using the latest Standard Assets Image Effects (Pro only) package.");
		if (!s.isSupported)
		{
			NotSupported ();
			return false;
		}
		else
		{
			return false;
		}
	}
		
		
	protected void NotSupported ()
	{
		enabled = false;
		isSupported = false;
		return;
	}


	public bool CheckResources () {
		//CheckSupport (true);

		aoMaterial = CheckShaderAndCreateMaterial (aoShader, aoMaterial);

		if (!isSupported)
			ReportAutoDisable ();
		return isSupported;
	}

	void OnDisable () {
		if (aoMaterial)
			DestroyImmediate (aoMaterial);
		aoMaterial = null;
	}

	[ImageEffectOpaque]
	void OnRenderImage (RenderTexture source, RenderTexture destination) {

		SurforgeSSAOSettings ssaoSettings = (SurforgeSSAOSettings)GetComponent<SurforgeSSAOSettings>();

		if (CheckResources () == false) {
			Graphics.Blit (source, destination);
			return;
		}


		Matrix4x4 P = ssaoSettings.matrix;

		var invP= P.inverse;
		Vector4 projInfo = new Vector4

		((-2.0f / (ssaoSettings.res * P[0])),
		 (-2.0f / (ssaoSettings.res * P[5])),
			 
		 ((1.0f - P[2]) / P[0]),
		 ((1.0f + P[6]) / P[5]));

		radius = radius * ssaoSettings.aa;
			
		aoMaterial.SetVector ("_ProjInfo", projInfo); // used for unprojection
		aoMaterial.SetMatrix ("_ProjectionInv", invP); // only used for reference
		aoMaterial.SetTexture ("_Rand", rand); // not needed for DX11 :)
		aoMaterial.SetFloat ("_Radius", radius);
		aoMaterial.SetFloat ("_Radius2", radius*radius);
		aoMaterial.SetFloat ("_Intensity", intensity);
		aoMaterial.SetFloat ("_BlurFilterDistance", blurFilterDistance);

		aoMaterial.SetFloat ("_samplesMult", 1.0f);

		int rtW = ssaoSettings.res;
		int rtH = ssaoSettings.res;
			
			
		RenderTexture tmpRt  = RenderTexture.GetTemporary (rtW, rtH);

		Graphics.Blit (source, tmpRt, aoMaterial, 0);


		aoMaterial.SetTexture ("_AOTex", tmpRt);
		Graphics.Blit (source, destination, aoMaterial, 2);

		RenderTexture.ReleaseTemporary (tmpRt);
	}

}

